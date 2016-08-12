using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RobotOM;
using BHoMR = BHoM.Structural.Results;
using BHoMBR = BHoM.Base.Results;
using Robot_Adapter.Utility;

namespace Robot_Adapter.Structural.Interface
{
    public partial class RobotAdapter : BHoM.Structural.Interface.IResultAdapter
    {
        public bool GetBarForces(List<int> bars, List<int> cases, int divisions)
        {
            RobotApplication RobotApp = Robot;

            BHoMBR.ResultServer<BHoMR.BarForce> resultServer = new BHoMBR.ResultServer<BHoMR.BarForce>(Robot.Project.FileName);

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
            List<BHoMR.BarForce> barForces = new List<BHoMR.BarForce>();
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

                    barForces.Add(new BHoMR.BarForce(idBar, idCase, idPoint, 1, fx, fy, fz, mx, my, mz));
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

        public bool GetBarForces(List<string> bars, List<string> cases, int divisions, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.ResultSet<BHoMR.BarForce>> results)
        {
            throw new NotImplementedException();
        }

        public bool GetBarStresses()
        {
            throw new NotImplementedException();
        }

        public bool GetModalResults()
        {
            throw new NotImplementedException();
        }

        public bool GetNodeAccelerations()
        {
            throw new NotImplementedException();
        }

        public bool GetNodeDisplacements()
        {
            throw new NotImplementedException();
        }

        public bool GetNodeReactions()
        {
            throw new NotImplementedException();
        }

        public bool GetNodeReactions(List<string> nodes, List<string> cases, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.ResultSet<BHoMR.NodeReaction>> results)
        {
            throw new NotImplementedException();
        }

        public bool GetNodeVelocities()
        {
            throw new NotImplementedException();
        }

        public bool GetPanelForces()
        {
            throw new NotImplementedException();
        }

        public bool GetPanelStress()
        {
            throw new NotImplementedException();
        }
    }
}
