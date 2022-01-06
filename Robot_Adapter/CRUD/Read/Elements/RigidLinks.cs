/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Base;
using BH.oM.Physical.Materials;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<RigidLink> ReadRigidLinks(IList ids = null)
        {
            List<int> linksIds = CheckAndGetIds<RigidLink>(ids);
            List<RigidLink> bhomRigidLinks = new List<RigidLink>();

            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<int, Node> bhomNodes = bhomNodesList.ToDictionary(x => GetAdapterId<int>(x));
            Dictionary<string, LinkConstraint> bhomLinkConstraints = ReadLinkConstraint().ToDictionary(x => x.Name.ToString());

            if (linksIds == null || linksIds.Count == 0)
            {
                for (int i = 1; i <= m_RobotApplication.Project.Structure.Nodes.RigidLinks.Count; i++)
                {
                    IRobotNodeRigidLinkDef robotRigidLink = m_RobotApplication.Project.Structure.Nodes.RigidLinks.Get(i) as IRobotNodeRigidLinkDef;

                    RigidLink bhomRigidLink = Convert.FromRobot(robotRigidLink, bhomNodes, bhomLinkConstraints);

                    if (bhomRigidLink != null)
                    {
                        SetAdapterId(bhomRigidLink, i);
                        bhomRigidLinks.Add(bhomRigidLink);
                    }
                    else
                        Engine.Reflection.Compute.RecordError("Failed to extract at least one RigidLink from the Robot API.");
                }
            }
            else
            {
                for (int i = 0; i < linksIds.Count; i++)
                {
                    IRobotNodeRigidLinkDef robotRigidLink = m_RobotApplication.Project.Structure.Nodes.RigidLinks.Get(linksIds[i]) as IRobotNodeRigidLinkDef;
                    RigidLink bhomRigidLink = Convert.FromRobot(robotRigidLink, bhomNodes, bhomLinkConstraints);

                    if (bhomRigidLink != null)
                    {
                        SetAdapterId(bhomRigidLink, i);
                        bhomRigidLinks.Add(bhomRigidLink);
                    }
                    else
                        Engine.Reflection.Compute.RecordError("Failed to extract at least one RigidLink from the Robot API.");
                }
            }

            return bhomRigidLinks;
        }

        /***************************************************/

    }
}



