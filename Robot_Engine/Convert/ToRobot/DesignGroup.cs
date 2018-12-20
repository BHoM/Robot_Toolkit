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

using BH.oM.Structure.Design;
using RobotOM;
using System.Collections.Generic;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static IList<RDimGroup> FromBHoMObjects(RobotApplication robot, List<DesignGroup> bhomdesignGroups)
        {
            List<RDimGroup> robotSteelDesignGroups = new List<RDimGroup>();
            foreach (DesignGroup bhomdesignGroup in bhomdesignGroups)
            {
                RDimServer RDServer = robot.Kernel.GetExtension("RDimServer");
                RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
                RDimStream RDStream = RDServer.Connection.GetStream();
                RDimGroups RDGroups = RDServer.GroupsService;
                RDimGrpProfs RDGroupProfs = RDServer.Connection.GetGrpProfs();
                RDimGroup designGroup = RDGroups.New(0, bhomdesignGroup.Number);
                designGroup.Name = bhomdesignGroup.Name;
                designGroup.Material = bhomdesignGroup.MaterialName;
                RDStream.Clear();
                RDStream.WriteText(Convert.FromSelectionList(bhomdesignGroup.MemberIds));
                designGroup.SetMembList(RDStream);
                designGroup.SetProfs(RDGroupProfs);
                RDGroups.Save(designGroup);
                robotSteelDesignGroups.Add(designGroup);
            }
            return robotSteelDesignGroups;
        }

        /***************************************************/

    }

}
