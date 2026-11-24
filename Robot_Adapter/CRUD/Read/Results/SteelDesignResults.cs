/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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

using System;
using System.Collections.Generic;
using BH.oM.Adapter;
using BH.oM.Adapters.Robot;
using BH.oM.Analytical.Results;
using BH.oM.Structure.Elements;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public IEnumerable<IResult> ReadResults(SteelMemberVerificationRequest request, ActionConfig actionConfig = null)
        {
            List<IResult> results = new List<IResult>();

            if (request == null)
                return results;

            if (!request.UltimateLimitState && !request.ServiceabilityLimitState)
            {
                Engine.Base.Compute.RecordError($"Neither {nameof(request.UltimateLimitState)} nor {nameof(request.ServiceabilityLimitState)} is enabled on the request, so no steel member verification was run.");
                return results;
            }

            IRDimCalcEngine engine = null;
            IRDimCalcConf calcConf = null;
            int robotUnitsFlag = -1;

            try
            {
                RDimServer server = m_RobotApplication.Kernel.GetExtension("RDimServer");
                server.Mode = IRDimServerMode.I_DSM_STEEL;
                RDimStream stream = server.Connection.GetStream();
                engine = server.CalculEngine;
                IRDimCalcParam calcParam = engine.GetCalcParam();
                calcConf = engine.GetCalcConf();

                //Members to verify
                List<int> memberIds = CheckAndGetIds<Bar>(request.ObjectIds);
                stream.Clear();
                stream.WriteText(memberIds == null || memberIds.Count == 0 ? "all" : Convert.ToRobotSelectionString(memberIds));
                calcParam.SetObjsList(IRDimCalcParamVerifType.I_DCPVT_MEMBERS_VERIF, stream);

                //Limit states
                calcParam.SetLimitState(IRDimCalcParamLimitStateType.I_DCPLST_ULTIMATE, request.UltimateLimitState ? 1 : 0);
                calcParam.SetLimitState(IRDimCalcParamLimitStateType.I_DCPLST_SERVICEABILITY, request.ServiceabilityLimitState ? 1 : 0);

                //Load cases. If none are given, every case and combination in the model is used.
                //Robot rejects a null stream here (E_POINTER), even though its API guide says null means all cases, so the full list is written out instead.
                List<int> caseIds = request.Cases == null || request.Cases.Count == 0 ? null : GetCaseNumbers(request.Cases);
                string caseList = caseIds == null || caseIds.Count == 0
                    ? m_RobotApplication.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_CASE).ToText()
                    : Convert.ToRobotSelectionString(caseIds);

                if (string.IsNullOrWhiteSpace(caseList))
                {
                    Engine.Base.Compute.RecordError("No load cases were found to verify the members against.");
                    return results;
                }

                stream.Clear();
                stream.WriteText(caseList);
                calcParam.SetLoadsList(stream);

                //Ask for default (SI) units rather than the units in the Robot preferences, and restore the user's setting afterwards
                robotUnitsFlag = calcConf.IsFlagSet(IRDimCalcConfFlagType.I_DCCFT_ROBOTUNITS);
                calcConf.SetFlag(IRDimCalcConfFlagType.I_DCCFT_ROBOTUNITS, 0);

                engine.SetCalcConf(calcConf);
                engine.SetCalcParam(calcParam);
                engine.Solve(null);

                IRDimAllRes allResults = engine.Results();

                if (allResults == null || allResults.ObjectsCount == 0)
                {
                    Engine.Base.Compute.RecordWarning("Robot returned no steel member verification results. Check that the model has been analysed and that the requested members have a steel member type assigned.");
                    return results;
                }

                if (allResults.ObjectsType != IRDimAllResObjectType.I_DAROT_VERIFIED_MEMBER)
                {
                    Engine.Base.Compute.RecordError($"Robot returned results of type {allResults.ObjectsType} instead of member verification results.");
                    return results;
                }

                string designCode = allResults.CodeName;

                if (request.IncludeCodeResults && request.UltimateLimitState && (designCode == null || !designCode.Contains("1993")))
                    Engine.Base.Compute.RecordNote($"Code specific results are only supported for EN 1993-1-1 so far. They are not read for {designCode}.");

                for (int i = 0; i < allResults.ObjectsCount; i++)
                {
                    int memberNumber = allResults.ObjectUsrNo(i);
                    IRDimDetailedRes memberResult = allResults.Get(memberNumber) as IRDimDetailedRes;

                    if (memberResult == null)
                    {
                        Engine.Base.Compute.RecordWarning($"Could not read the verification result for member {memberNumber}.");
                        continue;
                    }

                    results.Add(memberResult.FromRobot(designCode, request.IncludeCodeResults));
                }
            }
            catch (Exception e)
            {
                Engine.Base.Compute.RecordError($"Steel member verification failed in Robot: {e.Message}");
            }
            finally
            {
                if (engine != null && calcConf != null && robotUnitsFlag != -1)
                {
                    calcConf.SetFlag(IRDimCalcConfFlagType.I_DCCFT_ROBOTUNITS, robotUnitsFlag);
                    engine.SetCalcConf(calcConf);
                }
            }

            return results;
        }

        /***************************************************/
    }
}
