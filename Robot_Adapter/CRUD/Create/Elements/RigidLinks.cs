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
using System.Linq;
using BH.oM.Structure.Elements;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<RigidLink> rigidLinks)
        {
            foreach (RigidLink rLink in rigidLinks)
            {
                //Check link is not null
                if (!CheckNotNull(rLink))
                    continue;

                //Check primary node and try to extract Id
                int primaryId;
                if (!CheckInputObjectAndExtractAdapterIdInt(rLink.PrimaryNode, out primaryId, oM.Base.Debugging.EventType.Error, typeof(RigidLink)))
                    continue;

                List<int> secondaryIds = new List<int>();
                bool secondaryNodesChecked = true;

                foreach (Node node in rLink.SecondaryNodes)
                {
                    //Loop through and check that secondary ids can be extracted from rigid links
                    int nodeId;
                    if (CheckInputObjectAndExtractAdapterIdInt(node, out nodeId, oM.Base.Debugging.EventType.Error, typeof(RigidLink)))
                    {
                        secondaryIds.Add(nodeId);
                    }
                    else
                    {
                        secondaryNodesChecked = false;
                        break;
                    }
                }

                if (!secondaryNodesChecked)
                    continue;

                string[] str = secondaryIds.Select(x => x.ToString() + ",").ToArray();
                string secondaryNodes = string.Join("", str).TrimEnd(',');

                string contraintName = "";

                if (CheckNotNull(rLink.Constraint, oM.Base.Debugging.EventType.Warning, typeof(RigidLink)))
                    contraintName = rLink.Constraint.Name;

                m_RobotApplication.Project.Structure.Nodes.RigidLinks.Set(primaryId, secondaryNodes, contraintName);
            }
            return true;
        }

        /***************************************************/

    }

}







