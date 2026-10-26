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
using BH.Engine.Geometry;
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

        [Description("Sorts BHoM Grids into the X and Y axes of a Robot Cartesian grid system, as pairs of position and label. Only straight grids parallel to the global X or Y axis can be converted.")]
        public static void ToRobot(this IEnumerable<Grid> grids, out List<KeyValuePair<double, string>> xAxes, out List<KeyValuePair<double, string>> yAxes)
        {
            xAxes = new List<KeyValuePair<double, string>>();
            yAxes = new List<KeyValuePair<double, string>>();

            foreach (Grid grid in grids)
            {
                if (grid == null)
                {
                    Engine.Base.Compute.RecordError("Can not push a null Grid.");
                    continue;
                }

                if (grid.Curve == null)
                {
                    Engine.Base.Compute.RecordError($"Grid '{grid.Name}' has a null Curve and has not been pushed.");
                    continue;
                }

                if (!TryGetGridLineEnds(grid.Curve, out Point start, out Point end))
                {
                    Engine.Base.Compute.RecordWarning($"Grid '{grid.Name}' is not a straight line and has not been pushed. Only straight, axis-aligned grids are supported.");
                    continue;
                }

                // Direction of the line, used to decide if it is parallel to the Y axis (an X axis in Robot) or to the X axis
                Vector planDirection = new Vector { X = end.X - start.X, Y = end.Y - start.Y };
                if (planDirection.Length() < Tolerance.MicroDistance)
                {
                    Engine.Base.Compute.RecordWarning($"Grid '{grid.Name}' has no length in the XY plane and has not been pushed.");
                    continue;
                }

                planDirection = planDirection.Normalise();

                if (Math.Abs(planDirection.X) < Tolerance.Angle)
                    xAxes.Add(new KeyValuePair<double, string>(start.X, grid.Name));
                else if (Math.Abs(planDirection.Y) < Tolerance.Angle)
                    yAxes.Add(new KeyValuePair<double, string>(start.Y, grid.Name));
                else
                    Engine.Base.Compute.RecordWarning($"Grid '{grid.Name}' is skewed and has not been pushed. Only axis-aligned grids are supported.");
            }
        }

        /***************************************************/

        [Description("Merges axes, as pairs of position and label, into a Robot axis sequence. Axes already in the sequence are kept, unless they share a label or position with one of the new axes, in which case they are replaced. Returns the total number of axes in the sequence.")]
        public static int ToRobot(this List<KeyValuePair<double, string>> axes, RobotStructuralAxisSequenceList robotAxes)
        {
            if (robotAxes == null)
                return 0;

            // Robot axes are stored as a start position followed by spacings, so sort and drop any coincident new axes
            List<KeyValuePair<double, string>> newAxes = new List<KeyValuePair<double, string>>();
            foreach (KeyValuePair<double, string> axis in axes.OrderBy(x => x.Key))
            {
                if (newAxes.Count > 0 && axis.Key - newAxes.Last().Key < Tolerance.Distance)
                {
                    Engine.Base.Compute.RecordWarning($"Grid '{axis.Value}' is at the same position as grid '{newAxes.Last().Value}' and has not been pushed.");
                    continue;
                }

                string label = string.IsNullOrWhiteSpace(axis.Value) ? axis.Key.ToString() : axis.Value;
                newAxes.Add(new KeyValuePair<double, string>(axis.Key, label));
            }

            // Keep the axes already in Robot, so that pushing a subset of grids does not remove the others
            List<KeyValuePair<double, string>> merged = robotAxes.FromRobot();
            foreach (KeyValuePair<double, string> axis in newAxes)
            {
                merged.RemoveAll(x => x.Value == axis.Value || Math.Abs(x.Key - axis.Key) < Tolerance.Distance);
                merged.Add(axis);
            }

            merged = merged.OrderBy(x => x.Key).ToList();

            robotAxes.Clear();

            if (merged.Count == 0)
                return 0;

            robotAxes.StartPosition = merged[0].Key;

            // Robot stores a single axis as a start position plus an empty sequence, and reports AxisCount as 0 for it
            if (merged.Count == 1)
                robotAxes.AddSequence(0, 0);

            for (int i = 1; i < merged.Count; i++)
                robotAxes.AddSequence(merged[i].Key - merged[i - 1].Key, 1);

            // TODO: confirm in Robot whether SetLabelFormat needs to be called first for custom labels to stick
            for (int i = 0; i < merged.Count; i++)
                robotAxes.SetAxisLabel(i + 1, merged[i].Value);

            return merged.Count;
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private static bool TryGetGridLineEnds(ICurve curve, out Point start, out Point end)
        {
            start = null;
            end = null;

            // IIsLinear has no implementation for NurbsCurve
            if (curve is NurbsCurve || !curve.IIsLinear())
                return false;

            start = curve.IStartPoint();
            end = curve.IEndPoint();
            return true;
        }

        /***************************************************/
    }
}
