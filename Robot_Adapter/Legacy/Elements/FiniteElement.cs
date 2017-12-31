namespace Robot_Adapter.Structural.Elements
{
    /// <summary>
    /// Finite elements class for 2D planar shell finite element objects
    /// </summary>
    public class FiniteElement
    {
        /// <summary>
        /// Gets the FE meshes from a Robot model using the fast query method
        /// </summary>
        /// <param name="panel_ids"></param>
        /// <param name="coords"></param>
        /// <param name="vertex_indices"></param>
        /// <param name="str_nodes"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        //public static bool GetFEMeshQuery(BHoM.Global.Project project, out int[] panel_ids, out double[][] coords, out Dictionary<int, int[]> vertex_indices, out Dictionary<int, BH.oM.Structural.Node> str_nodes, string filePath = "LiveLink")
        //{
        //    RobotApplication robot = null;
        //    if (filePath == "LiveLink") robot = new RobotApplication();

        //    //First call getnodesquery to get node points
        //    double[][] nodeCoords = null;

        //    Dictionary<int, BH.oM.Structural.Node> _str_nodes = new Dictionary<int, BH.oM.Structural.Node>();
        //    Robot_Adapter.Node.GetNodesQuery(project, filePath);
        //    Dictionary<int, int> _nodeIds = new Dictionary<int, int>();
        //    for (int i = 0; i < _str_nodes.Count; i++)
        //    {
        //        _nodeIds.Add(_str_nodes.ElementAt(i).Value.Number, i);
        //    }

        //    RobotResultQueryParams result_params = (RobotResultQueryParams)robot.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
        //    RobotStructure rstructure = robot.Project.Structure;
        //    RobotSelection FE_sel = rstructure.Selections.CreateFull(IRobotObjectType.I_OT_FINITE_ELEMENT);
        //    IRobotResultQueryReturnType query_return = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
        //    RobotSelection cas_sel = rstructure.Selections.Create(IRobotObjectType.I_OT_CASE);
        //    try { cas_sel.FromText(robot.Project.Structure.Cases.Get(1).Number.ToString()); } catch { }

        //    if (cas_sel.Count > 0) result_params.Selection.Set(IRobotObjectType.I_OT_CASE, cas_sel);
        //    result_params.Selection.Set(IRobotObjectType.I_OT_NODE, FE_sel); result_params.ResultIds.SetSize(5);
        //    result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
        //    result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
        //    result_params.SetParam(IRobotResultParamType.I_RPT_SMOOTHING, IRobotFeResultSmoothing.I_FRS_IN_ELEMENT_CENTER);
        //    result_params.ResultIds.SetSize(5);
        //    result_params.ResultIds.Set(1, 564);
        //    result_params.ResultIds.Set(2, 565);
        //    result_params.ResultIds.Set(3, 566);
        //    result_params.ResultIds.Set(4, 567);
        //    result_params.ResultIds.Set(5, 1252);

        //    RobotResultRowSet row_set = new RobotResultRowSet();
        //    bool ok = false;
        //    RobotResultRow result_row = default(RobotResultRow);

        //    List<int> _panel_ids = new List<int>();
        //    Dictionary<int, int[]> _vertex_indices = new Dictionary<int, int[]>();
        //    int kounta = 0;

        //    while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
        //    {
        //        query_return = rstructure.Results.Query(result_params, row_set);
        //        ok = row_set.MoveFirst();
        //        while (ok)
        //        {
        //            result_row = row_set.CurrentRow;
        //            int panel_num = (int)row_set.CurrentRow.GetValue(1252);
        //            _panel_ids.Add(panel_num);

        //            int number_of_indices = (row_set.CurrentRow.IsAvailable(567)) ? 4 : 3;
        //            int[] temp_indices = new int[number_of_indices];
        //            for (int i = 0; i < number_of_indices; i++)
        //            {
        //                temp_indices[i] = _nodeIds[(int)row_set.CurrentRow.GetValue(564 + i)];
        //            }

        //            _vertex_indices.Add(kounta, temp_indices);
        //            kounta++;
        //            ok = row_set.MoveNext();
        //        }
        //        row_set.Clear();
        //    }
        //    result_params.Reset();

        //    panel_ids = _panel_ids.ToArray();
        //    vertex_indices = _vertex_indices;
        //    coords = nodeCoords;
        //    str_nodes = _str_nodes;
        //    return true;
        //}

    }
}
