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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BH.oM.Geometry;
using BH.oM.Spatial.SettingOut;
using RobotOM;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Converts a Robot Cartesian grid system to BHoM Grids. Robot axes have no length, so the grid lines are drawn out to the extents of the perpendicular axes.")]
        public static List<Grid> FromRobot(this RobotStructuralAxisGridCartesian cartesian)
        {
            List<Grid> grids = new List<Grid>();

            if (cartesian == null)
                return grids;

            List<KeyValuePair<double, string>> xAxes = cartesian.X.FromRobot();
            List<KeyValuePair<double, string>> yAxes = cartesian.Y.FromRobot();

            GetGridExtents(xAxes, out double minX, out double maxX);
            GetGridExtents(yAxes, out double minY, out double maxY);

            // Axes in the X list sit at a constant X, so they run parallel to the Y axis (and vice versa)
            foreach (KeyValuePair<double, string> axis in xAxes)
            {
                Line line = new Line { Start = new Point { X = axis.Key, Y = minY }, End = new Point { X = axis.Key, Y = maxY } };
                grids.Add(new Grid { Name = axis.Value, Curve = line });
            }

            foreach (KeyValuePair<double, string> axis in yAxes)
            {
                Line line = new Line { Start = new Point { X = minX, Y = axis.Key }, End = new Point { X = maxX, Y = axis.Key } };
                grids.Add(new Grid { Name = axis.Value, Curve = line });
            }

            // TODO: confirm the unit of RotationAngle in Robot (assumed degrees) and whether positions are relative to GetRelativeToPoint
            double rotation = cartesian.RotationAngle;
            if (Math.Abs(rotation) > Tolerance.Angle)
            {
                foreach (Grid grid in grids)
                    grid.Curve = Engine.Geometry.Modify.IRotate(grid.Curve, Point.Origin, Vector.ZAxis, rotation * Math.PI / 180);
            }

            return grids;
        }

        /***************************************************/

        [Description("Reads the axes of a Robot axis sequence as pairs of position and label, in the order they are stored in Robot.")]
        public static List<KeyValuePair<double, string>> FromRobot(this RobotStructuralAxisSequenceList axes)
        {
            List<KeyValuePair<double, string>> result = new List<KeyValuePair<double, string>>();

            if (axes == null)
                return result;

            // A single axis is reported with AxisCount 0 but a SequenceCount of 1
            int axisCount = axes.AxisCount == 0 && axes.SequenceCount > 0 ? 1 : axes.AxisCount;
            for (int i = 1; i <= axisCount; i++)
            {
                if (axes.GetAxis(i, out string label, out double position, out bool singleOut))
                    result.Add(new KeyValuePair<double, string>(position, label));
            }

            return result;
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private static void GetGridExtents(List<KeyValuePair<double, string>> axes, out double min, out double max)
        {
            // With fewer than two perpendicular axes there is nothing to span, so fall back to a fixed length either side
            const double fallbackHalfLength = 5;

            if (axes.Count >= 2)
            {
                min = axes.Min(x => x.Key);
                max = axes.Max(x => x.Key);
            }
            else
            {
                double centre = axes.Count == 1 ? axes[0].Key : 0;
                min = centre - fallbackHalfLength;
                max = centre + fallbackHalfLength;
            }
        }

        /***************************************************/
    }
}
