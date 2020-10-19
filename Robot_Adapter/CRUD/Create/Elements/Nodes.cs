/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Elements;
using BH.Engine.Structure;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            if (nodes != null)
            {
                List<Node> nodeList = nodes.ToList();
                int nodeNum = 0;
                IRobotStructureCache rcache = m_RobotApplication.Project.Structure.CreateCache();
                IRobotSelection nodeSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);

                foreach (Node node in nodes)
                {
                    if (!CheckInputObject(node))
                        continue;

                    nodeNum = System.Convert.ToInt32(node.CustomData[AdapterIdName]);


                    oM.Geometry.Point position = Engine.Structure.Query.Position(node);
                    rcache.AddNode(nodeNum, position.X, position.Y, position.Z);
                }
                m_RobotApplication.Project.Structure.ApplyCache(rcache as RobotStructureCache);
                IRobotNodeServer robotNodeCol = m_RobotApplication.Project.Structure.Nodes;

                Dictionary<int, HashSet<string>> nodeTags = GetTypeTags(typeof(Node));

                foreach (Node node in nodes)
                {
                    nodeNum = System.Convert.ToInt32(node.CustomData[AdapterIdName]);
                    nodeTags[nodeNum] = node.Tags;

                    if (node.Support != null)
                        robotNodeCol.Get(nodeNum).SetLabel(IRobotLabelType.I_LT_SUPPORT, node.Support.DescriptionOrName());

                }

                m_tags[typeof(Node)] = nodeTags;
            }
            return true;
        }

        /***************************************************/

    }

}


