using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;
using BHoM.Base.Results;
using BHoM.Structural.Results;
using Robot_Adapter.Base;

namespace Robot_Adapter.Results
{
    /// <summary>
    /// 
    /// </summary>
    public static class BarResults
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RobotApp"></param>
        /// <param name="resultServer"></param>
        /// <param name="bars"></param>
        /// <param name="cases"></param>
        /// <param name="divisions"></param>
        /// <returns></returns>
        public static bool LoadBarForces(RobotApplication RobotApp, ResultServer<BarForce> resultServer, List<string> bars, List<string> cases, int divisions)
        {           
            RobotResultQueryParams queryParams = (RobotResultQueryParams)RobotApp.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            List<int> results = new List<int>();
            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY);
            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ);
            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX);
            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY);
            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ);

            queryParams.ResultIds.SetSize(results.Count);

            for (int i = 0; i < results.Count; i++)
            {
                int id = (int)results[i];
                queryParams.ResultIds.Set(i + 1, id);
            }

            RobotSelection barSelection = RobotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            RobotSelection caseSelection = RobotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);

            if (bars == null) barSelection.FromText("all"); else barSelection.FromText(Utils.GetSelectionString(bars));
            if (cases == null) caseSelection.FromText("all"); else caseSelection.FromText(Utils.GetSelectionString(cases));

            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.Selection.Set(IRobotObjectType.I_OT_BAR, barSelection);
            queryParams.SetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT, divisions);
            RobotResultRowSet rowSet = new RobotResultRowSet();

            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
            bool anythingCalculated = false;

            int counter = 0;
            List<BarForce> barForces = new List<BarForce>();
            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = RobotApp.Kernel.Structure.Results.Query(queryParams, rowSet);
                bool isOk = anythingCalculated = rowSet.MoveFirst();
                while (isOk)
                {
                    RobotResultRow row = rowSet.CurrentRow;
                    int idCase = (int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
                    int idBar = (int)row.GetParam(IRobotResultParamType.I_RPT_BAR);
                    int idPoint = (int)row.GetParam(IRobotResultParamType.I_RPT_BAR_DIV_POINT);

                    double fx = row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
                    double fy = row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY);
                    double fz = row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ);
                    double mx = row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX);
                    double my = row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY);
                    double mz = row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ);

                    barForces.Add(new BarForce(idBar, idCase, idPoint, 1, fx, fy, fz, mx, my, mz));
                    isOk = rowSet.MoveNext();
                    counter++;

                    if (counter % 1000000 == 0)
                    {
                        resultServer.StoreData(barForces);
                        barForces.Clear();
                    }
                }
            }
            resultServer.StoreData(barForces);
            return true;
        }
        //public static BHoM.Structural.Results.Bars.BarForceCollection GetBarForcesQuery(BHoM.Global.Project project, string selection = "all", string filePath = "")
        //{
        //    BHoM.Structural.LoadcaseFactory loadcases = project.Structure.Loadcases;
        //    loadcases.ForceUniqueByNumber();

        //    BHoM.Structural.Results.Bars.BarForceCollection barforcecol = new BHoM.Structural.Results.Bars.BarForceCollection();
        //    RobotApplication robot = new RobotApplication();

        //    if (robot.Project.Structure.Results.Status == IRobotResultStatusType.I_RST_AVAILABLE)
        //    {
        //        RobotResultQueryParams result_params = robot.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
        //        RobotSelection bar_sel = robot.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_BAR);
        //        RobotSelection cas_sel = robot.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_CASE);
        //        string test = cas_sel.ToString();

        //        result_params.ResultIds.SetSize(11);
        //        result_params.ResultIds.Set(1, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
        //        result_params.ResultIds.Set(2, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY);
        //        result_params.ResultIds.Set(3, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ);
        //        result_params.ResultIds.Set(4, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX);
        //        result_params.ResultIds.Set(5, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY);
        //        result_params.ResultIds.Set(6, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ);
        //        result_params.ResultIds.Set(7, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX);
        //        result_params.ResultIds.Set(8, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN);
        //        result_params.ResultIds.Set(9, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_FX_SX);
        //        result_params.ResultIds.Set(10, 17);
        //        result_params.ResultIds.Set(11, 271);

        //        result_params.SetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT, 2);
        //        result_params.Selection.Set(IRobotObjectType.I_OT_BAR, bar_sel);
        //        result_params.Selection.Set(IRobotObjectType.I_OT_CASE, cas_sel);
        //        result_params.SetParam(IRobotResultParamType.I_RPT_MODE, 0);
        //        result_params.SetParam(IRobotResultParamType.I_RPT_MODE_CMB, IRobotModeCombinationType.I_MCT_CQC);
        //        result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
        //        result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);

        //        RobotResultRowSet row_set = new RobotResultRowSet();
        //        IRobotResultQueryReturnType query_return = new IRobotResultQueryReturnType();
        //        query_return = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
        //        RobotResultServer rserver = robot.Kernel.Structure.Results;

        //        while (query_return != IRobotResultQueryReturnType.I_RQRT_DONE)
        //        {
        //            query_return = rserver.Query(result_params, row_set);

        //            bool ok;
        //            ok = row_set.MoveFirst();
        //            while (ok)
        //            {
        //                IRobotResultRow result_row = row_set.CurrentRow;
        //                int curr_bar = (int)result_row.GetParam(IRobotResultParamType.I_RPT_BAR);
        //                int curr_cas = (int)result_row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
        //                int div_pnt = (int)result_row.GetParam(IRobotResultParamType.I_RPT_BAR_DIV_POINT);
        //                int div_count = (int)result_row.GetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT);
        //                double length = result_row.GetValue(17);

        //                BHoM.Structural.Results.Bars.BarForce barForce = 
        //                    new BHoM.Structural.Results.Bars.BarForce(loadcases.Create(curr_cas, "Temp"), curr_bar, div_pnt);

        //                barForce.BarDivisions = div_count;
        //                barForce.RelativePosition = div_pnt / (div_count - 1);
        //                if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX))
        //                {
        //                    barForce.FX = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
        //                }
        //                if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY))
        //                {
        //                    barForce.FY = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY);
        //                }
        //                if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ))
        //                {
        //                    barForce.FZ = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ);
        //                }
        //                if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX))
        //                {
        //                    barForce.MX = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX);
        //                }
        //                if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY))
        //                {
        //                    barForce.MY = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY);
        //                }
        //                if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ))
        //                {
        //                    barForce.MZ = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ);
        //                }
        //                if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX))
        //                {
        //                    barForce.SMax = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX);
        //                }
        //                if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN))
        //                {
        //                    barForce.SMin = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN);
        //                }

        //                barforcecol.Add(barForce);
        //                ok = row_set.MoveNext();
        //                result_row = null;
        //           }
        //            row_set.Clear();
        //            row_set = null;
        //          }
        //    }
        //    return barforcecol;
        //}


    }
}
