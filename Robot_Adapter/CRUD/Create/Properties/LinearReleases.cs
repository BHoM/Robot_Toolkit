/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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


using BH.oM.Structure.Constraints;
using RobotOM;
using System.Collections.Generic;
using BH.Engine.Structure;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        /***************************************************/

        private bool CreateCollection(IEnumerable<Constraint4DOF> linearReleases)
        {
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel robotLabel = robotLabelServer.Create(IRobotLabelType.I_LT_LINEAR_RELEASE, "");
            List<Constraint4DOF> linearReleasesToUpdate = new List<Constraint4DOF>();
            foreach (Constraint4DOF linearRelease in linearReleases)
            {
                if (CheckNotNull(linearRelease, oM.Base.Debugging.EventType.Warning))
                {
                    Convert.ToRobot(robotLabel.Data, linearRelease);
                    robotLabelServer.StoreWithName(robotLabel, linearRelease.DescriptionOrName());
                    SetAdapterId(linearRelease, linearRelease.DescriptionOrName());
                }
            }
            return true;
        }

        /***************************************************/
    }

}





