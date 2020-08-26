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

using System.Collections.Generic;
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

        private List<FEMesh> ReadMeshes(List<string> ids = null)
        {
            SortedDictionary<int, FEMesh> bhomMeshes = new SortedDictionary<int, FEMesh>();

            List<FEMeshFace> meshFaces = new List<FEMeshFace>();

            Dictionary<int, Node> bhomNodes = ReadNodesQuery().ToDictionary(x => System.Convert.ToInt32(x.CustomData[AdapterIdName]));

            Dictionary<int, List<Node>> meshNodes_allMeshes = new Dictionary<int, List<Node>>();

            Dictionary<int, List<int>> meshNodeIds_allMeshes = new Dictionary<int, List<int>>();

            RobotResultQueryParams queryParams = m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            RobotSelection fe_sel = m_RobotApplication.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_FINITE_ELEMENT);

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
                while (isOk)
                {
                    RobotResultRow row = rowSet.CurrentRow;

                    int panelNumber = System.Convert.ToInt32(row.GetValue(1252));

                    List<int> meshNodeIds = (meshNodeIds_allMeshes.ContainsKey(panelNumber) ? meshNodeIds_allMeshes[panelNumber] : new List<int>());
                    List<int> meshFaceNodeIds = new List<int>();
                    FEMeshFace meshFace = new FEMeshFace();

                    int[] robotNodeIds = new int[3];

                    int faceId = row.GetParam(IRobotResultParamType.I_RPT_ELEMENT);
                    meshFace.CustomData[AdapterIdName] = faceId;

                    robotNodeIds[0] = System.Convert.ToInt32(row.GetValue(564));
                    robotNodeIds[1] = System.Convert.ToInt32(row.GetValue(565));
                    robotNodeIds[2] = System.Convert.ToInt32(row.GetValue(566));
                    if (row.IsAvailable(567))
                    {
                        System.Array.Resize(ref robotNodeIds, 4);
                        robotNodeIds[3] = System.Convert.ToInt32(row.GetValue(567));
                    }

                    for (int i = 0; i < robotNodeIds.Count(); i++)
                    {
                        if (!meshNodeIds.Contains(robotNodeIds[i]))
                        {
                            meshNodeIds.Add(robotNodeIds[i]);                            
                        }
                        meshFace.NodeListIndices.Add(meshNodeIds.IndexOf(robotNodeIds[i]));
                    }                    

                    if (!meshNodeIds_allMeshes.ContainsKey(panelNumber))
                        meshNodeIds_allMeshes.Add(panelNumber, meshNodeIds);


                    meshFaces.Add(meshFace);

                    if (bhomMeshes.ContainsKey(panelNumber))
                    {
                        bhomMeshes[panelNumber].Faces.Add(meshFace);
                        bhomMeshes[panelNumber].CustomData[AdapterIdName] = panelNumber;
                    }
                    else
                    {
                        FEMesh mesh = new FEMesh();
                        mesh.Faces.Add(meshFace);
                        mesh.CustomData[AdapterIdName] = panelNumber;
                        bhomMeshes.Add(panelNumber, mesh);
                    }
                    isOk = rowSet.MoveNext();
                }
                rowSet.Clear();
            }
            queryParams.Reset();
            foreach (int panelId in meshNodeIds_allMeshes.Keys)
            {
                foreach (int nodeId in meshNodeIds_allMeshes[panelId])
                {
                    bhomMeshes[panelId].Nodes.Add(bhomNodes[nodeId]);
                }
            }

            RobotObjObjectServer robotObjectServer = m_RobotApplication.Project.Structure.Objects;

            foreach (KeyValuePair<int, FEMesh> kvp in bhomMeshes)
            {
                int id = kvp.Key;
                FEMesh mesh = kvp.Value;

                //Get local orientations for each face
                List<Vector> normals = mesh.Normals();

                //Check if all orientations are the same
                bool sameOrientation = true;

                for (int i = 0; i < normals.Count - 1; i++)
                {
                    sameOrientation &= normals[i].Angle(normals[i + 1]) < Tolerance.Angle;
                }

                if (sameOrientation)
                {
                    Vector normal = normals.First();
                    RobotObjObject robotPanel = robotObjectServer.Get(id) as RobotObjObject;

                    double x, y, z; robotPanel.Main.Attribs.GetDirX(out x, out y, out z);

                    bool flip = robotPanel.Main.Attribs.DirZ == 1;
                    flip = Convert.FromRobotCheckFlipNormal(normal, flip);

                    if (flip)
                    {
                        normal = normal.Reverse();
                        foreach (FEMeshFace face in mesh.Faces)
                            face.NodeListIndices.Reverse();
                    }

                    //Set local orientation
                    double orientationAngle = Engine.Structure.Compute.OrientationAngleAreaElement(normal, new Vector { X = x, Y = y, Z = z });
                    mesh.Faces.ForEach(f => f.OrientationAngle = orientationAngle);
                }
                else
                {
                    Engine.Reflection.Compute.RecordWarning("Local orientations of FEMeshes with varying orientations can not be extracted.");
                }
            }

            return bhomMeshes.Values.ToList();
        }

        /***************************************************/
    }

}


