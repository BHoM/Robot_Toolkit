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
using System.Linq;
using BH.oM.Structure.Constraints;
using RobotOM;
using BH.Engine.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        /***************************************************/

        private bool CreateCollection(IEnumerable<Constraint6DOF> constraints)
        {
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel robotLabel = robotLabelServer.Create(IRobotLabelType.I_LT_SUPPORT, "");
            RobotNodeSupportData supportData = robotLabel.Data;

            foreach(Constraint6DOF constraint in constraints)
            {
                string name = constraint.Name;
                Convert.RobotConstraint(robotLabel.Data, constraint);
                if (robotLabelServer.Exist(IRobotLabelType.I_LT_SUPPORT, constraint.Name) == -1)
                {
                    robotLabel = robotLabelServer.Get(IRobotLabelType.I_LT_SUPPORT, constraint.Name);
                    Constraint6DOF robotConstraint = Convert.ToBHoMObject(supportData, robotLabel.Name);
                    BH.Engine.Reflection.Compute.RecordWarning("Support " + name + " already exists in the model, the properties will be overwritten");
                }
                robotLabelServer.StoreWithName(robotLabel, name);
            }
            return true;            

        }

        /***************************************************/

    }

}


