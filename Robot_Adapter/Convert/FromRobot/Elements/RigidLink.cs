/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

using BH.oM.Geometry;
using RobotOM;
using BH.oM.Structure.Elements;
using System.Collections.Generic;
using BH.oM.Structure.Constraints;
using System.Linq;


namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static RigidLink FromRobot(this IRobotNodeRigidLinkDef robotRigidLink, Dictionary<int, Node> bhomNodes, Dictionary<string, LinkConstraint> bhomLinkConstraints)
        {
            if (robotRigidLink == null)
                return null;

            RigidLink bhomRigidLink = new RigidLink();
            Node primaryNode;
            if (bhomNodes.TryGetValue(robotRigidLink.Master, out primaryNode))
                bhomRigidLink.PrimaryNode = primaryNode;
            else
                Engine.Base.Compute.RecordError("Failed to extract the PrimaryNode for at least one RigidLink.");

            List<int> secondaryIds = FromRobotSelectionString(robotRigidLink.Slaves);

            bhomRigidLink.SecondaryNodes = new List<Node>();
            foreach (int secondaryId in secondaryIds)
            {
                Node secondaryNode;
                if (bhomNodes.TryGetValue(secondaryId, out secondaryNode))
                    bhomRigidLink.SecondaryNodes.Add(secondaryNode);
                else
                    Engine.Base.Compute.RecordError("Failed to extract at least one SecondaryNode for at least one RigidLink.");
            }

            LinkConstraint constraint;
            if (!string.IsNullOrEmpty(robotRigidLink.LabelName) && bhomLinkConstraints.TryGetValue(robotRigidLink.LabelName, out constraint))
                bhomRigidLink.Constraint = constraint;

            return bhomRigidLink;
        }

        /***************************************************/

    }
}





