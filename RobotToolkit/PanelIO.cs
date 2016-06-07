using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;
using BHoM.Geometry;
using BHoM.Global;
using BHoM.Materials;

namespace RobotToolkit
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
        public static bool GetPanels(RobotApplication robot, out List<Panel> outputBars, string barNumbers = "all")
        {
            ObjectManager<int, Panel> panels = new ObjectManager<int, Panel>(Utils.NUM_KEY, FilterOption.UserData);
            ObjectManager<ThicknessProperty> thicknesses = new ObjectManager<ThicknessProperty>();
            ObjectManager<Material> materials = new ObjectManager<Material>();
            RobotSelection panel_sel = robot.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_PANEL);
            IRobotCollection panel_col = robot.Project.Structure.Objects.GetMany(panel_sel);
            IRobotLabel thickness = null;
            IRobotLabel material = null;
            panel_sel = null;
            outputBars = new List<Panel>();
            robot.Project.Structure.Objects.BeginMultiOperation();

            for (int i = 1; i <= panel_col.Count; i++)
            {
                RobotObjObject rpanel = (RobotObjObject)panel_col.Get(i);

                IRobotCollection edge_pnt_col = rpanel.Main.DefPoints;

                if (rpanel.Main.GetGeometry().Type == IRobotGeoObjectType.I_GOT_CONTOUR && rpanel.Main.Attribs.Meshed == 1)
                {
                    Group<Curve> c = GeometryHelper.CGeoObject(rpanel) as Group<Curve>;
                    if (c != null)
                    {
                        Panel panel = new Panel(c);
                        int panelNum = rpanel.Number;
                        panels.Add(panelNum, panel);

                        if (rpanel.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) != 0)
                        {
                            thickness = rpanel.GetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS);

                            ThicknessProperty property = thicknesses[thickness.Name];
                            if (property == null)
                            {
                                property = thicknesses.Add(thickness.Name, PropertyIO.GetThickness(thickness));
                            }
                            panel.ThicknessProperty = property;
                        }

                        if (rpanel.HasLabel(IRobotLabelType.I_LT_MATERIAL) != 0)
                        {
                            material = rpanel.GetLabel(IRobotLabelType.I_LT_MATERIAL);
                            Material m = materials[material.Name];
                            if (material == null)
                            {
                                m = materials.Add(material.Name, PropertyIO.GetMaterial(material));
                            }
                            panel.Material = m;
                        }
                    }
                }
            }
            outputBars = panels.ToList();
            return true;
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
        public static bool CreatePanels(RobotApplication robot, List<Panel> panels, out List<string> ids)
        {
            string key = Utils.NUM_KEY;
            robot.Project.Structure.Bars.BeginMultiOperation();
            Dictionary<string, string> addedThicknesses = new Dictionary<string, string>();
            Dictionary<string, string> addedMaterials = new Dictionary<string, string>();
            RobotObjObjectServer objServer = robot.Project.Structure.Objects;
            ids = new List<string>();
            foreach (BHoM.Structural.Panel panel in panels)
            {
                Group<Curve> c = panel.Edges;
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
                    rpanel.Main.Geometry = GeometryHelper.CreateContour(robot, c.ToList()) as RobotGeoObject;

                    rpanel.Initialize();
                    rpanel.Update();
                    string currentThickness = "";
                    if (panel.ThicknessProperty != null && !addedThicknesses.TryGetValue(panel.ThicknessProperty.Name, out currentThickness))
                    {
                        PropertyIO.CreateThicknessProperty(robot, panel.ThicknessProperty);
                        currentThickness = panel.ThicknessProperty.Name;
                        addedThicknesses.Add(currentThickness, currentThickness);
                    }

                    string material = "";
                    if (panel.Material != null && !addedMaterials.TryGetValue(panel.Material.Name, out material))
                    {
                        PropertyIO.CreateMaterial(robot, panel.Material);
                        material = panel.Material.Name;
                        addedMaterials.Add(material, material);
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
            return true;
        }
    
        /// <summary>
        /// Create Robot openings in panels
        /// </summary>
        /// <param name="PanelNumbers"></param>
        /// <param name="EdgePointCoords"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CreateOpenings(List<int> PanelNumbers, List<double[,]> EdgePointCoords, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            robot.Project.Structure.Objects.BeginMultiOperation();
            for (int i = 0; i < PanelNumbers.Count; i++)
            {
                RobotPointsArray ptArray = new RobotPointsArray();
                ptArray.SetSize(EdgePointCoords[i].GetLength(0));
                for (int j = 0; j < EdgePointCoords[i].GetLength(0); j++)
                    ptArray.Set(j + 1, EdgePointCoords[i][j, 0], EdgePointCoords[i][j, 1], EdgePointCoords[i][j, 2]);

                robot.Project.Structure.Objects.CreateContour(PanelNumbers[i], ptArray);
                RobotSelection selection = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_OBJECT);

                selection.AddOne(PanelNumbers[i]);
                RobotObjObject rcontour = (RobotObjObject)robot.Project.Structure.Objects.Get(PanelNumbers[i]);

                rcontour.Initialize();
            }
            robot.Project.Structure.Objects.EndMultiOperation();
            robot.Project.Structure.Objects.AutoRecalcHoles = true;
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
