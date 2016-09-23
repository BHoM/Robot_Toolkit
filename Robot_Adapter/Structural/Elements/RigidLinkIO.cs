using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Structural.Elements;
using Robot_Adapter.Base;
using BHoM.Structural.Interface;

namespace Robot_Adapter.Structural.Elements
{
    public class RigidLinkIO
    {
        /// <summary>
        /// Set rigid links by master and slave nodes
        /// </summary>
        /// <param name="masterNodeNumber"></param>
        /// <param name="slaveNodeNumbers"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool SetRigidLinks(RobotApplication robot, List<RigidLink> links, out List<string> ids)
        {          
            IRobotLabel rigidLink = null;
            ids = new List<string>();
            List<string> nodeIds = null;
            foreach (RigidLink link in links)
            {
                List<Node> nodes = new List<Node>();
                nodes.Add(link.MasterNode);
                nodes.AddRange(link.SlaveNodes);
                NodeIO.CreateNodes(robot, nodes, out nodeIds);

                if (robot.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_NODE_RIGID_LINK, Utils.GetName(link)) != 0)
                {
                    robot.Project.Structure.Labels.Get(IRobotLabelType.I_LT_NODE_RIGID_LINK, Utils.GetName(link));
                }
                else
                {
                    robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_NODE_RIGID_LINK, Utils.GetName(link));
                }
                RobotSelection slaveNodeSel = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);

                slaveNodeSel.FromText(Utils.GetSelectionString<Node>(link.SlaveNodes));
                robot.Project.Structure.Nodes.RigidLinks.Set(int.Parse(link.MasterNode[Utils.NUM_KEY].ToString()), Utils.GetSelectionString<Node>(link.SlaveNodes), Utils.GetName(link));

                IRobotNodeRigidLinkData rlinkData = rigidLink.Data;
                robot.Project.Structure.Labels.Store(rigidLink);
                
                ids.Add(Utils.GetName(link));
                rlinkData.RX = true; rlinkData.RY = true; rlinkData.RZ = true; rlinkData.UX = true; rlinkData.UY = true; rlinkData.UZ = true;                
            }
            return true;          
        }

        public static List<string> GetRigidLinks(RobotApplication robot, out List<RigidLink> linksOut, ObjectSelection selectionType, List<string> linkIds = null)
        {
            IRobotNodeRigidLinkServer server = robot.Project.Structure.Nodes.RigidLinks;
            linksOut = new List<RigidLink>();
            List<string> ids = new List<string>();
            for (int i = 1; i < server.Count; i++)
            {
                RobotNodeRigidLinkDef def = server.Get(i);
                if (selectionType == ObjectSelection.FromInput && linkIds != null)
                {
                    if (!linkIds.Contains(def.LabelName)) continue;
                }
                
                List<Node> nodes = new List<Node>();
                List<string> nodeIds = Utils.GetIdsAsTextFromText(def.Slaves);
                nodeIds.Add(def.Master.ToString());
                NodeIO.GetNodes(robot, out nodes, ObjectSelection.Selected, nodeIds);

                Node master = nodes.First(n => n[Utils.NUM_KEY].ToString() == def.Master.ToString());
                nodes.Remove(master);
                RigidLink l = new RigidLink(master, nodes);
                linksOut.Add(l);
                ids.Add(def.LabelName);
            }
            return ids;
        }
    }
}
