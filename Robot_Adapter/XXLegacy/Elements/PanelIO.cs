/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using RobotOM;
//using BHoMB = BH.oM.Base;
//using BHoMG = BH.oM.Geometry;
//using BHoME = BH.oM.Structure.Elements;
//using BHoMP = BH.oM.Structure.Properties;
//using BHoMM = BHoM.Materials;
//using Robot_Adapter.Base;
//using BH.oM.Structure.Interface;
//using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;

//namespace Robot_Adapter.Structural.Elements
//{
//    /// <summary>
//    /// Robot panel objects
//    /// </summary>
//    public class PanelIO
//    {
//        /// <summary>
//        /// Get the first free panel number (by free object number)
//        /// </summary>
//        /// <param name="freePanelNum"></param>
//        /// <param name="FilePath"></param>
//        public static void GetFreePanelNumber(out int freePanelNum, string FilePath = "LiveLink")
//        {
//            RobotApplication robot = null;
//            if (FilePath == "LiveLink") robot = new RobotApplication();
//            freePanelNum = robot.Project.Structure.Objects.FreeNumber;
//        }

//        /// <summary>
//        /// Get panel objects from Robot using the COM interface
//        /// </summary>
//        /// <param name="ids"></param>
//        /// <param name="coords"></param>
//        /// <param name="FilePath"></param>
//        /// <returns></returns>
//        public static List<string> GetPanels(RobotApplication robot, out List<BHoME.Panel> outputBars, ObjectSelection selection, List<string> barNumbers = null)
//        {
//            BHoMB.ObjectManager<string, BHoME.Panel> panels = new BHoMB.ObjectManager<string, BHoME.Panel>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);
//            BHoMB.ObjectManager<BHoMP.PanelProperty> thicknesses = new BHoMB.ObjectManager<BHoMP.PanelProperty>();
//            BHoMB.ObjectManager<BHoMM.Material> materials = new BHoMB.ObjectManager<BHoMM.Material>();

//            RobotSelection panel_sel = selection == ObjectSelection.Selected ?
//                robot.Project.Structure.Selections.Get(IRobotObjectType.I_OT_PANEL) :
//                robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);


//            if (selection == ObjectSelection.FromInput)
//            {
//                panel_sel.FromText(Utils.GetSelectionString(barNumbers));
//            }
//            else if (selection == ObjectSelection.All)
//            {
//                panel_sel.FromText("all");
//            }

//            IRobotCollection panel_col = robot.Project.Structure.Objects.GetMany(panel_sel);
//            IRobotLabel thickness = null;
//            IRobotLabel material = null;

//            panel_sel = null;
//            outputBars = new List<BHoME.Panel>();
//            robot.Project.Structure.Objects.BeginMultiOperation();
//            List<string> outIds = new List<string>();

//            for (int i = 1; i <= panel_col.Count; i++)
//            {
//                RobotObjObject rpanel = (RobotObjObject)panel_col.Get(i);

//                double x = 0;
//                double y = 0;
//                double z = 0;

//                rpanel.Main.Attribs.GetDirX(out x, out y, out z);

//                IRobotCollection edge_pnt_col = rpanel.Main.DefPoints;

//                if (rpanel.Main.GetGeometry().Type == IRobotGeoObjectType.I_GOT_CONTOUR && rpanel.Main.Attribs.Meshed == 1)
//                {
//                    outIds.Add(rpanel.Number.ToString());
//                    BHoMG.Group<BHoMG.Curve> c = Geometry.Geometry.CGeoObject(rpanel) as BHoMG.Group<BHoMG.Curve>;
//                    if (c != null)
//                    {                       
//                        int panelNum = rpanel.Number;
//                        BHoME.Panel panel = panels.Add(panelNum.ToString(), new BHoME.Panel(BHoMG.Curve.Join(c)));
//                        //panel.External_Contours.AddRange(BHoMG.Curve.Join(c));

//                        if (rpanel.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) != 0)
//                        {
//                            thickness = rpanel.GetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS);

//                            BHoMP.PanelProperty property = thicknesses[thickness.Name];
//                            if (property == null)
//                            {
//                                property = thicknesses.Add(thickness.Name, PropertyIO.GetThickness(thickness, PropertyIO.GetPanelType(rpanel.StructuralType)));
//                            }
//                            panel.PanelProperty= property;

//                            if (panel.PanelProperty != null && rpanel.HasLabel(IRobotLabelType.I_LT_MATERIAL) != 0)
//                            {
//                                material = rpanel.GetLabel(IRobotLabelType.I_LT_MATERIAL);
//                                BHoMM.Material m = materials[material.Name];
//                                if (material == null)
//                                {
//                                    m = materials.Add(material.Name, PropertyIO.GetMaterial(material));
//                                }
//                                panel.PanelProperty.Material = m;
//                            }
//                        }
//                        panel.CustomData["dirX"] = new BHoMG.Vector(x, y, z);      
//                    }
//                }
//            }

//            outputBars = panels.GetRange(outIds);
//            return outIds;
//        }

//        /// <summary>
//        /// Get panel objects from Robot using the COM interface
//        /// </summary>
//        /// <param name="ids"></param>
//        /// <param name="coords"></param>
//        /// <param name="FilePath"></param>
//        /// <returns></returns>
//        public static List<string> GetOpenings(RobotApplication robot, out List<BHoME.Opening> outputBars, ObjectSelection selection, List<string> barNumbers = null)
//        {
//            BHoMB.ObjectManager<string, BHoME.Opening> panels = new BHoMB.ObjectManager<string, BHoME.Opening>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);
         
//            RobotSelection panel_sel = selection == ObjectSelection.Selected ?
//                robot.Project.Structure.Selections.Get(IRobotObjectType.I_OT_GEOMETRY) :
//                robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_GEOMETRY);

//            if (selection == ObjectSelection.FromInput)
//            {
//                panel_sel.FromText(Utils.GetSelectionString(barNumbers));
//            }
//            else if (selection == ObjectSelection.All)
//            {
//                panel_sel.FromText("all");
//            }

//            IRobotCollection panel_col = robot.Project.Structure.Objects.GetMany(panel_sel);

//            panel_sel = null;
//            outputBars = new List<BHoME.Opening>();
//            robot.Project.Structure.Objects.BeginMultiOperation();
//            List<string> outIds = new List<string>();

//            for (int i = 1; i <= panel_col.Count; i++)
//            {
//                RobotObjObject rpanel = (RobotObjObject)panel_col.Get(i);

//                double x = 0;
//                double y = 0;
//                double z = 0;

//                rpanel.Main.Attribs.GetDirX(out x, out y, out z);
                
//                if (rpanel.Main.GetGeometry().Type == IRobotGeoObjectType.I_GOT_CONTOUR && rpanel.Main.Attribs.Meshed != 1)
//                {
//                    outIds.Add(rpanel.Number.ToString());
//                    BHoMG.Group<BHoMG.Curve> c = Geometry.Geometry.CGeoObject(rpanel) as BHoMG.Group<BHoMG.Curve>;
//                    if (c != null)
//                    {
//                        int panelNum = rpanel.Number;
//                        BHoME.Opening panel = panels.Add(panelNum.ToString(), new BHoME.Opening(new BHoMG.Group<BHoMG.Curve>(BHoMG.Curve.Join(c))));
//                        //panel.External_Contours.AddRange(BHoMG.Curve.Join(c));                        
//                    }
//                }
//            }

//            outputBars = panels.GetRange(outIds);
//            return outIds;
//        }


//        /// <summary>
//        /// Get contour geometry from Robot using the COM interface
//        /// </summary>
//        /// <param name="ids"></param>
//        /// <param name="coords"></param>
//        /// <param name="FilePath"></param>
//        /// <returns></returns>
//        public static bool GetContours(out int[] ids, out double[][,] coords, string FilePath = "LiveLink")
//        {
//            RobotApplication robot = null;
//            if (FilePath == "LiveLink") robot = new RobotApplication();
//            RobotSelection selection = robot.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_GEOMETRY);
//            IRobotCollection collection = robot.Project.Structure.Objects.GetMany(selection);

//            List<int> _ids = new List<int>();
//            List<double[,]> _coords = new List<double[,]>();
//            List<RobotObjObject> _rcontours = new List<RobotObjObject>();

//            robot.Project.Structure.Objects.BeginMultiOperation();

//            for (int i = 0; i < collection.Count; i++)
//            {
//                RobotObjObject tempContour = (RobotObjObject)collection.Get(i + 1);

//                if (tempContour.Main.Geometry.Type == IRobotGeoObjectType.I_GOT_CONTOUR)
//                {
//                    _ids.Add(tempContour.Number);
//                    _rcontours.Add(tempContour);
//                    IRobotCollection pnts = tempContour.Main.DefPoints;
//                    double[,] pointArray = new double[pnts.Count, 3];
//                    for (int j = 0; j < pnts.Count; j++)
//                    {
//                        RobotGeoPoint3D pnt = (RobotGeoPoint3D)pnts.Get(j + 1);
//                        pointArray[j, 0] = pnt.X;
//                        pointArray[j, 1] = pnt.Y;
//                        pointArray[j, 2] = pnt.Z;
//                    }
//                    _coords.Add(pointArray);
//                }
//                else
//                    continue;
//            }
//            robot.Project.Structure.Objects.EndMultiOperation();
//            coords = _coords.ToArray();
//            ids = _ids.ToArray();
//            return true;
//        }

//        /// <summary>
//        /// Create Robot panels
//        /// </summary>
//        /// <param name="PanelNumbers"></param>
//        /// <param name="EdgePointCoords"></param>
//        /// <param name="thicknessNames"></param>
//        /// <param name="FilePath"></param>
//        /// <returns></returns>
//        public static bool CreatePanels(RobotApplication robot, List<BHoME.Panel> panels, out List<string> ids)
//        {
//            string key = Utils.NUM_KEY;
//            robot.Interactive = 0;
//            robot.Project.Structure.Objects.BeginMultiOperation();
//            Dictionary<string, string> addedThicknesses = new Dictionary<string, string>();
//            Dictionary<string, string> addedMaterials = new Dictionary<string, string>();
//            RobotObjObjectServer objServer = robot.Project.Structure.Objects;
//            ids = new List<string>();
//            // List<Opening> openings = new List<Opening>();
//            foreach (BHoME.Panel panel in panels)
//            {
//                int edgeCount = panel.External_Contours.Count;
//                BHoMG.Group<BHoMG.Curve> c = panel.External_Contours;
//                c.AddRange(panel.Internal_Contours);

//                try
//                {
//                    if (c != null)
//                    {
//                        string id = "";
//                        for (int i = 0; i < c.Count; i++)
//                        {
//                            RobotObjObject rpanel = null;
//                            object number = panel[key];
//                            int panelNum = 0;

//                            if (number != null && int.TryParse(number.ToString(), out panelNum) && panelNum > 0)
//                            {
//                                id = panelNum.ToString();
//                                if (objServer.Exist(panelNum) == -1)
//                                {
//                                    rpanel = objServer.Get(panelNum) as RobotObjObject;
//                                }
//                            }
//                            else
//                            {
//                                panelNum = objServer.FreeNumber;
//                                id += " " + panelNum;
//                            }

//                            if (rpanel == null)
//                            { 
//                                if (c[i] is BHoMG.Polyline)
//                                {
//                                    objServer.CreateContour(panelNum, Geometry.Geometry.CreatePointArray(c[i] as BHoMG.Polyline));
//                                    rpanel = objServer.Get(panelNum) as RobotObjObject;

//                                }
//                                else
//                                {
//                                    rpanel = objServer.Create(panelNum);
//                                    rpanel.Main.Geometry = Geometry.Geometry.CreateContour(robot, c[i]) as RobotGeoObject;
//                                    rpanel.Initialize();
//                                }
//                            }
//                            else
//                            {
//                                rpanel.Main.Geometry = Geometry.Geometry.CreateContour(robot, c[i]) as RobotGeoObject;
//                                rpanel.Initialize();
//                            }

//                            ids.Add(panelNum.ToString());

//                            if (i < edgeCount)
//                            {
//                                string currentThickness = "";
//                                // Claddings
//                                if (panel.PanelProperty != null && panel.PanelProperty is BHoMP.LoadingPanelProperty)
//                                {
//                                    BHoMP.LoadingPanelProperty loadProp = panel.PanelProperty as BHoMP.LoadingPanelProperty;

//                                    if (loadProp.LoadApplication == BHoMP.LoadPanelSupportConditions.AllSides)
//                                    {
//                                        rpanel.SetLabel(IRobotLabelType.I_LT_CLADDING, "Two-way");             
//                                    }
//                                    if (loadProp.LoadApplication == BHoMP.LoadPanelSupportConditions.TwoSides && loadProp.ReferenceEdge % 2 == 0)
//                                    {
//                                        rpanel.SetLabel(IRobotLabelType.I_LT_CLADDING, "One-way X");
//                                    }
//                                    if (loadProp.LoadApplication == BHoMP.LoadPanelSupportConditions.TwoSides && loadProp.ReferenceEdge % 2 == 1)
//                                    {
//                                        rpanel.SetLabel(IRobotLabelType.I_LT_CLADDING, "One-way Y");
//                                    }
//                                }
//                                // Panels
//                                else
//                                {
//                                    if (panel.PanelProperty != null && !addedThicknesses.TryGetValue(panel.PanelProperty.Name, out currentThickness))
//                                    {
//                                        PropertyIO.CreateThicknessProperty(robot, panel.PanelProperty);
//                                        currentThickness = panel.PanelProperty.Name;
//                                        addedThicknesses.Add(currentThickness, currentThickness);
//                                    }

//                                    if (!string.IsNullOrEmpty(currentThickness))
//                                    {
//                                        rpanel.Main.Attribs.Meshed = 1;
//                                        rpanel.Mesh.Params.SurfaceParams.Generation.Type = IRobotMeshGenerationType.I_MGT_ELEMENT_SIZE;
//                                        rpanel.Mesh.Params.SurfaceParams.Generation.ElementSize = 1;
//                                        rpanel.Mesh.Params.SurfaceParams.Method.Method = IRobotMeshMethodType.I_MMT_DELAUNAY;
//                                        rpanel.Mesh.Params.SurfaceParams.Delaunay.RegularMesh = true;

//                                        rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, currentThickness);
//                                    }

//                                    //rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, currentThickness);
//                                    if (panel.Material != null) rpanel.SetLabel(IRobotLabelType.I_LT_MATERIAL, panel.Material.Name);

//                                }
//                            }
//                            rpanel.Update();
//                        }

//                        if (!string.IsNullOrEmpty(id))
//                        {
//                            if (panel.CustomData.ContainsKey(key))
//                            {
//                                panel.CustomData[key] = id.Trim();
//                            }
//                            else
//                            {
//                                panel.CustomData.Add(key, id.Trim());
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    //string message = ex.Message;
//                }
//            }

//            List<string> openingIds = null;
//            robot.Project.Structure.Objects.EndMultiOperation();
//            robot.Interactive = 1;
//            //CreateOpenings(robot, openings, out openingIds);
//            return true;
//        }

//        public void CreateEmittersOnPanel(int panelNum, int nodeNum)
//        {
//            RobotApplication robot = new RobotApplication();

//            RobotObjObject rpanel = robot.Project.Structure.Objects.Get(panelNum) as RobotObjObject;

//            rpanel.Main.Attribs.Meshed = 1;
//            rpanel.Mesh.Params.SurfaceParams.Generation.Type = IRobotMeshGenerationType.I_MGT_ELEMENT_SIZE;
//            rpanel.Mesh.Params.SurfaceParams.Generation.ElementSize = 1;
//            rpanel.Mesh.Params.SurfaceParams.Method.Method = IRobotMeshMethodType.I_MMT_DELAUNAY;
//            rpanel.Mesh.Params.SurfaceParams.Delaunay.EmittersUser = true;
//            rpanel.Mesh.Params.SurfaceParams.Delaunay.H0 = 0.3;
//            rpanel.Mesh.Params.SurfaceParams.Delaunay.Q = 1.2;

//            RobotEmitter emitter = robot.CmpntFactory.Create(IRobotComponentType.I_CT_EMITTER);
//            emitter.H0 = 0.3;

//            RobotNode emitterNode = robot.Project.Structure.Objects.Get(nodeNum) as RobotNode;
//            emitterNode.SetEmitter(emitter);
//        }

//        /// <summary>
//        /// Create Robot openings in panels
//        /// </summary>
//        /// <param name="PanelNumbers"></param>
//        /// <param name="EdgePointCoords"></param>
//        /// <param name="FilePath"></param>
//        /// <returns></returns>
//        public static bool CreateOpenings(RobotApplication robot, List<BHoME.Opening> panels, out List<string> ids)
//        {
//            string key = Utils.NUM_KEY;

//            robot.Interactive = 0;
//            robot.Project.Structure.Objects.BeginMultiOperation();
//            RobotObjObjectServer objServer = robot.Project.Structure.Objects;
//            ids = new List<string>();
//            foreach (BH.oM.Structure.Elements.Opening panel in panels)
//            {
//                try
//                {
//                    BHoMG.Group<BHoMG.Curve> c = panel.Edges;
//                    if (c != null)
//                    {
//                        RobotObjObject rpanel = null;

//                        int panelNum = objServer.FreeNumber;
//                        object number = panel[key];

//                        if (number != null)
//                        {
//                            int.TryParse(number.ToString(), out panelNum);
//                        }
//                        else
//                        {
//                            panel.CustomData.Add(key, panelNum);
//                        }

//                        if (objServer.Exist(panelNum) == -1)
//                        {
//                            rpanel = objServer.Get(panelNum) as RobotObjObject;
//                        }
//                        else
//                        {
//                            rpanel = objServer.Create(panelNum);
//                        }
//                        ids.Add(panelNum.ToString());
//                        rpanel.Main.Geometry = Geometry.Geometry.CreateContour(robot, c[0]) as RobotGeoObject;

//                        rpanel.Initialize();
//                        rpanel.Update();
//                    }
//                }
//                catch (Exception ex)
//                { }
//            }
//            robot.Project.Structure.Objects.EndMultiOperation();

//            robot.Interactive = 1;
//            return true;
//        }

//        /// <summary>
//        /// Update panel geometry by redefinition of edge point coordinates
//        /// </summary>
//        /// <param name="PanelNumbers"></param>
//        /// <param name="EdgePointCoords"></param>
//        /// <param name="FilePath"></param>
//        /// <returns></returns>
//        public static bool UpdateGeometry(List<int> PanelNumbers, List<double[,]> EdgePointCoords, string FilePath = "LiveLink")
//        {
//            RobotApplication robot = null;
//            if (FilePath == "LiveLink") robot = new RobotApplication();
//            robot.Project.Structure.Objects.BeginMultiOperation();
//            for (int i = 0; i < PanelNumbers.Count; i++)
//            {
//                RobotSelection selection = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_OBJECT);
//                selection.AddOne(PanelNumbers[i]);
//                RobotObjObject rpanel = (RobotObjObject)robot.Project.Structure.Objects.Get(PanelNumbers[i]);

//                // if (rpanel.Main.GetGeometry().Type == IRobotGeoObjectType.I_GOT_CONTOUR)
//                //{
//                IRobotGeoContour contourGeometry = (IRobotGeoContour)rpanel.Main.GetGeometry();
//                contourGeometry.Clear();

//                for (int j = 0; j < EdgePointCoords[i].GetLength(0); j++)
//                {
//                    RobotGeoSegment segment = new RobotGeoSegment();
//                    segment.P1.Set(EdgePointCoords[i][j, 0], EdgePointCoords[i][j, 1], EdgePointCoords[i][j, 2]);
//                    contourGeometry.Add(segment);
//                }
//                contourGeometry.Initialize();
//                rpanel.Initialize();
//                // }

//            }
//            robot.Project.Structure.Objects.EndMultiOperation();
//            return true;
//        }

//        /// <summary>
//        /// Delete panels in a Robot model
//        /// </summary>
//        /// <param name="selString"></param>
//        /// <param name="FilePath"></param>
//        public static void DeletePanels(string selString, string FilePath = "LiveLink")
//        {
//            RobotApplication robot = null;
//            if (FilePath == "LiveLink") robot = new RobotApplication();
//            RobotSelection sel = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);
//            sel.AddText(selString);

//            robot.Project.Structure.Bars.DeleteMany(sel);
//        }
//    }
//}





