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
            grids.ToRobot(out List<KeyValuePair<double, string>> xAxes, out List<KeyValuePair<double, string>> yAxes);

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

            if (Math.Abs(cartesian.RotationAngle) > Tolerance.Angle)
            {
                Engine.Base.Compute.RecordError($"The grid system '{m_GridSystemName}' in Robot is rotated, so grids can not be added to it. Grids have not been pushed.");
                return false;
            }

            // Grids already in the system are kept, and only replaced if a pushed grid has the same name or position
            int xCount = xAxes.Count > 0 ? xAxes.ToRobot(cartesian.X) : cartesian.X.FromRobot().Count;
            int yCount = yAxes.Count > 0 ? yAxes.ToRobot(cartesian.Y) : cartesian.Y.FromRobot().Count;

            cartesian.Save();

            Engine.Base.Compute.RecordNote($"Pushed {xAxes.Count} X and {yAxes.Count} Y grid(s) out of {grids.Count()} to the Robot grid system '{m_GridSystemName}'.");

            // Robot stores a lone axis but does not list or draw it in the Structural Axis dialog
            if (xCount == 1 || yCount == 1)
                Engine.Base.Compute.RecordWarning($"Only one grid is in the {(xCount == 1 ? "X" : "Y")} direction of '{m_GridSystemName}'. Robot does not list a single axis in its Structural Axis dialog, so at least two per direction are needed to see them.");

            gridManager.Activate(index, true);

            return true;
        }

        /***************************************************/

        private const string m_GridSystemName = "BHoM Grids";

        /***************************************************/
    }
}
