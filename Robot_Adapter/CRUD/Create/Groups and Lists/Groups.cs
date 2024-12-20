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

                foreach (var group in GroupsFromTags(type))
                {
                    string name = group.Key;
                    List<int> indecies = group.Value;

                    if (indecies.Count == 0)
                    {
                        //If the group no longer has any elements, delete it
                        int index = groupServ.Find(robotType, name);
                        groupServ.Delete(robotType, index);
                    }
                    else
                    {
                        //Otherwise, create it. Creatign a new will overwrite any pre-existing group
                        string selection = indecies.ToRobotSelectionString();
                        groupServ.Create(robotType, name, selection);
                    }
                }
            }

            return true;
        }

        /***************************************************/

        private Dictionary<string, List<int>> GroupsFromTags(Type type)
        {
            Dictionary<int, HashSet<string>> elementTags;
            if (!m_tags.TryGetValue(type, out elementTags))
                elementTags = new Dictionary<int, HashSet<string>>();

            HashSet<string> existingGroups;
            if (m_exisitingGroups.TryGetValue(type, out existingGroups))
                existingGroups = new HashSet<string>();

            Dictionary<string, List<int>> groups = new Dictionary<string, List<int>>();

            foreach (string name in existingGroups)
            {
                groups[name] = new List<int>(); //Initialise all pre-existing groups (groups in the model when the action started)
            }

            foreach (var elemTag in elementTags)    //Populate groups with all elements with the tag corresponding to the group name
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







