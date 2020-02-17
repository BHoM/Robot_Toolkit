/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using System.Collections.Generic;
using RobotOM;
using BH.oM.Structure.Constraints;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Constraint4DOF> ReadConstraints4DOF(List<string> ids = null)
        {
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotNamesArray robotLabelNames = robotLabelServer.GetAvailableNames(IRobotLabelType.I_LT_LINEAR_RELEASE);     
            List<Constraint4DOF> linearReleases = new List<Constraint4DOF>();
            IRobotLabel robotLabel = null;
            for (int i = 1; i <= robotLabelNames.Count; i++)
            {
                string robotLabelName = robotLabelNames.Get(i);
                if (robotLabelServer.IsUsed(IRobotLabelType.I_LT_LINEAR_RELEASE, robotLabelName))
                    robotLabel = robotLabelServer.Get(IRobotLabelType.I_LT_LINEAR_RELEASE, robotLabelNames.Get(i));
                else
                    robotLabel = robotLabelServer.CreateLike(IRobotLabelType.I_LT_LINEAR_RELEASE, "", robotLabelName);
                if (robotLabel == null)
                    BH.Engine.Reflection.Compute.RecordWarning("Failed to read label '" + robotLabelName);
                else
                { 
                    Constraint4DOF linearRelease = Convert.FromRobot(robotLabel, robotLabelName);
                    linearRelease.CustomData.Add(AdapterIdName, robotLabel.Name);
                    linearRelease.Name = robotLabelName;
                    linearReleases.Add(linearRelease);
                }
            }
            return linearReleases;
        }

        /***************************************************/

    }
}


