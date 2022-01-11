/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using System.Collections.Generic;
using System.Collections;
using BH.oM.Structure.Elements;
using RobotOM;
using System.Linq;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Structure;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<FEMesh> ReadMeshes(IList ids = null)
        {
            SortedDictionary<int, FEMesh> bhomMeshes = new SortedDictionary<int, FEMesh>();

            Dictionary<int, Node> bhomNodes = ReadNodesQuery().ToDictionary(x => GetAdapterId<int>(x));

            Dictionary<int, List<int>> meshNodeIds_allMeshes = new Dictionary<int, List<int>>();

            RobotResultQueryParams queryParams = m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);


            //Setting case selection to only pull the mesh faces once
            RobotSelection caseSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            try
            {
                caseSelection.FromText(m_RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
            }
            catch
            {
                m_RobotApplication.Project.Structure.Cases.CreateSimple(1, "Dead Load", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                caseSelection.FromText(m_RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
            }


            RobotSelection fe_sel;
            if (ids == null || ids.Count == 0)
            {
                fe_sel = m_RobotApplication.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_FINITE_ELEMENT);
            }
            else
            {
                fe_sel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_FINITE_ELEMENT);
                List<int> panelIds = CheckAndGetIds<Panel>(ids); //Using Panel as type as this enables to ask for the FEMesh while providing the corresponding Panel obejcts
                RobotSelection panelSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);
                panelSelection.FromText(Convert.ToRobotSelectionString(panelIds));
                IRobotCollection robotPanels = m_RobotApplication.Project.Structure.Objects.GetMany(panelSelection);

                for (int i = 1; i <= robotPanels.Count; i++)
                {
                    RobotObjObject robotPanel = (RobotObjObject)robotPanels.Get(i);
                    fe_sel.AddText(robotPanel.FiniteElems);
                }
            }


            queryParams.ResultIds.SetSize(5);
            queryParams.ResultIds.Set(1, 564);    //Corresponds to first node number of the mesh face topology          
            queryParams.ResultIds.Set(2, 565);    //Corresponds to second node number of the mesh face topology
            queryParams.ResultIds.Set(3, 566);    //Corresponds to third node number of the mesh face topology
            queryParams.ResultIds.Set(4, 567);    //Corresponds to fourth node number of the mesh face topology (if it exists/mesh face is 4 node)
            queryParams.ResultIds.Set(5, 1252);   //Corresponds to the panel number to which the mesh face belongs

            queryParams.Selection.Set(IRobotObjectType.I_OT_FINITE_ELEMENT, fe_sel);
            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            queryParams.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            queryParams.SetParam(IRobotResultParamType.I_RPT_SMOOTHING, IRobotFeResultSmoothing.I_FRS_IN_ELEMENT_CENTER);

            RobotResultRowSet rowSet = new RobotResultRowSet();
            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;

            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = m_RobotApplication.Kernel.Structure.Results.Query(queryParams, rowSet);

                bool isOk = rowSet.MoveFirst();

                //Fetches each face, one by one
                while (isOk)
                {
                    RobotResultRow row = rowSet.CurrentRow;

                    int panelNumber;
                    if (row.IsAvailable(1252))
                        panelNumber = System.Convert.ToInt32(row.GetValue(1252));
                    else
                    {
                        Engine.Base.Compute.RecordError("At least one FEMeshFace could not be extracted from Robot");
                        isOk = rowSet.MoveNext();
                        continue;
                    }

                    List<int> meshNodeIds;

                    if (!meshNodeIds_allMeshes.TryGetValue(panelNumber, out meshNodeIds))
                    {
                        meshNodeIds = new List<int>();
                        meshNodeIds_allMeshes[panelNumber] = meshNodeIds;
                    }
                    
                    List<int> meshFaceNodeIds = new List<int>();
                    FEMeshFace meshFace = new FEMeshFace();

                    List<int> robotNodeIds = new List<int>();

                    int faceId = row.GetParam(IRobotResultParamType.I_RPT_ELEMENT);
                    SetAdapterId(meshFace, faceId);

                    if (row.IsAvailable(564) && row.IsAvailable(565) && row.IsAvailable(566))
                    {
                        robotNodeIds.Add(System.Convert.ToInt32(row.GetValue(564)));
                        robotNodeIds.Add(System.Convert.ToInt32(row.GetValue(565)));
                        robotNodeIds.Add(System.Convert.ToInt32(row.GetValue(566)));
                    }
                    else
                    {
                        Engine.Base.Compute.RecordError($"Failed to extract topological information for at least one FEMeshFace for FEMesh with index {panelNumber}. This face will not be pulled.");
                        isOk = rowSet.MoveNext();
                        continue;
                    }


                    if (row.IsAvailable(567))
                    {
                        robotNodeIds.Add(System.Convert.ToInt32(row.GetValue(567)));
                    }

                    for (int i = 0; i < robotNodeIds.Count(); i++)
                    {
                        if (!meshNodeIds.Contains(robotNodeIds[i]))
                        {
                            meshNodeIds.Add(robotNodeIds[i]);                            
                        }
                        meshFace.NodeListIndices.Add(meshNodeIds.IndexOf(robotNodeIds[i]));
                    }                    

                    if (bhomMeshes.ContainsKey(panelNumber))
                    {
                        bhomMeshes[panelNumber].Faces.Add(meshFace);
                    }
                    else
                    {
                        FEMesh mesh = new FEMesh();
                        mesh.Faces.Add(meshFace);
                        bhomMeshes.Add(panelNumber, mesh);
                    }
                    isOk = rowSet.MoveNext();
                }
                rowSet.Clear();
            }
            queryParams.Reset();

            foreach (KeyValuePair<int,List<int>> kvp in meshNodeIds_allMeshes)
            {
                FEMesh mesh;
                if (bhomMeshes.TryGetValue(kvp.Key, out mesh))
                {
                    SetAdapterId(mesh, kvp.Key);
                    foreach (int nodeId in kvp.Value)
                    {
                        Node node;
                        if (bhomNodes.TryGetValue(nodeId, out node))
                        {
                            mesh.Nodes.Add(node);
                        }
                        else
                            Engine.Base.Compute.RecordError($"Failed to find node with id {nodeId}. FEMesh with id {kvp.Key} might be invalid.");
                    }
                }
                else
                    Engine.Base.Compute.RecordError($"Failed to assign nodes to FEMesh with id {kvp.Key}.");
            }


            RobotObjObjectServer robotObjectServer = m_RobotApplication.Project.Structure.Objects;

            foreach (KeyValuePair<int, FEMesh> kvp in bhomMeshes)
            {
                int id = kvp.Key;
                FEMesh mesh = kvp.Value;

                try
                {
                    //Get local orientations for each face
                    List<Vector> normals = mesh.Normals().Where(x => x!= null).ToList();

                    //Check if all orientations are the same
                    bool sameOrientation = true;

                    for (int i = 0; i < normals.Count - 1; i++)
                    {
                        sameOrientation &= normals[i].Angle(normals[i + 1]) < Tolerance.Angle;
                        if (!sameOrientation)
                            break;
                    }

                    if (sameOrientation && normals.Count != 0)
                    {
                        Vector normal = normals.First();
                        RobotObjObject robotPanel = robotObjectServer.Get(id) as RobotObjObject;

                        double x, y, z;
                        robotPanel.Main.Attribs.GetDirX(out x, out y, out z);

                        bool flip = robotPanel.Main.Attribs.DirZ == 1;
                        flip = Convert.FromRobotCheckFlipNormal(normal, flip);

                        if (flip)
                        {
                            normal *= -1;
                            foreach (FEMeshFace face in mesh.Faces)
                                face.NodeListIndices.Reverse();
                        }

                        //Set local orientation
                        double orientationAngle = Engine.Structure.Compute.OrientationAngleAreaElement(normal, new Vector { X = x, Y = y, Z = z });
                        mesh.Faces.ForEach(f => f.OrientationAngle = orientationAngle);
                    }
                    else
                    {
                        Engine.Base.Compute.RecordWarning("Local orientations of FEMeshes with varying orientations can not be extracted.");
                    }
                }
                catch (System.Exception e)
                {
                    string message = "Failed to read local orientations for an FEMesh. Exception message: " + e.Message;

                    if (!string.IsNullOrEmpty(e.InnerException?.Message))
                    {
                        message += "\nInnerException: " + e.InnerException.Message;
                    }

                    Engine.Base.Compute.RecordWarning(message);
                }
                
            }

            return bhomMeshes.Values.ToList();
        }

        /***************************************************/
    }

}




