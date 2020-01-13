/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<BH.oM.Adapters.Robot.DesignGroup> ReadDesignGroups()
        {
            RobotApplication robot = m_RobotApplication;
            RDimServer RDServer = m_RobotApplication.Kernel.GetExtension("RDimServer");
            RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
            RDimStream RDStream = RDServer.Connection.GetStream();
            RDimGroups RDGroups = RDServer.GroupsService;
            RDimGrpProfs RDGroupProfs = RDServer.Connection.GetGrpProfs();
            List<BH.oM.Adapters.Robot.DesignGroup> designGroupList = new List<BH.oM.Adapters.Robot.DesignGroup>();

            for (int i = 0; i <= RDGroups.Count - 1; i++)
            {
                int designGroupNumber = RDGroups.GetUserNo(i);
                RDimGroup designGroup = RDGroups.Get(designGroupNumber);
                BH.oM.Adapters.Robot.DesignGroup bhomDesignGroup = new BH.oM.Adapters.Robot.DesignGroup();
                bhomDesignGroup.Name = designGroup.Name;
                bhomDesignGroup.Number = designGroup.UsrNo;
                bhomDesignGroup.CustomData[AdapterIdName] = designGroup.UsrNo;
                bhomDesignGroup.CustomData[Engine.Robot.Convert.AdapterName] = designGroup.Name;
                bhomDesignGroup.MaterialName = designGroup.Material;
                designGroup.GetMembList(RDStream);
                if (RDStream.Size(IRDimStreamType.I_DST_TEXT) > 0)
                    bhomDesignGroup.MemberIds = Engine.Robot.Convert.ToSelectionList(RDStream.ReadText());
                designGroupList.Add(bhomDesignGroup);
            }
            return designGroupList;
        }

        /***************************************************/

    }

}

