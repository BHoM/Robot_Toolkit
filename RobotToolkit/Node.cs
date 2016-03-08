using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM;

namespace RobotToolkit
{
    /// <summary>
    /// Node objects
    /// </summary>
    public class Node
    {
        public static BHoM.Structural.Constraint GetConstraint(BHoM.Structural.ConstraintFactory factory, IRobotNode node)
        {
            if (node.HasLabel(IRobotLabelType.I_LT_SUPPORT) != 0)
            {
                IRobotLabel nodeSupport = node.GetLabel(IRobotLabelType.I_LT_SUPPORT);
                RobotNodeSupportData nodeData = nodeSupport.Data;

                if (!string.IsNullOrEmpty(nodeSupport.Name))
                {
                    bool[] data = new bool[6];
                    data[0] = nodeData.UX == -1;
                    data[1] = nodeData.UY == -1;
                    data[2] = nodeData.UZ == -1;
                    data[3] = nodeData.RX == -1;
                    data[4] = nodeData.RY == -1;
                    data[5] = nodeData.RZ == -1;

                    double[] spring = new double[6];
                    spring[0] = nodeData.KX;
                    spring[1] = nodeData.KY;
                    spring[2] = nodeData.KZ;
                    spring[3] = nodeData.HX;
                    spring[4] = nodeData.HY;
                    spring[5] = nodeData.HZ;

                    return factory.Create(nodeSupport.Name, data, spring);
                }
            }
            return factory.Create("Free", "ffffff");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="selection"></param>
        /// <param name="filePath"></param>
        public static void GetNodesQuery(BHoM.Global.Project project, string selection = "all", string filePath = "")
        {
            RobotApplication robot = new RobotApplication();
            if (filePath != "")
            {
                robot.Project.Open(filePath);
            }
            RobotResultQueryParams result_params = robot.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            RobotStructure rstructure = robot.Project.Structure;
            RobotSelection nod_sel = rstructure.Selections.Create(IRobotObjectType.I_OT_NODE);
            IRobotResultQueryReturnType query_return = default(IRobotResultQueryReturnType);
            
            nod_sel.FromText(selection);
            result_params.ResultIds.SetSize(3);
            result_params.ResultIds.Set(1, 0);
            result_params.ResultIds.Set(2, 1);
            result_params.ResultIds.Set(3, 2);

            result_params.Selection.Set(IRobotObjectType.I_OT_NODE, nod_sel);
            result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            query_return = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
            RobotResultRowSet row_set = new RobotResultRowSet();
            bool ok = false;

            RobotResultRow result_row = default(RobotResultRow);
            int nod_num = 0;
            int kounta = 0;

            BHoM.Structural.NodeFactory factory = project.Structure.Nodes;

            while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
            {
                query_return = robot.Project.Structure.Results.Query(result_params, row_set);
                ok = row_set.MoveFirst();
                while (ok)
                {
                    result_row = row_set.CurrentRow;
                    nod_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_NODE);
                    factory.Create(nod_num, (double)row_set.CurrentRow.GetValue(0), (double)row_set.CurrentRow.GetValue(1), (double)row_set.CurrentRow.GetValue(2));
                    kounta++;
                    ok = row_set.MoveNext();
                }
                row_set.Clear();
            }
            result_params.Reset();
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="nodes"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool GetNodes(BHoM.Global.Project project, string nodes = "all", string filePath = "")
        {
            RobotApplication robot = new RobotApplication();
            if (filePath != "")
            {
                robot.Project.Open(filePath);
            }
            RobotSelection selection = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            selection.FromText(nodes);
            RobotNodeCollection collection = (RobotNodeCollection)robot.Project.Structure.Nodes.GetMany(selection);
            BHoM.Structural.NodeFactory bHomNodes = project.Structure.Nodes;
            BHoM.Structural.ConstraintFactory bHomConstraints = project.Structure.Constraints;
           
            for (int i = 0; i < collection.Count; i++)
            {
                
                RobotNode rnode = (RobotNode)collection.Get(i + 1);

                BHoM.Structural.Node node = bHomNodes.Create(rnode.Number, rnode.X, rnode.Y, rnode.Z);
                node.Constraint = GetConstraint(bHomConstraints, rnode);
            }
            return true;
        }

       /// <summary>
        /// Create nodes using the fast cache method
        /// </summary>
        /// <param name="str_nodes"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CreateNodesByCache(BHoM.Structural.Node[] str_nodes, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotStructureCache structureCache = robot.Project.Structure.CreateCache();

            foreach (BHoM.Structural.Node node in str_nodes)
            {
                structureCache.AddNode(node.Number, node.X, node.Y, node.Z);
            }

            RobotStructureApplyInfo applyInfo = robot.Project.Structure.ApplyCache(structureCache);


            return true;
        }

        /// <summary>
        /// Delete nodes by selection. Selection is in Robot string format. 
        /// </summary>
        /// <param name="selString"></param>
        /// <param name="FilePath"></param>
        public static void DeleteNodes(string selString, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotSelection sel = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            sel.AddText(selString);


            robot.Project.Structure.Nodes.DeleteMany(sel);
        }

        /// <summary>
        /// Delete isolated nodes. 
        /// </summary>
        /// <param name="FilePath"></param>
        public static void DeleteIsolatedNodes(string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotSelection sel = robot.Project.Structure.Selections.CreatePredefined(IRobotPredefinedSelection.I_PS_NODE_USER_NODES);
         
            robot.Project.Structure.Nodes.DeleteMany(sel);
        }

        /// <summary>
        /// Set nodal restraints
        /// </summary>
        /// <param name="str_nodes"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool SetRestraints(List<BHoM.Structural.Node> str_nodes, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            

            RobotSelection nodeSelection = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            foreach (BHoM.Structural.Node node in str_nodes)
            {
                nodeSelection.FromText(node.Number.ToString());
                robot.Project.Structure.Nodes.SetLabel(nodeSelection, IRobotLabelType.I_LT_SUPPORT, node.ConstraintName);
            }

                
            
            return true;
        }
   
        /// <summary>
        /// Set rigid links by master and slave nodes
        /// </summary>
        /// <param name="masterNodeNumber"></param>
        /// <param name="slaveNodeNumbers"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool SetRigidLinks(int masterNodeNumber, List<int> slaveNodeNumbers, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();

            RobotSelection slaveNodeSel = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            foreach (int slaveNodeNumber in slaveNodeNumbers)
            {
                slaveNodeSel.AddOne(slaveNodeNumber);
            }
            
            if (robot.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_NODE_RIGID_LINK, "Rigid") != 1)
            {
                IRobotLabel rigidLink = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_NODE_RIGID_LINK, "Rigid");
                IRobotNodeRigidLinkData rlinkData = rigidLink.Data;
                robot.Project.Structure.Labels.Store(rigidLink);
                rlinkData.RX = true; rlinkData.RY = true; rlinkData.RZ = true; rlinkData.UX = true; rlinkData.UY = true; rlinkData.UZ = true;
             }
            robot.Project.Structure.Nodes.RigidLinks.Set(masterNodeNumber, slaveNodeSel.ToText(), "Rigid");



            return true;
        }

    }
}
