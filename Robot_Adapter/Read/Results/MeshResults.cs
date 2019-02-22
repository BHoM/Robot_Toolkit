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
using BH.oM.DataManipulation.Queries;
using System.Collections.ObjectModel;
using BH.oM.Base;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<MeshResults> ReadMeshResults(FilterQuery query = null)
        {
            IList ids = (IList)query.Equalities["ObjectIds"];
            IList cases = (IList)query.Equalities["Cases"];
            MeshResultLayer layer = (MeshResultLayer)query.Equalities["Layer"];
            MeshResultSmoothingType smoothing = (MeshResultSmoothingType)query.Equalities["Smoothing"];
            double layerPosition = (double)query.Equalities["LayerPosition"];            
            MeshResultType resultType = (MeshResultType)query.Equalities["ResultType"];
            IRobotFeLayerType robotMeshLayer = Convert.FromBHoMEnum(layer);
            Cartesian userCoordinateSystem = (Cartesian)query.Equalities["CoordinateSystem"];
            if (robotMeshLayer == IRobotFeLayerType.I_FLT_LOWER)
                layerPosition = 0;
            if (robotMeshLayer == IRobotFeLayerType.I_FLT_MIDDLE)
                layerPosition = 0.5;
            if (robotMeshLayer == IRobotFeLayerType.I_FLT_UPPER)
                layerPosition = 1;

            IRobotFeResultSmoothing robotFESmoothing = Convert.FromBHoMEnum(smoothing);

            IRobotStructure robotStructureServer = m_RobotApplication.Project.Structure;

            List<BH.oM.Structure.Elements.Node> nodes = ReadNodesQuery();
            List<BH.oM.Structure.Elements.PanelPlanar> panels = new List<oM.Structure.Elements.PanelPlanar>();
            if (ids.Count == 0)
            {
                panels = ReadPanels();
            }
            else
            {
                List<int> panelIds = new List<int>();
                foreach (object obj in ids)
                {
                    if (obj.GetType() == typeof(BH.oM.Structure.Elements.PanelPlanar))
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
                    panels.AddRange(ReadPanels(panelIds as dynamic));
                }
            }
            

            List<BH.oM.Geometry.Point> nodePointList = nodes.Select(x => Engine.Structure.Query.Position(x)).ToList();

            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            List<int> results = new List<int>();
            results.AddRange(BH.Engine.Robot.Convert.MeshResultParameters(resultType)["ResultsToInclude"] as dynamic);
            results.Add(0);
            results.Add(1);
            results.Add(2);

            queryParams.ResultIds.SetSize(results.Count);

            for (int j = 0; j < results.Count; j++)
            {
                int id = (int)results[j];
                queryParams.ResultIds.Set(j + 1, id);
            }

            List<MeshResults> meshResultsCollection = new List<MeshResults>();
            foreach (BH.oM.Structure.Elements.PanelPlanar panel in panels)
            {

                Cartesian coordinateSystem = (userCoordinateSystem != null) ? userCoordinateSystem : panel.CustomData["CoordinateSystem"] as dynamic;
                List<MeshResult> meshResults = new List<MeshResult>();

                RobotSelection caseSelection = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_CASE);
                caseSelection.FromText((cases == null || cases.Count == 0) ? "all" : Convert.ToRobotSelectionString(GetCaseNumbers(cases)));

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
                if (coordinateSystem != null)
                {
                    queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X_DEFTYPE, IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN);
                    queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X, new double[] { coordinateSystem.X.X, coordinateSystem.X.Y, coordinateSystem.X.Z });
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
                        if (smoothing != MeshResultSmoothingType.ByFiniteElementCentres)
                        {
                            //idNode = System.Convert.ToInt32(row.GetParam(IRobotResultParamType.I_RPT_NODE));

                            BH.oM.Geometry.Point nodePoint = BH.Engine.Geometry.Create.Point(row.GetValue(0), row.GetValue(1), row.GetValue(2));
                            idNode = System.Convert.ToInt32(nodes.ElementAt(nodePointList.IndexOf(BH.Engine.Geometry.Query.ClosestPoint(nodePoint, nodePointList))).CustomData[AdapterId]);
                        }

                        int idFiniteElement = 0;
                        if (smoothing == MeshResultSmoothingType.ByFiniteElementCentres)
                            if (queryParams.IsParamSet(IRobotResultParamType.I_RPT_ELEMENT))
                                idFiniteElement = System.Convert.ToInt32(row.GetParam(IRobotResultParamType.I_RPT_ELEMENT));

                        switch (resultType)
                        {
                            case MeshResultType.Stresses:
                                MeshStress meshStress = new MeshStress(idPanel.ToString(),
                                                                        idNode.ToString(),
                                                                        idFiniteElement.ToString(),
                                                                        idCase.ToString(),
                                                                        0,
                                                                        layer,
                                                                        layerPosition,
                                                                        smoothing,
                                                                        coordinateSystem,
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_SXX),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_SYY),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_SXY),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_TXX),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_TYY),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_PRINCIPAL_S1),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_PRINCIPAL_S2),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_PRINCIPAL_S1_2));

                                meshResults.Add(meshStress);
                                break;
                            case MeshResultType.Forces:

                                MeshForce meshForce = new MeshForce(idPanel.ToString(),
                                                                        idNode.ToString(),
                                                                        idFiniteElement.ToString(),
                                                                        idCase.ToString(),
                                                                        0,
                                                                        layer,
                                                                        layerPosition,
                                                                        smoothing,
                                                                        coordinateSystem,
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_NXX),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_NYY),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_NXY),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_MXX),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_MYY),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_MXY),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_QXX),
                                                                        row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_QYY));

                                meshResults.Add(meshForce);
                                break;

                            case MeshResultType.VonMises:
                                MeshVonMises meshVonMises = new MeshVonMises(  idPanel.ToString(),
                                                                            idNode.ToString(),
                                                                            idFiniteElement.ToString(),
                                                                            idCase.ToString(),
                                                                            0,
                                                                            layer,
                                                                            layerPosition,
                                                                            smoothing,
                                                                            coordinateSystem,
                                                                            row.GetValue((int)IRobotFeResultType.I_FRT_COMPLEX_S_MISES),
                                                                            row.GetValue((int)IRobotFeResultType.I_FRT_COMPLEX_N_MISES),
                                                                            row.GetValue((int)IRobotFeResultType.I_FRT_COMPLEX_M_MISES));
                                meshResults.Add(meshVonMises);
                                break;

                            case MeshResultType.Displacements:
                                MeshDisplacement meshDisplacement = new MeshDisplacement(idPanel.ToString(),
                                                                            idNode.ToString(),
                                                                            idFiniteElement.ToString(),
                                                                            idCase.ToString(),
                                                                            0,
                                                                            layer,
                                                                            layerPosition,
                                                                            smoothing,
                                                                            coordinateSystem,
                                                                            row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_UXX),
                                                                            row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_UYY),
                                                                            row.GetValue((int)IRobotFeResultType.I_FRT_DETAILED_WNORM),                                                     
                                                                            0,
                                                                            0,
                                                                            0);
                                meshResults.Add(meshDisplacement);
                                break;

                        }
                        isOk = rowSet.MoveNext();
                    }
                }
                ReadOnlyCollection<MeshResult> meshResultReadOnlyCollection = new ReadOnlyCollection<MeshResult>(meshResults);
                meshResultsCollection.Add(new MeshResults(System.Convert.ToString(panel.CustomData[AdapterId]), layer, smoothing, meshResultReadOnlyCollection));
            }
            return meshResultsCollection;
        }

    }
}

/***************************************************/
