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

using System.Collections.Generic;
using RobotOM;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected bool Update(IEnumerable<FramingElementDesignProperties> framingElementDesignPropertiesList)
        {

            foreach (FramingElementDesignProperties framEleDesProps in framingElementDesignPropertiesList)
            {
                if (!CheckNotNull(framEleDesProps, oM.Base.Debugging.EventType.Warning))
                    continue;

                // Check if the label exists before trying to update it
                if (m_RobotApplication.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name) == -1)
                {
                    Engine.Base.Compute.RecordWarning($"FramingElementDesignProperties '{framEleDesProps.Name}' does not exist in Robot and cannot be updated. Use Create instead.");
                    continue;
                }

                IRobotLabel memberType = m_RobotApplication.Project.Structure.Labels.Get(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name);
                
                if (memberType == null)
                {
                    Engine.Base.Compute.RecordWarning($"Failed to retrieve FramingElementDesignProperties '{framEleDesProps.Name}' from Robot.");
                    continue;
                }

                // Use the ToRobot method with the configured design code
                Convert.ToRobot(memberType, framEleDesProps, RobotConfig.DatabaseSettings.SteelDesignCode);
                
                m_RobotApplication.Project.Structure.Labels.Store(memberType);
            }            
            return true;
        }

        /***************************************************/
    }
}






