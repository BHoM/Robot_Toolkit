﻿using BHoM.Base.Results;
using BHoM.Structural.Results;
using Robot_Adapter.Base;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot_Adapter.Structural.Results
{
    public class SlabReinforcementResults
    {
        public static bool GetSlabReinforcement(RobotApplication Robot, ResultServer<SlabReinforcement> resultServer, List<string> panelNumbers, List<string> loadcaseNumbers)
        {
            IRobotObjObjectServer server = Robot.Project.Structure.Objects;

            RConcrSlabRequiredReinfCalcParams FEParams = new RConcrSlabRequiredReinfCalcParams();
            //RobotFeResultParams FEParams = new RobotFeResultParams();

            RobotResultQueryParams queryParams = (RobotResultQueryParams)Robot.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
            
            List<int> results = new List<int>();
            results.Add((int)IRobotFeResultType.I_FRT_REINF_E_AX_TOP);
            results.Add((int)IRobotFeResultType.I_FRT_REINF_E_AX_BOTTOM);
            results.Add((int)IRobotFeResultType.I_FRT_REINF_E_AY_TOP);
            results.Add((int)IRobotFeResultType.I_FRT_REINF_E_AY_BOTTOM);
            
            //results.Add(IRobotFeResultType.I_VRRT_AX_BOTTOM);
            //results.Add((int)IRobotViewReinforcementResultType.I_VRRT_AX_TOP);
            //results.Add((int)IRobotViewReinforcementResultType.I_VRRT_AY_BOTTOM);
            //results.Add((int)IRobotViewReinforcementResultType.I_VRRT_AY_TOP);
            queryParams.ResultIds.SetSize(results.Count);

            for (int i = 0; i < results.Count; i++)
            {
                int id = (int)results[i];
                queryParams.ResultIds.Set(i + 1, id);
            }

            // Creating RobotSelection for loadcases and panels
            RobotSelection caseSelection = Robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            RobotSelection panelSelection = Robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);

            if (panelNumbers == null || panelNumbers.Count == 0) panelSelection.FromText("all"); else panelSelection.FromText(Utils.GetSelectionString(panelNumbers));
            if (loadcaseNumbers == null || loadcaseNumbers.Count == 0) caseSelection.FromText("all"); else caseSelection.FromText(Utils.GetSelectionString(loadcaseNumbers));

            queryParams.Selection.Set(IRobotObjectType.I_OT_PANEL, panelSelection);
            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            queryParams.SetParam(IRobotResultParamType.I_RPT_REINFORCE_CALC_METHOD, IRConcrReinforceCalcType.I_CRCT_BENDING_COMPRESSION_TENSION);
            queryParams.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            queryParams.SetParam(IRobotResultParamType.I_RPT_SMOOTHING, IRobotFeResultSmoothing.I_FRS_SMOOTHING_WITHIN_A_PANEL); // TODO: check furthur with eddie
            //queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X_DEFTYPE, IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN);
            //queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X, new double[] { 1, 0, 0 }); // TODO: check the outcome later and discuss with eddie

            RobotResultRowSet rowSet = new RobotResultRowSet();
            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;

            int counter = 0;
            List<SlabReinforcement> reinforcementLevel = new List<SlabReinforcement>(); // TODO: Create ReinforcementLevel class in BHoM.Structural.Results


            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = Robot.Kernel.Structure.Results.Query(queryParams, rowSet);

                bool isOk = rowSet.MoveFirst();
                while (isOk)
                {
                    List<double> line = new List<double>();

                    RobotResultRow row = rowSet.CurrentRow;
                    int idCase = -1; //(int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
                    int idPanel = (int)row.GetParam(IRobotResultParamType.I_RPT_PANEL);
                    int idNode = (int)row.GetParam(IRobotResultParamType.I_RPT_NODE);

                    line.Add(idPanel);
                    line.Add(idNode);
                    line.Add(idCase);

                    double Axp = (double)row.GetValue((int)IRobotFeResultType.I_FRT_REINF_E_AX_TOP);
                    double Axm = (double)row.GetValue((int)IRobotFeResultType.I_FRT_REINF_E_AX_BOTTOM);
                    double Ayp = (double)row.GetValue((int)IRobotFeResultType.I_FRT_REINF_E_AY_TOP);
                    double Aym = (double)row.GetValue((int)IRobotFeResultType.I_FRT_REINF_E_AY_BOTTOM);                    

                    reinforcementLevel.Add(new SlabReinforcement(idPanel, idNode, idCase,1, Axm, Axp, Aym, Ayp)); // WhatAbout timestep?? Ask eddie
                    isOk = rowSet.MoveNext();
                    counter++;

                    if (counter % 1000000 == 0 && resultServer.CanStore)
                    {
                        resultServer.StoreData(reinforcementLevel);
                        reinforcementLevel.Clear();
                    }
                }
            }
            resultServer.StoreData(reinforcementLevel);
            return true;
        }
    }
}