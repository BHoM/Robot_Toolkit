using System.Collections.Generic;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.oM.Structural.Properties;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Node> ReadNodes(List<string> ids = null)
        {
            IRobotCollection robotNodes = m_RobotApplication.Project.Structure.Nodes.GetAll();
            List<Node> bhomNodes = new List<Node>();
            List<Constraint6DOF> constraints = ReadConstraints6DOF();
            Dictionary<int, HashSet<string>> nodeTags = GetTypeTags(typeof(Node));
            HashSet<string> tags = new HashSet<string>();
            if (ids == null)
            {
                for (int i = 1; i <= robotNodes.Count; i++)
                {
                    RobotNode robotNode = robotNodes.Get(i);
                    Node bhomNode = BH.Engine.Robot.Convert.ToBHoMObject(robotNode);
                    bhomNode.CustomData[AdapterId] = robotNode.Number;
                    if (nodeTags != null && !nodeTags.TryGetValue(robotNode.Number, out tags))
                        bhomNode.Tags = tags;

                    bhomNodes.Add(bhomNode);
                }
            }
            else if (ids != null && ids.Count > 0)
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    RobotNode robotNode = m_RobotApplication.Project.Structure.Nodes.Get(System.Convert.ToInt32(ids[i])) as RobotNode;
                    Node bhomNode = BH.Engine.Robot.Convert.ToBHoMObject(robotNode);
                    bhomNode.CustomData[AdapterId] = robotNode.Number;
                    if (nodeTags != null && !nodeTags.TryGetValue(robotNode.Number, out tags))
                        bhomNode.Tags = tags;

                    bhomNodes.Add(bhomNode);
                }
            }
            return bhomNodes;
        }

        /***************************************************/              

        //Fast query method - only returns basic node information, not full node objects
        private List<Node> ReadNodesQuery(List<string> ids = null)
        {
            List<Node> bhomNodes = new List<Node>();

            RobotResultQueryParams result_params = m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            RobotSelection nod_sel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            IRobotResultQueryReturnType query_return = default(IRobotResultQueryReturnType);

            nod_sel.FromText("all");
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
                query_return = m_RobotApplication.Project.Structure.Results.Query(result_params, row_set);
                ok = row_set.MoveFirst();
                while (ok)
                {
                    result_row = row_set.CurrentRow;
                    nod_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_NODE);
                    BH.oM.Geometry.Point point = new BH.oM.Geometry.Point
                    {
                        X = (double)row_set.CurrentRow.GetValue(0),
                        Y = (double)row_set.CurrentRow.GetValue(1),
                        Z = (double)row_set.CurrentRow.GetValue(2)
                    };
                    Node bhomNode = new Node { Position = point, Name = nod_num.ToString() };
                    bhomNode.CustomData[AdapterId] = nod_num.ToString();
                    bhomNodes.Add(bhomNode);
                    point = null;
                    kounta++;
                    ok = row_set.MoveNext();
                }
                row_set.Clear();
            }
            result_params.Reset();
            return bhomNodes;
        }
        
        /***************************************************/

    }

}

