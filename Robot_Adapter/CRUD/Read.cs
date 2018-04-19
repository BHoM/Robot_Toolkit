using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.Engine.Serialiser;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Base;
using BH.oM.Common.Materials;
using BH.oM.Structural.Design;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {         
        
        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> Read(Type type, IList indices = null)
        {
            if (type == typeof(Node))
                return ReadNodes();
            if (type == typeof(Bar))
                return ReadBars();
            if (type == typeof(Constraint6DOF))
                return ReadConstraints6DOF();
            if (type == typeof(Material))
                return ReadMaterial();
            if (type == typeof(PanelPlanar))
                return new List<PanelPlanar>();
            if (type == typeof(MeshFace))
                return new List<MeshFace>();
            if (type == typeof(IProperty2D))
                return new List<IProperty2D>();
            if (type == typeof(RigidLink))
                return new List<RigidLink>();
            if (type == typeof(LoadCombination))
                return new List<LoadCombination>();
            if (type == typeof(LinkConstraint))
                return new List<LinkConstraint>();
            if (type == typeof(BarRelease))
                return ReadBarRelease();
            if (type == typeof(Loadcase))
                return ReadLoadCase();
            if (typeof(ISectionProperty).IsAssignableFrom(type))
                return ReadSectionProperties();
            else if (type == typeof(ILoad) || type.GetInterfaces().Contains(typeof(ILoad)))
                return new List<ILoad>(); //TODO: Implement load extraction
            if (type.IsGenericType && type.Name == typeof(BHoMGroup<IBHoMObject>).Name)
                return new List<BHoMGroup<IBHoMObject>>();
            //if (type == typeof(Node))
            //   return  (this.UseNodeQueryMethod)? ReadNodesQuery() : ReadNodes();
            //if (type == typeof(Bar))
            //    return (this.UseBarQueryMethod) ? ReadBarsQuery() : ReadBars();
            //if (type == typeof(DesignGroup))
            //    return ReadDesignGroups();
            //else
            return null;         
        }

        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        public List<Bar> ReadBars(List<string> ids = null)
        {
            IRobotCollection robotBars = m_RobotApplication.Project.Structure.Bars.GetAll();

            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.CustomData[AdapterId].ToString());
            Dictionary<string, BarRelease> bhombarReleases = ReadBarRelease().ToDictionary(x => x.Name.ToString());
            Dictionary<string, ISectionProperty> bhomSections = ReadSectionProperties().ToDictionary(x => x.Name.ToString());
            Dictionary<int, HashSet<string>> barTags = GetTypeTags(typeof(Bar));
            HashSet<string> tags = new HashSet<string>();

            m_RobotApplication.Project.Structure.Bars.BeginMultiOperation();
            if (ids == null)
            {
                for (int i = 1; i <= robotBars.Count; i++)
                {
                    RobotBar robotBar = robotBars.Get(i);
                    Bar bhomBar = BH.Engine.Robot.Convert.ToBHoMObject(robotBar, bhomNodes, bhomSections, bhombarReleases);
                    bhomBar.CustomData[AdapterId] = robotBar.Number;
                    if (barTags != null && !barTags.TryGetValue(robotBar.Number, out tags))
                        bhomBar.Tags = tags;
                    bhomBars.Add(bhomBar);
                }
            }
            else if (ids != null && ids.Count > 0)
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    RobotBar robotBar = m_RobotApplication.Project.Structure.Bars.Get(System.Convert.ToInt32(ids[i])) as RobotBar;
                    Bar bhomBar = BH.Engine.Robot.Convert.ToBHoMObject(robotBar, bhomNodes, bhomSections, bhombarReleases);
                    bhomBar.CustomData[AdapterId] = robotBar.Number;
                    if (barTags != null && !barTags.TryGetValue(robotBar.Number, out tags))
                        bhomBar.Tags = tags;
                    bhomBars.Add(bhomBar);
                }
            }
            m_RobotApplication.Project.Structure.Bars.EndMultiOperation();

            return bhomBars;
        }

        /***************************************************/

        public List<Node> ReadNodes(List<string> ids = null)
        {
            IRobotCollection robotNodes = m_RobotApplication.Project.Structure.Nodes.GetAll();
            List<Node> bhomNodes = new List<Node>();
            List<Constraint6DOF> constraints = ReadConstraints6DOF();
            Dictionary<int, HashSet<string>> nodeTags = GetTypeTags(typeof(Node));
            HashSet<string> tags = new HashSet<string>();
            if (ids == null)
            {
                for (int i = 1; i <= robotNodes.Count; i++)
                {
                    RobotNode robotNode = robotNodes.Get(i);
                    Node bhomNode = BH.Engine.Robot.Convert.ToBHoMObject(robotNode);
                    bhomNode.CustomData[AdapterId] = robotNode.Number;
                    if (nodeTags != null && !nodeTags.TryGetValue(robotNode.Number, out tags))
                        bhomNode.Tags = tags;

                    bhomNodes.Add(bhomNode);
                }
            }
            else if (ids != null && ids.Count > 0)
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    RobotNode robotNode = m_RobotApplication.Project.Structure.Nodes.Get(System.Convert.ToInt32(ids[i])) as RobotNode;
                    Node bhomNode = BH.Engine.Robot.Convert.ToBHoMObject(robotNode);
                    bhomNode.CustomData[AdapterId] = robotNode.Number;
                    if (nodeTags != null && !nodeTags.TryGetValue(robotNode.Number, out tags))
                        bhomNode.Tags = tags;

                    bhomNodes.Add(bhomNode);
                }
            }
            return bhomNodes;
        }

        /***************************************************/

        public List<Constraint6DOF> ReadConstraints6DOF(List<string> ids = null)
        {
            IRobotCollection robSupport = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_SUPPORT);
            List<Constraint6DOF> constList = new List<Constraint6DOF>();

            for (int i = 1; i <= robSupport.Count; i++)
            {
                RobotNodeSupport rNodeSupp = robSupport.Get(i);
                Constraint6DOF bhomSupp = BH.Engine.Robot.Convert.ToBHoMObject(rNodeSupp);
                bhomSupp.CustomData.Add(AdapterId, rNodeSupp.Name);
                //if (m_SupportTaggs != null)
                //    bhomSupp.ApplyTaggedName(m_SupportTaggs[rNodeSupp.Name]);
                constList.Add(bhomSupp);
            }
            return constList;
        }

        /***************************************************/

        public List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            IRobotCollection secProps = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_SECTION);
            List<ISectionProperty> bhomSectionProps = new List<ISectionProperty>();
            Dictionary<string, Material> materials = ReadMaterial().ToDictionary(x => x.Name);
            //int counter = 1;
            for (int i = 1; i <= secProps.Count; i++)
            {
                IRobotLabel rSection = secProps.Get(i);
                IRobotBarSectionData secData = rSection.Data as IRobotBarSectionData;
                int a = 1;
                if (rSection.Name == "CR1")
                    a = 1;

                if (materials.ContainsKey(secData.MaterialName))
                {
                    //IRobotLabel rLable = m_RobotApplication.Project.Structure.Labels.Get(IRobotLabelType.I_LT_MATERIAL, secData.MaterialName);
                    ISectionProperty bhomSec = BH.Engine.Robot.Convert.IBHoMSection(secData, materials[secData.MaterialName]);
                    if(bhomSec != null)
                    {
                        bhomSec.Material = materials[secData.MaterialName];
                        bhomSec.Name = rSection.Name;
                        bhomSec.CustomData.Add(AdapterId, rSection.Name);
                        //if (m_SectionPropertyTaggs != null)
                        //    bhomSec.ApplyTaggedName(m_SectionPropertyTaggs[rSection.Name]);
                        bhomSectionProps.Add(bhomSec);
                        //counter++;
                    }
                }
            }
            return bhomSectionProps;
        }

        /***************************************************/

        public List<Material> ReadMaterial(List<string> ids = null)
        {
            IRobotCollection rMaterials = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_MATERIAL);
            List<Material> bhomMaterials = new List<Material>();

            for (int i = 1; i <= rMaterials.Count; i++)
            {
                IRobotLabel rMatLable = rMaterials.Get(i);
                IRobotMaterialData mData = rMatLable.Data as IRobotMaterialData;
                MaterialType bhomMatType = BH.Engine.Robot.Convert.GetMaterialType(mData.Type);
                Material bhomMat = BH.Engine.Common.Create.Material(mData.Name, bhomMatType, mData.E, mData.NU, mData.LX, mData.RO);
                bhomMat.CustomData.Add(AdapterId, mData.Name);
                //if (m_MaterialTaggs != null)
                //    bhomMat.ApplyTaggedName(m_MaterialTaggs[mData.Name]);
                bhomMaterials.Add(bhomMat);
            }
            return bhomMaterials;
        }

        /***************************************************/

        public List<BarRelease> ReadBarRelease(List<string> ids = null)
        {
            IRobotCollection releaseCollection = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_RELEASE);
            List<BarRelease> bhomReleases = new List<BarRelease>();

            for (int i = 1; i <= releaseCollection.Count; i++)
            {
                IRobotLabel rReleaseLabel = releaseCollection.Get(i);
                IRobotBarReleaseData rReleaseData = rReleaseLabel.Data as IRobotBarReleaseData;
                BarRelease bhomMBarRelease = new BarRelease
                {
                    Name = rReleaseLabel.Name,
                    StartRelease = BH.Engine.Robot.Convert.BHoMRelease(rReleaseData.StartNode),
                    EndRelease = BH.Engine.Robot.Convert.BHoMRelease(rReleaseData.EndNode)
                };
                bhomMBarRelease.CustomData.Add(AdapterId, rReleaseLabel.Name);
                bhomReleases.Add(bhomMBarRelease);
            }
            return bhomReleases;
        }

        /***************************************************/

        public List<Loadcase> ReadLoadCase(List<string> ids = null)
        {
            RobotCaseCollection rLoadCases = m_RobotApplication.Project.Structure.Cases.GetAll();
            List<Loadcase> bhomLoadCases = new List<Loadcase>();

            for (int i = 1; i <= rLoadCases.Count; i++)
            {
                IRobotCase rLoadCase = rLoadCases.Get(i) as IRobotCase;
                Loadcase lCase = BH.Engine.Structure.Create.Loadcase(rLoadCase.Name, rLoadCase.Number, BH.Engine.Robot.Convert.BHoMLoadNature(rLoadCase.Nature));
                lCase.CustomData[AdapterId] = rLoadCase.Number;
                bhomLoadCases.Add(lCase);
            }
            return bhomLoadCases;
        }

        ///***************************************************/

        //public List<DesignGroup> ReadDesignGroups()
        //{
        //    RobotApplication robot = this.RobotApplication;
        //    RDimServer RDServer = this.RobotApplication.Kernel.GetExtension("RDimServer");
        //    RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
        //    RDimStream RDStream = RDServer.Connection.GetStream();
        //    RDimGroups RDGroups = RDServer.GroupsService;
        //    RDimGrpProfs RDGroupProfs = RDServer.Connection.GetGrpProfs();
        //    List<DesignGroup> designGroupList = new List<DesignGroup>();

        //    for (int i = 0; i <= RDGroups.Count - 1; i++)
        //    {
        //        int designGroupNumber = RDGroups.GetUserNo(i);
        //        RDimGroup designGroup = RDGroups.Get(designGroupNumber);
        //        DesignGroup bhomDesignGroup = new DesignGroup();
        //        bhomDesignGroup.Name = designGroup.Name;
        //        bhomDesignGroup.Number = designGroup.UsrNo;
        //        bhomDesignGroup.CustomData[AdapterId] = designGroup.UsrNo;
        //        bhomDesignGroup.CustomData[Engine.Robot.Convert.AdapterName] = designGroup.Name;
        //        bhomDesignGroup.MaterialName = designGroup.Material;
        //        designGroup.GetMembList(RDStream);
        //        string test = RDStream.ReadText();
        //        if (RDStream.Size(IRDimStreamType.I_DST_TEXT) > 0)
        //            bhomDesignGroup.MemberIds = Engine.Robot.Convert.ToSelectionList(RDStream.ReadText());
        //        designGroupList.Add(bhomDesignGroup);
        //    }
        //    return designGroupList;
        //}

        /***************************************************/

        ////Fast query method - only returns basic node information, not full node objects
        //public List<Node> ReadNodesQuery(List<string> ids = null)
        //{
        //    List<Node> bhomNodes = new List<Node>();

        //    RobotResultQueryParams result_params = this.RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

        //    RobotSelection nod_sel = this.RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
        //    IRobotResultQueryReturnType query_return = default(IRobotResultQueryReturnType);

        //    nod_sel.FromText("all");
        //    result_params.ResultIds.SetSize(3);
        //    result_params.ResultIds.Set(1, 0);
        //    result_params.ResultIds.Set(2, 1);
        //    result_params.ResultIds.Set(3, 2);

        //    result_params.Selection.Set(IRobotObjectType.I_OT_NODE, nod_sel);
        //    result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
        //    result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
        //    query_return = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
        //    RobotResultRowSet row_set = new RobotResultRowSet();
        //    bool ok = false;

        //    RobotResultRow result_row = default(RobotResultRow);
        //    int nod_num = 0;
        //    int kounta = 0;

        //    while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
        //    {
        //        query_return = this.RobotApplication.Project.Structure.Results.Query(result_params, row_set);
        //        ok = row_set.MoveFirst();
        //        while (ok)
        //        {
        //            result_row = row_set.CurrentRow;
        //            nod_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_NODE);
        //            BH.oM.Geometry.Point point = new BH.oM.Geometry.Point
        //            {
        //                X = (double)row_set.CurrentRow.GetValue(0),
        //                Y = (double)row_set.CurrentRow.GetValue(1),
        //                Z = (double)row_set.CurrentRow.GetValue(2)
        //            };
        //            Node bhomNode = new Node { Position = point, Name = nod_num.ToString() };
        //            bhomNode.CustomData[AdapterId] = nod_num.ToString();
        //            bhomNodes.Add(bhomNode);
        //            point = null;
        //            kounta++;
        //            ok = row_set.MoveNext();
        //        }
        //        row_set.Clear();
        //    }
        //    result_params.Reset();
        //    return bhomNodes;
        //}

        /***************************************************/

        ////Fast query method - returns basic bar information, not full bar objects
        //public List<Bar> ReadBarsQuery(List<string> ids = null)
        //{
        //    List<Bar> bhomBars = new List<Bar>();
        //    IEnumerable<Node> bhomNodesList = ReadNodesQuery();
        //    Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.Name.ToString());

        //    RobotResultQueryParams result_params = default(RobotResultQueryParams);
        //    result_params = (RobotResultQueryParams)this.RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
        //    RobotStructure rstructure = default(RobotStructure);
        //    rstructure = this.RobotApplication.Project.Structure;
        //    RobotSelection cas_sel = default(RobotSelection);
        //    RobotSelection bar_sel = default(RobotSelection);
        //    IRobotResultQueryReturnType query_return = default(IRobotResultQueryReturnType);

        //    bool ok = false;
        //    RobotResultRow result_row = default(RobotResultRow);
        //    int bar_num = 0;

        //    int nod1 = 0;
        //    int nod2 = 0;

        //    int nod1_id = 15;
        //    int nod2_id = 16;

        //    bar_sel = this.RobotApplication.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_BAR);
        //    cas_sel = this.RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
        //    try
        //    {
        //        cas_sel.FromText(this.RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
        //    }
        //    catch
        //    {
        //        this.RobotApplication.Project.Structure.Cases.CreateSimple(1, "Dead Load", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
        //        cas_sel.FromText(this.RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
        //    }

        //    result_params.ResultIds.SetSize(5);
        //    result_params.ResultIds.Set(1, nod1_id);
        //    result_params.ResultIds.Set(2, nod2_id);
        //    result_params.ResultIds.Set(3, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
        //    result_params.ResultIds.Set(4, 269);
        //    result_params.ResultIds.Set(5, 270);

        //    result_params.SetParam(IRobotResultParamType.I_RPT_BAR_RELATIVE_POINT, 0);
        //    result_params.Selection.Set(IRobotObjectType.I_OT_BAR, bar_sel);
        //    result_params.Selection.Set(IRobotObjectType.I_OT_CASE, cas_sel);
        //    result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
        //    result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
        //    RobotResultRowSet row_set = new RobotResultRowSet();

        //    while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
        //    {
        //        query_return = rstructure.Results.Query(result_params, row_set);
        //        ok = row_set.MoveFirst();
        //        while (ok)
        //        {
        //            result_row = row_set.CurrentRow;
        //            bar_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_BAR);

        //            nod1 = (int)result_row.GetValue(nod1_id);
        //            nod2 = (int)result_row.GetValue(nod2_id);

        //            Node startNode = null; bhomNodes.TryGetValue(nod1.ToString(), out startNode);
        //            Node endNode = null; bhomNodes.TryGetValue(nod2.ToString(), out endNode);
        //            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode, Name = bar_num.ToString() };

        //            bhomBar.SectionProperty = null;
        //            //bhomBar.OrientationAngle = robotBar.Gamma * 180 / Math.PI;
        //            bhomBar.Name = bar_num.ToString();

        //            bhomBar.CustomData[AdapterId] = bar_num.ToString();
        //            bhomBars.Add(bhomBar);

        //            ok = row_set.MoveNext();
        //        }
        //        row_set.Clear();
        //    }
        //    result_params.Reset();
        //    return bhomBars;
        //}

        ///***************************************************/

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

