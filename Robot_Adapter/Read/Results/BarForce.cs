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

        private List<BarForce> ReadBarForce(IList ids = null, IList cases = null, int divisions = 5)
        {
            List<BarForce> barforces = new List<BarForce>();
            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
            List<int> results = new List<int>();

            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY);
            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ);
            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX);
            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY);
            results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ);

            queryParams.ResultIds.SetSize(results.Count);

            for (int i = 0; i < results.Count; i++)
            {
                int id = (int)results[i];
                queryParams.ResultIds.Set(i + 1, id);
            }

            RobotSelection barSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            RobotSelection caseSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);

            if (ids == null || ids.Count == 0)
                barSelection.FromText("all");
            else
                barSelection.FromText(BH.Engine.Robot.Convert.ToRobotSelectionString(CheckAndGetIds(ids)));

            if (cases == null || cases.Count == 0)
                caseSelection.FromText("all");
            else
                caseSelection.FromText(BH.Engine.Robot.Convert.ToRobotSelectionString(GetCaseNumbers(cases)));

            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.Selection.Set(IRobotObjectType.I_OT_BAR, barSelection);
            queryParams.SetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT, divisions);
            RobotResultRowSet rowSet = new RobotResultRowSet();

            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;

            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = m_RobotApplication.Kernel.Structure.Results.Query(queryParams, rowSet);
                bool isOk = rowSet.MoveFirst();
                while (isOk)
                {
                    RobotResultRow row = rowSet.CurrentRow;
                    int idCase = (int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
                    int idBar = (int)row.GetParam(IRobotResultParamType.I_RPT_BAR);
                    int idPoint = (int)row.GetParam(IRobotResultParamType.I_RPT_BAR_DIV_POINT);
                    int division = (int)row.GetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT);

                    double fx = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX) : 0;
                    double fy = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY) : 0;
                    double fz = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ) : 0;
                    double mx = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX) : 0;
                    double my = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY) : 0;
                    double mz = row.IsAvailable((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ) ? row.GetValue((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ) : 0;
                    double position = (1 / (System.Convert.ToDouble(division) - 1)) * (System.Convert.ToDouble(idPoint) - 1);

                    BarForce barForce = new BarForce
                    {
                        ResultCase = idCase.ToString(),
                        ObjectId = idBar.ToString(),
                        Divisions = division,
                        Position = position,
                        FX = fx * -1,
                        FY = fy * -1,
                        FZ = fz * -1,
                        MX = mx * -1,
                        MY = my * -1,
                        MZ = mz
                    };

                    barforces.Add(barForce);
                    isOk = rowSet.MoveNext();

                }
            }
            return barforces;
        }      

        /***************************************************/

    }
}
