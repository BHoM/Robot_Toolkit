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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

                List<Grid> systemGrids = cartesian.FromRobot();

                grids.AddRange(names == null ? systemGrids : systemGrids.Where(x => names.Contains(x.Name)));
            }

            return grids;
        }

        /***************************************************/
    }
}
