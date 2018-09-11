using System.Collections.Generic;
using BH.oM.Structure.Results;
using RobotOM;
using System.Collections;
using BH.Engine.Robot;
using BH.oM.Geometry;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<MeshNodeStress> ReadMeshNodeStresses(IList ids = null, 
                                                    IList cases = null, 
                                                    CoordinateSystem coordinateSystem = null,
                                                    MeshResultType layer = MeshResultType.AbsoluteMaximum,
                                                    MeshResultSmoothingType smoothing = MeshResultSmoothingType.ByPanel)
                                                    
        {
            IRobotObjObjectServer server = m_RobotApplication.Project.Structure.Objects;
            IRobotFeLayerType robotMeshLayer = Convert.FromBHoMEnum(layer);
            IRobotFeResultSmoothing robotFESmoothing = Convert.FromBHoMEnum(smoothing);

            RobotFeResultParams FEParams = new RobotFeResultParams();
            List<MeshNodeStress> meshNodeStresses = new List<MeshNodeStress>();

            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            List<int> results = new List<int>();

            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SXX);
            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SYY);
            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SXY);
            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_TXX);
            results.Add((int)IRobotFeResultType.I_FRT_DETAILED_TYY);

            queryParams.ResultIds.SetSize(results.Count);

            for (int j = 0; j < results.Count; j++)
            {
                int id = (int)results[j];
                queryParams.ResultIds.Set(j + 1, id);
            }

            RobotSelection caseSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            RobotSelection panelSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);


            string caseSelectionString = (ids != null)? Convert.ToRobotSelectionString(GetCaseNumbers(cases)) : "all";
            string panelSelectionString = (cases != null)? Convert.ToRobotSelectionString(CheckAndGetIds(ids)) : "all";

            if (ids == null || ids.Count == 0) panelSelection.FromText("all"); else panelSelection.FromText(panelSelectionString);
            if (ids == null || cases.Count == 0) caseSelection.FromText("all"); else caseSelection.FromText(caseSelectionString);

            queryParams.Selection.Set(IRobotObjectType.I_OT_PANEL, panelSelection);
            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            queryParams.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            queryParams.SetParam(IRobotResultParamType.I_RPT_SMOOTHING, (int)robotFESmoothing);
            if (coordinateSystem != null)
            {
                queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X_DEFTYPE, IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN);
                queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X, new double[] { coordinateSystem.X.X, coordinateSystem.X.Y,coordinateSystem.X.Z});
            }
            queryParams.SetParam(IRobotResultParamType.I_RPT_LAYER, (int)robotMeshLayer);

            RobotResultRowSet rowSet = new RobotResultRowSet();
            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
               
            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = m_RobotApplication.Kernel.Structure.Results.Query(queryParams, rowSet);

                bool isOk = rowSet.MoveFirst();
                while (isOk)
                {
                    List<double> line = new List<double>();

                    RobotResultRow row = rowSet.CurrentRow;
                    int idCase = (int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
                    int idPanel = (int)row.GetParam(IRobotResultParamType.I_RPT_PANEL);
                    int idNode = (int)row.GetParam(IRobotResultParamType.I_RPT_NODE);

                    MeshNodeStress meshNodeStress = new MeshNodeStress();
                    meshNodeStress.ObjectId = idPanel.ToString();
                    meshNodeStress.NodeId = idNode.ToString();
                    meshNodeStress.Case = idCase.ToString();
                    meshNodeStress.SXX = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_SXX);
                    meshNodeStress.SXY = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_SXY);
                    meshNodeStress.SYY = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_SYY);
                    meshNodeStress.TXX = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_TXX);
                    meshNodeStress.TYY = row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_TYY);

                    meshNodeStresses.Add(meshNodeStress);

                    isOk = rowSet.MoveNext();
                }
            }

            return meshNodeStresses;
        }

    }
}

/***************************************************/
