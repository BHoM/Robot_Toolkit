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

using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using RobotOM;
using BH.Engine.Adapter;


namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/
             
        private bool CreateCollection<T>(IEnumerable<BH.oM.Base.BHoMGroup<T>> groups) where T : BH.oM.Base.IBHoMObject
        {
            RobotGroupServer rGroupServer = m_RobotApplication.Project.Structure.Groups;
            foreach (BHoMGroup<T> group in groups)
            {
                //Check group is not null
                if (!CheckNotNull(group))
                    continue;

                Type groupType = group.GetType();


                if (!CheckNotNull(group.Elements, oM.Base.Debugging.EventType.Error, groupType) ||
                   group.Elements.Any(x => !CheckNotNull(x, oM.Base.Debugging.EventType.Error, groupType)))
                    continue;

                if (group.Elements.Any(x => !x.HasAdapterIdFragment(AdapterIdFragmentType)))
                {
                    Engine.Base.Compute.RecordError("The Elements of the Group needs to be pre pushed/pulled to assign their Adapter Ids. The Group containing the element(s) with missing Ids have not been created.");
                    continue;
                }

                IRobotObjectType rType = Convert.RobotObjectType(typeof(T));
                string members = group.Elements.Select(x => GetAdapterId<int>(x)).ToRobotSelectionString();

                if (string.IsNullOrWhiteSpace(group.Name))
                {
                    Engine.Base.Compute.RecordError("BHoMGroup must have a name to be pushed to Robot");
                    continue;
                }

                rGroupServer.Create(rType, group.Name, members);
            }
            return true;
        }

        /***************************************************/

        private bool ApplyTagsAsGroups()
        {

            RobotGroupServer groupServ = m_RobotApplication.Project.Structure.Groups;

            foreach (var typeTags in m_tags)
            {
                Type type = typeTags.Key;
                IRobotObjectType robotType = Convert.RobotObjectType(type);

                if (robotType == IRobotObjectType.I_OT_UNDEFINED)
                    continue;

                foreach (var group in GroupsFromTags(typeTags.Value))
                {
                    string name = group.Key;
                    string selection = group.Value.ToRobotSelectionString();

                    groupServ.Create(robotType, name, selection);
                }
            }

            return true;
        }

        /***************************************************/

        private Dictionary<string, List<int>> GroupsFromTags(Dictionary<int, HashSet<string>> elementTags)
        {
            Dictionary<string, List<int>> groups = new Dictionary<string, List<int>>();

            foreach (var elemTag in elementTags)
            {
                int id = elemTag.Key;

                foreach (string tag in elemTag.Value)
                {
                    if (groups.ContainsKey(tag))
                        groups[tag].Add(id);
                    else
                        groups[tag] = new List<int> { id };
                }
            }
            return groups;
        }

        /***************************************************/
    }

}





