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

using RobotOM;
using BH.oM.Structure.Constraints;
using System.Collections.Generic;
using BH.oM.Base;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        private List<BHoMObject> ReadLabels(IRobotLabelType robotLabelType)
        {
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotNamesArray robotLabelNames = robotLabelServer.GetAvailableNames(robotLabelType);
            List<BHoMObject> objects = new List<BHoMObject>();
            RobotNodeSupport robotLabel = null;
            for (int i = 1; i <= robotLabelNames.Count; i++)
            {
                string robotLabelName = robotLabelNames.Get(i);
                if (robotLabelServer.IsUsed(robotLabelType, robotLabelName))
                    robotLabel = robotLabelServer.Get(robotLabelType, robotLabelNames.Get(i)) as dynamic;
                else
                    robotLabel = robotLabelServer.CreateLike(robotLabelType, "", robotLabelName) as dynamic;
                if (robotLabel == null)
                    BH.Engine.Reflection.Compute.RecordWarning("Failed to read label '" + robotLabelName);
                else
                {
                    BHoMObject obj = Convert.FromRobot(robotLabel, robotLabelName);
                    obj.CustomData.Add(AdapterIdName, robotLabel.Name);
                    obj.Name = robotLabel.Name;
                    objects.Add(obj);
                }
            }
            return objects;
        }

        /***************************************************/
    }
}

