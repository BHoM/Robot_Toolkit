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
using BH.oM.Structure.Results;
using RobotOM;
using System.Linq;
using System.Collections.ObjectModel;
using BH.oM.Analytical.Results;
using BH.oM.Structure.Requests;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.Adapter;
using BH.Engine.Structure;
using BH.Engine.Base;
using BH.oM.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public IEnumerable<IResult> ReadResults(MeshResultRequest request, ActionConfig actionConfig = null)
        {
            MeshResultLayer layer = request.Layer;
            MeshResultSmoothingType smoothing = request.Smoothing;

            if (smoothing == MeshResultSmoothingType.None)
            {
                Engine.Reflection.Compute.RecordWarning("Result extraction with no smoothing is currently not possible in the Robot adapter. To extract results please choose another smoothening type");
                return new List<IResult>();
            }

            double layerPosition = request.LayerPosition;
            IRobotFeLayerType robotMeshLayer = Convert.ToRobot(request.Layer);
            if (robotMeshLayer == IRobotFeLayerType.I_FLT_LOWER)
                layerPosition = 0;
            if (robotMeshLayer == IRobotFeLayerType.I_FLT_MIDDLE)
                layerPosition = 0.5;
            if (robotMeshLayer == IRobotFeLayerType.I_FLT_UPPER)
                layerPosition = 1;

            IRobotFeResultSmoothing robotFESmoothing = Convert.ToRobot(request.Smoothing);

            IRobotStructure robotStructureServer = m_RobotApplication.Project.Structure;

            List<BH.oM.Structure.Elements.Node> nodes = ReadNodesQuery();
            List<BH.oM.Structure.Elements.FEMesh> feMeshList = new List<oM.Structure.Elements.FEMesh>();
            
            if (request.ObjectIds == null || request.ObjectIds.Count == 0)
            {
                feMeshList = ReadMeshes();
            }
            else
            {
                List<object> meshIds = new List<object>();
                foreach (object obj in request.ObjectIds)
                {
                    if (obj is oM.Structure.Elements.FEMesh)
                        feMeshList.Add(obj as oM.Structure.Elements.FEMesh);
                    else
                        meshIds.Add(obj);
                }
                if (meshIds.Count > 0)
                {
                    feMeshList.AddRange(ReadMeshes(meshIds));
                }
            }

            List<Point> nodePointList = nodes.Select(x => Engine.Structure.Query.Position(x)).ToList();

            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            List<int> results = new List<int>();
            results.AddRange(MeshResultParameters(request));
            results.Add(0);
            results.Add(1);
            results.Add(2);

            queryParams.ResultIds.SetSize(results.Count);

            for (int j = 0; j < results.Count; j++)
            {
                int id = (int)results[j];
                queryParams.ResultIds.Set(j + 1, id);
            }

            RobotSelection caseSelection = GetCaseSelection(request);

            Basis globalXY = Basis.XY;

            if (request.ResultType == MeshResultType.Displacements)
            {
                Basis orientation = request.Orientation;

                if (orientation != null)
                {
                    bool same = orientation.X.Angle(globalXY.X) < Tolerance.Angle;
                    same &= orientation.Y.Angle(globalXY.Y) < Tolerance.Angle;
                    same &= orientation.Z.Angle(globalXY.Z) < Tolerance.Angle;

                    if (!same)
                        Engine.Reflection.Compute.RecordWarning("Mesh Displacements are always extracted in Global coordinates");
                }
            }

            List<MeshResult> meshResultsCollection = new List<MeshResult>();
            foreach (BH.oM.Structure.Elements.FEMesh feMesh in feMeshList)
            {
                Basis orientation = request.Orientation;

                if (orientation == null)
                {
                    try
                    {
                        //Get local orientations for each face
                        List<Basis> orientations = feMesh.LocalOrientations();

                        //Check if all orientations are the same
                        bool sameOrientation = true;

                        for (int i = 0; i < orientations.Count - 1; i++)
                        {
                            sameOrientation &= orientations[i].Z.Angle(orientations[i].Z) < Tolerance.Angle;
                            if (!sameOrientation)
                                break;
                        }

                        if (sameOrientation && orientations.Count > 0)
                            orientation = orientations.First();
                    }
                    catch (System.Exception)
                    {
                        Engine.Reflection.Compute.RecordWarning($"Could not extract local orientation for FEMesh with id {GetAdapterId<int>(feMesh)}. Default orientation will be used for this FEMesh.");
                    }
                }

                List<MeshElementResult> meshResults = new List<MeshElementResult>();

                RobotSelection panelSelection = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_PANEL);
                panelSelection.FromText(GetAdapterId<int>(feMesh).ToString());

                IEnumerable<string> nodeIdList = feMesh.Nodes.Select(x => GetAdapterId(x).ToString());
                string nodeIds = string.Join(" ", nodeIdList);

                IEnumerable<string> faceIdList = feMesh.Faces.Select(x => GetAdapterId(x).ToString());
                string faceIds = string.Join(" ", faceIdList);

                if (nodeIds == "" || faceIds == "")
                {
                    Engine.Reflection.Compute.RecordWarning($"Could not access finite element ids for FEMesh with id : { GetAdapterId<int>(feMesh) }. No results will be extracted for this element.");
                    continue;
                }

                RobotSelection finiteElementSelection = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_FINITE_ELEMENT);
                finiteElementSelection.FromText(faceIds);

                RobotSelection nodeSelection = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_NODE);
                nodeSelection.FromText(nodeIds);

                queryParams.Selection.Set(IRobotObjectType.I_OT_PANEL, panelSelection);
                queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
                queryParams.Selection.Set(IRobotObjectType.I_OT_FINITE_ELEMENT, finiteElementSelection);
                queryParams.Selection.Set(IRobotObjectType.I_OT_NODE, nodeSelection);
                queryParams.SetParam(IRobotResultParamType.I_RPT_NODE, 1);
                queryParams.SetParam(IRobotResultParamType.I_RPT_ELEMENT, 2);
                queryParams.SetParam(IRobotResultParamType.I_RPT_LOAD_CASE, 3);
                queryParams.SetParam(IRobotResultParamType.I_RPT_RESULT_POINT_COORDINATES, 4);
                queryParams.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
                queryParams.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
                queryParams.SetParam(IRobotResultParamType.I_RPT_SMOOTHING, (int)robotFESmoothing);
                queryParams.SetParam(IRobotResultParamType.I_RPT_LAYER, (int)robotMeshLayer);
                if (robotMeshLayer == IRobotFeLayerType.I_FLT_ARBITRARY)
                {
                    queryParams.SetParam(IRobotResultParamType.I_RPT_LAYER_ARBITRARY_VALUE, layerPosition);
                }
                if (orientation != null)
                {
                    queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X_DEFTYPE, IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN);
                    queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X, new double[] { orientation.X.X, orientation.X.Y, orientation.X.Z });
                }
                else
                {
                    queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X_DEFTYPE, IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN);
                    queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X, new double[] { 0, 0, 0 });
                }

                RobotResultRowSet rowSet = new RobotResultRowSet();
                IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;

                while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
                {
                    ret = m_RobotApplication.Kernel.Structure.Results.Query(queryParams, rowSet);

                    bool isOk = rowSet.MoveFirst();
                    while (isOk)
                    {
                        RobotResultRow row = rowSet.CurrentRow;

                        int mode = (int)row.GetParam(IRobotResultParamType.I_RPT_MODE);
                        if (request.Modes.Count < 1 || request.Modes.Contains(mode.ToString()))
                        { 
                            int idCase = 0;
                            if (queryParams.IsParamSet(IRobotResultParamType.I_RPT_LOAD_CASE))
                                idCase = System.Convert.ToInt32(row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE));

                            int idFEMesh = GetAdapterId<int>(feMesh);

                            int idNode = 0;
                            if (request.Smoothing != MeshResultSmoothingType.ByFiniteElementCentres)
                            {
                                //idNode = System.Convert.ToInt32(row.GetParam(IRobotResultParamType.I_RPT_NODE));

                                Point nodePoint = BH.Engine.Geometry.Create.Point(row.GetValue(0), row.GetValue(1), row.GetValue(2));
                                idNode = GetAdapterId<int>(nodes.ElementAt(nodePointList.IndexOf(BH.Engine.Geometry.Query.ClosestPoint(nodePoint, nodePointList))));
                            }

                            int idFiniteElement = 0;
                            if (request.Smoothing == MeshResultSmoothingType.ByFiniteElementCentres || request.Smoothing == MeshResultSmoothingType.None)
                                if (queryParams.IsParamSet(IRobotResultParamType.I_RPT_ELEMENT))
                                    idFiniteElement = System.Convert.ToInt32(row.GetParam(IRobotResultParamType.I_RPT_ELEMENT));

                            switch (request.ResultType)
                            {
                                case MeshResultType.Stresses:
                                    meshResults.Add(GetMeshStress(row, idFEMesh, idNode, idFiniteElement, idCase, mode, layer, layerPosition, smoothing, orientation));
                                    break;
                                case MeshResultType.Forces:
                                    meshResults.Add(GetMeshForce(row, idFEMesh, idNode, idFiniteElement, idCase, mode, layer, layerPosition, smoothing, orientation));
                                    break;
                                case MeshResultType.VonMises:
                                    meshResults.Add(GetMeshVonMises(row, idFEMesh, idNode, idFiniteElement, idCase, mode, layer, layerPosition, smoothing, orientation));
                                    break;
                                case MeshResultType.Displacements:
                                    meshResults.Add(GetMeshDisplacement(row, idFEMesh, idNode, idFiniteElement, idCase, mode, layer, layerPosition, smoothing, (Basis)globalXY));
                                    break;
                                case MeshResultType.MeshModeShape:
                                    meshResults.Add(GetMeshModeShape(row, idFEMesh, idNode, idFiniteElement, idCase, mode, layer, layerPosition, smoothing, (Basis)globalXY));
                                    break;
                            }
                        }
                        isOk = rowSet.MoveNext();
                    }
                }

                foreach (var resultByCase in meshResults.GroupBy(x => new { x.ResultCase, x.TimeStep, x.ModeNumber }))
                {
                    System.IComparable loadCase = resultByCase.Key.ResultCase;
                    double timeStep = resultByCase.Key.TimeStep;
                    int modeNumber = resultByCase.Key.ModeNumber;
                    List<MeshElementResult> resultList = resultByCase.ToList();
                    resultList.Sort();
                    MeshResult meshResult = new MeshResult(GetAdapterId<int>(feMesh), loadCase, modeNumber, timeStep, layer, layerPosition, smoothing, new ReadOnlyCollection<MeshElementResult>(resultList));
                    meshResultsCollection.Add(meshResult);
                }
            }

            meshResultsCollection.Sort();
            return meshResultsCollection;
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private MeshStress GetMeshStress(RobotResultRow row, int idFEMesh, int idNode, int idFiniteElement, int idCase, int mode, MeshResultLayer layer, double layerPosition, MeshResultSmoothingType smoothing, oM.Geometry.Basis orientation)
        {
            return new MeshStress(idFEMesh,
                                  idNode,
                                  idFiniteElement,
                                  idCase,
                                  mode,
                                  0,
                                  layer,
                                  layerPosition,
                                  smoothing,
                                  orientation,
                                  TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_SXX),
                                  TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_SYY),
                                  TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_SXY),
                                  TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_TXX),
                                  TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_TYY),
                                  TryGetValue(row, (int)IRobotFeResultType.I_FRT_PRINCIPAL_S1),
                                  TryGetValue(row, (int)IRobotFeResultType.I_FRT_PRINCIPAL_S2),
                                  TryGetValue(row, (int)IRobotFeResultType.I_FRT_PRINCIPAL_S1_2));
        }


        /***************************************************/

        private MeshForce GetMeshForce(RobotResultRow row, int idFEMesh, int idNode, int idFiniteElement, int idCase, int mode, MeshResultLayer layer, double layerPosition, MeshResultSmoothingType smoothing, oM.Geometry.Basis orientation)
        {
            return new MeshForce(idFEMesh,
                                idNode,
                                idFiniteElement,
                                idCase,
                                mode,
                                0,
                                layer,
                                layerPosition,
                                smoothing,
                                orientation,
                                TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_NXX),
                                TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_NYY),
                                TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_NXY),
                                TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_MXX),
                                TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_MYY),
                                TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_MXY),
                                TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_QXX),
                                TryGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_QYY));
        }


        /***************************************************/

        private MeshVonMises GetMeshVonMises(RobotResultRow row, int idFEMesh, int idNode, int idFiniteElement, int idCase, int mode, MeshResultLayer layer, double layerPosition, MeshResultSmoothingType smoothing, oM.Geometry.Basis orientation)
        {
            return new MeshVonMises(idFEMesh,
                                    idNode,
                                    idFiniteElement,
                                    idCase,
                                    mode,
                                    0,
                                    layer,
                                    layerPosition,
                                    smoothing,
                                    orientation,
                                    TryGetValue(row, (int)IRobotFeResultType.I_FRT_COMPLEX_S_MISES),
                                    TryGetValue(row, (int)IRobotFeResultType.I_FRT_COMPLEX_N_MISES),
                                    TryGetValue(row, (int)IRobotFeResultType.I_FRT_COMPLEX_M_MISES));
        }


        /***************************************************/

        private MeshDisplacement GetMeshDisplacement(RobotResultRow row, int idFEMesh, int idNode, int idFiniteElement, int idCase, int mode, MeshResultLayer layer, double layerPosition, MeshResultSmoothingType smoothing, oM.Geometry.Basis orientation)
        {

            Vector u = new Vector
            {
                X = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UX),
                Y = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UY),
                Z = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UZ),
            };

            return new MeshDisplacement(idFEMesh,
                                        idNode,
                                        idFiniteElement,
                                        idCase,
                                        mode,
                                        0,
                                        layer,
                                        layerPosition,
                                        smoothing,
                                        orientation,
                                        u.X,
                                        u.Y,
                                        u.Z,
                                        0,
                                        0,
                                        0);
        }

        /***************************************************/

        private MeshModeShape GetMeshModeShape(RobotResultRow row, int idFEMesh, int idNode, int idFiniteElement, int idCase, int mode, MeshResultLayer layer, double layerPosition, MeshResultSmoothingType smoothing, oM.Geometry.Basis orientation)
        {

            Vector u = new Vector
            {
                X = TryGetValue(row, 234), // T_EIGEN_UX_1
                Y = TryGetValue(row, 235), // T_EIGEN_UY_1
                Z = TryGetValue(row, 236), // T_EIGEN_UZ_1
                // add rotations
            };

            Vector r = new Vector
            {
                X = TryGetValue(row, 237), // T_EIGEN_RX_1
                Y = TryGetValue(row, 238), // T_EIGEN_RY_1
                Z = TryGetValue(row, 239), // T_EIGEN_RZ_1
            };

            return new MeshModeShape(idFEMesh,
                                        idNode,
                                        idFiniteElement,
                                        idCase,
                                        mode,
                                        0,
                                        layer,
                                        layerPosition,
                                        smoothing,
                                        orientation,
                                        u.X,
                                        u.Y,
                                        u.Z,
                                        r.X,
                                        r.Y,
                                        r.Z);
        }

        /***************************************************/


        public static List<int> MeshResultParameters(MeshResultRequest request)
        {
            List<int> results = new List<int>();
            switch (request.ResultType)
            {
                case MeshResultType.Stresses:
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SYY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SXY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_TXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_TYY);
                    results.Add((int)IRobotFeResultType.I_FRT_PRINCIPAL_S1);
                    results.Add((int)IRobotFeResultType.I_FRT_PRINCIPAL_S1_2);
                    results.Add((int)IRobotFeResultType.I_FRT_PRINCIPAL_S2);
                    break;

                case MeshResultType.Forces:
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NYY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NXY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MYY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MXY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_QXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_QYY);
                    break;

                case MeshResultType.VonMises:
                    results.Add((int)IRobotFeResultType.I_FRT_COMPLEX_S_MISES);
                    results.Add((int)IRobotFeResultType.I_FRT_COMPLEX_N_MISES);
                    results.Add((int)IRobotFeResultType.I_FRT_COMPLEX_M_MISES);
                    break;

                case MeshResultType.Displacements:
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UZ);
                    //results.Add((int)IRobotFeResultType.I_FRT_DETAILED_UXX);
                    //results.Add((int)IRobotFeResultType.I_FRT_DETAILED_UYY);
                    //results.Add((int)IRobotFeResultType.I_FRT_DETAILED_WNORM);
                    //results.Add((int)IRobotFeResultType.I_FRT_DETAILED_RXX);
                    //results.Add((int)IRobotFeResultType.I_FRT_DETAILED_RYY);
                    //results.Add((int)IRobotFeResultType.I_FRT_DETAILED_RNORM);

                    break;

                case MeshResultType.MeshModeShape:
                    results.Add(234);
                    results.Add(235);
                    results.Add(236);
                    results.Add(237);
                    results.Add(238);
                    results.Add(239);
                    break;
            }

            return results;
        }

        /***************************************************/


    }
}

/***************************************************/



