using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit
{
    public class Bar
    {
        public static bool GetBarsQuery(out Dictionary<int,BHoM.Structural.Bar> str_bars, string FilePath = "")
        {
            RobotApplication robot = new RobotApplication();
            if (FilePath != "")
            {
                robot.Project.Open(FilePath);
            }

            //Get Nodes
            Dictionary<int, BHoM.Structural.Node> str_nodes = new Dictionary<int, BHoM.Structural.Node>();
            RobotToolkit.Node.GetNodesQuery(out str_nodes, FilePath);

            RobotResultQueryParams result_params = default(RobotResultQueryParams);
            result_params = (RobotResultQueryParams)robot.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
            RobotStructure rstructure = default(RobotStructure);
            rstructure = robot.Project.Structure;
            RobotSelection cas_sel = default(RobotSelection);
            RobotSelection bar_sel = default(RobotSelection);
            IRobotResultQueryReturnType query_return = default(IRobotResultQueryReturnType);

            Dictionary<int, BHoM.Structural.Bar> _str_bars = new Dictionary<int, BHoM.Structural.Bar>();
            bool ok = false;
            RobotResultRow result_row = default(RobotResultRow);
            int bar_num = 0;

            double nod1 = 0;
            double nod2 = 0;

            int nod1_id = 15;
            int nod2_id = 16;

            bar_sel = rstructure.Selections.CreateFull(IRobotObjectType.I_OT_BAR);
            cas_sel = rstructure.Selections.Create(IRobotObjectType.I_OT_CASE);
            cas_sel.FromText(rstructure.Cases.Get(1).Number.ToString()); // arbitrary case, it just needs something

            result_params.ResultIds.SetSize(5);
            result_params.ResultIds.Set(1, nod1_id);
            result_params.ResultIds.Set(2, nod2_id);
            result_params.ResultIds.Set(3, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
            result_params.ResultIds.Set(4, 269); // I honestly don't know what these are, but they need to stay there in order for all the beams to come out #GG 20150219 
            result_params.ResultIds.Set(5, 270);


            result_params.SetParam(IRobotResultParamType.I_RPT_BAR_RELATIVE_POINT, 0);
            result_params.Selection.Set(IRobotObjectType.I_OT_BAR, bar_sel);
            result_params.Selection.Set(IRobotObjectType.I_OT_CASE, cas_sel);
            result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            RobotResultRowSet row_set = new RobotResultRowSet();

            while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
            {
                query_return = rstructure.Results.Query(result_params, row_set);
                ok = row_set.MoveFirst();
                while (ok)
                {
                    result_row = row_set.CurrentRow;
                    bar_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_BAR);

                    nod1 = (int)result_row.GetValue(nod1_id);
                    nod2 = (int)result_row.GetValue(nod2_id);

                    _str_bars.Add(bar_num, new BHoM.Structural.Bar(str_nodes[(int)nod1], str_nodes[(int)nod2], bar_num));

                    ok = row_set.MoveNext();
                }
                row_set.Clear();
            }
            result_params.Reset();

            str_bars = _str_bars;
            return false;
        }

        public static bool GetBars(out Dictionary<int, BHoM.Structural.Bar> str_bars, string FilePath = "")
        {
            RobotApplication robot = new RobotApplication();
            if (FilePath != "")
            {
                robot.Project.Open(FilePath);
            }

            Dictionary<int, BHoM.Structural.Node> str_nodes = new Dictionary<int, BHoM.Structural.Node>();
            RobotToolkit.Node.GetNodesQuery(out str_nodes, FilePath);
             
            RobotBarCollection collection = (RobotBarCollection)robot.Project.Structure.Bars.GetAll();
            str_bars = new Dictionary<int, BHoM.Structural.Bar>();

            robot.Project.Structure.Bars.BeginMultiOperation();

            for (int i = 0; i < collection.Count; i++)
            {
                RobotBar rbar = (RobotBar)collection.Get(i + 1);

                BHoM.Structural.Bar str_bar = new BHoM.Structural.Bar(str_nodes[rbar.StartNode], str_nodes[rbar.EndNode], rbar.Number);
                str_bar.OrientationAngle = rbar.Gamma;
                str_bar.SetSectionPropertyName(rbar.GetLabelName(IRobotLabelType.I_LT_BAR_SECTION));
                
                str_bars.Add(rbar.Number, str_bar);

            }

            robot.Project.Structure.Bars.EndMultiOperation();
           
            return true;
        }
   
        public static bool CreateBarsByCache(BHoM.Structural.Bar[] str_bars, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotStructureCache structureCache = robot.Project.Structure.CreateCache();

            Dictionary<int, object> node_dictionary = new Dictionary<int, object>();
            string[] avail_mem_type_names = RobotToolkit.Label.GetAllBarMemberTypeNames(robot);
            Dictionary<string, string> mem_types = new Dictionary<string, string>();

            for (int i = 0; i < str_bars.Length;i++)
            {
                BHoM.Structural.Bar bar = str_bars[i];
                BHoM.Structural.Node start_node = bar.GetStartNode();
                BHoM.Structural.Node end_node = bar.GetEndNode();

                if (!node_dictionary.ContainsKey(start_node.Number))
                {
                    node_dictionary.Add(start_node.Number, start_node);
                    structureCache.AddNode(start_node.Number, start_node.X, start_node.Y, start_node.Z);
                }
                if (!node_dictionary.ContainsKey(end_node.Number))
                {
                    node_dictionary.Add(end_node.Number, end_node);
                    structureCache.AddNode(end_node.Number, end_node.X, end_node.Y, end_node.Z);
                }

                structureCache.AddBar(bar.Number, bar.StartNodeNumber, bar.EndNodeNumber, "UB 305x165x40", "S355", 0);
                structureCache.SetBarLabel(bar.Number, IRobotLabelType.I_LT_BAR_SECTION, bar.SectionProperty.Name);
                if (avail_mem_type_names.Contains(bar.TypeName) == false) try { mem_types.Add(bar.TypeName, bar.TypeName); }
                    catch { }
                structureCache.SetBarLabel(bar.Number, IRobotLabelType.I_LT_MEMBER_TYPE, bar.TypeName);
            }
            for (int i = 0; i < mem_types.Count; i++)
            {
                IRobotLabel mem_type_label = robot.Project.Structure.Labels.CreateLike(IRobotLabelType.I_LT_MEMBER_TYPE, mem_types.ElementAt(i).Value, "Beam");
                robot.Project.Structure.Labels.Store(mem_type_label);
            }           

            RobotStructureApplyInfo applyInfo = robot.Project.Structure.ApplyCache(structureCache);
            
            return true;
        }

        public static bool CreateBars(BHoM.Structural.Bar[] str_bars, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();

            robot.Project.Structure.Bars.BeginMultiOperation();
            foreach (BHoM.Structural.Bar bar in str_bars)
            {
                robot.Project.Structure.Bars.Create(bar.Number, bar.StartNodeNumber, bar.EndNodeNumber);
            }
            robot.Project.Structure.Bars.EndMultiOperation();

        return true;
        }

        public static void DeleteBars(string selString, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotSelection sel = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            sel.AddText(selString);
        }
        
    }
}
