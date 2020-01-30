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
                string faceList = "";

                IRobotStructure objServer = m_RobotApplication.Project.Structure;
                RobotObjObject mesh = null;
                fEMesh.Faces = new List<FEMeshFace>(fEMesh.Faces);

                for (int i = 0; i < fEMesh.Faces.Count; i++)
                {
                    FEMeshFace fMeshFace = fEMesh.Faces[i];

                    IRobotNumbersArray ptarray = new RobotNumbersArray();
                    if (fMeshFace.NodeListIndices.Count == 3)
                    {
                        ptarray.SetSize(3);
                        ptarray.Set(1, System.Convert.ToInt32(fEMesh.Nodes[fMeshFace.NodeListIndices[0]].CustomData[AdapterIdName]));
                        ptarray.Set(2, System.Convert.ToInt32(fEMesh.Nodes[fMeshFace.NodeListIndices[1]].CustomData[AdapterIdName]));
                        ptarray.Set(3, System.Convert.ToInt32(fEMesh.Nodes[fMeshFace.NodeListIndices[2]].CustomData[AdapterIdName]));
                    }
                    else if (fMeshFace.NodeListIndices.Count == 4)
                    {
                        ptarray.SetSize(4);
                        ptarray.Set(1, System.Convert.ToInt32(fEMesh.Nodes[fMeshFace.NodeListIndices[0]].CustomData[AdapterIdName]));
                        ptarray.Set(2, System.Convert.ToInt32(fEMesh.Nodes[fMeshFace.NodeListIndices[1]].CustomData[AdapterIdName]));
                        ptarray.Set(3, System.Convert.ToInt32(fEMesh.Nodes[fMeshFace.NodeListIndices[2]].CustomData[AdapterIdName]));
                        ptarray.Set(4, System.Convert.ToInt32(fEMesh.Nodes[fMeshFace.NodeListIndices[3]].CustomData[AdapterIdName]));
                    }

                    FEMeshFace clone = fMeshFace.GetShallowClone() as FEMeshFace;
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
                if (fEMesh.Property is LoadingPanelProperty)
                    mesh.SetLabel(IRobotLabelType.I_LT_CLADDING, fEMesh.Property.Name);

                else
                    mesh.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, fEMesh.Property.Name);
            }

            return true;
        }

        /***************************************************/

    }
}
