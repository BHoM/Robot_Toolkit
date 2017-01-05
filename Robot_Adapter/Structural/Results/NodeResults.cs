using Robot_Adapter.Base;
using BHoM.Base.Results;
using BHoM.Structural.Results;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot_Adapter.Structural.Results
{
    public static class NodeResults
    {
        public static bool GetNodeReacions(RobotApplication RobotApp, ResultServer<NodeReaction> resultServer, List<string> nodeNumbers, List<string> loadcaseNumber)
        {
            RobotResultQueryParams queryParams = (RobotResultQueryParams)RobotApp.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

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

            RobotSelection nodeSelection = RobotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            RobotSelection caseSelection = RobotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);

            if (nodeNumbers == null || nodeNumbers.Count == 0) nodeSelection.FromText("all"); else nodeSelection.FromText(Utils.GetSelectionString(nodeNumbers));
            if (loadcaseNumber == null || loadcaseNumber.Count == 0) caseSelection.FromText("all"); else caseSelection.FromText(Utils.GetSelectionString(loadcaseNumber));

            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.Selection.Set(IRobotObjectType.I_OT_NODE, nodeSelection);
            RobotResultRowSet rowSet = new RobotResultRowSet();

            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
            bool anythingCalculated = false;

            int counter = 0;
            List<NodeReaction> nodeForces = new List<NodeReaction>();

            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = RobotApp.Kernel.Structure.Results.Query(queryParams, rowSet);
                bool isOk = anythingCalculated = rowSet.MoveFirst();
                while (isOk)
                {
                    RobotResultRow row = rowSet.CurrentRow;
                    int idCase = (int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
                    int idnode = (int)row.GetParam(IRobotResultParamType.I_RPT_NODE);

                    double fx = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_FX);
                    double fy = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_FY);
                    double fz = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_FZ);
                    double mx = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_MX);
                    double my = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_MY);
                    double mz = row.GetValue((int)IRobotExtremeValueType.I_EVT_REACTION_MZ);

                    nodeForces.Add(new NodeReaction(idnode, idCase, 1, fx, fy, fz, mx, my, mz));
                    isOk = rowSet.MoveNext();
                    counter++;

                    if (counter % 1000000 == 0)
                    {
                        resultServer.StoreData(nodeForces);
                        nodeForces.Clear();
                    }
                }
            }
            resultServer.StoreData(nodeForces);
            return true;

        }

        internal static void GetNodeCoordinates(RobotApplication robot, ResultServer<NodeCoordinates> resultServer, List<string> nodes)
        {
            RobotResultQueryParams result_params = robot.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            RobotStructure rstructure = robot.Project.Structure;
            RobotSelection nod_sel = rstructure.Selections.Create(IRobotObjectType.I_OT_NODE);
            IRobotResultQueryReturnType query_return = default(IRobotResultQueryReturnType);

            if (nodes != null && nodes.Count > 0)
                nod_sel.FromText(Utils.GetSelectionString(nodes));
            else
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

            List<NodeCoordinates> nodeCoords = new List<NodeCoordinates>();

            while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
            {
                query_return = robot.Project.Structure.Results.Query(result_params, row_set);
                ok = row_set.MoveFirst();
                while (ok)
                {
                    result_row = row_set.CurrentRow;
                    nod_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_NODE);
                    nodeCoords.Add(new NodeCoordinates(nod_num.ToString(), (double)row_set.CurrentRow.GetValue(0), (double)row_set.CurrentRow.GetValue(1), (double)row_set.CurrentRow.GetValue(2)));                  
                    kounta++;
                    ok = row_set.MoveNext();
                }
                row_set.Clear();
            }
            result_params.Reset();
            resultServer.StoreData(nodeCoords);
        }  

        public static bool GetNodeDisplacements(RobotApplication RobotApp, ResultServer<NodeDisplacement> resultServer, List<string> nodeNumbers, List<string> loadcaseNumber)
        {
            RobotResultQueryParams queryParams = (RobotResultQueryParams)RobotApp.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            List<int> results = new List<int>();
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UX);
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UY);
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UZ);
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RX);
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RY);
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RZ);

            queryParams.ResultIds.SetSize(results.Count);

            for (int i = 0; i < results.Count; i++)
            {
                int id = (int)results[i];
                queryParams.ResultIds.Set(i + 1, id);
            }

            RobotSelection nodeSelection = RobotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            RobotSelection caseSelection = RobotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);

            if (nodeNumbers == null || nodeNumbers.Count == 0) nodeSelection.FromText("all"); else nodeSelection.FromText(Utils.GetSelectionString(nodeNumbers));
            if (loadcaseNumber == null || loadcaseNumber.Count == 0) caseSelection.FromText("all"); else caseSelection.FromText(Utils.GetSelectionString(loadcaseNumber));

            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.Selection.Set(IRobotObjectType.I_OT_NODE, nodeSelection);
            RobotResultRowSet rowSet = new RobotResultRowSet();

            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
            bool anythingCalculated = false;

            int counter = 0;
            List<NodeDisplacement> nodeForces = new List<NodeDisplacement>();
            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = RobotApp.Kernel.Structure.Results.Query(queryParams, rowSet);
                bool isOk = anythingCalculated = rowSet.MoveFirst();
                while (isOk)
                {
                    RobotResultRow row = rowSet.CurrentRow;
                    int idCase = (int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
                    int idnode = (int)row.GetParam(IRobotResultParamType.I_RPT_NODE);

                    double fx = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UX);
                    double fy = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UY);
                    double fz = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UZ);
                    double mx = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RX);
                    double my = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RY);
                    double mz = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RZ);

                    nodeForces.Add(new NodeDisplacement(idnode, idCase, 1, fx, fy, fz, mx, my, mz));
                    isOk = rowSet.MoveNext();
                    counter++;

                    if (counter % 1000000 == 0)
                    {
                        resultServer.StoreData(nodeForces);
                        nodeForces.Clear();
                    }
                }
            }
            resultServer.StoreData(nodeForces);
            return true;
        }
    }
}
