/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
using System.Collections;
using BH.Engine.Robot;
using BH.oM.Geometry.CoordinateSystem;
using System.Linq;
using BH.oM.Data.Requests;
using System.Collections.ObjectModel;
using BH.oM.Common;
using BH.oM.Structure.Requests;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.Base;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public IEnumerable<IResult> ReadResults(MeshResultRequest request)
        {
            MeshResultLayer layer = request.Layer;
            MeshResultSmoothingType smoothing = request.Smoothing;

            if (smoothing == MeshResultSmoothingType.None)
            {
                Engine.Reflection.Compute.RecordWarning("Result extraction with no smoothing is currently possible in the Robot adapter. To extract results please choose another smoothening type");
                return new List<IResult>();
            }

            double layerPosition = request.LayerPosition;
            IRobotFeLayerType robotMeshLayer = Engine.Robot.Convert.FromBHoMEnum(request.Layer);
            if (robotMeshLayer == IRobotFeLayerType.I_FLT_LOWER)
                layerPosition = 0;
            if (robotMeshLayer == IRobotFeLayerType.I_FLT_MIDDLE)
                layerPosition = 0.5;
            if (robotMeshLayer == IRobotFeLayerType.I_FLT_UPPER)
                layerPosition = 1;

            IRobotFeResultSmoothing robotFESmoothing = Engine.Robot.Convert.FromBHoMEnum(request.Smoothing);

            IRobotStructure robotStructureServer = m_RobotApplication.Project.Structure;

            List<BH.oM.Structure.Elements.Node> nodes = ReadNodesQuery();
            List<BH.oM.Structure.Elements.Panel> panels = new List<oM.Structure.Elements.Panel>();
            if (request.ObjectIds == null || request.ObjectIds.Count == 0)
            {
                panels = ReadPanelsLight();
            }
            else
            {
                List<int> panelIds = new List<int>();
                foreach (object obj in request.ObjectIds)
                {
                    if (obj.GetType() == typeof(BH.oM.Structure.Elements.Panel))
                    {
                        panels.Add(obj as dynamic);
                    }
                    else if (obj.GetType() == typeof(string) || obj.GetType() == typeof(int))
                    {
                        panelIds.Add(System.Convert.ToInt32(obj));
                    }
                }
                if (panelIds.Count > 0)
                {
                    panels.AddRange(ReadPanelsLight(panelIds as dynamic));
                }
            }


            List<BH.oM.Geometry.Point> nodePointList = nodes.Select(x => Engine.Structure.Query.Position(x)).ToList();

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
            foreach (BH.oM.Structure.Elements.Panel panel in panels)
            {

                Basis orientation = request.Orientation ?? (Basis)(panel.CustomData["CoordinateSystem"] as dynamic);
                List<MeshElementResult> meshResults = new List<MeshElementResult>();

                RobotSelection panelSelection = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_PANEL);
                panelSelection.FromText(panel.CustomData[BH.Engine.Robot.Convert.AdapterID].ToString());

                RobotSelection finiteElementSelection = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_FINITE_ELEMENT);
                finiteElementSelection.FromText(panel.CustomData["RobotFiniteElementIds"].ToString());

                RobotSelection nodeSelection = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_NODE);
                nodeSelection.FromText(panel.CustomData["RobotNodeIds"].ToString());

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
                        int idCase = 0;
                        if (queryParams.IsParamSet(IRobotResultParamType.I_RPT_LOAD_CASE))
                            idCase = System.Convert.ToInt32(row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE));

                        int idPanel = System.Convert.ToInt32(panel.CustomData[AdapterId]);

                        int idNode = 0;
                        if (request.Smoothing != MeshResultSmoothingType.ByFiniteElementCentres)
                        {
                            //idNode = System.Convert.ToInt32(row.GetParam(IRobotResultParamType.I_RPT_NODE));

                            BH.oM.Geometry.Point nodePoint = BH.Engine.Geometry.Create.Point(row.GetValue(0), row.GetValue(1), row.GetValue(2));
                            idNode = System.Convert.ToInt32(nodes.ElementAt(nodePointList.IndexOf(BH.Engine.Geometry.Query.ClosestPoint(nodePoint, nodePointList))).CustomData[AdapterId]);
                        }

                        int idFiniteElement = 0;
                        if (request.Smoothing == MeshResultSmoothingType.ByFiniteElementCentres || request.Smoothing == MeshResultSmoothingType.None)
                            if (queryParams.IsParamSet(IRobotResultParamType.I_RPT_ELEMENT))
                                idFiniteElement = System.Convert.ToInt32(row.GetParam(IRobotResultParamType.I_RPT_ELEMENT));

                        switch (request.ResultType)
                        {
                            case MeshResultType.Stresses:
                                meshResults.Add(GetMeshStress(row, idPanel, idNode, idFiniteElement, idCase, layer, layerPosition, smoothing, orientation));
                                break;
                            case MeshResultType.Forces:
                                meshResults.Add(GetMeshForce(row, idPanel, idNode, idFiniteElement, idCase, layer, layerPosition, smoothing, orientation));
                                break;
                            case MeshResultType.VonMises:
                                meshResults.Add(GetMeshVonMises(row, idPanel, idNode, idFiniteElement, idCase, layer, layerPosition, smoothing, orientation));
                                break;
                            case MeshResultType.Displacements:
                                meshResults.Add(GetMeshDisplacement(row, idPanel, idNode, idFiniteElement, idCase, layer, layerPosition, smoothing, (Basis)globalXY));
                                break;

                        }
                        isOk = rowSet.MoveNext();
                    }
                }

                foreach (var resultByCase in meshResults.GroupBy(x => new { x.ResultCase, x.TimeStep }))
                {
                    System.IComparable loadCase = resultByCase.Key.ResultCase;
                    double timeStep = resultByCase.Key.TimeStep;
                    List<MeshElementResult> resultList = resultByCase.ToList();
                    resultList.Sort();
                    MeshResult meshResult = new MeshResult(panel.CustomData[AdapterId].ToString(), loadCase, timeStep, layer, layerPosition, smoothing, new ReadOnlyCollection<MeshElementResult>(resultList));
                    meshResultsCollection.Add(meshResult);
                }
            }

            return meshResultsCollection;
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private MeshStress GetMeshStress(RobotResultRow row, int idPanel, int idNode, int idFiniteElement, int idCase, MeshResultLayer layer, double layerPosition, MeshResultSmoothingType smoothing, oM.Geometry.Basis orientation)
        {
            return new MeshStress(idPanel,
                                  idNode,
                                  idFiniteElement,
                                  idCase,
                                  0,
                                  layer,
                                  layerPosition,
                                  smoothing,
                                  orientation,
                                  CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_SXX),
                                  CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_SYY),
                                  CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_SXY),
                                  CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_TXX),
                                  CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_TYY),
                                  CheckGetValue(row, (int)IRobotFeResultType.I_FRT_PRINCIPAL_S1),
                                  CheckGetValue(row, (int)IRobotFeResultType.I_FRT_PRINCIPAL_S2),
                                  CheckGetValue(row, (int)IRobotFeResultType.I_FRT_PRINCIPAL_S1_2));
        }


        /***************************************************/

        private MeshForce GetMeshForce(RobotResultRow row, int idPanel, int idNode, int idFiniteElement, int idCase, MeshResultLayer layer, double layerPosition, MeshResultSmoothingType smoothing, oM.Geometry.Basis orientation)
        {
            return new MeshForce(idPanel,
                                idNode,
                                idFiniteElement,
                                idCase,
                                0,
                                layer,
                                layerPosition,
                                smoothing,
                                orientation,
                                CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_NXX),
                                CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_NYY),
                                CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_NXY),
                                CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_MXX),
                                CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_MYY),
                                CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_MXY),
                                CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_QXX),
                                CheckGetValue(row, (int)IRobotFeResultType.I_FRT_DETAILED_QYY));
        }


        /***************************************************/

        private MeshVonMises GetMeshVonMises(RobotResultRow row, int idPanel, int idNode, int idFiniteElement, int idCase, MeshResultLayer layer, double layerPosition, MeshResultSmoothingType smoothing, oM.Geometry.Basis orientation)
        {
            return new MeshVonMises(idPanel,
                                    idNode,
                                    idFiniteElement,
                                    idCase,
                                    0,
                                    layer,
                                    layerPosition,
                                    smoothing,
                                    orientation,
                                    CheckGetValue(row, (int)IRobotFeResultType.I_FRT_COMPLEX_S_MISES),
                                    CheckGetValue(row, (int)IRobotFeResultType.I_FRT_COMPLEX_N_MISES),
                                    CheckGetValue(row, (int)IRobotFeResultType.I_FRT_COMPLEX_M_MISES));
        }


        /***************************************************/

        private MeshDisplacement GetMeshDisplacement(RobotResultRow row, int idPanel, int idNode, int idFiniteElement, int idCase, MeshResultLayer layer, double layerPosition, MeshResultSmoothingType smoothing, oM.Geometry.Basis orientation)
        {

            Vector u = new Vector
            {
                X = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UX),
                Y = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UY),
                Z = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UZ),
            };

            return new MeshDisplacement(idPanel,
                                        idNode,
                                        idFiniteElement,
                                        idCase,
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
            }

            return results;
        }

        /***************************************************/

        private IEnumerable<IResult> ReadMeshResults(FilterRequest query = null)
        {
            //Method to support backwards compatibility
            object idsObj = null;
            object casesObj = null;

            IList ids = null;
            IList cases = null;

            if (query.Equalities.TryGetValue("ObjectIds", out idsObj))
                ids = idsObj as IList;
            if (query.Equalities.TryGetValue("Cases", out casesObj))
                cases = casesObj as IList;


            MeshResultRequest request = Engine.Structure.Create.IResultRequest(query.Type, ids?.Cast<object>(), cases?.Cast<object>(), 0) as MeshResultRequest;

            if (request == null)
                return new List<MeshResult>();

            request.ResultType = (MeshResultType)query.Equalities["ResultType"];
            request.LayerPosition = (double)query.Equalities["LayerPosition"];
            request.Layer = (MeshResultLayer)query.Equalities["Layer"];
            request.Smoothing = (MeshResultSmoothingType)query.Equalities["Smoothing"];
            request.Orientation = (BH.oM.Geometry.Basis)query.Equalities["CoordinateSystem"];

            return ReadResults(request);
        }

        /***************************************************/

    }
}

/***************************************************/
