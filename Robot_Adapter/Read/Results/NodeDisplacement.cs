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
using System.Linq;
using BH.oM.Base;
using BH.oM.Structure.Results;
using RobotOM;
using BH.oM.Common;
using System;
using System.Collections;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {      
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<NodeDisplacement> ReadNodeDisplacement(IList ids = null, IList cases = null, int divisions = 5)
        {
            List<NodeDisplacement> nodeDisplacements = new List<NodeDisplacement>();

            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            List<int> results = new List<int>();
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UX);
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UY);
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UZ);
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RX);
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RY);
            results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RZ);

            queryParams.ResultIds.SetSize(results.Count);

            for (int i = 0; i < results.Count; i++)
            {
                int id = (int)results[i];
                queryParams.ResultIds.Set(i + 1, id);
            }

            RobotSelection nodeSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            RobotSelection caseSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);

            if (ids == null || ids.Count == 0)
                nodeSelection.FromText("all");
            else
                nodeSelection.FromText(BH.Engine.Robot.Convert.ToRobotSelectionString(CheckAndGetIds(ids)));

            if (cases == null || cases.Count == 0)
                caseSelection.FromText("all");
            else
                caseSelection.FromText(BH.Engine.Robot.Convert.ToRobotSelectionString(GetCaseNumbers(cases)));

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

                    double ux = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UX);
                    double uy = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UY);
                    double uz = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_UZ);
                    double rx = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RX);
                    double ry = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RY);
                    double rz = row.GetValue((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_NODE_RZ);

                    NodeDisplacement nodeDisp = new NodeDisplacement
                    {
                        ResultCase = idCase.ToString(),
                        ObjectId = idNode.ToString(),
                        UX = ux,
                        UY = uy,
                        UZ = uz,
                        RX = rx,
                        RY = ry,
                        RZ = rz
                    };

                    nodeDisplacements.Add(nodeDisp);
                    isOk = rowSet.MoveNext();
                }
            }
            return nodeDisplacements;
        }

        /***************************************************/

    }
}
