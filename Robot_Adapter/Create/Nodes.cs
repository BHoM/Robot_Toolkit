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
               
        private bool CreateCollection(IEnumerable<BH.oM.Structural.Elements.Node> nodes)
        {
            if (nodes != null)
            {
                m_RobotApplication.Interactive = 0;
                List<Node> nodeList = nodes.ToList();
                int nodeNum = 0;
                IRobotStructureCache rcache = m_RobotApplication.Project.Structure.CreateCache();
                IRobotSelection nodeSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
                int freeNum = m_RobotApplication.Project.Structure.Nodes.FreeNumber;

                foreach (Node node in nodes)
                {
                    nodeNum = System.Convert.ToInt32(node.CustomData[AdapterId]);
                    rcache.AddNode(nodeNum, node.Position.X, node.Position.Y, node.Position.Z);
                }
                m_RobotApplication.Project.Structure.ApplyCache(rcache as RobotStructureCache);
                IRobotNodeServer robotNodeCol = m_RobotApplication.Project.Structure.Nodes;

                Dictionary<int, HashSet<string>> nodeTags = GetTypeTags(typeof(Node));

                foreach (Node node in nodes)
                {
                    nodeNum = System.Convert.ToInt32(node.CustomData[AdapterId]);
                    nodeTags[nodeNum] = node.Tags;

                    if (node.Constraint != null)
                        robotNodeCol.Get(nodeNum).SetLabel(IRobotLabelType.I_LT_SUPPORT, node.Constraint.Name);
                }

                m_tags[typeof(Node)] = nodeTags;
                m_RobotApplication.Interactive = 1;
            }
            return true;
        }

        /***************************************************/

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

