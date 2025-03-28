/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using BH.oM.Structure.Elements;
using BH.oM.Structure.SurfaceProperties;
using RobotOM;
using System.Collections.Generic;
using BH.oM.Adapter;
using System.Linq;
using BH.oM.Structure.Constraints;
using BH.Engine.Structure;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected bool Update(IEnumerable<Constraint4DOF> linearReleases)
        {
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            foreach (Constraint4DOF constraint in linearReleases)
            {
                IRobotLabel robotLabel = robotLabelServer.Get(IRobotLabelType.I_LT_LINEAR_RELEASE, constraint.Name);
                Constraint4DOF robotConstraint = Convert.FromRobot(robotLabel, robotLabel.Data as IRobotLinearReleaseData);
                Constraint4DOFComparer constraint4DOFComparer = new Constraint4DOFComparer();
                if (constraint4DOFComparer.Equals(constraint, robotConstraint))
                    continue;
                else
                {
                    Convert.ToRobot(robotLabel.Data, constraint);
                    robotLabelServer.StoreWithName(robotLabel, constraint.Name);
                    BH.Engine.Base.Compute.RecordWarning("Linear Release '" + constraint.Name + "' already exists in the model, the properties have been overwritten");
                }

            }
            return true;
        }

        /***************************************************/

    }
}






