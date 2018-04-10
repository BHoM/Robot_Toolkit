using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Results;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using RobotOM;
using BH.Engine.Robot;
using BHEG = BH.Engine.Geometry;
using BH.oM.Common;
using System;
using System.Collections;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        protected override IEnumerable<IResult> ReadResults(Type type, IList ids = null, IList cases = null, int divisions = 5)
        {
            if (type == typeof(BarForce))
                return ReadBarForce();
            if (type == typeof(NodeDisplacement))
                return ReadNodeDisplacement();
            return base.ReadResults(type, ids, cases, divisions);
        }
        
        public List<BarForce> ReadBarForce(IList ids = null, IList cases = null, int divisions = 5)
        {
            List<BarForce> barforces = new List<BarForce>();
            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
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

            RobotSelection barSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            RobotSelection caseSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);

            barSelection.FromText("all");
            caseSelection.FromText("all");

            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.Selection.Set(IRobotObjectType.I_OT_BAR, barSelection);
            queryParams.SetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT, divisions);
            RobotResultRowSet rowSet = new RobotResultRowSet();

            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;

            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = m_RobotApplication.Kernel.Structure.Results.Query(queryParams, rowSet);
                bool isOk = rowSet.MoveFirst();
                while (isOk)
                {
                    RobotResultRow row = rowSet.CurrentRow;
                    int idCase = (int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
                    int idBar = (int)row.GetParam(IRobotResultParamType.I_RPT_BAR);
                    int idPoint = (int)row.GetParam(IRobotResultParamType.I_RPT_BAR_DIV_POINT);
                    int division = (int)row.GetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT);

                    double fx = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX) : 0;
                    double fy = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY) : 0;
                    double fz = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ) : 0;
                    double mx = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX) : 0;
                    double my = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY) : 0;
                    double mz = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ) : 0;
                    BarForce tempbForce = new BarForce { FX = fx, FY = fy, FZ = fz, MX = mx, MY = my, MZ = mz };
                    tempbForce.Case = idCase.ToString();
                    tempbForce.ObjectId = idBar.ToString();
                    tempbForce.Divisions = idPoint;
                    tempbForce.Position = (1 / (System.Convert.ToDouble(division) - 1)) * (System.Convert.ToDouble(idPoint) - 1); 
                    barforces.Add(tempbForce);
                    isOk = rowSet.MoveNext();

                }
            }
            return barforces;
        }

        public List<NodeDisplacement> ReadNodeDisplacement(IList ids = null, IList cases = null, int divisions = 5)
        {
            List<NodeDisplacement> nodeDisplacements = new List<NodeDisplacement>();

            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

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

            RobotSelection nodeSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            RobotSelection caseSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);

            nodeSelection.FromText("all");
            caseSelection.FromText("all");

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

                    double ux = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UX);
                    double uy = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UY);
                    double uz = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UZ);
                    double rx = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RX);
                    double ry = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RY);
                    double rz = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RZ);

                    NodeDisplacement tempNodeDisp = new NodeDisplacement { UX = ux, UY = uy, UZ = uz, RX = rx, RY = ry, RZ = rz };
                    tempNodeDisp.Case = idCase.ToString();
                    tempNodeDisp.ObjectId = idNode.ToString();
                    nodeDisplacements.Add(tempNodeDisp);
                    isOk = rowSet.MoveNext();
                }
            }
            return nodeDisplacements;
        }

    }
}
