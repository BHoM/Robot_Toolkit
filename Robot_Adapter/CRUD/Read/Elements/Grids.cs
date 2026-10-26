/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Geometry;
using BH.oM.Spatial.SettingOut;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Grid> ReadGrids(IList ids = null)
        {
            List<Grid> grids = new List<Grid>();
            HashSet<string> names = ids == null ? null : new HashSet<string>(ids.Cast<object>().Select(x => x.ToString()));

            RobotStructuralAxisGridMngr gridManager = m_RobotApplication.Project.AxisMngr;

            for (int i = 1; i <= gridManager.Count; i++)
            {
                IRobotStructuralAxisGrid gridSystem = gridManager.Get(i);

                RobotStructuralAxisGridCartesian cartesian = gridSystem as RobotStructuralAxisGridCartesian;
                if (cartesian == null)
                {
                    Engine.Base.Compute.RecordWarning($"Can not pull {gridSystem.Type} grid systems from Robot, offender: {gridSystem.Name}. Only Cartesian grid systems are supported.");
                    continue;
                }

                List<KeyValuePair<string, double>> xAxes = ReadGridAxes(cartesian.X);
                List<KeyValuePair<string, double>> yAxes = ReadGridAxes(cartesian.Y);

                GetGridExtents(xAxes, out double minX, out double maxX);
                GetGridExtents(yAxes, out double minY, out double maxY);

                List<Grid> systemGrids = new List<Grid>();

                // Axes in the X list sit at a constant X, so they run parallel to the Y axis (and vice versa)
                foreach (KeyValuePair<string, double> axis in xAxes)
                {
                    Line line = new Line { Start = new Point { X = axis.Value, Y = minY }, End = new Point { X = axis.Value, Y = maxY } };
                    systemGrids.Add(new Grid { Name = axis.Key, Curve = line });
                }

                foreach (KeyValuePair<string, double> axis in yAxes)
                {
                    Line line = new Line { Start = new Point { X = minX, Y = axis.Value }, End = new Point { X = maxX, Y = axis.Value } };
                    systemGrids.Add(new Grid { Name = axis.Key, Curve = line });
                }

                // TODO: confirm the unit of RotationAngle in Robot (assumed degrees) and whether positions are relative to GetRelativeToPoint
                double rotation = cartesian.RotationAngle;
                if (Math.Abs(rotation) > 1e-9)
                {
                    foreach (Grid grid in systemGrids)
                        grid.Curve = Engine.Geometry.Modify.IRotate(grid.Curve, Point.Origin, Vector.ZAxis, rotation * Math.PI / 180);
                }

                grids.AddRange(names == null ? systemGrids : systemGrids.Where(x => names.Contains(x.Name)));
            }

            return grids;
        }

        /***************************************************/

        private static List<KeyValuePair<string, double>> ReadGridAxes(RobotStructuralAxisSequenceList axes)
        {
            List<KeyValuePair<string, double>> result = new List<KeyValuePair<string, double>>();

            // A single axis is reported with AxisCount 0 but a SequenceCount of 1
            int axisCount = axes.AxisCount == 0 && axes.SequenceCount > 0 ? 1 : axes.AxisCount;
            for (int i = 1; i <= axisCount; i++)
            {
                if (axes.GetAxis(i, out string label, out double position, out bool singleOut))
                    result.Add(new KeyValuePair<string, double>(label, position));
            }

            return result;
        }

        /***************************************************/

        private static void GetGridExtents(List<KeyValuePair<string, double>> axes, out double min, out double max)
        {
            // Robot axes have no length, so grid lines are drawn out to the extents of the perpendicular axes
            // With fewer than two of them there is nothing to span, so fall back to a fixed length either side
            const double fallbackHalfLength = 5;

            if (axes.Count >= 2)
            {
                min = axes.Min(x => x.Value);
                max = axes.Max(x => x.Value);
            }
            else
            {
                double centre = axes.Count == 1 ? axes[0].Value : 0;
                min = centre - fallbackHalfLength;
                max = centre + fallbackHalfLength;
            }
        }

        /***************************************************/

    }
}
