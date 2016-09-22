using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoMB = BHoM.Base;
using BHoMG = BHoM.Geometry;
using BHoME = BHoM.Structural.Elements;
using BHoMP = BHoM.Structural.Properties;
using BHoMM = BHoM.Materials;
using Robot_Adapter.Base;
using BHoM.Structural.Interface;

namespace Robot_Adapter.Structural.Elements
{
    /// <summary>
    /// Robot panel objects
    /// </summary>
    public class PanelIO
    {
        /// <summary>
        /// Get the first free panel number (by free object number)
        /// </summary>
        /// <param name="freePanelNum"></param>
        /// <param name="FilePath"></param>
        public static void GetFreePanelNumber(out int freePanelNum, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            freePanelNum = robot.Project.Structure.Objects.FreeNumber;
        }

        /// <summary>
        /// Get panel objects from Robot using the COM interface
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="coords"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static List<string> GetPanels(RobotApplication robot, out List<BHoME.Panel> outputBars, ObjectSelection selection, List<string> barNumbers = null)
        {
            BHoMB.ObjectManager<string, BHoME.Panel> panels = new BHoMB.ObjectManager<string, BHoME.Panel>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);
            BHoMB.ObjectManager<BHoMP.PanelProperty> thicknesses = new BHoMB.ObjectManager<BHoMP.PanelProperty>();
            BHoMB.ObjectManager<BHoMM.Material> materials = new BHoMB.ObjectManager<BHoMM.Material>();

            RobotSelection panel_sel = selection == ObjectSelection.Selected ?
                robot.Project.Structure.Selections.Get(IRobotObjectType.I_OT_PANEL) :
                robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);

            if (selection == ObjectSelection.FromInput)
            {
                panel_sel.FromText(Utils.GetSelectionString(barNumbers));
            }
            else if (selection == ObjectSelection.All)
            {
                panel_sel.FromText("all");
            }

            IRobotCollection panel_col = robot.Project.Structure.Objects.GetMany(panel_sel);
            IRobotLabel thickness = null;
            IRobotLabel material = null;

            panel_sel = null;
            outputBars = new List<BHoME.Panel>();
            robot.Project.Structure.Objects.BeginMultiOperation();
            List<string> outIds = new List<string>();

            for (int i = 1; i <= panel_col.Count; i++)
            {
                RobotObjObject rpanel = (RobotObjObject)panel_col.Get(i);

                IRobotCollection edge_pnt_col = rpanel.Main.DefPoints;

                if (rpanel.Main.GetGeometry().Type == IRobotGeoObjectType.I_GOT_CONTOUR && rpanel.Main.Attribs.Meshed == 1)
                {
                    outIds.Add(rpanel.Number.ToString());
                    BHoMG.Group<BHoMG.Curve> c = Geometry.GeometryHelper.CGeoObject(rpanel) as BHoMG.Group<BHoMG.Curve>;
                    if (c != null)
                    {
                        BHoME.Panel panel = new BHoME.Panel(c);
                        int panelNum = rpanel.Number;
                        panels.Add(panelNum.ToString(), panel);

                        if (rpanel.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) != 0)
                        {
                            thickness = rpanel.GetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS);

                            BHoMP.PanelProperty property = thicknesses[thickness.Name];
                            if (property == null)
                            {
                                property = thicknesses.Add(thickness.Name, PropertyIO.GetThickness(thickness, PropertyIO.GetPanelType(rpanel.StructuralType)));
                            }
                            panel.PanelProperty= property;
                        }

                        if (rpanel.HasLabel(IRobotLabelType.I_LT_MATERIAL) != 0)
                        {
                            material = rpanel.GetLabel(IRobotLabelType.I_LT_MATERIAL);
                            BHoMM.Material m = materials[material.Name];
                            if (material == null)
                            {
                                m = materials.Add(material.Name, PropertyIO.GetMaterial(material));
                            }
                            panel.Material = m;
                        }
                    }
                }
            }

            outputBars = panels.GetRange(outIds);
            return outIds;
        }


        /// <summary>
        /// Get contour geometry from Robot using the COM interface
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="coords"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool GetContours(out int[] ids, out double[][,] coords, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotSelection selection = robot.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_GEOMETRY);
            IRobotCollection collection = robot.Project.Structure.Objects.GetMany(selection);

            List<int> _ids = new List<int>();
            List<double[,]> _coords = new List<double[,]>();
            List<RobotObjObject> _rcontours = new List<RobotObjObject>();

            robot.Project.Structure.Objects.BeginMultiOperation();

            for (int i = 0; i < collection.Count; i++)
            {
                RobotObjObject tempContour = (RobotObjObject)collection.Get(i + 1);

                if (tempContour.Main.Geometry.Type == IRobotGeoObjectType.I_GOT_CONTOUR)
                {
                    _ids.Add(tempContour.Number);
                    _rcontours.Add(tempContour);
                    IRobotCollection pnts = tempContour.Main.DefPoints;
                    double[,] pointArray = new double[pnts.Count, 3];
                    for (int j = 0; j < pnts.Count; j++)
                    {
                        RobotGeoPoint3D pnt = (RobotGeoPoint3D)pnts.Get(j + 1);
                        pointArray[j, 0] = pnt.X;
                        pointArray[j, 1] = pnt.Y;
                        pointArray[j, 2] = pnt.Z;
                    }
                    _coords.Add(pointArray);
                }
                else
                    continue;
            }
            robot.Project.Structure.Objects.EndMultiOperation();
            coords = _coords.ToArray();
            ids = _ids.ToArray();
            return true;
        }

        /// <summary>
        /// Create Robot panels
        /// </summary>
        /// <param name="PanelNumbers"></param>
        /// <param name="EdgePointCoords"></param>
        /// <param name="thicknessNames"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CreatePanels(RobotApplication robot, List<BHoME.Panel> panels, out List<string> ids)
        {
            string key = Utils.NUM_KEY;
            robot.Interactive = 0;
            robot.Project.Structure.Objects.BeginMultiOperation();
            Dictionary<string, string> addedThicknesses = new Dictionary<string, string>();
            Dictionary<string, string> addedMaterials = new Dictionary<string, string>();
            RobotObjObjectServer objServer = robot.Project.Structure.Objects;
            ids = new List<string>();
            // List<Opening> openings = new List<Opening>();
            foreach (BHoME.Panel panel in panels)
            {
                int edgeCount = panel.External_Contours.Count;
                BHoMG.Group<BHoMG.Curve> c = panel.External_Contours;
                c.AddRange(panel.Internal_Contours);

                try
                {
                    if (c != null)
                    {
                        string id = "";
                        for (int i = 0; i < c.Count; i++)
                        {
                            RobotObjObject rpanel = null;
                            int panelNum = objServer.FreeNumber;
                            object number = panel[key];

                            id += " " + panelNum;

                            if (number != null && int.TryParse(number.ToString(), out panelNum))
                            {
                                id = panelNum.ToString();
                                if (objServer.Exist(panelNum) == -1)
                                {
                                    rpanel = objServer.Get(panelNum) as RobotObjObject;
                                }
                            }

                            if (rpanel == null)
                            { 
                                rpanel = objServer.Create(panelNum);
                            }

                            ids.Add(panelNum.ToString());

                            rpanel.Main.Geometry = Geometry.GeometryHelper.CreateContour(robot, c[i].Explode()) as RobotGeoObject;

                            rpanel.Initialize();
                            rpanel.Update();
                            if (i < edgeCount)
                            {
                                string material = "";
                                if (panel.Material != null && !addedMaterials.TryGetValue(panel.Material.Name, out material))
                                {
                                    PropertyIO.CreateMaterial(robot, panel.Material);
                                    material = panel.Material.Name;
                                    addedMaterials.Add(material, material);
                                }

                                string currentThickness = "";
                                if (panel.PanelProperty != null && !addedThicknesses.TryGetValue(panel.PanelProperty.Name, out currentThickness))
                                {
                                    PropertyIO.CreateThicknessProperty(robot, panel.PanelProperty, panel.Material.Name);
                                    currentThickness = panel.PanelProperty.Name;
                                    addedThicknesses.Add(currentThickness, currentThickness);
                                }

                                if (!string.IsNullOrEmpty(currentThickness))
                                {
                                    rpanel.Main.Attribs.Meshed = 1;
                                    rpanel.Mesh.Params.SurfaceParams.Generation.Type = IRobotMeshGenerationType.I_MGT_ELEMENT_SIZE;
                                    rpanel.Mesh.Params.SurfaceParams.Generation.ElementSize = 1;
                                    rpanel.Mesh.Params.SurfaceParams.Method.Method = IRobotMeshMethodType.I_MMT_DELAUNAY;
                                    rpanel.Mesh.Params.SurfaceParams.Delaunay.RegularMesh = true;
                                    rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, currentThickness);
                                }

                                rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, currentThickness);
                                rpanel.SetLabel(IRobotLabelType.I_LT_MATERIAL, material);

                                rpanel.Update();
                            }
                        }

                        if (!string.IsNullOrEmpty(id))
                        {
                            if (panel.CustomData.ContainsKey(key))
                            {
                                panel.CustomData[key] = id.Trim();
                            }
                            else
                            {
                                panel.CustomData.Add(key, id.Trim());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            List<string> openingIds = null;
            robot.Project.Structure.Objects.EndMultiOperation();
            robot.Interactive = 1;
            //CreateOpenings(robot, openings, out openingIds);
            return true;
        }

        /// <summary>
        /// Create Robot openings in panels
        /// </summary>
        /// <param name="PanelNumbers"></param>
        /// <param name="EdgePointCoords"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CreateOpenings(RobotApplication robot, List<BHoME.Opening> panels, out List<string> ids)
        {
            string key = Utils.NUM_KEY;


            robot.Interactive = 0;
            robot.Project.Structure.Objects.BeginMultiOperation();
            RobotObjObjectServer objServer = robot.Project.Structure.Objects;
            ids = new List<string>();
            foreach (BHoM.Structural.Elements.Opening panel in panels)
            {
                try
                {
                    BHoMG.Group<BHoMG.Curve> c = panel.Edges;
                    if (c != null)
                    {
                        RobotObjObject rpanel = null;

                        int panelNum = objServer.FreeNumber;
                        object number = panel[key];

                        if (number != null)
                        {
                            int.TryParse(number.ToString(), out panelNum);
                        }
                        else
                        {
                            panel.CustomData.Add(key, panelNum);
                        }

                        if (objServer.Exist(panelNum) == -1)
                        {
                            rpanel = objServer.Get(panelNum) as RobotObjObject;
                        }
                        else
                        {
                            rpanel = objServer.Create(panelNum);
                        }
                        ids.Add(panelNum.ToString());
                        rpanel.Main.Geometry = Geometry.GeometryHelper.CreateContour(robot, c.ToList()) as RobotGeoObject;

                        rpanel.Initialize();
                        rpanel.Update();
                    }
                }
                catch (Exception ex)
                { }
            }
            robot.Project.Structure.Objects.EndMultiOperation();

            robot.Interactive = 1;
            return true;
        }

        /// <summary>
        /// Update panel geometry by redefinition of edge point coordinates
        /// </summary>
        /// <param name="PanelNumbers"></param>
        /// <param name="EdgePointCoords"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool UpdateGeometry(List<int> PanelNumbers, List<double[,]> EdgePointCoords, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            robot.Project.Structure.Objects.BeginMultiOperation();
            for (int i = 0; i < PanelNumbers.Count; i++)
            {
                RobotSelection selection = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_OBJECT);
                 selection.AddOne(PanelNumbers[i]);
                RobotObjObject rpanel = (RobotObjObject)robot.Project.Structure.Objects.Get(PanelNumbers[i]);

               // if (rpanel.Main.GetGeometry().Type == IRobotGeoObjectType.I_GOT_CONTOUR)
                //{
                    IRobotGeoContour contourGeometry = (IRobotGeoContour)rpanel.Main.GetGeometry();
                    contourGeometry.Clear();
                    
                    for (int j = 0; j < EdgePointCoords[i].GetLength(0); j++)
                    {
                        RobotGeoSegment segment = new RobotGeoSegment();
                        segment.P1.Set(EdgePointCoords[i][j, 0], EdgePointCoords[i][j, 1], EdgePointCoords[i][j, 2]);
                        contourGeometry.Add(segment);
                    }
                    contourGeometry.Initialize();
                    rpanel.Initialize();
               // }
         
            }
            robot.Project.Structure.Objects.EndMultiOperation();
            return true;
        }

        /// <summary>
        /// Delete panels in a Robot model
        /// </summary>
        /// <param name="selString"></param>
        /// <param name="FilePath"></param>
        public static void DeletePanels(string selString, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotSelection sel = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);
            sel.AddText(selString);

            robot.Project.Structure.Bars.DeleteMany(sel);
        }


    }
}
