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

using BH.oM.Structure.Elements;
using BH.oM.Structure.SurfaceProperties;
using RobotOM;
using System.Collections.Generic;
using BH.oM.Adapter;
using System.Linq;
using BH.oM.Structure.Constraints;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected bool Update(IEnumerable<Constraint6DOF> supports)
        {
            bool success = true;
            m_RobotApplication.Interactive = 0;
            RobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            try
            {
                foreach (Constraint6DOF support in supports)
                {
                    RobotNodeSupport robotSupport = m_RobotApplication.Project.Structure.Labels.Get(IRobotLabelType.I_LT_SUPPORT, support.Name) as RobotNodeSupport;
                    Engine.Robot.Convert.RobotConstraint(robotSupport.Data, support);
                    robotLabelServer.Store(robotSupport);
                }
            }
            finally
            {
                m_RobotApplication.Interactive = 1;
            }

            return success;
        }

        /***************************************************/
    }
}

