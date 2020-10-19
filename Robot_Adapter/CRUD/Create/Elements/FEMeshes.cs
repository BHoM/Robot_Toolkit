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
using System.Linq;
using System;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SurfaceProperties;
using BH.Engine.Structure;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<FEMesh> fEMeshes)
        {
            int fMeshFaceIdx = m_RobotApplication.Project.Structure.FiniteElems.FreeNumber;
            foreach (FEMesh fEMesh in fEMeshes)
            {
                if (!CheckNotNull(fEMesh))
                    continue;

                string faceList = "";

                IRobotStructure objServer = m_RobotApplication.Project.Structure;
                RobotObjObject mesh = null;
                fEMesh.Faces = new List<FEMeshFace>(fEMesh.Faces);

                for (int i = 0; i < fEMesh.Faces.Count; i++)
                {
                    FEMeshFace fMeshFace = fEMesh.Faces[i];

                    if (!CheckNotNull(fMeshFace, oM.Reflection.Debugging.EventType.Error, typeof(FEMesh)))
                        continue;


                    if (fMeshFace.NodeListIndices.Count < 3 || fMeshFace.NodeListIndices.Count > 4)
                    {
                        Engine.Reflection.Compute.RecordError("The Robot adapter can only handle mesh faces with three or four nodes. Face with more indecies not pushed to Robot.");
                        continue;
                    }

                    IRobotNumbersArray ptarray = new RobotNumbersArray();
                    ptarray.SetSize(fMeshFace.NodeListIndices.Count);

                    bool createNodesSuccess = true;

                    for (int j = 0; j < fMeshFace.NodeListIndices.Count; j++)
                    {
                        Node node = fEMesh.Nodes[fMeshFace.NodeListIndices[i]];
                        //Checks that the node is not null and has AdapterId assigned
                        if (createNodesSuccess &= CheckInputObject(node, oM.Reflection.Debugging.EventType.Error, typeof(FEMesh)))
                        {
                            ptarray.Set(i, System.Convert.ToInt32(fEMesh.Nodes[fMeshFace.NodeListIndices[i]].CustomData[AdapterIdName]));
                        }
                        else
                            break;

                    }

                    if (!createNodesSuccess)
                        continue;

                    FEMeshFace clone = fMeshFace.GetShallowClone() as FEMeshFace;

                    if (clone.CustomData == null)
                        clone.CustomData = new Dictionary<string, object>();

                    clone.CustomData[AdapterIdName] = fMeshFaceIdx;
                    fEMesh.Faces[i] = clone;

                    faceList = faceList + fMeshFaceIdx.ToString() + ",";

                    objServer.FiniteElems.Create(fMeshFaceIdx, ptarray);

                    fMeshFaceIdx += 1;
                }

                faceList.TrimEnd(',');

                int elemNumber = objServer.Objects.FreeNumber;
                fEMesh.CustomData[AdapterIdName] = elemNumber;
                objServer.Objects.CreateOnFiniteElems(faceList, elemNumber);
                mesh = objServer.Objects.Get(elemNumber) as RobotObjObject;

                //Get local orientations for each face
                try
                {
                    List<Basis> orientations = fEMesh.LocalOrientations();

                    //Check if all orientations are the same
                    bool sameOrientation = true;

                    for (int i = 0; i < orientations.Count - 1; i++)
                    {
                        sameOrientation &= orientations[i].X.Angle(orientations[i + 1].X) < Tolerance.Angle;
                        sameOrientation &= orientations[i].Z.Angle(orientations[i + 1].Z) < Tolerance.Angle;

                        if (!sameOrientation)
                            break;
                    }

                    if (orientations.Count != 0 && sameOrientation)
                    {
                        mesh.Main.Attribs.DirZ = Convert.ToRobotFlipPanelZ(orientations.First().Z);
                        Vector xDir = orientations.First().X;
                        mesh.Main.Attribs.SetDirX(IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN, xDir.X, xDir.Y, xDir.Z);
                        mesh.Update();
                    }
                    else
                    {
                        Engine.Reflection.Compute.RecordWarning("Local orientions of the pushed FEMesh varies across the faces. Could not set local orientations to Robot.");
                    }
                }
                catch (Exception)
                {
                    Engine.Reflection.Compute.RecordWarning("Failed to set local orientations for FEMesh.");
                }


                if (CheckNotNull(fEMesh.Property, oM.Reflection.Debugging.EventType.Warning, typeof(FEMesh)))
                {
                    if (fEMesh.Property is LoadingPanelProperty)
                        mesh.SetLabel(IRobotLabelType.I_LT_CLADDING, fEMesh.Property.DescriptionOrName());

                    else
                        mesh.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, fEMesh.Property.DescriptionOrName());
                }
            }

            return true;
        }


        /***************************************************/

    }
}
