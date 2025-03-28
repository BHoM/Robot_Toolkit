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
using BH.oM.Structure.Constraints;
using BH.Engine.Structure;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

         private bool CreateCollection(IEnumerable<BarRelease> releases)
        {
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel robotLabel = robotLabelServer.Create(IRobotLabelType.I_LT_BAR_RELEASE, "");
            IRobotBarReleaseData robotLabelData = robotLabel.Data;

            foreach (BarRelease release in releases)
            {
                //Check release as well as start and end are not null
                if (CheckNotNull(release, oM.Base.Debugging.EventType.Warning)
                    && CheckNotNull(release.StartRelease, oM.Base.Debugging.EventType.Warning, typeof(BarRelease))
                    && CheckNotNull(release.EndRelease, oM.Base.Debugging.EventType.Warning, typeof(BarRelease)))
                {
                    Convert.ToRobot(robotLabelData.StartNode, release.StartRelease);
                    Convert.ToRobot(robotLabelData.EndNode, release.EndRelease);
                    robotLabelServer.StoreWithName(robotLabel, release.DescriptionOrName());
                    SetAdapterId(release, release.DescriptionOrName());
                }
            }
            return true;
        }

        /***************************************************/

    }

}







