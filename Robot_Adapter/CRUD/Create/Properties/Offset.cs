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

using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Offsets;
using BH.Engine.Structure;
using RobotOM;


namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<Offset> offsets)
        {
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel robotLabel = robotLabelServer.Create(IRobotLabelType.I_LT_BAR_OFFSET, "");

            foreach (Offset offset in offsets)
            {
                //Check release as well as start and end are not null
                if (CheckNotNull(offset, oM.Base.Debugging.EventType.Warning)
                    && CheckNotNull(offset.Start, oM.Base.Debugging.EventType.Warning, typeof(Offset))
                    && CheckNotNull(offset.End, oM.Base.Debugging.EventType.Warning, typeof(Offset)))
                {
                    Convert.ToRobot(robotLabel.Data, offset);
                    robotLabelServer.StoreWithName(robotLabel, offset.DescriptionOrName());
                    SetAdapterId(offset, offset.DescriptionOrName());
                }
            }
            return true;
        }

        /***************************************************/

    }

}





