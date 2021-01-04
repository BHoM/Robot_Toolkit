/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Analytical.Results;
using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using BH.Engine.Geometry;
using BH.Engine.Structure;
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

        public IEnumerable<IResult> ReadResults(BarResultRequest request, ActionConfig actionConfig = null)
        {
            if (request.DivisionType == DivisionType.ExtremeValues)
            {
                Engine.Reflection.Compute.RecordError("Robot API method extreme values gives unreliable results and runs very slow.\nTo get extreme values out of Robot, change to EvenlyDistributed for division type, increase the division points and use the method AbsoluteMaxForce from the BHoM_Engine");
                return new List<BarResult>();
            }

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

            List<int> barIds = CheckAndGetIds<Bar>(request.ObjectIds);

            if (barIds == null || barIds.Count == 0)
                barSelection.FromText("all");
            else
                barSelection.FromText(Convert.ToRobotSelectionString(barIds));

            queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            queryParams.Selection.Set(IRobotObjectType.I_OT_BAR, barSelection);
            queryParams.SetParam(IRobotResultParamType.I_RPT_BAR_DIV_COUNT, request.Divisions);
            

            RobotResultRowSet rowSet = new RobotResultRowSet();

            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;


            List<Bar> bars = new List<Bar>();
            Cartesian globalXY = new Cartesian(Point.Origin, Vector.XAxis, Vector.YAxis, Vector.ZAxis);

            if (request.ResultType == BarResultType.BarDisplacement)
            {
                if (request.ObjectIds == null || request.ObjectIds.Count == 0)
                {
                    bars = ReadBarsQuery();
                }
                else
                {
                    List<object> barIdObj = new List<object>();
                    foreach (object obj in request.ObjectIds)
                    {
                        if (obj is Bar)
                            bars.Add(obj as Bar);
                        else
                            barIdObj.Add(obj);
                    }
                    if (barIds.Count > 0)
                    {
                        bars.AddRange(ReadBarsQuery(CheckAndGetIds<Bar>(barIdObj).Select(x => x.ToString()).ToList()));
                    }
                }
            }


            Dictionary<int, TransformMatrix> transformations = new Dictionary<int, TransformMatrix>();

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
                    int mode = -1; //TODO: extract mode number

                    switch (request.ResultType)
                    {
                        case BarResultType.BarForce:
                            barResults.Add(GetBarForce(row, idCase, idBar, division, position, mode));
                            break;
                        case BarResultType.BarDeformation:
                            barResults.Add(GetBarDeformation(row, idCase, idBar, division, position, mode));
                            break;
                        case BarResultType.BarStress:
                            barResults.Add(GetBarStress(row, idCase, idBar, division, position, mode));
                            break;
                        case BarResultType.BarDisplacement:
                            TransformMatrix localToGlobal;

                            if (!transformations.TryGetValue(idBar, out localToGlobal))
                            {
                                Bar bar = bars.First(x => GetAdapterId<int>(x) == idBar);
                                Cartesian local = bar.CoordinateSystem();
                                local.Origin = Point.Origin;
                                localToGlobal = Engine.Geometry.Create.OrientationMatrix(globalXY, local);
                                transformations[idBar] = localToGlobal;
                            }
                             
                            barResults.Add(GetBarDisplacement(row, idCase, idBar, division, position, mode, localToGlobal));
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
        

        private BarForce GetBarForce(RobotResultRow row, int idCase, int idBar, int divisions, double position, int mode)
        {
            double fx = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
            double fy = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FY);
            double fz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ);

            double mx = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MX);
            double my = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MY);
            double mz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ);

            return new BarForce(idBar, idCase, mode, 0, position, divisions, -fx, fy, fz, mx, -my, mz);
        }

        /***************************************************/

        private BarDisplacement GetBarDisplacement(RobotResultRow row, int idCase, int idBar, int divisions, double position, int mode, TransformMatrix localToGlobal)
        {
            double ux = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UX);
            double uy = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UY);
            double uz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UZ);

            double rx = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RX);
            double ry = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RY);
            double rz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RZ);

            Point u = new Point { X = ux, Y = uy, Z = uz };
            Point r = new Point { X = rx, Y = ry, Z = rz };

            u = u.Transform(localToGlobal);
            r = r.Transform(localToGlobal);

            return new BarDisplacement(idBar, idCase, mode, 0, position, divisions, u.X, u.Y, u.Z, r.X, r.Y, r.Z);
        }

        /***************************************************/

        private BarDeformation GetBarDeformation(RobotResultRow row, int idCase, int idBar, int divisions, double position, int mode)
        {
            double ux = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DEFLECTION_UX);
            double uy = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DEFLECTION_UY);
            double uz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_DEFLECTION_UZ);

            return new BarDeformation(idBar, idCase, mode, 0, position, divisions, ux, uy, uz, 0, 0, 0);
        }

        /***************************************************/

        private BarStress GetBarStress(RobotResultRow row, int idCase, int idBar, int divisions, double position, int mode)
        {
            double fxSx = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_FX_SX);
            double sMax = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX);
            double sMaxMy = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX_MY);
            double sMaxMz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX_MZ);
            double sMin = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN);
            double sMinMy = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN_MY);
            double sMinMz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN_MZ);
            double t = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_T);
            double ty = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_TY);
            double tz = TryGetValue(row, (int)IRobotExtremeValueType.I_EVT_STRESS_BAR_TZ);

            return new BarStress(idBar, idCase, mode, 0, position, divisions, fxSx, ty, tz, sMaxMy, sMinMy, sMaxMz, sMinMz, sMax, sMin);
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


