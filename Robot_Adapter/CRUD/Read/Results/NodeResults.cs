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
using BH.oM.Base;
using BH.oM.Structure.Results;
using BH.oM.Structure.Requests;
using RobotOM;
using BH.oM.Common;
using BH.oM.Adapter;
using System;
using System.Collections;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public IEnumerable<IResult> ReadResults(NodeResultRequest request, ActionConfig actionConfig = null)
        {
            List<NodeResult> nodeResults = new List<NodeResult>();

            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            List<int> results = NodeResultParameters(request);

            if (results.Count == 0)
            {
                Engine.Reflection.Compute.RecordError("Unable to extract results of type " + request.ResultType + " from Robot");
            }

            queryParams.ResultIds.SetSize(results.Count);

            for (int i = 0; i < results.Count; i++)
            {
                int id = (int)results[i];
                queryParams.ResultIds.Set(i + 1, id);
            }

            RobotSelection nodeSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            RobotSelection caseSelection = GetCaseSelection(request);

            if (request.ObjectIds == null || request.ObjectIds.Count == 0)
                nodeSelection.FromText("all");
            else
                nodeSelection.FromText(Convert.ToRobotSelectionString(CheckAndGetIds(request.ObjectIds)));

            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.Selection.Set(IRobotObjectType.I_OT_NODE, nodeSelection);
            queryParams.SetParam(IRobotResultParamType.I_RPT_MODE, 0);
            queryParams.SetParam(IRobotResultParamType.I_RPT_MODE_CMB, IRobotModeCombinationType.I_MCT_CQC);

            RobotResultRowSet rowSet = new RobotResultRowSet();

            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
            bool anythingCalculated = false;

            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = m_RobotApplication.Kernel.Structure.Results.Query(queryParams, rowSet);
                bool isOk = anythingCalculated = rowSet.MoveFirst();
                while (isOk)
                {
                    RobotResultRow row = rowSet.CurrentRow;
                    int idCase = (int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
                    int idNode = (int)row.GetParam(IRobotResultParamType.I_RPT_NODE);

                    switch (request.ResultType)
                    {
                        case NodeResultType.NodeReaction:
                            nodeResults.Add(GetNodeReaction(row, idCase, idNode));
                            break;
                        case NodeResultType.NodeDisplacement:
                            nodeResults.Add(GetNodeDisplacement(row, idCase, idNode));
                            break;
                    }

                    isOk = rowSet.MoveNext();
                }
            }
            return nodeResults;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private NodeDisplacement GetNodeDisplacement(RobotResultRow row, int idCase, int idNode)
        {
            double ux = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UX);
            double uy = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UY);
            double uz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UZ);

            double rx = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RX);
            double ry = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RY);
            double rz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RZ);

            return new NodeDisplacement
            {
                ResultCase = idCase,
                ObjectId = idNode,
                UX = ux,
                UY = uy,
                UZ = uz,
                RX = rx,
                RY = ry,
                RZ = rz
            };
        }

        /***************************************************/

        private NodeReaction GetNodeReaction(RobotResultRow row, int idCase, int idNode)
        {
            double fx = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_REACTION_FX);
            double fy = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_REACTION_FY);
            double fz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_REACTION_FZ);

            double mx = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_REACTION_MX);
            double my = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_REACTION_MY);
            double mz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_REACTION_MZ);

            return new NodeReaction
            {
                ResultCase = idCase,
                ObjectId = idNode,
                FX = fx,
                FY = fy,
                FZ = fz,
                MX = mx,
                MY = my,
                MZ = mz
            };
        }

        /***************************************************/

        private List<int> NodeResultParameters(NodeResultRequest request)
        {
            List<int> results = new List<int>();

            switch (request.ResultType)
            {
                case NodeResultType.NodeReaction:
                    results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_FX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_FY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_FZ);
                    results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_MX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_MY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_REACTION_MZ);
                    break;
                case NodeResultType.NodeDisplacement:
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UZ);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RZ);
                    break;
                case NodeResultType.NodeVelocity:
                    break;
                case NodeResultType.NodeAcceleration:
                    break;
                default:
                    break;
            }
            
            return results;
        }

        /***************************************************/

    }
}

