using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit.Results.Bars
{
    /// <summary>
    /// 
    /// </summary>
    public class BarForces
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="selection"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static BHoM.Structural.Results.Bars.BarForceCollection 
            GetBarForcesQuery(BHoM.Global.Project project, string selection = "all", string filePath = "")
        {
            BHoM.Structural.LoadcaseFactory loadcases = project.Structure.Loadcases;
            loadcases.ForceUniqueByNumber();
            
            BHoM.Structural.Results.Bars.BarForceCollection barforcecol = new BHoM.Structural.Results.Bars.BarForceCollection();
            RobotApplication robot = new RobotApplication();
            
            if (robot.Project.Structure.Results.Status == IRobotResultStatusType.I_RST_AVAILABLE)
            {
                RobotResultQueryParams result_params = robot.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
                RobotSelection bar_sel = robot.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_BAR);
                RobotSelection cas_sel = robot.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_CASE);
                string test = cas_sel.ToString();
                
                result_params.ResultIds.SetSize(11);
                result_params.ResultIds.Set(1, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
                result_params.ResultIds.Set(2, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY);
                result_params.ResultIds.Set(3, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ);
                result_params.ResultIds.Set(4, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX);
                result_params.ResultIds.Set(5, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY);
                result_params.ResultIds.Set(6, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ);
                result_params.ResultIds.Set(7, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX);
                result_params.ResultIds.Set(8, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN);
                result_params.ResultIds.Set(9, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_FX_SX);
                result_params.ResultIds.Set(10, 17);
                result_params.ResultIds.Set(11, 271);

                result_params.SetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT, 2);
                result_params.Selection.Set(IRobotObjectType.I_OT_BAR, bar_sel);
                result_params.Selection.Set(IRobotObjectType.I_OT_CASE, cas_sel);
                result_params.SetParam(IRobotResultParamType.I_RPT_MODE, 0);
                result_params.SetParam(IRobotResultParamType.I_RPT_MODE_CMB, IRobotModeCombinationType.I_MCT_CQC);
                result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
                result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
                
                RobotResultRowSet row_set = new RobotResultRowSet();
                IRobotResultQueryReturnType query_return = new IRobotResultQueryReturnType();
                query_return = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
                RobotResultServer rserver = robot.Kernel.Structure.Results;

                while (query_return != IRobotResultQueryReturnType.I_RQRT_DONE)
                {
                    query_return = rserver.Query(result_params, row_set);

                    bool ok;
                    ok = row_set.MoveFirst();
                    while (ok)
                    {
                        IRobotResultRow result_row = row_set.CurrentRow;
                        int curr_bar = (int)result_row.GetParam(IRobotResultParamType.I_RPT_BAR);
                        int curr_cas = (int)result_row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
                        int div_pnt = (int)result_row.GetParam(IRobotResultParamType.I_RPT_BAR_DIV_POINT);
                        int div_count = (int)result_row.GetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT);
                        double length = result_row.GetValue(17);
                                              
                        BHoM.Structural.Results.Bars.BarForce barForce = 
                            new BHoM.Structural.Results.Bars.BarForce(loadcases.Create(curr_cas, "Temp"), curr_bar, div_pnt);

                        barForce.Divisions = div_count;
                        barForce.RelativePosition = div_pnt / (div_count - 1);
                        if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX))
                        {
                            barForce.FX = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
                        }
                        if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY))
                        {
                            barForce.FY = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY);
                        }
                        if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ))
                        {
                            barForce.FZ = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ);
                        }
                        if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX))
                        {
                            barForce.MX = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX);
                        }
                        if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY))
                        {
                            barForce.MY = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY);
                        }
                        if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ))
                        {
                            barForce.MZ = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ);
                        }
                        if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX))
                        {
                            barForce.SMax = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX);
                        }
                        if (result_row.IsAvailable((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN))
                        {
                            barForce.SMin = result_row.GetValue((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN);
                        }

                        barforcecol.Add(barForce);
                        ok = row_set.MoveNext();
                        result_row = null;
                   }
                    row_set.Clear();
                    row_set = null;
                  }
            }
            return barforcecol;
        }
    }
}
