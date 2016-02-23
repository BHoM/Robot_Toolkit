using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;

namespace RobotToolkit.Results.Bars
{
    class BarForces
    {
    //    public void GetBarForces(int[] barNumbers, int[] loadcaseNumbers,
    //        out BHoM.Structural.Results.Bars.BarForceCollection barForceCollection,
    //        string FilePath = "LiveLink")
    //    {
    //        RobotApplication robot = null;
    //        if (FilePath == "LiveLink") robot = new RobotApplication();

    //        RobotResultQueryParams result_params = default(RobotResultQueryParams);
    //        result_params = robot.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
    //        RobotStructure rstructure = default(RobotStructure);
    //        rstructure = robot.Project.Structure;
    //        RobotSelection bar_sel = default(RobotSelection);
    //        RobotSelection lcase_sel = default(RobotSelection);
    //        IRobotResultQueryReturnType query_return = default(IRobotResultQueryReturnType);


    //        result_params.ResultIds.SetSize(3);
    //        result_params.ResultIds.Set(1, 0);
    //        result_params.ResultIds.Set(2, 1);
    //        result_params.ResultIds.Set(3, 2);


    //        result_params.Selection.Set(IRobotObjectType.I_OT_NODE, nod_sel);
    //        result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
    //        result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
    //        query_return = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
    //        RobotResultRowSet row_set = new RobotResultRowSet();
    //        bool ok = false;

    //        RobotResultRow result_row = default(RobotResultRow);
    //        int nod_num = 0;
    //        int kounta = 0;

    //        List<double[]> coords = new List<double[]>();
    //        List<int> ids = new List<int>();
    //        Dictionary<String, StructuralComponents.Node> _str_nodes = new Dictionary<String, StructuralComponents.Node>();

    //        while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
    //        {
    //            query_return = rstructure.Results.Query(result_params, row_set);
    //            ok = row_set.MoveFirst();
    //            while (ok)
    //            {
    //                result_row = row_set.CurrentRow;
    //                nod_num = result_row.GetParam(IRobotResultParamType.I_RPT_NODE);
    //                ids.Add(nod_num);
    //                double[] coord = new double[] { row_set.CurrentRow.GetValue(0), row_set.CurrentRow.GetValue(1), row_set.CurrentRow.GetValue(2) };
    //                StructuralComponents.Node nod = new StructuralComponents.Node(coord[0], coord[1], coord[2], nod_num);
    //                _str_nodes.Add(nod_num.ToString(), nod);
    //                coords.Add(coord);
    //                kounta++;
    //                ok = row_set.MoveNext();
    //            }
    //            row_set.Clear();
    //        }
    //        result_params.Reset();

    //        nodeIds = ids.ToArray();
    //        nodeCoords = coords.ToArray();
    //        str_nodes = _str_nodes;

    //    }
    }
}
