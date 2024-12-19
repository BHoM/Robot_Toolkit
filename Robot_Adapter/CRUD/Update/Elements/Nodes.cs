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
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.Engine.Structure;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected bool Update(IEnumerable<Node> nodes)
        {
            Dictionary<int, HashSet<string>> nodeTags = GetTypeTags(typeof(Node));
            foreach (Node node in nodes)
            {

                int nodeId;
                if (!CheckInputObjectAndExtractAdapterIdInt(node, out nodeId, oM.Base.Debugging.EventType.Warning))
                    continue;

                RobotNode robotNode = m_RobotApplication.Project.Structure.Nodes.Get(nodeId) as RobotNode;
                if (robotNode == null)
                    return false;

                if (node.Support != null && !string.IsNullOrWhiteSpace(node.Support.DescriptionOrName()))
                    robotNode.SetLabel(IRobotLabelType.I_LT_SUPPORT, node.Support.DescriptionOrName());

                oM.Geometry.Point position = node.Position;

                //Check point is not null
                if (!CheckNotNull(position, oM.Base.Debugging.EventType.Error, typeof(Node)))
                    continue;

                robotNode.X = position.X;
                robotNode.Y = position.Y;
                robotNode.Z = position.Z;
                nodeTags[nodeId] = node.Tags;
            }
            m_tags[typeof(Node)] = nodeTags;
            return true;
        }

        /***************************************************/
        
     }
}






