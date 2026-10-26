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
using System.Linq;
using BH.oM.Geometry;
using BH.oM.Spatial.SettingOut;
using BH.Engine.Geometry;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<Grid> grids)
        {
            List<KeyValuePair<double, string>> xAxes = new List<KeyValuePair<double, string>>();
            List<KeyValuePair<double, string>> yAxes = new List<KeyValuePair<double, string>>();

            foreach (Grid grid in grids)
            {
                if (!CheckNotNull(grid) || !CheckNotNull(grid.Curve, oM.Base.Debugging.EventType.Error, typeof(Grid)))
                    continue;

                if (!TryGetGridLineEnds(grid.Curve, out Point start, out Point end))
                {
                    Engine.Base.Compute.RecordWarning($"Grid '{grid.Name}' is not a straight line and has not been pushed. Only straight, axis-aligned grids are supported.");
                    continue;
                }

                // Direction of the line, used to decide if it is parallel to the Y axis (an X axis in Robot) or to the X axis
                double length = Math.Sqrt((end.X - start.X) * (end.X - start.X) + (end.Y - start.Y) * (end.Y - start.Y));
                if (length < 1e-9)
                {
                    Engine.Base.Compute.RecordWarning($"Grid '{grid.Name}' has no length in the XY plane and has not been pushed.");
                    continue;
                }

                double dirX = (end.X - start.X) / length;
                double dirY = (end.Y - start.Y) / length;
                const double directionTolerance = 1e-3;

                if (Math.Abs(dirX) < directionTolerance)
                    xAxes.Add(new KeyValuePair<double, string>(start.X, grid.Name));
                else if (Math.Abs(dirY) < directionTolerance)
                    yAxes.Add(new KeyValuePair<double, string>(start.Y, grid.Name));
                else
                    Engine.Base.Compute.RecordWarning($"Grid '{grid.Name}' is skewed and has not been pushed. Only axis-aligned grids are supported.");
            }

            if (xAxes.Count == 0 && yAxes.Count == 0)
            {
                Engine.Base.Compute.RecordWarning("None of the Grids could be pushed to Robot. Only straight lines parallel to the global X or Y axis are supported.");
                return true;
            }

            // Use a dedicated grid system so that any grids the user has set up in Robot are left alone
            RobotStructuralAxisGridMngr gridManager = m_RobotApplication.Project.AxisMngr;
            int index = gridManager.FindByName(m_GridSystemName);

            if (index < 1 || index > gridManager.Count)
            {
                gridManager.Create(IRobotStructuralAxisGridType.I_SAGT_CARTESIAN, m_GridSystemName);
                index = gridManager.FindByName(m_GridSystemName);
            }

            RobotStructuralAxisGridCartesian cartesian = gridManager.Get(index) as RobotStructuralAxisGridCartesian;
            if (cartesian == null)
            {
                Engine.Base.Compute.RecordError($"A grid system named '{m_GridSystemName}' exists in Robot but is not Cartesian. Grids have not been pushed.");
                return false;
            }

            SetGridAxes(cartesian.X, xAxes);
            SetGridAxes(cartesian.Y, yAxes);

            cartesian.Save();

            Engine.Base.Compute.RecordNote($"Pushed {xAxes.Count} X and {yAxes.Count} Y grid(s) out of {grids.Count()} to the Robot grid system '{m_GridSystemName}'.");

            // Robot stores a lone axis but does not list or draw it in the Structural Axis dialog
            if (xAxes.Count == 1 || yAxes.Count == 1)
                Engine.Base.Compute.RecordWarning($"Only one grid was pushed in the {(xAxes.Count == 1 ? "X" : "Y")} direction. Robot does not list a single axis in its Structural Axis dialog, so push at least two per direction to see them.");
            gridManager.Activate(index, true);

            return true;
        }

        /***************************************************/

        private static bool TryGetGridLineEnds(ICurve curve, out Point start, out Point end)
        {
            start = null;
            end = null;

            Polyline polyline = curve as Polyline;
            if (curve is Line || (polyline != null && polyline.ControlPoints.Count == 2))
            {
                start = curve.IStartPoint();
                end = curve.IEndPoint();
                return true;
            }

            return false;
        }

        /***************************************************/

        private static void SetGridAxes(RobotStructuralAxisSequenceList robotAxes, List<KeyValuePair<double, string>> axes)
        {
            robotAxes.Clear();

            // Robot axes are stored as a start position followed by spacings, so sort and drop any coincident axes
            List<KeyValuePair<double, string>> sorted = new List<KeyValuePair<double, string>>();
            foreach (KeyValuePair<double, string> axis in axes.OrderBy(x => x.Key))
            {
                if (sorted.Count > 0 && axis.Key - sorted.Last().Key < 1e-6)
                {
                    Engine.Base.Compute.RecordWarning($"Grid '{axis.Value}' is at the same position as grid '{sorted.Last().Value}' and has not been pushed.");
                    continue;
                }
                sorted.Add(axis);
            }

            if (sorted.Count == 0)
                return;

            robotAxes.StartPosition = sorted[0].Key;
            
            // Robot stores a single axis as a start position plus an empty sequence, and reports AxisCount as 0 for it
            if (sorted.Count == 1)
                robotAxes.AddSequence(0, 0);

            for (int i = 1; i < sorted.Count; i++)
                robotAxes.AddSequence(sorted[i].Key - sorted[i - 1].Key, 1);

            // TODO: confirm in Robot whether SetLabelFormat needs to be called first for custom labels to stick
            for (int i = 0; i < sorted.Count; i++)
            {
                string label = string.IsNullOrWhiteSpace(sorted[i].Value) ? sorted[i].Key.ToString() : sorted[i].Value;
                robotAxes.SetAxisLabel(i + 1, label);
            }
        }

        /***************************************************/

        private const string m_GridSystemName = "BHoM Grids";

        /***************************************************/

    }
}
