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

                if (objects.First() is Property2D)
                {
                    success = CreateCollection(objects as IEnumerable<Property2D>);
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
                if (m_SupportTaggs.ContainsKey(constList[i].Name))
                    m_SupportTaggs[constList[i].Name] = constList[i].TaggedName();
                else
                    m_SupportTaggs.Add(constList[i].Name, constList[i].TaggedName());
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
            List<ISectionProperty> secPropList = secProp.ToList();
            IRobotLabel lable = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, "");
            IRobotBarSectionData secData = lable.Data;

            for (int i = 0; i < secPropList.Count; i++)
            {
                BH.Engine.Robot.Convert.ISectionType(secPropList[i], secData);
                m_RobotApplication.Project.Structure.Labels.StoreWithName(lable, secPropList[i].Name);
                if (m_SectionPropertyTaggs.ContainsKey(secPropList[i].Name))
                    m_SectionPropertyTaggs[secPropList[i].Name] = secPropList[i].TaggedName();
                else
                    m_SectionPropertyTaggs.Add(secPropList[i].Name, secPropList[i].TaggedName());
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Material> mat)
        {
            List<Material> matList = mat.ToList();
            IRobotLabel lable = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_MATERIAL, "");
            IRobotMaterialData matData = lable.Data;

            for (int i = 0; i < matList.Count; i++)
            {
                BH.Engine.Robot.Convert.RobotMaterial(matData, matList[i]);
                m_RobotApplication.Project.Structure.Labels.StoreWithName(lable, matList[i].Name);
                if (m_MaterialTaggs.ContainsKey(matList[i].Name))
                    m_MaterialTaggs[matList[i].Name] = matList[i].TaggedName();
                else
                    m_MaterialTaggs.Add(matList[i].Name, matList[i].TaggedName());
            }
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Loadcase> loadCase)
        {
            List<Loadcase> caseList = loadCase.ToList();
            IRobotCaseServer caseServer = m_RobotApplication.Project.Structure.Cases;

            for (int i = 0; i < caseList.Count; i++)
            {
                int subNature;
                IRobotCaseNature rNature = BH.Engine.Robot.Convert.RobotLoadNature(caseList[i], out subNature);
                m_RobotApplication.Project.Structure.Cases.CreateSimple(System.Convert.ToInt32(caseList[i].CustomData[AdapterId]), caseList[i].Name, rNature, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                IRobotSimpleCase sCase = caseServer.Get(System.Convert.ToInt32(caseList[i].CustomData[AdapterId])) as IRobotSimpleCase;
                if (subNature >= 0)
                    sCase.SetNatureExt(subNature);
            }
            return true;
        }

        /***************************************************/


        private bool CreateCollection(IEnumerable<BH.oM.Structural.Elements.Node> nodes)
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

            foreach (Node node in nodes)
            {
                nodeNum = System.Convert.ToInt32(node.CustomData[AdapterId]);
                if (m_NodeTaggs.ContainsKey(nodeNum))
                    m_NodeTaggs[nodeNum] = node.TaggedName();
                else
                    m_NodeTaggs.Add(nodeNum, node.TaggedName());
                if (node.Constraint != null)
                    robotNodeCol.Get(nodeNum).SetLabel(IRobotLabelType.I_LT_SUPPORT, node.Constraint.Name);
            }
            m_RobotApplication.Interactive = 1;
            return true;
        }

        /***************************************************/

        public bool CreateCollection(IEnumerable<Bar> bhomBars)
        {
            
            m_RobotApplication.Interactive = 0;
            List<Bar> bars = bhomBars.ToList();
            IRobotStructureCache rcache = m_RobotApplication.Project.Structure.CreateCache();
            RobotSelection rSelect = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            string str = "";
            int barNum = 0;
            int freeNum = m_RobotApplication.Project.Structure.Bars.FreeNumber;
            foreach (Bar bhomBar in bars)
            {
                barNum = System.Convert.ToInt32(bhomBar.CustomData[AdapterId]);
                rcache.AddBar(barNum,
                              System.Convert.ToInt32(bhomBar.StartNode.CustomData[AdapterId]),
                              System.Convert.ToInt32(bhomBar.EndNode.CustomData[AdapterId]),
                              bhomBar.SectionProperty.Name,
                              bhomBar.SectionProperty.Material.Name,
                              bhomBar.OrientationAngle * Math.PI / 180);

                str = str + barNum.ToString() + ",";                            
            }
            str.TrimEnd(',');

            m_RobotApplication.Project.Structure.ApplyCache(rcache as RobotStructureCache);
            IRobotBarServer barServer = m_RobotApplication.Project.Structure.Bars;
            barServer.BeginMultiOperation();
            rSelect.FromText(str);
            barServer.SetTensionCompression(rSelect, IRobotBarTensionCompression.I_BTC_TENSION_ONLY);

            //foreach (Bar bhomBar in bars)
            //{
            //    RobotBar rBar = barServer.Get(System.Convert.ToInt32(bhomBar.CustomData[AdapterId])) as RobotBar;
            //    rBar.NameTemplate = bhomBar.TaggedName();
            //    BH.Engine.Robot.Convert.SetFEAType(rBar, bhomBar);
            //}

            barServer.EndMultiOperation();
            m_RobotApplication.Interactive = 1;
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
            RobotObjObjectServer objServer = m_RobotApplication.Project.Structure.Objects;
            foreach (MeshFace face in meshFaces)
            {
                IRobotPointsArray ptarray = new RobotPointsArray();
                RobotObjObject mesh = null;
                if (face.Nodes.Count == 3)
                {
                    ptarray.SetSize(3);
                    ptarray.Set(1, face.Nodes[0].Position.X, face.Nodes[0].Position.Y, face.Nodes[0].Position.Z);
                    ptarray.Set(2, face.Nodes[1].Position.X, face.Nodes[1].Position.Y, face.Nodes[1].Position.Z);
                    ptarray.Set(3, face.Nodes[2].Position.X, face.Nodes[2].Position.Y, face.Nodes[2].Position.Z);
                }
                else
                {
                    ptarray.SetSize(4);
                    ptarray.Set(1, face.Nodes[0].Position.X, face.Nodes[0].Position.Y, face.Nodes[0].Position.Z);
                    ptarray.Set(2, face.Nodes[1].Position.X, face.Nodes[1].Position.Y, face.Nodes[1].Position.Z);
                    ptarray.Set(3, face.Nodes[2].Position.X, face.Nodes[2].Position.Y, face.Nodes[2].Position.Z);
                    ptarray.Set(4, face.Nodes[3].Position.X, face.Nodes[3].Position.Y, face.Nodes[3].Position.Z);
                }

                RobotPointsArray ptArray = ptarray as RobotPointsArray;
                objServer.CreateContour(System.Convert.ToInt32(face.CustomData[AdapterId]), ptArray);
                mesh = objServer.Get(System.Convert.ToInt32(face.CustomData[AdapterId])) as RobotObjObject;
                if (face.Property is LoadingPanelProperty)
                {
                    LoadingPanelProperty panalProp = face.Property as LoadingPanelProperty;
                    if (panalProp.LoadApplication == LoadPanelSupportConditions.AllSides)
                    {
                        mesh.SetLabel(IRobotLabelType.I_LT_CLADDING, "Two-way");
                    }
                    if (panalProp.LoadApplication == LoadPanelSupportConditions.TwoSides && panalProp.ReferenceEdge % 2 == 1)
                    {
                        mesh.SetLabel(IRobotLabelType.I_LT_CLADDING, "One-way X");
                    }
                    if (panalProp.LoadApplication == LoadPanelSupportConditions.TwoSides && panalProp.ReferenceEdge % 2 == 0)
                    {
                        mesh.SetLabel(IRobotLabelType.I_LT_CLADDING, "One-way Y");
                    }
                }
                mesh.Update();
            }

            return true;
        }

        /***************************************************/

        public bool CreateCollection(IEnumerable<Property2D> properties)
        {
            RobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            foreach (Property2D property in properties)
            {
                IRobotLabel lable = labelServer.Create(IRobotLabelType.I_LT_PANEL_THICKNESS, property.Name);
                RobotThicknessData thicknessData = lable.Data;
                thicknessData.MaterialName = property.Material.Name;
                BH.Engine.Robot.Convert.ThicknessProperty(thicknessData, property);
                labelServer.Store(lable);
            }

            return true;
        }

        /***************************************************/

        public bool CreateCollection(IEnumerable<LoadCombination> lComabinations)
        {

            foreach (LoadCombination lComb in lComabinations)
            {
                RobotCaseCombination rCaseCombination = m_RobotApplication.Project.Structure.Cases.CreateCombination(System.Convert.ToInt32(lComb.CustomData[AdapterId]), lComb.Name, IRobotCombinationType.I_CBT_ULS, IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_COMB);
                for (int i = 0; i < lComb.LoadCases.Count; i++)
                {
                    rCaseCombination.CaseFactors.New(System.Convert.ToInt32(lComb.LoadCases[i].Item2.CustomData[AdapterId]), lComb.LoadCases[i].Item1);
                }
                updateview();
            }
        
            return true;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

