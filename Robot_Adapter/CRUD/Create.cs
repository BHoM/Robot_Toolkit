using System.Collections.Generic;
using System.Linq;
using System;
using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using RobotOM;

using BH.Engine.Robot;
using BHEG = BH.Engine.Geometry;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Index Adapter Interface                   ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;
            if (objects.Count() > 0)
            {
                var watch = new System.Diagnostics.Stopwatch();
                if (objects.First() is Constraint6DOF)
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    success = CreateCollection(objects as IEnumerable<Constraint6DOF>);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                }

                if (objects.First() is RigidLink)
                {
                    success = CreateCollection(objects as IEnumerable<RigidLink>);
                }

                if (objects.First() is BarRelease)
                {
                    success = CreateCollection(objects as IEnumerable<BarRelease>);
                }

                if (objects.First() is LinkConstraint)
                {
                    success = CreateCollection(objects as IEnumerable<LinkConstraint>);
                }

                if (typeof(ISectionProperty).IsAssignableFrom(objects.First().GetType()))
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    success = CreateCollection(objects as IEnumerable<ISectionProperty>);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                }

                if (objects.First() is Material)
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    success = CreateCollection(objects as IEnumerable<Material>);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                }

                if (objects.First() is Loadcase)
                {
                    success = CreateCollection(objects as IEnumerable<Loadcase>);
                }

                if (objects.First() is MeshFace)
                {
                    success = CreateCollection(objects as IEnumerable<MeshFace>);
                }

                if (objects.First() is Node)
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    success = CreateCollection(objects as IEnumerable<Node>);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                }

                if (objects.First() is Bar)
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    success = CreateCollection(objects as IEnumerable<Bar>);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                }

                if (typeof(ILoad).IsAssignableFrom(objects.First().GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<ILoad>);
                }

                if (objects.First() is PanelPlanar)
                {
                    success = CreateCollection(objects as IEnumerable<PanelPlanar>);
                }

                if (typeof(IProperty2D).IsAssignableFrom(objects.First().GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<IProperty2D>);
                }

                if (objects.First() is LoadCombination)
                {
                    success = CreateCollection(objects as IEnumerable<LoadCombination>);
                }

            }
            //success = CreateObjects(objects as dynamic);
            updateview();
            return success;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Constraint6DOF> constraints)
        {
            List<Constraint6DOF> constList = constraints.ToList();
            IRobotLabel lable = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_SUPPORT, "");
            IRobotNodeSupportData suppData = lable.Data;

            for (int i = 0; i < constList.Count; i++)
            {
                BH.Engine.Robot.Convert.RobotConstraint(suppData, constList[i]);
                m_RobotApplication.Project.Structure.Labels.StoreWithName(lable, constList[i].Name);
                //if (m_SupportTaggs.ContainsKey(constList[i].Name))
                //    m_SupportTaggs[constList[i].Name] = constList[i].TaggedName();
                //else
                //    m_SupportTaggs.Add(constList[i].Name, constList[i].TaggedName());
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<LinkConstraint> linkConstraints)
        {
            IRobotLabel rigidLink = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_NODE_RIGID_LINK, "");
            IRobotNodeRigidLinkData rLinkData = rigidLink.Data;

            foreach (LinkConstraint lConst in linkConstraints)
            {
                m_RobotApplication.Project.Structure.Labels.StoreWithName(rigidLink, lConst.Name);

                rLinkData.UX = lConst.XtoX;
                rLinkData.UY = lConst.YtoY;
                rLinkData.UZ = lConst.ZtoZ;
                rLinkData.RX = lConst.XXtoXX;
                rLinkData.RY = lConst.YYtoYY;
                rLinkData.RZ = lConst.ZZtoZZ;
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<RigidLink> rigidLinks)
        {
            foreach (RigidLink rLink in rigidLinks)
            {
                string[] str = rLink.SlaveNodes.Select(x => x.CustomData[AdapterId].ToString() + ",").ToList().ToArray();
                string slaves = string.Join("", str).TrimEnd(',');
                m_RobotApplication.Project.Structure.Nodes.RigidLinks.Set(System.Convert.ToInt32(rLink.MasterNode.CustomData[AdapterId]), slaves, rLink.Constraint.Name);
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<ISectionProperty> secProp)
        {
            IRobotLabel label = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, "");
            IRobotBarSectionData secData = label.Data;

            foreach (ISectionProperty p in secProp)
            {
                string match = BH.Engine.Robot.Convert.Match(m_dbSecPropNames, p);
                if (match != null)
                {
                    string matName = BH.Engine.Robot.Convert.Match(m_dbMaterialNames, p.Material);
                    if (matName == null)
                        matName = p.Material.Name;
                    
                    secData.LoadFromDBase(match);
                    secData.MaterialName = matName;
                    m_RobotApplication.Project.Structure.Labels.StoreWithName(label, match);
                }
                else
                {
                    BH.Engine.Robot.Convert.ISectionProperty(p, secData);
                    m_RobotApplication.Project.Structure.Labels.StoreWithName(label, p.Name);
                }
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Material> mat)
        {
            IRobotLabel label = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_MATERIAL, "");
            IRobotMaterialData matData = label.Data;

            foreach (Material m in mat)
            {
                string match = BH.Engine.Robot.Convert.Match(m_dbMaterialNames, m);
                if (match != null)
                {
                    matData.LoadFromDBase(match);
                    m_RobotApplication.Project.Structure.Labels.StoreWithName(label, match);
                }
                else
                {
                    BH.Engine.Robot.Convert.RobotMaterial(matData, m);
                    m_RobotApplication.Project.Structure.Labels.StoreWithName(label, m.Name);
                }
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Loadcase> loadCase)
        {
            List<Loadcase> caseList = loadCase.ToList();
            IRobotCaseServer caseServer = m_RobotApplication.Project.Structure.Cases;

            m_RobotApplication.Project.Structure.Cases.BeginMultiOperation();
            for (int i = 0; i < caseList.Count; i++)
            {                
                int subNature;
                IRobotCaseNature rNature = BH.Engine.Robot.Convert.RobotLoadNature(caseList[i], out subNature);
                m_RobotApplication.Project.Structure.Cases.CreateSimple(caseList[i].Number, caseList[i].Name, rNature, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);                
                IRobotSimpleCase sCase = caseServer.Get(caseList[i].Number) as IRobotSimpleCase;
                if (subNature >= 0)
                    sCase.SetNatureExt(subNature);
            }
            m_RobotApplication.Project.Structure.Cases.EndMultiOperation();
            return true;
        }

        /***************************************************/


        private bool CreateCollection(IEnumerable<BH.oM.Structural.Elements.Node> nodes)
        {
            if (nodes != null)
            {
                m_RobotApplication.Interactive = 0;
                List<Node> nodeList = nodes.ToList();
                int nodeNum = 0;
                IRobotStructureCache rcache = m_RobotApplication.Project.Structure.CreateCache();
                IRobotSelection nodeSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
                int freeNum = m_RobotApplication.Project.Structure.Nodes.FreeNumber;

                foreach (Node node in nodes)
                {
                    nodeNum = System.Convert.ToInt32(node.CustomData[AdapterId]);
                    rcache.AddNode(nodeNum, node.Position.X, node.Position.Y, node.Position.Z);
                }
                m_RobotApplication.Project.Structure.ApplyCache(rcache as RobotStructureCache);
                IRobotNodeServer robotNodeCol = m_RobotApplication.Project.Structure.Nodes;

                Dictionary<int, HashSet<string>> nodeTags = GetTypeTags(typeof(Node));

                foreach (Node node in nodes)
                {
                    nodeNum = System.Convert.ToInt32(node.CustomData[AdapterId]);
                    nodeTags[nodeNum] = node.Tags;

                    if (node.Constraint != null)
                        robotNodeCol.Get(nodeNum).SetLabel(IRobotLabelType.I_LT_SUPPORT, node.Constraint.Name);
                }

                m_tags[typeof(Node)] = nodeTags;
                m_RobotApplication.Interactive = 1;
            }
            return true;
        }

        /***************************************************/

        public bool CreateCollection(IEnumerable<Bar> bhomBars)
        {
            if (bhomBars != null)
            {
                IRobotStructure rStructure = m_RobotApplication.Project.Structure;
                IRobotBarServer barServer = rStructure.Bars;
                m_RobotApplication.Interactive = 0;
                barServer.BeginMultiOperation();
                List<Bar> bars = bhomBars.ToList();
                IRobotStructureCache rcache = rStructure.CreateCache();
                RobotSelection rSelect = rStructure.Selections.Create(IRobotObjectType.I_OT_BAR);
                string tensionBars = "";
                string compressionBars = "";
                string axialBars = "";
                int barNum = 0;
                int freeNum = m_RobotApplication.Project.Structure.Bars.FreeNumber;
                Dictionary<int, HashSet<string>> barTags = GetTypeTags(typeof(Bar));
                foreach (Bar bhomBar in bars)
                {
                    string sectionName = BH.Engine.Robot.Convert.Match(m_dbSecPropNames, bhomBar.SectionProperty);
                    string materialName = BH.Engine.Robot.Convert.Match(m_dbMaterialNames, bhomBar.SectionProperty.Material);
                    if (sectionName == null)
                        sectionName = bhomBar.SectionProperty.Name;
                    if (materialName == null)
                        materialName = bhomBar.SectionProperty.Material.Name;
                    barNum = System.Convert.ToInt32(bhomBar.CustomData[AdapterId]);
                    if (bhomBar.SectionProperty != null)
                    {
                        rcache.AddBar(barNum,
                                      System.Convert.ToInt32(bhomBar.StartNode.CustomData[AdapterId]),
                                      System.Convert.ToInt32(bhomBar.EndNode.CustomData[AdapterId]),
                                      sectionName,
                                      materialName,
                                      bhomBar.OrientationAngle * 180 / Math.PI);
                        rcache.SetBarLabel(barNum, IRobotLabelType.I_LT_BAR_RELEASE, bhomBar.Release.Name);
                    }

                    else
                    {
                        barServer.Create(barNum,
                                         System.Convert.ToInt32(bhomBar.StartNode.CustomData[AdapterId]),
                                         System.Convert.ToInt32(bhomBar.EndNode.CustomData[AdapterId]));
                    }

                    if (bhomBar.FEAType == BarFEAType.TensionOnly)
                        tensionBars = tensionBars + barNum.ToString() + ",";
                    else if (bhomBar.FEAType == BarFEAType.CompressionOnly)
                        compressionBars = compressionBars + barNum.ToString() + ",";
                    else if (bhomBar.FEAType == BarFEAType.Axial)
                        axialBars = axialBars + barNum.ToString() + ",";
                }
                tensionBars.TrimEnd(',');
                compressionBars.TrimEnd(',');
                axialBars.TrimEnd(',');

                m_RobotApplication.Project.Structure.ApplyCache(rcache as RobotStructureCache);

                rSelect.FromText(tensionBars);
                barServer.SetTensionCompression(rSelect, IRobotBarTensionCompression.I_BTC_TENSION_ONLY);
                rSelect.FromText(compressionBars);
                barServer.SetTensionCompression(rSelect, IRobotBarTensionCompression.I_BTC_COMPRESSION_ONLY);
                barServer.SetTrussBar(axialBars, true);

                foreach (Bar bhomBar in bars)
                {
                    RobotBar rBar = barServer.Get(System.Convert.ToInt32(bhomBar.CustomData[AdapterId])) as RobotBar;
                    barNum = System.Convert.ToInt32(bhomBar.CustomData[AdapterId]);
                    barTags[barNum] = bhomBar.Tags;
                    BH.Engine.Robot.Convert.SetFEAType(rBar, bhomBar);
                }
                m_tags[typeof(Bar)] = barTags;
                barServer.EndMultiOperation();
                m_RobotApplication.Interactive = 1;
            }
            return true;
        }

        /***************************************************/

        public bool CreateCollection(IEnumerable<ILoad> loads)
        {
            RobotCaseServer caseServer = m_RobotApplication.Project.Structure.Cases;
            RobotGroupServer rGroupServer = m_RobotApplication.Project.Structure.Groups;

            foreach (ILoad load in loads)
            {
                IRobotCase rCase = caseServer.Get(load.Loadcase.Number);
                RobotSimpleCase sCase = rCase as RobotSimpleCase;
                BH.Engine.Robot.Convert.IRobotLoad(load, sCase, rGroupServer);
            }

            return true;
        }

        /***************************************************/

        public bool CreateCollection(IEnumerable<PanelPlanar> panels)
        {
            m_RobotApplication.Interactive = 0;
            m_RobotApplication.Project.Structure.Objects.BeginMultiOperation();
            RobotObjObjectServer objServer = m_RobotApplication.Project.Structure.Objects;

            foreach (PanelPlanar panel in panels)
            {
                List<ICurve> segments = new List<ICurve>();
                foreach (Edge edge in panel.ExternalEdges)
                {
                    segments.AddRange(BHEG.Query.ISubParts(edge.Curve).ToList());
                }

                RobotGeoObject contour;
                RobotObjObject rpanel = objServer.Create(System.Convert.ToInt32(panel.CustomData[AdapterId]));
                if (segments.Count > 1)
                {
                    contour = m_RobotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CONTOUR);
                    foreach (ICurve crv in segments)
                    {
                        RobotGeoSegment segment = m_RobotApplication.CmpntFactory.Create(BH.Engine.Robot.Convert.SegmentType(crv));
                        (contour as RobotGeoContour).Add(BH.Engine.Robot.Convert.Segment(crv, segment));
                    }
                }
                else
                {
                    contour = m_RobotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CIRCLE);
                    BH.Engine.Robot.Convert.SingleContourGeometry(segments[0], contour);
                }

                rpanel.Main.Geometry = contour as RobotGeoObject;
                rpanel.Initialize();

                rpanel.Main.Attribs.Meshed = 1;
                rpanel.Update();
                if (panel.Property is LoadingPanelProperty)
                    rpanel.SetLabel(IRobotLabelType.I_LT_CLADDING, panel.Property.Name);

                else
                    rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, panel.Property.Name);
            }
            m_RobotApplication.Project.Structure.Objects.EndMultiOperation();
            m_RobotApplication.Interactive = 1;
            return true;
        }

        /***************************************************/

        public bool CreateCollection<T>(IEnumerable<BH.oM.Base.BHoMGroup<T>> groups) where T : BH.oM.Base.IBHoMObject
        {

            RobotGroupServer rGroupServer = m_RobotApplication.Project.Structure.Groups;
            foreach (BHoMGroup<T> group in groups)
            {
                IRobotObjectType rType = BH.Engine.Robot.Convert.RobotObjectType(typeof(T));
                string members = group.Elements.Select(x => int.Parse(x.CustomData[BH.Engine.Robot.Convert.AdapterID].ToString())).GeterateIdString();
                rGroupServer.Create(rType, group.Name, members);
            }

            return true;
        }

        /***************************************************/

        public bool CreateCollection(IEnumerable<MeshFace> meshFaces)
        {
            int nbOfDistinctProps = meshFaces.Select(x => x.Property).Distinct(Comparer<IProperty2D>()).Count();
            if (nbOfDistinctProps == 1)
            {
                string faceList = "";
                IRobotStructure objServer = m_RobotApplication.Project.Structure;
                RobotObjObject mesh = null;
                foreach (MeshFace face in meshFaces)
                {
                    IRobotNumbersArray ptarray = new RobotNumbersArray();
                    if (face.Nodes.Count == 3)
                    {
                        ptarray.SetSize(3);
                        ptarray.Set(1, System.Convert.ToInt32(face.Nodes[0].CustomData[AdapterId]));
                        ptarray.Set(2, System.Convert.ToInt32(face.Nodes[1].CustomData[AdapterId]));
                        ptarray.Set(3, System.Convert.ToInt32(face.Nodes[2].CustomData[AdapterId]));
                    }
                    else
                    {
                        ptarray.SetSize(4);
                        ptarray.Set(1, System.Convert.ToInt32(face.Nodes[0].CustomData[AdapterId]));
                        ptarray.Set(2, System.Convert.ToInt32(face.Nodes[1].CustomData[AdapterId]));
                        ptarray.Set(3, System.Convert.ToInt32(face.Nodes[2].CustomData[AdapterId]));
                        ptarray.Set(4, System.Convert.ToInt32(face.Nodes[3].CustomData[AdapterId]));
                    }
                    int faceNum = System.Convert.ToInt32(face.CustomData[AdapterId]);
                    RobotNumbersArray ptArray = ptarray as RobotNumbersArray;
                    faceList = faceList + faceNum.ToString() + ",";

                    objServer.FiniteElems.Create(faceNum, ptArray);
                }
                faceList.TrimEnd(',');

                objServer.Objects.CreateOnFiniteElems(faceList, objServer.Objects.FreeNumber);
                mesh = objServer.Objects.Get(objServer.Objects.FreeNumber - 1) as RobotObjObject;
                //mesh.Main.Attribs.Meshed = 1;
                //mesh.Update();
                if (meshFaces.First().Property is LoadingPanelProperty)
                    mesh.SetLabel(IRobotLabelType.I_LT_CLADDING, meshFaces.First().Property.Name);

                else
                    mesh.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, meshFaces.First().Property.Name);

                return true;
            }

            else
            {
                throw new Exception("All meshFaces should have the same property");
            }
        }

        /***************************************************/

        public bool CreateCollection(IEnumerable<IProperty2D> properties)
        {
            RobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel lable = null;
            string name = "";
            foreach (IProperty2D property in properties)
            {
                if (property is LoadingPanelProperty)
                {
                    name = BH.Engine.Robot.Convert.ThicknessProperty(lable, property);
                    lable = labelServer.CreateLike(IRobotLabelType.I_LT_CLADDING, property.Name, name);
                }

                else
                {
                    lable = labelServer.Create(IRobotLabelType.I_LT_PANEL_THICKNESS, name);
                    name = BH.Engine.Robot.Convert.ThicknessProperty(lable, property);
                }

                labelServer.StoreWithName(lable, name);
            }

            return true;
        }

        /***************************************************/

        public bool CreateCollection(IEnumerable<LoadCombination> lComabinations)
        {

            foreach (LoadCombination lComb in lComabinations)
            {
                RobotCaseCombination rCaseCombination = m_RobotApplication.Project.Structure.Cases.CreateCombination(lComb.Number, lComb.Name, IRobotCombinationType.I_CBT_ULS, IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_COMB);
                for (int i = 0; i < lComb.LoadCases.Count; i++)
                {
                    rCaseCombination.CaseFactors.New(lComb.LoadCases[i].Item2.Number, lComb.LoadCases[i].Item1);
                }
                updateview();
            }
        
            return true;
        }

        public bool CreateCollection(IEnumerable<BarRelease> releases)
        {
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel lable = labelServer.Create(IRobotLabelType.I_LT_BAR_RELEASE, "");
            IRobotBarReleaseData rData = lable.Data;

            foreach (BarRelease bRelease in releases)
            {
                BH.Engine.Robot.Convert.RobotRelease(rData.StartNode, bRelease.StartRelease);
                BH.Engine.Robot.Convert.RobotRelease(rData.EndNode, bRelease.EndRelease);
                labelServer.StoreWithName(lable, bRelease.Name);
            }
            return true;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

