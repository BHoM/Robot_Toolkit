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

using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Constraints;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        /***************************************************/

        private bool CreateCollection(IEnumerable<Constraint6DOF> constraints)
        {
            List<Constraint6DOF> constList = constraints.ToList();
            IRobotLabel label = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_SUPPORT, "");
            IRobotNodeSupportData suppData = label.Data;

            for (int i = 0; i < constList.Count; i++)
            {
                BH.Engine.Robot.Convert.RobotConstraint(suppData, constList[i]);
                m_RobotApplication.Project.Structure.Labels.StoreWithName(label, constList[i].Name);
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<LinkConstraint> linkConstraints)
        {
            IRobotLabel rigidLink = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_NODE_RIGID_LINK, "");
            IRobotNodeRigidLinkData rLinkData = rigidLink.Data;

            foreach (LinkConstraint lConst in linkConstraints)
            {
                rLinkData.UX = lConst.XtoX;
                rLinkData.UY = lConst.YtoY;
                rLinkData.UZ = lConst.ZtoZ;
                rLinkData.RX = lConst.XXtoXX;
                rLinkData.RY = lConst.YYtoYY;
                rLinkData.RZ = lConst.ZZtoZZ;

                m_RobotApplication.Project.Structure.Labels.StoreWithName(rigidLink, lConst.Name);
            }
            return true;
        }

        /***************************************************/

    }

}
