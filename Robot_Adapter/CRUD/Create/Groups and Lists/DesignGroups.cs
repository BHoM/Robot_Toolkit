/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<BH.oM.Adapters.Robot.DesignGroup> bhomdesignGroups)
        {
            foreach (DesignGroup bhomdesignGroup in bhomdesignGroups)
            {
                if (!CheckNotNull(bhomdesignGroup, oM.Reflection.Debugging.EventType.Warning))
                    continue;

                RDimServer RDServer = m_RobotApplication.Kernel.GetExtension("RDimServer");
                RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
                RDimStream RDStream = RDServer.Connection.GetStream();
                RDimGroups RDGroups = RDServer.GroupsService;
                RDimGrpProfs RDGroupProfs = RDServer.Connection.GetGrpProfs();               
                RDimGroup designGroup = RDGroups.New(0, bhomdesignGroup.Number);

                if(!string.IsNullOrWhiteSpace(bhomdesignGroup.Name))
                    designGroup.Name = bhomdesignGroup.Name;

                if(!string.IsNullOrWhiteSpace(bhomdesignGroup.MaterialName))
                    designGroup.Material = bhomdesignGroup.MaterialName;

                RDStream.Clear();
                RDStream.WriteText(Convert.FromSelectionList(bhomdesignGroup.MemberIds));
                designGroup.SetMembList(RDStream);
                designGroup.SetProfs(RDGroupProfs);
                RDGroups.Save(designGroup);
            }
            return true;
        }
                
        /***************************************************/
    }

}




