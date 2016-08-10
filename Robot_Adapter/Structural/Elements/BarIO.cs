using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoMB = BHoM.Base;
using BHoME = BHoM.Structural.Elements;
using BHoMP = BHoM.Structural.Properties;
using BHoMM = BHoM.Materials;
using Robot_Adapter.Utility;


namespace Robot_Adapter.Structural.Elements
{
    /// <summary>
    /// Robot bar class, for all bar objects and operations
    /// </summary>
    public class BarIO
    {       
        /// <summary>
        /// Gets Robot bars using the faster 'query' method. This does not return all Robot bar data
        /// as the only information returned is in double format. To get all bar data use 'GetBars' method.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool GetBarsQuery(RobotApplication robot, out BHoMB.ObjectManager<int, BHoME.Bar> bars, string barNumbers = "all")
        {
            //Get Nodes
            BHoMB.ObjectManager<int, BHoME.Node> nodes = null;
            Robot_Adapter.Structural.Elements.NodeIO.GetNodesQuery(robot, out nodes);
            bars = new BHoMB.ObjectManager<int, BHoME.Bar>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);

            RobotResultQueryParams result_params = default(RobotResultQueryParams);
            result_params = (RobotResultQueryParams)robot.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
            RobotStructure rstructure = default(RobotStructure);
            rstructure = robot.Project.Structure;
            RobotSelection cas_sel = default(RobotSelection);
            RobotSelection bar_sel = default(RobotSelection);
            IRobotResultQueryReturnType query_return = default(IRobotResultQueryReturnType);

            bool ok = false;
            RobotResultRow result_row = default(RobotResultRow);
            int bar_num = 0;

            int nod1 = 0;
            int nod2 = 0;

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
                    BHoME.Bar b = new BHoME.Bar(nodes[nod1], nodes[nod2]);
                    bars.Add(bar_num,b);

                    ok = row_set.MoveNext();
                }
                row_set.Clear();
            }
            result_params.Reset();

            return false;
        }
        
        /// <summary>
        /// Get bars method, gets bars from a Robot model and all associated data. Much slower than
        /// the get bars query as it uses the COM interface. 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="barNumbers"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool GetBars(RobotApplication robot, out List<BHoME.Bar> outputBars, string barNumbers = "all")
        {
            BHoMB.ObjectManager<int, BHoME.Bar> bars = new BHoMB.ObjectManager<int, BHoME.Bar>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);
            BHoMB.ObjectManager<BHoMP.SectionProperty> sections = new BHoMB.ObjectManager<BHoMP.SectionProperty>();
            BHoMB.ObjectManager<BHoMP.BarRelease> releases = new BHoMB.ObjectManager<BHoMP.BarRelease>();
            BHoMB.ObjectManager<BHoMP.BarConstraint> constraints = new BHoMB.ObjectManager<BHoMP.BarConstraint>();
            BHoMB.ObjectManager<BHoMM.Material> materials = new BHoMB.ObjectManager<BHoMM.Material>();

            RobotSelection barSelection = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            
            barSelection.FromText(barNumbers);

            IRobotCollection barServer = robot.Project.Structure.Bars.GetMany(barSelection) as IRobotCollection;
            RobotNodeServer nodeServer = robot.Project.Structure.Nodes as RobotNodeServer;
           
            robot.Project.Structure.Bars.BeginMultiOperation();

            BHoMB.ObjectManager<int, BHoME.Node> nodes = new BHoMB.ObjectManager<int, BHoME.Node>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);

            for (int i = 0; i < barServer.Count; i++)
            {
                RobotBar rbar = (RobotBar)barServer.Get(i + 1);

                BHoME.Node n1 = nodes[rbar.StartNode] as BHoME.Node;
                BHoME.Node n2 = nodes[rbar.EndNode] as BHoME.Node;
                if (n1 == null)
                {
                    RobotNode n = nodeServer.Get(rbar.StartNode) as RobotNode;
                    n1 = nodes.Add(n.Number, new BHoME.Node(n.X, n.Y, n.Z));
                }
                if (n2 == null)
                {
                    RobotNode n = nodeServer.Get(rbar.EndNode) as RobotNode;
                    n2 = nodes.Add(n.Number, new BHoME.Node(n.X, n.Y, n.Z));
                }
                BHoME.Bar str_bar = bars.Add(rbar.Number, new BHoME.Bar(n1, n2));
                str_bar.OrientationAngle = rbar.Gamma;
                if (rbar.HasLabel(IRobotLabelType.I_LT_BAR_SECTION) == -1)
                {
                    IRobotLabel rLabel = rbar.GetLabel(IRobotLabelType.I_LT_BAR_SECTION);

                    BHoMP.SectionProperty property = sections[rLabel.Name];
                    if (property == null)
                    {
                        property = sections.Add(rLabel.Name, PropertyIO.GetSection(rLabel));
                    }
                    str_bar.SectionProperty = property;
                }

                if (rbar.HasLabel(IRobotLabelType.I_LT_BAR_RELEASE) == -1)
                {
                    IRobotLabel rLabel = rbar.GetLabel(IRobotLabelType.I_LT_BAR_RELEASE);

                    BHoMP.BarRelease release = releases[rLabel.Name];
                    if (release == null)
                    {
                        release = releases.Add(rLabel.Name, GetRelease(rLabel));
                    }
                    str_bar.Release = release;
                }

                if (rbar.HasLabel(IRobotLabelType.I_LT_BAR_ELASTIC_GROUND) == -1)
                {
                    IRobotLabel rLabel = rbar.GetLabel(IRobotLabelType.I_LT_BAR_ELASTIC_GROUND);

                    BHoMP.BarConstraint spring = constraints[rLabel.Name];
                    if (spring == null)
                    {
                        spring = constraints.Add(rLabel.Name, GetBarSpring(rLabel));
                    }
                    str_bar.Spring = spring;
                }
                if (rbar.HasLabel(IRobotLabelType.I_LT_MATERIAL) != 0)
                {
                    IRobotLabel material = rbar.GetLabel(IRobotLabelType.I_LT_MATERIAL);
                    BHoMM.Material m = materials[material.Name];
                    if (material == null)
                    {
                        m = materials.Add(material.Name, PropertyIO.GetMaterial(material));
                    }
                    str_bar.Material = m;
                }

                #region Section data
                //IRobotLabel sec_label = rbar.GetLabel(IRobotLabelType.I_LT_BAR_SECTION);                
                //IRobotBarSectionData sec_data = sec_label.Data;

                //var values = IRobotBarSectionDataValue.GetValues(typeof(IRobotBarSectionDataValue));
                //userInfo.Add("ShapeType", sec_data.ShapeType.ToString());
                //userInfo.Add("c", sec_data.NonstdCount.ToString());

                //foreach (var val in values)
                //{
                //    {
                //        userInfo.Add(val.ToString(), sec_data.GetValue((IRobotBarSectionDataValue)val));
                //    }
                //}

                //if (sec_data.NonstdCount != 0)
                //{
                //    IRobotBarSectionNonstdData sec_nonstd = sec_data.GetNonstd(1);
                //    var nonstdValues = IRobotBarSectionNonstdDataValue.GetValues(typeof(IRobotBarSectionNonstdDataValue));

                //    foreach (var val in nonstdValues)
                //    {
                //        try
                //        {
                //            userInfo.Add(val.ToString(), sec_nonstd.GetValue((IRobotBarSectionNonstdDataValue)val));
                //        }
                //        catch { }
                //    }
                //}

                //if (sec_data.IsSpecial)
                //{
                //    IRobotBarSectionSpecialData spec_data = sec_data.Special;
                //    var specialvalues = IRobotBarSectionSpecialDataValue.GetValues(typeof(IRobotBarSectionSpecialDataValue));

                //    foreach (var val in specialvalues)
                //    {
                //        try
                //        {
                //            userInfo.Add(val.ToString(), spec_data.GetValue((IRobotBarSectionSpecialDataValue)val));
                //        }
                //        catch { }
                //    }
                //}


                //try
                //{
                //    userInfo.Add("member1", sec_data.Members.GetValue(IRobotBarSectionComponentShape.I_BSCS_C,IRobotBarSectionDataValue.I_BSDV_DIM1));                    
                //}
                //catch { }

                //try
                //{
                //    userInfo.Add("member2", sec_data.Members.GetValue(IRobotBarSectionComponentShape.I_BSCS_I, IRobotBarSectionDataValue.I_BSDV_DIM1));
                //}
                //catch { }

                //try
                //{
                //    userInfo.Add("member3", sec_data.Members.GetValue(IRobotBarSectionComponentShape.I_BSCS_L, IRobotBarSectionDataValue.I_BSDV_DIM1));
                //}
                //catch { }

                //try
                //{
                //    userInfo.Add("member4", sec_data.Members.GetValue(IRobotBarSectionComponentShape.I_BSCS_UNDEFINED, IRobotBarSectionDataValue.I_BSDV_DIM1));
                //}
                //catch { }
                #endregion
            }

            robot.Project.Structure.Bars.EndMultiOperation();

            outputBars = bars.ToList();

            return true;
        }
   
        /// <summary>
        /// Creates bars using the fast cache method. 
        /// </summary>
        /// <param name="str_bars"></param>
        /// <returns></returns>
        public static bool CreateBarsByCache(RobotApplication robot, List<BHoME.Bar> str_bars)
        {
            RobotStructureCache structureCache = robot.Project.Structure.CreateCache();
            
            string[] avail_mem_type_names = Robot_Adapter.Utility.Label.GetAllBarMemberTypeNames(robot);

            BHoMB.ObjectManager<int, BHoME.Node> nodes = new BHoMB.ObjectManager<int, BHoME.Node>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);

            Dictionary<string, bool> added_Nodes = new Dictionary<string, bool>();

            RobotNamesArray sec_names = robot.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_BAR_SECTION);
            string defaultSectionName = sec_names.Get(1).ToString();

            RobotNamesArray mat_names = robot.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_MATERIAL);
            string defaultMaterialName = mat_names.Get(1).ToString();
            object checkValue = false;

            int nodeNum = robot.Project.Structure.Nodes.FreeNumber;
            int barNum = robot.Project.Structure.Bars.FreeNumber;

            string key = Utils.NUM_KEY;
            for (int i = 0; i < str_bars.Count;i++)
            {
                BHoME.Bar bar = str_bars[i];
                BHoME.Node start_node = bar.StartNode;
                BHoME.Node end_node = bar.EndNode;

                if (!start_node.CustomData.TryGetValue(key, out checkValue))
                {
                    start_node.CustomData.Add(key, nodeNum);
                    structureCache.AddNode(nodeNum++, start_node.X, start_node.Y, start_node.Z);
                }
                if (!end_node.CustomData.TryGetValue(key, out checkValue))
                {
                    end_node.CustomData.Add(key, nodeNum);
                    structureCache.AddNode(nodeNum++, end_node.X, end_node.Y, end_node.Z);
                }
                bar.CustomData.Add(key, barNum);
                structureCache.AddBar(barNum++, (int)start_node.CustomData[key], (int)end_node.CustomData[key], defaultSectionName, defaultMaterialName, 0);

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
        public static bool CreateBars(RobotApplication robot, List<BHoME.Bar> str_bars, out List<string> ids)
        {
            string key = Utils.NUM_KEY;
            robot.Project.Structure.Bars.BeginMultiOperation();
            Dictionary<string, string> addedSections = new Dictionary<string, string>();
            Dictionary<string, string> addedReleases = new Dictionary<string, string>();
            Dictionary<string, string> addedSprings = new Dictionary<string, string>();
            Dictionary<string, string> addedMaterials = new Dictionary<string, string>();
            RobotBarServer barServer = robot.Project.Structure.Bars;
            RobotBar robotBar = null;
            List<string> nodeIds = new List<string>();
            ids = new List<string>();
            foreach (BHoME.Bar bar in str_bars)
            {
                int barNum =  barServer.FreeNumber;
                int nodeNum1 = 0;
                int nodeNum2 = 0;
                object number = bar[key];

                if (number != null)
                {
                    int.TryParse(number.ToString(), out barNum);
                }
                else
                {
                    bar.CustomData.Add(key, barNum);
                }

                NodeIO.CreateNodes(robot, new List<BHoME.Node>() { bar.StartNode, bar.EndNode }, out nodeIds);
             
                nodeNum1 = int.Parse(bar.StartNode[key].ToString());           
                nodeNum2 = int.Parse(bar.EndNode[key].ToString());
                             
                if (barServer.Exist(barNum) == -1)
                {
                    robotBar = barServer.Get(barNum) as RobotBar;
                    robotBar.StartNode = nodeNum1;
                    robotBar.EndNode = nodeNum2;
                }
                else
                {
                    robot.Project.Structure.Bars.Create(barNum, nodeNum1, nodeNum2);
                    robotBar = barServer.Get(barNum) as RobotBar;
                }

                robotBar.Gamma = bar.OrientationAngle;

                string currentSection = "";
                if (bar.SectionProperty != null && !addedSections.TryGetValue(bar.SectionProperty.Name, out currentSection))
                {
                    PropertyIO.CreateBarProperty(robot, bar.SectionProperty);
                    currentSection = bar.SectionProperty.Name;
                    addedSections.Add(currentSection, currentSection);
                }

                string currentRelease = "";
                if (bar.Release != null && !addedReleases.TryGetValue(bar.Release.Name, out currentRelease))
                {
                    CreateRelease(robot, bar.Release);
                    currentRelease = bar.SectionProperty.Name;
                    addedReleases.Add(currentRelease, currentRelease);
                }

                string elasticGround = "";
                if (bar.Spring != null && !addedSprings.TryGetValue(bar.Spring.Name, out elasticGround))
                {
                    CreateBarSpring(robot, bar.Spring);
                    elasticGround = bar.Spring.Name;
                    addedSprings.Add(elasticGround, elasticGround);
                }

                string material = "";
                if (bar.Material != null && !addedMaterials.TryGetValue(bar.Material.Name, out material))
                {
                    PropertyIO.CreateMaterial(robot, bar.Material);
                    material = bar.Material.Name;
                    addedMaterials.Add(material, material);
                }

                robotBar.SetLabel(IRobotLabelType.I_LT_BAR_SECTION, currentSection);
                robotBar.SetLabel(IRobotLabelType.I_LT_BAR_RELEASE, currentRelease);
                robotBar.SetLabel(IRobotLabelType.I_LT_BAR_ELASTIC_GROUND, elasticGround);
                robotBar.SetLabel(IRobotLabelType.I_LT_MATERIAL, material);
                ids.Add(barNum.ToString());
            }
            robot.Project.Structure.Bars.EndMultiOperation();

            return true;
        }

        private static void CreateBarSpring(RobotApplication robot, BHoMP.BarConstraint spring)
        {
            IRobotLabelServer labels = robot.Project.Structure.Labels;
            if (labels.Exist(IRobotLabelType.I_LT_BAR_ELASTIC_GROUND, spring.Name) == 0)
            {
                IRobotLabel barLabel = labels.Create(IRobotLabelType.I_LT_BAR_ELASTIC_GROUND, spring.Name);
                IRobotBarElasticGroundData groundData = barLabel.Data;
                groundData.HX = spring.Rotational.Value;
                groundData.KY = spring.Horizontal.Value;
                groundData.KZ = spring.Vertical.Value;

                if (spring.Horizontal.Type == BHoMP.DOFType.SpringNegative)
                {
                    groundData.SetOneDir(IRobotUpliftDirection.I_UD_UY, IRobotUpliftSense.I_US_MINUS);
                }
                else if (spring.Horizontal.Type == BHoMP.DOFType.SpringPositive)
                {
                    groundData.SetOneDir(IRobotUpliftDirection.I_UD_UY, IRobotUpliftSense.I_US_PLUS);
                }
                if (spring.Vertical.Type == BHoMP.DOFType.SpringNegative)
                {
                    groundData.SetOneDir(IRobotUpliftDirection.I_UD_UZ, IRobotUpliftSense.I_US_MINUS);
                }
                else if (spring.Vertical.Type == BHoMP.DOFType.SpringPositive)
                {
                    groundData.SetOneDir(IRobotUpliftDirection.I_UD_UZ, IRobotUpliftSense.I_US_PLUS);
                }

                labels.Store(barLabel);
            }
        }

        private static BHoMP.BarConstraint GetBarSpring(IRobotLabel barLabel)
        {
            IRobotBarElasticGroundData groundData = barLabel.Data;
            BHoMP.BarConstraint barSpring = new BHoMP.BarConstraint(barLabel.Name);

            IRobotUpliftSense upliftH = groundData.GetOneDir(IRobotUpliftDirection.I_UD_UY);
            IRobotUpliftSense upliftV = groundData.GetOneDir(IRobotUpliftDirection.I_UD_UZ);

            BHoMP.DOFType horizontalType = upliftH == IRobotUpliftSense.I_US_MINUS ? BHoMP.DOFType.SpringNegative : upliftH == IRobotUpliftSense.I_US_PLUS ? BHoMP.DOFType.SpringPositive : BHoMP.DOFType.Spring;
            BHoMP.DOFType verticalType = upliftV == IRobotUpliftSense.I_US_MINUS ? BHoMP.DOFType.SpringNegative : upliftH == IRobotUpliftSense.I_US_PLUS ? BHoMP.DOFType.SpringPositive : BHoMP.DOFType.Spring;

            barSpring.Rotational = new BHoMP.DOF(BHoMP.DOFType.Spring, groundData.HX);
            barSpring.Horizontal = new BHoMP.DOF(horizontalType, groundData.KY);
            barSpring.Vertical = new BHoMP.DOF(verticalType, groundData.KZ);

            return barSpring;
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

        public static void CreateRelease(RobotApplication robot, BHoMP.BarRelease barRelease)
        {
            IRobotLabelServer labels = robot.Project.Structure.Labels;
            if (labels.Exist(IRobotLabelType.I_LT_BAR_RELEASE, barRelease.Name) == 0)
            {
                IRobotLabel barLabel = labels.Create(IRobotLabelType.I_LT_BAR_RELEASE, barRelease.Name);
                IRobotBarReleaseData data = barLabel.Data;
                CreateEndReleaseData(data.StartNode, barRelease.StartConstraint);
                CreateEndReleaseData(data.EndNode, barRelease.EndConstraint);
                labels.Store(barLabel);
            }
        }
        
        public static void CreateEndReleaseData(IRobotBarEndReleaseData robotData, BHoMP.NodeConstraint bhomData)
        {
            robotData.UX = GetReleaseType(bhomData.UX.Type);
            robotData.UY = GetReleaseType(bhomData.UY.Type);
            robotData.UZ = GetReleaseType(bhomData.UZ.Type);
            robotData.RX = GetReleaseType(bhomData.RX.Type);
            robotData.RY = GetReleaseType(bhomData.RY.Type);
            robotData.RZ = GetReleaseType(bhomData.RZ.Type);

            robotData.KX = bhomData.UX.Value;
            robotData.KY = bhomData.UY.Value;
            robotData.KZ = bhomData.UZ.Value;
            robotData.HX = bhomData.RX.Value;
            robotData.HY = bhomData.RY.Value;
            robotData.HZ = bhomData.RZ.Value;
        }


        public static BHoMP.BarRelease GetRelease(IRobotLabel release)
        {
            //if (robotBar.HasLabel(IRobotLabelType.I_LT_BAR_RELEASE) == 0)
            //{
            //    IRobotLabel release = robotBar.GetLabel(IRobotLabelType.I_LT_BAR_RELEASE);
                IRobotBarReleaseData releaseData = release.Data as IRobotBarReleaseData;
                BHoMP.NodeConstraint startNode = GetEndRelease(releaseData.StartNode);
                BHoMP.NodeConstraint endNode = GetEndRelease(releaseData.EndNode);
                return new BHoMP.BarRelease(startNode, endNode, release.Name);
            //}
            //return null;
        }

        /// <summary>
        /// Get the degree of freedom type from the robot end release value
        /// </summary>
        /// <param name="endRelease"></param>
        /// <returns></returns>
        public static BHoMP.DOFType GetReleaseType(IRobotBarEndReleaseValue endRelease)
        {
            switch (endRelease)
            {
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC:
                    return BHoMP.DOFType.Spring;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_MINUS:
                    return BHoMP.DOFType.SpringNegative;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_PLUS:
                    return BHoMP.DOFType.SpringPositive;
                case IRobotBarEndReleaseValue.I_BERV_NONE:
                    return BHoMP.DOFType.Free;
                case IRobotBarEndReleaseValue.I_BERV_MINUS:
                    return BHoMP.DOFType.FixedNegative;
                case IRobotBarEndReleaseValue.I_BERV_PLUS:
                    return BHoMP.DOFType.FixedPositive;
                case IRobotBarEndReleaseValue.I_BERV_STD:
                    return BHoMP.DOFType.Fixed;
                case IRobotBarEndReleaseValue.I_BERV_NONLINEAR:
                    return BHoMP.DOFType.NonLinear;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED:
                    return BHoMP.DOFType.SpringRelative;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_MINUS:
                    return BHoMP.DOFType.SpringRelativeNegative;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_PLUS:
                    return BHoMP.DOFType.SpringRelativePositive;
                default:
                    return BHoMP.DOFType.Free;
            }
        }

        /// <summary>
        /// Get the  robot end release value from the degree of freedom type
        /// </summary>
        /// <param name="endRelease"></param>
        /// <returns></returns>
        public static IRobotBarEndReleaseValue GetReleaseType(BHoMP.DOFType endRelease)
        {
            switch (endRelease)
            {
                case BHoMP.DOFType.Spring:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC;
                case BHoMP.DOFType.SpringNegative:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_MINUS;
                case BHoMP.DOFType.SpringPositive:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_PLUS;
                case BHoMP.DOFType.Free:
                    return IRobotBarEndReleaseValue.I_BERV_NONE;
                case BHoMP.DOFType.FixedNegative:
                    return IRobotBarEndReleaseValue.I_BERV_MINUS;
                case BHoMP.DOFType.FixedPositive:
                    return IRobotBarEndReleaseValue.I_BERV_PLUS;
                case BHoMP.DOFType.Fixed:
                    return IRobotBarEndReleaseValue.I_BERV_STD;
                case BHoMP.DOFType.NonLinear:
                    return IRobotBarEndReleaseValue.I_BERV_NONLINEAR;
                case BHoMP.DOFType.SpringRelative:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED;
                case BHoMP.DOFType.SpringRelativeNegative:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_MINUS;
                case BHoMP.DOFType.SpringRelativePositive:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_PLUS;
                default:
                    return IRobotBarEndReleaseValue.I_BERV_NONE;
            }
        }

        /// <summary>
        /// Gets the end release from the robot end release data
        /// </summary>
        /// <param name="nodeData"></param>
        /// <returns></returns>
        public static BHoMP.NodeConstraint GetEndRelease(IRobotBarEndReleaseData nodeData)
        {
            BHoMP.NodeConstraint constraint = new BHoMP.NodeConstraint();
            constraint.UX = new BHoMP.DOF(GetReleaseType(nodeData.UX), nodeData.KX);
            constraint.UY = new BHoMP.DOF(GetReleaseType(nodeData.UY), nodeData.KY);
            constraint.UZ = new BHoMP.DOF(GetReleaseType(nodeData.UZ), nodeData.KZ);
            constraint.RX = new BHoMP.DOF(GetReleaseType(nodeData.RX), nodeData.HX);
            constraint.RY = new BHoMP.DOF(GetReleaseType(nodeData.RY), nodeData.HY);
            constraint.RZ = new BHoMP.DOF(GetReleaseType(nodeData.RZ), nodeData.HZ);

            return constraint;            
        }
    }
}
