using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.oM.Structure.Results;
using RobotOM;
using BH.oM.Common;
using System;
using System.Collections;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {        
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/
        
        private List<NodeReaction> ReadNodeReactions(IList ids = null, IList cases = null, int divisions = 5)
        {
            List<NodeReaction> nodeReactions = new List<NodeReaction>();

            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            List<int> results = new List<int>();
            results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_FX);
            results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_FY);
            results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_FZ);
            results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_MX);
            results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_MY);
            results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_MZ);

            queryParams.ResultIds.SetSize(results.Count);

            for (int i = 0; i < results.Count; i++)
            {
                int id = (int)results[i];
                queryParams.ResultIds.Set(i + 1, id);
            }

            RobotSelection nodeSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            RobotSelection caseSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);

            if (ids == null || ids.Count == 0)
                nodeSelection.FromText("all");
            else
                nodeSelection.FromText(BH.Engine.Robot.Convert.GeterateIdString(CheckAndGetIds(ids)));

            if (cases == null || cases.Count == 0)
                caseSelection.FromText("all");
            else
                caseSelection.FromText(BH.Engine.Robot.Convert.GeterateIdString(GetCases(cases)));

            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.Selection.Set(IRobotObjectType.I_OT_NODE, nodeSelection);
            RobotResultRowSet rowSet = new RobotResultRowSet();

            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
            bool anythingCalculated = false;

            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = m_RobotApplication.Kernel.Structure.Results.Query(queryParams, rowSet);
                bool isOk = anythingCalculated = rowSet.MoveFirst();
                while (isOk)
                {
                    RobotResultRow row = rowSet.CurrentRow;
                    int idCase = (int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
                    int idNode = (int)row.GetParam(IRobotResultParamType.I_RPT_NODE);

                    double fx = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_FX);
                    double fy = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_FY);
                    double fz = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_FZ);
                    double mx = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_MX);
                    double my = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_MY);
                    double mz = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_MZ);

                    NodeReaction nodeReaction = new NodeReaction
                    {
                        Case = idCase.ToString(),
                        ObjectId = idNode.ToString(),
                        FX = fx,
                        FY = fy,
                        FZ = fz,
                        MX = mx,
                        MY = my,
                        MZ = mz
                    };

                    nodeReactions.Add(nodeReaction);
                    isOk = rowSet.MoveNext();
                }
            }
            return nodeReactions;
        }

        /***************************************************/     

    }
}
