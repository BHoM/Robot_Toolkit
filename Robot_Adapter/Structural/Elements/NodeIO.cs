using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoMB = BHoM.Base;
using BHoME = BHoM.Structural.Elements;
using BHoMP = BHoM.Structural.Properties;
using Robot_Adapter.Base;
using BHoM.Structural.Interface;

namespace Robot_Adapter.Structural.Elements
{
    /// <summary>
    /// Node objects
    /// </summary>
    public class NodeIO
    {
        public static BHoMP.NodeConstraint GetConstraint(IRobotNode node)
        {
            if (node.HasLabel(IRobotLabelType.I_LT_SUPPORT) != 0)
            {
                IRobotLabel nodeSupport = node.GetLabel(IRobotLabelType.I_LT_SUPPORT);
                RobotNodeSupportData nodeData = nodeSupport.Data;

                //if (!string.IsNullOrEmpty(nodeSupport.Name))
                if (!nodeSupport.Name.Equals(null))
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

                    return new BHoMP.NodeConstraint(nodeSupport.Name, data, spring);
                }
            }
            return null;
        }

        public static void CreateConstraint(RobotApplication robot, BHoMP.NodeConstraint constraint)
        {
            IRobotLabel constraintLabel = null;
            if (robot.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_SUPPORT, constraint.Name) == 0)
            {
                constraintLabel = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_SUPPORT, constraint.Name);
            }
            else
            {
                constraintLabel = robot.Project.Structure.Labels.Get(IRobotLabelType.I_LT_SUPPORT, constraint.Name);
            }
            

            if (constraintLabel != null)
            {
                RobotNodeSupportData nodeData = constraintLabel.Data;

                if (constraint != null)
                {
                    nodeData.UX = constraint.UX == BHoMP.DOFType.Fixed ? -1 : 0;
                    nodeData.UY = constraint.UY == BHoMP.DOFType.Fixed ? -1 : 0;
                    nodeData.UZ = constraint.UZ == BHoMP.DOFType.Fixed ? -1 : 0;
                    nodeData.RX = constraint.RX == BHoMP.DOFType.Fixed ? -1 : 0;
                    nodeData.RY = constraint.RY == BHoMP.DOFType.Fixed ? -1 : 0;
                    nodeData.RZ = constraint.RZ == BHoMP.DOFType.Fixed ? -1 : 0;

                    nodeData.KX = constraint.KX;
                    nodeData.KY = constraint.KY;
                    nodeData.KZ = constraint.KZ;
                    nodeData.HX = constraint.HX;
                    nodeData.HY = constraint.HY;
                    nodeData.HZ = constraint.HZ;
                }
                robot.Project.Structure.Labels.Store(constraintLabel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="selection"></param>
        /// <param name="filePath"></param>
        public static void GetNodesQuery(RobotApplication robot, out BHoMB.ObjectManager<int, BHoME.Node> nodes, string selection = "all")
        {
            
            nodes = new BHoMB.ObjectManager<int, BHoME.Node>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);

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
            
            while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
            {
                query_return = robot.Project.Structure.Results.Query(result_params, row_set);
                ok = row_set.MoveFirst();
                while (ok)
                {
                    result_row = row_set.CurrentRow;
                    nod_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_NODE);
                    BHoME.Node n = nodes.Add(nod_num, new BHoME.Node());
                    n.Point = new BHoM.Geometry.Point((double)row_set.CurrentRow.GetValue(0), (double)row_set.CurrentRow.GetValue(1), (double)row_set.CurrentRow.GetValue(2));
                    n.CustomData.Add(Utils.NUM_KEY, nod_num);
                    
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
        public static List<string> GetNodes(RobotApplication robot, out List<BHoME.Node> nodeOut, ObjectSelection selectionType, List<string> nodeNumbers = null)
        {
            BHoMB.ObjectManager<string, BHoME.Node>  nodes = new BHoMB.ObjectManager<string, BHoME.Node>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);

            BHoMB.ObjectManager<BHoMP.NodeConstraint> constraints = new BHoMB.ObjectManager<BHoMP.NodeConstraint>();

            RobotSelection selection = selectionType == ObjectSelection.Selected ? 
                robot.Project.Structure.Selections.Get(IRobotObjectType.I_OT_NODE) :
                robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);

            if (selectionType == ObjectSelection.FromInput)
            {
                selection.FromText(Utils.GetSelectionString(nodeNumbers));
            }
            else if (selectionType == ObjectSelection.All)
            {
                selection.FromText("all");
            }
            
            RobotNodeCollection collection = (RobotNodeCollection)robot.Project.Structure.Nodes.GetMany(selection);

            List<string> outIds = new List<string>();
            for (int i = 0; i < collection.Count; i++)
            {
                RobotNode rnode = (RobotNode)collection.Get(i + 1);
                outIds.Add(rnode.Number.ToString());
                BHoME.Node node = new BHoME.Node(rnode.X, rnode.Y, rnode.Z);
                BHoME.Node existingNode = nodes.TryLookup(rnode.Number.ToString());

                if (existingNode != null)
                {
                    existingNode.Point = node.Point;
                }
                else
                {
                    nodes.Add(rnode.Number.ToString(), node);
                }
            
                if (rnode.HasLabel(IRobotLabelType.I_LT_SUPPORT) != 0)
                {
                    IRobotLabel supportLabel = rnode.GetLabel(IRobotLabelType.I_LT_SUPPORT);
                    BHoMP.NodeConstraint c = constraints.TryLookup(supportLabel.Name);
                    if (c == null)
                    {
                        c = GetConstraint(rnode);
                        constraints.Add(supportLabel.Name, c);
                    }
                    node.Constraint = c;
                }
                //else
                //{
                //    IRobotLabel suppLabel = rnode.GetLabel(IRobotLabelType.I_LT_SUPPORT);
                //    e
                //}
            }
            nodeOut = nodes.GetRange(outIds);

            return outIds;
        }

        /// <summary>
        /// Create nodes using the fast cache method
        /// </summary>
        /// <param name="str_nodes"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CreateNodesByCache(BHoME.Node[] str_nodes, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotStructureCache structureCache = robot.Project.Structure.CreateCache();

            foreach (BHoME.Node node in str_nodes)
            {
                object number = 0;
                if (node.CustomData.TryGetValue(Utils.NUM_KEY, out number))
                {
                    structureCache.AddNode((int)number, node.X, node.Y, node.Z);
                }
                else
                {
                    structureCache.AddNode(robot.Project.Structure.Nodes.FreeNumber, node.X, node.Y, node.Z);
                }
            }

            RobotStructureApplyInfo applyInfo = robot.Project.Structure.ApplyCache(structureCache);

            return true;
        }

        public static bool CreateNodes(RobotApplication robot, List<BHoME.Node> nodes, out List<string> ids)
        {
            RobotNodeServer nodeServer = robot.Project.Structure.Nodes;
            RobotNode node = null;
            Dictionary<string, string> addedConstraints = new Dictionary<string, string>();
            ids = new List<string>();
            int num = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                object number = nodes[i][Utils.NUM_KEY];

                if (number != null)
                {
                    int.TryParse(number.ToString(), out num);
                }
                else
                {
                    num = nodeServer.FreeNumber;
                    nodes[i].CustomData.Add(Utils.NUM_KEY, num);
                }

                if (nodeServer.Exist(num) == 0)
                { 
                    nodeServer.Create(num, nodes[i].X, nodes[i].Y, nodes[i].Z);
                    node = nodeServer.Get(num) as RobotNode;
                }
                else
                {
                    node = nodeServer.Get(num) as RobotNode;
                    node.X = nodes[i].X;
                    node.Y = nodes[i].Y;
                    node.Z = nodes[i].Z;
                }

                ids.Add(num.ToString());
                string nodeSupport = "";
                if (nodes[i].Constraint != null)
                {
                    if (!addedConstraints.TryGetValue(nodes[i].Constraint.Name, out nodeSupport))
                    {
                        CreateConstraint(robot, nodes[i].Constraint);
                        nodeSupport = nodes[i].Constraint.Name;
                        addedConstraints.Add(nodeSupport, nodeSupport);
                    }
                    node.SetLabel(IRobotLabelType.I_LT_SUPPORT, nodeSupport);              
                }
            }
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
        public static bool SetRestraints(RobotApplication robot, List<BHoME.Node> str_nodes)
        {            
            RobotSelection nodeSelection = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            foreach (BHoME.Node node in str_nodes)
            {
                object nodeNum = node[Utils.NUM_KEY];
                if (nodeNum != null)
                {
                    nodeSelection.FromText(nodeNum.ToString());
                    robot.Project.Structure.Nodes.SetLabel(nodeSelection, IRobotLabelType.I_LT_SUPPORT, node.ConstraintName);
                }
            }

                    
            return true;
        }
   
    
    }
}
