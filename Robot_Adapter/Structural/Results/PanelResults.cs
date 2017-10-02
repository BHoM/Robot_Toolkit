//using BH.oM.Base.Results;
//using BH.oM.Structural.Results;
//using Robot_Adapter.Base;
//using RobotOM;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Robot_Adapter.Structural.Results
//{
//    public class PanelResults
//    {
//        public static bool GetPanelForces(RobotApplication Robot, ResultServer<PanelForce> resultServer, List<string> panelNumbers, List<string> loadcaseNumbers)
//        {
//            IRobotObjObjectServer server = Robot.Project.Structure.Objects;

//            RobotFeResultParams FEParams = new RobotFeResultParams();

//            RobotResultQueryParams queryParams = (RobotResultQueryParams)Robot.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

//            List<int> results = new List<int>();
//            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NXX);
//            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NYY);
//            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NXY);
//            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MXX);
//            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MYY);
//            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MXY);
//            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_QXX);
//            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_QYY);
//            //ValueT
//            queryParams.ResultIds.SetSize(results.Count);

//            for (int i = 0; i < results.Count; i++)
//            {
//                int id = (int)results[i];
//                queryParams.ResultIds.Set(i + 1, id);
//            }

//            RobotSelection caseSelection = Robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
//            RobotSelection barSelection = Robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);

//            if (panelNumbers == null || panelNumbers.Count == 0) barSelection.FromText("all"); else barSelection.FromText(Utils.GetSelectionString(panelNumbers));
//            if (loadcaseNumbers == null || loadcaseNumbers.Count == 0) caseSelection.FromText("all"); else caseSelection.FromText(Utils.GetSelectionString(loadcaseNumbers));

//            queryParams.Selection.Set(IRobotObjectType.I_OT_PANEL, barSelection);
//            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
//            queryParams.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
//            queryParams.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
//            queryParams.SetParam(IRobotResultParamType.I_RPT_SMOOTHING, IRobotFeResultSmoothing.I_FRS_SMOOTHING_WITHIN_A_PANEL);
//            queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X_DEFTYPE, IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN);
//            queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X, new double[] { 0, 0, 0 });
//            queryParams.SetParam(IRobotResultParamType.I_RPT_LAYER, (int)IRobotFeLayerType.I_FLT_MIDDLE);
//            RobotResultRowSet rowSet = new RobotResultRowSet();
//            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;

//            int counter = 0;
//            List<PanelForce> panelForces = new List<PanelForce>();

//            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
//            {
//                ret = Robot.Kernel.Structure.Results.Query(queryParams, rowSet);

//                bool isOk = rowSet.MoveFirst();
//                while (isOk)
//                {
//                    List<double> line = new List<double>();

//                    RobotResultRow row = rowSet.CurrentRow;
//                    int idCase = (int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
//                    int idPanel = (int)row.GetParam(IRobotResultParamType.I_RPT_PANEL);
//                    int idNode = (int)row.GetParam(IRobotResultParamType.I_RPT_NODE);

//                    line.Add(idPanel);
//                    line.Add(idNode);
//                    line.Add(idCase);

//                    double fx = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_NXX);
//                    double fy = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_NYY);
//                    double fxy = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_NXY);
//                    double mx = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_MXX);
//                    double my = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_MYY);
//                    double mxy = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_MXY);
//                    double vx = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_QXX);
//                    double vy = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_QYY);

//                    panelForces.Add(new PanelForce(idPanel, idNode, idCase, 1, fx, fy, fxy, mx, my, mxy, vx, vy));
//                    isOk = rowSet.MoveNext();
//                    counter++;

//                    if (counter % 1000000 == 0 && resultServer.CanStore)
//                    {
//                        resultServer.StoreData(panelForces);
//                        panelForces.Clear();
//                    }
//                }
//            }

//            resultServer.StoreData(panelForces);
//            return true;
//        }

//        public static bool GetPanelStress(RobotApplication Robot, ResultServer<PanelStress> resultServer, List<string> panelNumbers, List<string> loadcaseNumbers)
//        {
            
//            IRobotObjObjectServer server = Robot.Project.Structure.Objects;

//            RobotFeResultParams FEParams = new RobotFeResultParams();
//            int counter = 0;
//            int index = 0;
//            // List<PanelForce> panelForces = new List<PanelForce>();
//            Dictionary<string, object[]> panelStress = new Dictionary<string, object[]>();

//            for (int i = (int)IRobotFeLayerType.I_FLT_UPPER; i <= (int)IRobotFeLayerType.I_FLT_LOWER; i++)
//            {
//                RobotResultQueryParams queryParams = (RobotResultQueryParams)Robot.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

//                List<int> results = new List<int>();
//                if (i != (int)IRobotFeLayerType.I_FLT_MIDDLE)
//                {
//                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SXX);
//                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SYY);
//                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SXY);
//                }
//                else
//                {
//                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_TXX);
//                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_TYY);
//                }
                
//                queryParams.ResultIds.SetSize(results.Count);

//                for (int j = 0; j < results.Count; j++)
//                {
//                    int id = (int)results[j];
//                    queryParams.ResultIds.Set(j + 1, id);
//                }

//                RobotSelection caseSelection = Robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
//                RobotSelection barSelection = Robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);

//                if (panelNumbers == null || panelNumbers.Count == 0) barSelection.FromText("all"); else barSelection.FromText(Utils.GetSelectionString(panelNumbers));
//                if (loadcaseNumbers == null || loadcaseNumbers.Count == 0) caseSelection.FromText("all"); else caseSelection.FromText(Utils.GetSelectionString(loadcaseNumbers));

//                queryParams.Selection.Set(IRobotObjectType.I_OT_PANEL, barSelection);
//                queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
//                queryParams.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
//                queryParams.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
//                queryParams.SetParam(IRobotResultParamType.I_RPT_SMOOTHING, IRobotFeResultSmoothing.I_FRS_SMOOTHING_WITHIN_A_PANEL);
//                queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X_DEFTYPE, IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN);
//                queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X, new double[] { 1, 0, 0 });
//                RobotResultRowSet rowSet = new RobotResultRowSet();
//                IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;           
//                queryParams.SetParam(IRobotResultParamType.I_RPT_LAYER, i);

//                while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
//                {
//                    ret = Robot.Kernel.Structure.Results.Query(queryParams, rowSet);

//                    bool isOk = rowSet.MoveFirst();
//                    while (isOk)
//                    {
//                        List<double> line = new List<double>();

//                        RobotResultRow row = rowSet.CurrentRow;
//                        int idCase = (int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
//                        int idPanel = (int)row.GetParam(IRobotResultParamType.I_RPT_PANEL);
//                        int idNode = (int)row.GetParam(IRobotResultParamType.I_RPT_NODE);

//                        string key = idPanel + ":" + idNode + ":" + idCase + ":" + 1;

//                        object[] data = null;
//                        if (i == (int)IRobotFeLayerType.I_FLT_UPPER)
//                        {
//                            data = new object[PanelStress.GetColumnHeaders().Length];
//                            data[0] = key;
//                            data[1] = idPanel;
//                            data[2] = idCase;
//                            data[3] = 1;
//                            data[4] = idNode;
//                            panelStress.Add(key, data);
//                            index = 5;
//                        }
//                        else
//                        {
//                            data = panelStress[key];
//                            index = 8;
//                        }

//                        if (i == (int)IRobotFeLayerType.I_FLT_MIDDLE)
//                        {
//                            data[data.Length - 2] = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_TXX);
//                            data[data.Length - 1] = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_TYY);
//                        }
//                        else
//                        {
//                            data[index] = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_SXX);
//                            data[index + 1] = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_SYY);
//                            data[index + 2] = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_SXY);
//                        }

//                        isOk = rowSet.MoveNext();
//                        counter++;                      
//                    }
//                }
//            }
//            List<PanelStress> stress = new List<PanelStress>();
//            foreach (object[] data in panelStress.Values)
//            {
//                PanelStress pS = new PanelStress();
//                pS.SetData(data);
//                stress.Add(pS);
//            }

//            resultServer.StoreData(stress);
//            return true;
//        }
//    }


//}
