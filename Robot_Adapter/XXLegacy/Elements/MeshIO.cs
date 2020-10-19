/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BH.Engine.Adapter;
//using BHoMB = BH.oM.Base;
//using BHoME = BH.oM.Structure.Elements;
//using BHoMP = BH.oM.Structure.Properties;
//using BHoMM = BHoM.Materials;
//using Robot_Adapter.Base;
//using BH.oM.Structure.Interface;

//namespace Robot_Adapter.Structural.Elements
//{
//    public class MeshIO
//    {
//        public static bool CreateMesh(RobotApplication robot, List<BHoME.FEMesh> meshes, out List<string> ids)
//        {
//            string key = Utils.NUM_KEY;
//            ids = new List<string>();
//            List<string> nodeIds = null;
//            List<int> feIds = new List<int>();
//            RobotObjObjectServer objServer = robot.Project.Structure.Objects;
//            RobotFiniteElementServer feServer = robot.Project.Structure.FiniteElems;
//            IRobotNumbersArray array = null;
//            IRobotPointsArray ptarray = null;

//            foreach (BHoME.FEMesh mesh in meshes)
//            {               
//                RobotObjObject rpanel = null;
//                object number = mesh[key];
//                int panelNum = 0;

//                if (mesh.PanelProperty != null && mesh.PanelProperty is BHoMP.LoadingPanelProperty)
//                {
//                    foreach (BHoME.FEFace face in mesh.Faces)
//                    {
//                        ptarray = new RobotPointsArray();
//                        if (face.IsQuad)
//                        {
//                            // Robot Points Array
//                            ptarray.SetSize(4);
//                            ptarray.Set(1, mesh.Nodes[face.NodeIndices[0]].X, mesh.Nodes[face.NodeIndices[0]].Y, mesh.Nodes[face.NodeIndices[0]].Z);
//                            ptarray.Set(2, mesh.Nodes[face.NodeIndices[1]].X, mesh.Nodes[face.NodeIndices[1]].Y, mesh.Nodes[face.NodeIndices[1]].Z);
//                            ptarray.Set(3, mesh.Nodes[face.NodeIndices[2]].X, mesh.Nodes[face.NodeIndices[2]].Y, mesh.Nodes[face.NodeIndices[2]].Z);
//                            ptarray.Set(4, mesh.Nodes[face.NodeIndices[3]].X, mesh.Nodes[face.NodeIndices[3]].Y, mesh.Nodes[face.NodeIndices[3]].Z);
//                        }
//                        else
//                        {
//                            ptarray.SetSize(3);
//                            ptarray.Set(1, mesh.Nodes[face.NodeIndices[0]].X, mesh.Nodes[face.NodeIndices[0]].Y, mesh.Nodes[face.NodeIndices[0]].Z);
//                            ptarray.Set(1, mesh.Nodes[face.NodeIndices[1]].X, mesh.Nodes[face.NodeIndices[1]].Y, mesh.Nodes[face.NodeIndices[1]].Z);
//                            ptarray.Set(3, mesh.Nodes[face.NodeIndices[2]].X, mesh.Nodes[face.NodeIndices[2]].Y, mesh.Nodes[face.NodeIndices[2]].Z);
//                        }

//                        RobotPointsArray ptArray = ptarray as RobotPointsArray;
//                        panelNum = objServer.FreeNumber;
//                        objServer.CreateContour(panelNum, ptArray);
//                        rpanel = objServer.Get(panelNum) as RobotObjObject;

//                        BHoMP.LoadingPanelProperty loadProp = mesh.PanelProperty as BHoMP.LoadingPanelProperty;

//                        if (loadProp.LoadApplication == BHoMP.LoadPanelSupportConditions.AllSides)
//                        {
//                            rpanel.SetLabel(IRobotLabelType.I_LT_CLADDING, "Two-way");
//                        }
//                        if (loadProp.LoadApplication == BHoMP.LoadPanelSupportConditions.TwoSides && loadProp.ReferenceEdge % 2 == 1)
//                        {
//                            rpanel.SetLabel(IRobotLabelType.I_LT_CLADDING, "One-way X");
//                        }
//                        if (loadProp.LoadApplication == BHoMP.LoadPanelSupportConditions.TwoSides && loadProp.ReferenceEdge % 2 == 0)
//                        {
//                            rpanel.SetLabel(IRobotLabelType.I_LT_CLADDING, "One-way Y");
//                        }
//                        rpanel.Update();
//                    }
//                }

//                else
//                {
//                    if (NodeIO.CreateNodes(robot, mesh.Nodes, out nodeIds))
//                    {
//                        foreach (BHoME.FEFace face in mesh.Faces)
//                        {
//                            array = new RobotNumbersArray();
//                            if (face.IsQuad)
//                            {
//                                // Robot Numbers Array
//                                array.SetSize(4);
//                                array.Set(1, int.Parse(mesh.Nodes[face.NodeIndices[0]][key].ToString()));
//                                array.Set(2, int.Parse(mesh.Nodes[face.NodeIndices[1]][key].ToString()));
//                                array.Set(3, int.Parse(mesh.Nodes[face.NodeIndices[2]][key].ToString()));
//                                array.Set(4, int.Parse(mesh.Nodes[face.NodeIndices[3]][key].ToString()));
//                            }
//                            else
//                            {
//                                array.SetSize(3);
//                                array.Set(1, int.Parse(mesh.Nodes[face.NodeIndices[0]][key].ToString()));
//                                array.Set(2, int.Parse(mesh.Nodes[face.NodeIndices[1]][key].ToString()));
//                                array.Set(3, int.Parse(mesh.Nodes[face.NodeIndices[2]][key].ToString()));
//                            }

//                            feIds.Add(feServer.FreeNumber);
//                            feServer.Create(feIds[feIds.Count - 1], array);
//                        }
//                    }

//                    if (number != null && int.TryParse(number.ToString(), out panelNum) && panelNum > 0)
//                    {
//                        if (objServer.Exist(panelNum) == -1)
//                        {
//                            rpanel = objServer.Get(panelNum) as RobotObjObject;
//                        }
//                        rpanel = objServer.Get(panelNum) as RobotObjObject;
//                        if (rpanel != null)
//                        {
//                            string thick = rpanel.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) > 0 ?
//                                rpanel.GetLabelName(IRobotLabelType.I_LT_PANEL_THICKNESS) : "";

//                            string reo = rpanel.HasLabel(IRobotLabelType.I_LT_PANEL_REINFORCEMENT) > 0 ?
//                                rpanel.GetLabelName(IRobotLabelType.I_LT_PANEL_REINFORCEMENT) : "";

//                            string calcModel = rpanel.HasLabel(IRobotLabelType.I_LT_PANEL_CALC_MODEL) > 0 ?
//                                rpanel.GetLabelName(IRobotLabelType.I_LT_PANEL_CALC_MODEL) : "";

//                            objServer.Delete(panelNum);
//                            objServer.CreateOnFiniteElems(Utils.GetSelectionString(feIds), panelNum);

//                            rpanel = objServer.Get(panelNum) as RobotObjObject;
//                            if (!string.IsNullOrEmpty(thick)) rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, thick);
//                            if (!string.IsNullOrEmpty(reo)) rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_REINFORCEMENT, reo);
//                            if (!string.IsNullOrEmpty(calcModel)) rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_CALC_MODEL, calcModel);
//                        }
//                    }
//                    else if (mesh.PanelProperty != null && mesh.PanelProperty is BHoMP.ConstantThickness)
//                    {                        
//                            panelNum = objServer.FreeNumber;
//                            objServer.CreateOnFiniteElems(Utils.GetSelectionString(feIds), panelNum);
//                            rpanel = objServer.Get(panelNum) as RobotObjObject;

//                            PropertyIO.CreateThicknessProperty(robot, mesh.PanelProperty);
//                            rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, mesh.PanelProperty.Name);                     
//                    }
//                    string id = panelNum.ToString();

//                    ids.Add(id);


//                    if (!string.IsNullOrEmpty(id))
//                    {
//                        if (mesh.CustomData.ContainsKey(key))
//                        {
//                            mesh.CustomData[key] = id.Trim();
//                        }
//                        else
//                        {
//                            mesh.CustomData.Add(key, id.Trim());
//                        }
//                    }
//                }
//            }
//            return true;
//        }

//        internal static List<string> GetMeshes(RobotApplication robot, out List<BHoME.FEMesh> meshes, ObjectSelection selection, List<string> ids)
//        {
//            BHoMB.ObjectManager<string, BHoME.FEMesh> panels = new BHoMB.ObjectManager<string, BHoME.FEMesh>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);
//            BHoMB.ObjectManager<string, BHoME.Node> nodeManager = new BHoMB.ObjectManager<string, BHoME.Node>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);
//            BHoMB.ObjectManager<BHoMP.PanelProperty> thicknesses = new BHoMB.ObjectManager<BHoMP.PanelProperty>();
//            BHoMB.ObjectManager<BHoMM.Material> materials = new BHoMB.ObjectManager<BHoMM.Material>();
//            List<BHoME.Node> nodes = new List<BHoME.Node>();
//            Dictionary<string, int> nodeNumberToIndex = new Dictionary<string, int>();

//            RobotSelection finiteElementSelection = null;
//            IRobotCollection fECollection;
//            RobotSelection panel_sel = selection == ObjectSelection.Selected ?
//                robot.Project.Structure.Selections.Get(IRobotObjectType.I_OT_PANEL) :
//                robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);

//            if (selection == ObjectSelection.FromInput)
//            {
//                panel_sel.FromText(Utils.GetSelectionString(ids));
//            }
//            else if (selection == ObjectSelection.All)
//            {
//                panel_sel.FromText("all");
//            }

//            IRobotCollection panel_col = robot.Project.Structure.Objects.GetMany(panel_sel);
//            IRobotLabel thickness = null;
//            IRobotLabel material = null;
//            RobotObjMesh rMesh = null;
//            List<string> outIds = new List<string>();

//            robot.Project.Structure.Objects.BeginMultiOperation();

//            for (int i = 1; i <= panel_col.Count; i++)
//            {
//                RobotObjObject rpanel = (RobotObjObject)panel_col.Get(i);
                
//                if (rpanel.Main.GetGeometry().Type == IRobotGeoObjectType.I_GOT_CONTOUR && rpanel.Main.Attribs.Meshed == 1)
//                {
//                    outIds.Add(rpanel.Number.ToString());
//                    rMesh = rpanel.Mesh;
//                    if (rMesh.IsGenerated)
//                    {
//                        List<string> nodeIds = NodeIO.GetNodes(robot, out nodes, ObjectSelection.FromInput, Utils.GetIdsAsTextFromText(rpanel.Nodes));
//                        for (int idx = 0; idx < nodeIds.Count; idx++)
//                        {
//                            nodeNumberToIndex.Add(nodeIds[i], idx);
//                        }

//                        finiteElementSelection = robot.Project.Structure.Selections.Get(IRobotObjectType.I_OT_FINITE_ELEMENT);
//                        finiteElementSelection.FromText(rpanel.FiniteElems);
//                        fECollection = robot.Project.Structure.FiniteElems.GetMany(finiteElementSelection);

//                        BHoME.FEMesh mesh = new BHoME.FEMesh();

//                        int panelNum = rpanel.Number;
//                        panels.Add(panelNum.ToString(), mesh);

//                        mesh.Nodes = nodes;
//                        for (int fE = 1; fE <= fECollection.Count; fE++)
//                        {
//                            BHoME.FEFace face = new BHoME.FEFace();
//                            face.NodeIndices = new List<int>();
//                            RobotFiniteElement element = fECollection.Get(fE) as RobotFiniteElement;

//                            for (int nodeIdx = 1; nodeIdx <= element.Nodes.Count; nodeIdx++)
//                            {
//                                int nodeNumber = element.Nodes.Get(nodeIdx);
//                                int vertexIndex = 0;
//                                if (nodeNumberToIndex.TryGetValue(nodeNumber.ToString(), out vertexIndex))
//                                {
//                                    face.NodeIndices.Add(vertexIndex);
//                                }
//                            }
//                        }

//                        if (rpanel.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) != 0)
//                        {
//                            thickness = rpanel.GetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS);

//                            BHoMP.PanelProperty property = thicknesses[thickness.Name];
//                            if (property == null)
//                            {
//                                property = thicknesses.Add(thickness.Name, PropertyIO.GetThickness(thickness, PropertyIO.GetPanelType(rpanel.StructuralType)));
//                            }
//                            mesh.PanelProperty = property;

//                            if (mesh.PanelProperty != null && rpanel.HasLabel(IRobotLabelType.I_LT_MATERIAL) != 0)
//                            {
//                                material = rpanel.GetLabel(IRobotLabelType.I_LT_MATERIAL);
//                                BHoMM.Material m = materials[material.Name];
//                                if (material == null)
//                                {
//                                    m = materials.Add(material.Name, PropertyIO.GetMaterial(material));
//                                }
//                                mesh.PanelProperty.Material = m;
//                            }
//                        }
//                    }
//                }
//            }

//            meshes = panels.GetRange(outIds);
//            return outIds;
//        }
//    }
//}

