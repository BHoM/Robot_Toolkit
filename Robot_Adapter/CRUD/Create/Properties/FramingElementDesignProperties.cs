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
        /****           Private Methods                 ****/
        /***************************************************/
        
        private bool Create(FramingElementDesignProperties framEleDesProps) //TODO: move the label part to convert such that duplicate code in Update can be removed
        {

            if (!CheckNotNull(framEleDesProps, oM.Base.Debugging.EventType.Warning))
                return false;

            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel label = labelServer.CreateLike(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name, labelServer.GetDefault(IRobotLabelType.I_LT_MEMBER_TYPE));

            // Use the ToRobot method with the configured design code
            Convert.ToRobot(label, framEleDesProps, RobotConfig.DatabaseSettings.SteelDesignCode);

            labelServer.Store(label);
                    

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<FramingElementDesignProperties> framEleDesPropsList)
        {
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;

            foreach (FramingElementDesignProperties framEleDesProps in framEleDesPropsList)
            {

                if (!CheckNotNull(framEleDesProps, oM.Base.Debugging.EventType.Warning))
                    continue;

                IRobotLabel label = labelServer.CreateLike(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name, labelServer.GetDefault(IRobotLabelType.I_LT_MEMBER_TYPE));

                // Use the ToRobot method with the configured design code
                Convert.ToRobot(label, framEleDesProps, RobotConfig.DatabaseSettings.SteelDesignCode);

                labelServer.Store(label);
            }
            return true;
        }

        /***************************************************/

    }

}







