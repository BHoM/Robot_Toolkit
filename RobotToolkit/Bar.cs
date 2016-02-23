using System.Collections.Generic;
using System.Linq;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit
{
    /// <summary>
    /// Robot bar class, for all bar objects and operations
    /// </summary>
    public class Bar 
    {
        /// <summary>
        /// Gets Robot bars using the faster 'query' method. This does not return all Robot bar data
        /// as the only information returned is in double format. To get all bar data use 'GetBars' method.
        /// </summary>
        /// <param name="str_bars"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool GetBarsQuery(out Dictionary<int,BHoM.Structural.Bar> str_bars, string FilePath = "")
        {
       
            RobotApplication robot = new RobotApplication();
            if (FilePath != "")
            {
                robot.Visible = 0;
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
            if (FilePath != "") { robot.Project.Close(); }
            return false;
        }

        /// <summary>
        /// Get bars method, gets bars from a Robot model and all associated data. Much slower than
        /// the get bars query as it uses the COM interface. 
        /// </summary>
        /// <param name="str_bars"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool GetBars(out Dictionary<int, BHoM.Structural.Bar> str_bars, string FilePath = "")
        {
            RobotApplication robot = new RobotApplication();
            if (FilePath != "")
            {
                robot.Visible = 0;
                robot.Project.Open(FilePath);
            }

            Dictionary<int, BHoM.Structural.Node> str_nodes = new Dictionary<int, BHoM.Structural.Node>();
            RobotToolkit.Node.GetNodes(out str_nodes, FilePath);
             
            RobotBarCollection collection = (RobotBarCollection)robot.Project.Structure.Bars.GetAll();
            str_bars = new Dictionary<int, BHoM.Structural.Bar>();

            robot.Project.Structure.Bars.BeginMultiOperation();

            for (int i = 0; i < collection.Count; i++)
            {
                RobotBar rbar = (RobotBar)collection.Get(i + 1);

                BHoM.Structural.Bar str_bar = new BHoM.Structural.Bar(str_nodes[rbar.StartNode], str_nodes[rbar.EndNode], rbar.Number);
                str_bar.OrientationAngle = rbar.Gamma;
                IRobotLabel sec_label = rbar.GetLabel(IRobotLabelType.I_LT_BAR_SECTION);
                BHoM.Collections.Dictionary<string, object> userInfo = new BHoM.Collections.Dictionary<string, object>();
                
                IRobotBarSectionData sec_data = sec_label.Data;

                var values = IRobotBarSectionDataValue.GetValues(typeof(IRobotBarSectionDataValue));
                userInfo.Add("ShapeType", sec_data.ShapeType.ToString());
                userInfo.Add("c", sec_data.NonstdCount.ToString());

                foreach (var val in values)
                {
                    {
                        userInfo.Add(val.ToString(), sec_data.GetValue((IRobotBarSectionDataValue)val));
                    }
                }

                
                if (sec_data.NonstdCount != 0)
                {
                    IRobotBarSectionNonstdData sec_nonstd = sec_data.GetNonstd(1);
                    var nonstdValues = IRobotBarSectionNonstdDataValue.GetValues(typeof(IRobotBarSectionNonstdDataValue));

                    foreach (var val in nonstdValues)
                    {
                        try
                        {
                            userInfo.Add(val.ToString(), sec_nonstd.GetValue((IRobotBarSectionNonstdDataValue)val));
                        }
                        catch { }
                    }
                }
  
                    str_bar.SetSectionProperty(SectionProperties.SectionProperty.Get(sec_label));

                str_bar.UserData = userInfo;
                str_bars.Add(rbar.Number, str_bar);
            }

            robot.Project.Structure.Bars.EndMultiOperation();
            if (FilePath != "")  { robot.Project.Close(); }

            return true;
        }
   
        /// <summary>
        /// Creates bars using the fast cache method. 
        /// </summary>
        /// <param name="str_bars"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CreateBarsByCache(BHoM.Structural.Bar[] str_bars, string FilePath = "LiveLink")
        {
            RobotApplication robot = new RobotApplication();
            RobotStructureCache structureCache = robot.Project.Structure.CreateCache();

            Dictionary<int, object> node_dictionary = new Dictionary<int, object>();
            string[] avail_mem_type_names = RobotToolkit.Label.GetAllBarMemberTypeNames(robot);
            Dictionary<string, string> mem_types = new Dictionary<string, string>();

            RobotNamesArray sec_names = robot.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_BAR_SECTION);
            string defaultSectionName = sec_names.Get(1).ToString();

            RobotNamesArray mat_names = robot.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_MATERIAL);
            string defaultMaterialName = mat_names.Get(1).ToString();
            
            for (int i = 0; i < str_bars.Length;i++)
            {
                BHoM.Structural.Bar bar = str_bars[i];
                BHoM.Structural.Node start_node = bar.StartNode;
                BHoM.Structural.Node end_node = bar.EndNode;

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
                

                structureCache.AddBar(bar.Number, bar.StartNode.Number, bar.EndNode.Number, defaultSectionName, defaultMaterialName, 0);
                //if (bar.SectionProperty.Name != "")
                //{
                //    structureCache.SetBarLabel(bar.Number, IRobotLabelType.I_LT_BAR_SECTION, bar.SectionProperty.Name);
                //}
                //if (avail_mem_type_names.Contains(bar.DesignGroupName) == false) try { mem_types.Add(bar.DesignGroupName, bar.DesignGroupName); }
                //    catch { }
                //if (bar.DesignGroupName != "")
                //{
                //    structureCache.SetBarLabel(bar.Number, IRobotLabelType.I_LT_MEMBER_TYPE, bar.DesignGroupName);
                //}
            }
            //for (int i = 0; i < mem_types.Count; i++)
            //{
            //    IRobotLabel mem_type_label = robot.Project.Structure.Labels.CreateLike(IRobotLabelType.I_LT_MEMBER_TYPE, mem_types.ElementAt(i).Value, "Beam");
            //    robot.Project.Structure.Labels.Store(mem_type_label);
            //}           

           RobotStructureApplyInfo applyInfo = robot.Project.Structure.ApplyCache(structureCache);
           return true;
        }

        /// <summary>
        /// Creates bars using the slower COM interface. More control over the bar and node 
        /// numbering is possible using this method as bars are created one by one.
        /// </summary>
        /// <param name="str_bars"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CreateBars(BHoM.Structural.Bar[] str_bars, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();

            robot.Project.Structure.Bars.BeginMultiOperation();
            foreach (BHoM.Structural.Bar bar in str_bars)
            {
                robot.Project.Structure.Bars.Create(bar.Number, bar.StartNode.Number, bar.EndNode.Number);
            }
            robot.Project.Structure.Bars.EndMultiOperation();

        return true;
        }

        /// <summary>
        /// Deletes bars from a Robot model using a selection string. The selection string is similar
        /// in format to a Robot object selection string (can be a list of numbers as a string, or include 'to' words).
        /// </summary>
        /// <param name="selString"></param>
        /// <param name="FilePath"></param>
        public static void DeleteBars(string selString, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotSelection sel = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            sel.AddText(selString);
        }
        
    }
}
