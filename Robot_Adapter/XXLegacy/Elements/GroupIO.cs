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

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BH.oM.Base;
//using RobotOM;
//using BH.oM.Structure.Elements;
//using BH.oM.Structure.Loads;
//namespace Robot_Adapter.Structural.Elements
//{
//    public class GroupIO
//    {
//        public static IRobotObjectType GetObjectType(Type type)
//        {
//            if (type == typeof(Bar))
//            {
//                return IRobotObjectType.I_OT_BAR;
//            }
//            else if (type == typeof(Node))
//            {
//                return IRobotObjectType.I_OT_NODE;
//            }
//            else if (type == typeof(Panel))
//            {
//                return IRobotObjectType.I_OT_PANEL;
//            }
//            else if (typeof(ICase).IsAssignableFrom(type))
//            {
//                return IRobotObjectType.I_OT_CASE;
//            }
//            else
//            {
//                return IRobotObjectType.I_OT_OBJECT;
//            }
//        }

//        public static bool CreateGroups(RobotApplication robot, List<IGroup> groups, out List<string> ids)
//        {
//            ids = new List<string>();
//            RobotGroupServer server = robot.Project.Structure.Groups;
//            foreach (IGroup group in groups)
//            {
//                ids.Add(group.Name);
//                server.Create(GetObjectType(group.ObjectType), group.Name, Robot_Adapter.Base.Utils.GetSelectionString<BHoMObject>(group.Objects));
//            }
//            return true;
//        }

//        public static List<string> GetGroups(RobotApplication robot, out List<IGroup> groups)
//        {
//            List<string> ids = new List<string>();
//            RobotGroupServer server = robot.Project.Structure.Groups;
//            ObjectManager<string, Node> nodes = new ObjectManager<string, Node>(Robot_Adapter.Base.Utils.NUM_KEY, FilterOption.UserData);
//            ObjectManager<string, Bar> bars = new ObjectManager<string, Bar>(Robot_Adapter.Base.Utils.NUM_KEY, FilterOption.UserData);
//            ObjectManager<string, Panel> panels = new ObjectManager<string, Panel>(Robot_Adapter.Base.Utils.NUM_KEY, FilterOption.UserData);
//            groups = new List<IGroup>();
//            for (int i = (int)IRobotObjectType.I_OT_NODE; i <= (int)IRobotObjectType.I_OT_PANEL; i++)
//            {
//                for (int j = 1; j <= server.GetCount((IRobotObjectType)i); j++)
//                {
//                    RobotGroup robotGroup = server.Get((IRobotObjectType)i, j);
                    
//                    string list = robotGroup.SelList;
//                    List<string> groupObjectIds = Robot_Adapter.Base.Utils.GetIdsAsTextFromText(list);

//                    switch ((IRobotObjectType)i)
//                    {
//                        case IRobotObjectType.I_OT_NODE:
//                            groups.Add(new Group<Node>(robotGroup.Name, nodes.GetRange(groupObjectIds)));
//                            ids.Add(robotGroup.Name);
//                            break;
//                        case IRobotObjectType.I_OT_BAR:
//                            groups.Add(new Group<Bar>(robotGroup.Name, bars.GetRange(groupObjectIds)));
//                            ids.Add(robotGroup.Name);
//                            break;
//                        case IRobotObjectType.I_OT_PANEL:
//                            groups.Add(new Group<Panel>(robotGroup.Name, panels.GetRange(groupObjectIds)));
//                            ids.Add(robotGroup.Name);
//                            break;
//                    }
//                }
//            }

//            return ids;
//        }

//        public static IRobotCollection GetBarCollectionFromGroup(RobotApplication robot, string groupName)
//        {
//            RobotGroupServer server = robot.Project.Structure.Groups;

//            for (int j = 1; j <= server.GetCount(IRobotObjectType.I_OT_BAR); j++)
//            {
//                RobotGroup robotGroup = server.Get(IRobotObjectType.I_OT_BAR, j);

//                if (robotGroup.Name == groupName)
//                {
//                    string list = robotGroup.SelList;
//                    RobotSelection barSelection = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
//                    barSelection.FromText(list);

//                    return robot.Project.Structure.Bars.GetMany(barSelection) as IRobotCollection;
//                }
//            }

//            return null;
//        }



//        public static List<int> GetNumbersFromText(string selection)
//        {
//            List<int> output = new List<int>();
//            string[] numbers = selection.Split(' ');
//            for (int i = 0; i < numbers.Length; i++)
//            {
//                if (numbers[i].Length > 0)
//                {
//                    if (numbers[i].Contains("to"))
//                    {
//                        int increment = 1;
//                        if (numbers[i].Contains("By"))
//                        {
//                            string[] inc = numbers[i].Replace("By", ",").Split(',');
//                            numbers[i] = numbers[i].Replace("By" + inc[1], "");

//                            increment = int.Parse(inc[1]);
//                        }

//                        string[] range = numbers[i].Replace("to", ",").Split(',');
//                        int startNum = int.Parse(range[0]);
//                        int endNum = int.Parse(range[1]);

//                        for (int j = startNum; j <= endNum; j += increment)
//                        {
//                            output.Add(j);
//                        }
//                    }
//                    else
//                    {
//                        output.Add(int.Parse(numbers[i]));
//                    }
//                }
//            }
//            return output;
//        }
//    }
//}






