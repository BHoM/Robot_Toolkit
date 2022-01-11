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
using System.Linq;
using BH.oM.Base;
using BH.oM.Structure.Results;
using BH.oM.Structure.Requests;
using RobotOM;
using BH.oM.Analytical.Results;
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

        public IEnumerable<IResult> ReadResults(GlobalResultRequest request, ActionConfig actionConfig = null)
        {
            List<StructuralGlobalResult> globalResults = new List<StructuralGlobalResult>();

            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            List<int> results = GlobalResultParameters(request);

            if (results.Count == 0)
            {
                Engine.Base.Compute.RecordError("Unable to extract results of type " + request.ResultType + " from Robot");
            }

            queryParams.ResultIds.SetSize(results.Count);

            for (int i = 0; i < results.Count; i++)
            {
                int id = (int)results[i];
                queryParams.ResultIds.Set(i + 1, id);
            }

            RobotSelection caseSelection = GetCaseSelection(request);

            RobotSelection nodeSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            nodeSelection.AddOne(1); //Adding one node to selection to limit number of results extracted

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
                    int mode = -1; //TODO: extract mode number

                    switch (request.ResultType)
                    {
                        case GlobalResultType.Reactions:
                            globalResults.Add(GetGlobalReaction(row, idCase, mode));
                            break;
                    }

                    isOk = rowSet.MoveNext();
                }
            }
            return globalResults;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private GlobalReactions GetGlobalReaction(RobotResultRow row, int idCase, int mode)
        {
            double fx = TryGetValue(row, 121);
            double fy = TryGetValue(row, 122);
            double fz = TryGetValue(row, 123);

            double mx = TryGetValue(row, 124);
            double my = TryGetValue(row, 125);
            double mz = TryGetValue(row, 126);

            return new GlobalReactions(0, idCase, mode, 0, fx, fy, fz, mx, my, mz);
        }

        /***************************************************/

        private List<int> GlobalResultParameters(GlobalResultRequest request)
        {
            List<int> results = new List<int>();

            switch (request.ResultType)
            {
                case GlobalResultType.Reactions:
                    results.Add(121);   //Total reaction FX
                    results.Add(122);   //Total reaction FY
                    results.Add(123);   //Total reaction FZ
                    results.Add(124);   //Total reaction MX
                    results.Add(125);   //Total reaction MY
                    results.Add(126);   //Total reaction MZ
                    break;
                case GlobalResultType.ModalDynamics:
                default:
                    break;
            }

            return results;
        }

        /***************************************************/

    }
}



