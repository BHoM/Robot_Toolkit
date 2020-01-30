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
using RobotOM;
using BH.oM.Structure.Constraints;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        private List<LinkConstraint> ReadLinkConstraint(List<string> ids = null)
        {
            IRobotCollection linkConstrCollection = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_NODE_RIGID_LINK);
            List<LinkConstraint> bhomLinkConstraints = new List<LinkConstraint>();

            for (int i = 1; i <= linkConstrCollection.Count; i++)
            {
                IRobotLabel rLinkConstraintLabel = linkConstrCollection.Get(i);
                IRobotNodeRigidLinkData rLinkConstraintData = rLinkConstraintLabel.Data as IRobotNodeRigidLinkData;

                LinkConstraint bhomLinkConstraint = new LinkConstraint
                {
                    Name = rLinkConstraintLabel.Name,
                    XtoX = rLinkConstraintData.UX,
                    YtoY = rLinkConstraintData.UY,
                    ZtoZ = rLinkConstraintData.UZ,
                    XXtoXX = rLinkConstraintData.RX,
                    YYtoYY = rLinkConstraintData.RY,
                    ZZtoZZ = rLinkConstraintData.RZ
                };

                bhomLinkConstraints.Add(bhomLinkConstraint);
            }
            return bhomLinkConstraints;

        }
    }
}

