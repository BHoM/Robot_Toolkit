using System.Collections.Generic;
using System.Linq;
using System;
using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using RobotOM;

using BH.Engine.Robot;
using BHEG = BH.Engine.Geometry;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Index Adapter Interface                   ****/
        /***************************************************/
        
        /***************************************************/

        private bool CreateCollection(IEnumerable<Constraint6DOF> constraints)
        {
            List<Constraint6DOF> constList = constraints.ToList();
            IRobotLabel lable = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_SUPPORT, "");
            IRobotNodeSupportData suppData = lable.Data;

            for (int i = 0; i < constList.Count; i++)
            {
                BH.Engine.Robot.Convert.RobotConstraint(suppData, constList[i]);
                m_RobotApplication.Project.Structure.Labels.StoreWithName(lable, constList[i].Name);
                //if (m_SupportTaggs.ContainsKey(constList[i].Name))
                //    m_SupportTaggs[constList[i].Name] = constList[i].TaggedName();
                //else
                //    m_SupportTaggs.Add(constList[i].Name, constList[i].TaggedName());
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
                m_RobotApplication.Project.Structure.Labels.StoreWithName(rigidLink, lConst.Name);

                rLinkData.UX = lConst.XtoX;
                rLinkData.UY = lConst.YtoY;
                rLinkData.UZ = lConst.ZtoZ;
                rLinkData.RX = lConst.XXtoXX;
                rLinkData.RY = lConst.YYtoYY;
                rLinkData.RZ = lConst.ZZtoZZ;
            }
            return true;
        }

        /***************************************************/


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

