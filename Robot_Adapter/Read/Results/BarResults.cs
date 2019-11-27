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
using BH.oM.Structure.Requests;
using RobotOM;
using BH.oM.Common;
using System;
using System.Collections;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public IEnumerable<IResult> ReadResults(BarResultRequest request)
        {

            if (request.DivisionType == DivisionType.ExtremeValues)
                return ReadBarExtremeResults(request);

            List<BarResult> barResults = new List<BarResult>();
            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            List<int> results = BarResultParameters(request);

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

            RobotSelection barSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            RobotSelection caseSelection = GetCaseSelection(request);

            if (request.ObjectIds == null || request.ObjectIds.Count == 0)
                barSelection.FromText("all");
            else
                barSelection.FromText(BH.Engine.Robot.Convert.ToRobotSelectionString(CheckAndGetIds(request.ObjectIds)));

            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.Selection.Set(IRobotObjectType.I_OT_BAR, barSelection);
            queryParams.SetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT, request.Divisions);
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
                    double position = (1 / (System.Convert.ToDouble(division) - 1)) * (System.Convert.ToDouble(idPoint) - 1);

                    switch (request.ResultType)
                    {
                        case BarResultType.BarForce:
                            barResults.Add(GetBarForce(row, idCase, idBar, division, position));
                            break;
                        case BarResultType.BarDeformation:
                            barResults.Add(GetBarDeformation(row, idCase, idBar, division, position));
                            break;
                        case BarResultType.BarStress:
                            barResults.Add(GetBarStress(row, idCase, idBar, division, position));
                            break;
                        case BarResultType.BarDisplacement:
                            barResults.Add(GetBarDisplacement(row, idCase, idBar, division, position));
                            break;
                    }

                    isOk = rowSet.MoveNext();

                }
            }
            return barResults;

        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        public IEnumerable<IResult> ReadBarExtremeResults(BarResultRequest request)
        {
            List<BarResult> barResults = new List<BarResult>();
            RobotExtremeParams extremeParams = (RobotExtremeParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_EXTREME_PARAMS);
            RobotResultQueryParams queryParams = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            List<int> results = BarResultParameters(request);

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


            List<int> barIds = new List<int>();
            if (request.ObjectIds == null || request.ObjectIds.Count == 0)
            {
                m_RobotApplication.Project.Structure.Bars.BeginMultiOperation();
                IRobotCollection robotBars = m_RobotApplication.Project.Structure.Bars.GetAll();
                for (int i = 1; i <= robotBars.Count; i++)
                {
                    RobotBar robotBar = robotBars.Get(i);
                    barIds.Add(robotBar.Number);
                }
                m_RobotApplication.Project.Structure.Bars.EndMultiOperation();
            }
            else
                barIds = CheckAndGetIds(request.ObjectIds);


            List<int> casesIds;
            if (request.Cases == null || request.Cases.Count == 0)
            {
                RobotCaseCollection rLoadCases = m_RobotApplication.Project.Structure.Cases.GetAll();
                casesIds = new List<int>();
                for (int i = 0; i < rLoadCases.Count; i++)
                {
                    IRobotCase rLoadCase = rLoadCases.Get(i) as IRobotCase;
                    casesIds.Add(rLoadCase.Number);
                }
            }
            else
            {
                casesIds = GetCaseNumbers(request.Cases);
            }


            for (int i = 0; i < results.Count; i++)
            {

                extremeParams.ValueType = (IRobotExtremeValueType)results[i];


                foreach (int caseId in casesIds)
                {
                    RobotSelection caseSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
                    caseSelection.AddOne(caseId);

                    foreach (int barId in barIds)
                    {
                        RobotSelection barSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);


                        barSelection.AddOne(barId);
                        extremeParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
                        extremeParams.Selection.Set(IRobotObjectType.I_OT_BAR, barSelection);
                        RobotExtremeValue val = m_RobotApplication.Kernel.Structure.Results.Extremes.MaxValue(extremeParams);
                        double robotPosition = val.Position;


                        queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
                        queryParams.Selection.Set(IRobotObjectType.I_OT_BAR, barSelection);
                        queryParams.SetParam(IRobotResultParamType.I_RPT_BAR_RELATIVE_POINT, robotPosition);
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
                                double position = (int)row.GetParam(IRobotResultParamType.I_RPT_BAR_RELATIVE_POINT);
                                int division = 1;// (int)row.GetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT);
                                //double position = (1 / (System.Convert.ToDouble(division) - 1)) * (System.Convert.ToDouble(idPoint) - 1);

                                switch (request.ResultType)
                                {
                                    case BarResultType.BarForce:
                                        barResults.Add(GetBarForce(row, idCase, idBar, division, position));
                                        break;
                                    case BarResultType.BarDeformation:
                                        barResults.Add(GetBarDeformation(row, idCase, idBar, division, position));
                                        break;
                                    case BarResultType.BarStress:
                                        barResults.Add(GetBarStress(row, idCase, idBar, division, position));
                                        break;
                                    case BarResultType.BarDisplacement:
                                        barResults.Add(GetBarDisplacement(row, idCase, idBar, division, position));
                                        break;
                                }

                                isOk = rowSet.MoveNext();

                            }
                        }

                    }
                }
            }



            return barResults;
        }

        /***************************************************/

        private BarForce GetBarForce(RobotResultRow row, int idCase, int idBar, int divisions, double position)
        {
            double fx = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
            double fy = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY);
            double fz = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ);

            double mx = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX);
            double my = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY);
            double mz = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ);

            return new BarForce
            {
                ResultCase = idCase,
                ObjectId = idBar,
                Divisions = divisions,
                Position = position,
                FX = fx * -1,
                FY = fy * -1,
                FZ = fz * -1,
                MX = mx * -1,
                MY = my * -1,
                MZ = mz
            };
        }

        /***************************************************/

        private BarDisplacement GetBarDisplacement(RobotResultRow row, int idCase, int idBar, int divisions, double position)
        {
            double ux = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UX);
            double uy = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UY);
            double uz = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UZ);

            double rx = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RX);
            double ry = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RY);
            double rz = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RZ);

            return new BarDisplacement
            {
                ResultCase = idCase,
                ObjectId = idBar,
                Divisions = divisions,
                Position = position,
                UX = ux,
                UY = uy,
                UZ = uz,
                RX = rx,
                RY = ry,
                RZ = rz
            };
        }

        /***************************************************/

        private BarDeformation GetBarDeformation(RobotResultRow row, int idCase, int idBar, int divisions, double position)
        {
            double ux = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DEFLECTION_UX);
            double uy = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DEFLECTION_UY);
            double uz = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_DEFLECTION_UZ);

            return new BarDeformation
            {
                ResultCase = idCase,
                ObjectId = idBar,
                Divisions = divisions,
                Position = position,
                UX = ux,
                UY = uy,
                UZ = uz,
            };
        }

        /***************************************************/

        private BarStress GetBarStress(RobotResultRow row, int idCase, int idBar, int divisions, double position)
        {
            double fxSx = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_FX_SX);
            double sMax = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX);
            double sMaxMy = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX_MY);
            double sMaxMz = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX_MZ);
            double sMin = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN);
            double sMinMy = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN_MY);
            double sMinMz = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN_MZ);
            double t = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_T);
            double ty = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_TY);
            double tz = CheckGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_TZ);

            return new BarStress
            {
                ResultCase = idCase,
                ObjectId = idBar,
                Divisions = divisions,
                Position = position,
                Axial = fxSx,
                BendingY_Top = sMaxMy,
                BendingY_Bot = sMinMy,
                BendingZ_Top = sMaxMz,
                BendingZ_Bot = sMinMz,
                CombAxialBendingNeg = sMin,
                CombAxialBendingPos = sMax,
                ShearY = ty,
                ShearZ = tz
            };
        }

        /***************************************************/

        private List<int> BarResultParameters(BarResultRequest request)
        {
            List<int> results = new List<int>();
            switch (request.ResultType)
            {
                case BarResultType.BarForce:
                    results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ);
                    results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ);
                    break;
                case BarResultType.BarDeformation:
                    results.Add((int)IRobotExtremeValueType.I_EVT_DEFLECTION_UX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DEFLECTION_UY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DEFLECTION_UZ);
                    break;
                case BarResultType.BarStress:
                    results.Add((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_FX_SX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX_MY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX_MZ);
                    results.Add((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN);
                    results.Add((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN_MY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN_MZ);
                    results.Add((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_T);
                    results.Add((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_TY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_STRESS_BAR_TZ);
                    break;
                case BarResultType.BarStrain:
                    break;
                case BarResultType.BarDisplacement:
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UZ);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RX);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RY);
                    results.Add((int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RZ);
                    break;
                default:
                    break;
            }
            return results;
        }

        /***************************************************/
    }
}
