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

using System.ComponentModel;
using BH.oM.Adapters.Robot;
using RobotOM;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Converts the verification result of a single member from the Robot steel design module to a BHoM SteelMemberVerification. Values for limit states Robot did not check are set to NaN.")]
        public static SteelMemberVerification FromRobot(this IRDimDetailedRes result, string designCode, bool includeCodeResults = false)
        {
            if (result == null)
                return null;

            bool uls = result.IsLimitStateUltimate != 0;
            bool slsUy = result.IsLimitStateService_uy != 0;
            bool slsUz = result.IsLimitStateService_uz != 0;
            bool slsVx = result.IsLimitStateService_vx != 0;
            bool slsVy = result.IsLimitStateService_vy != 0;

            ISteelDesignCodeResults codeResults = uls && includeCodeResults ? FromRobotSteelCodeResults(result.CodeResults, designCode) : null;

            return new SteelMemberVerification(
                result.MemberUsrNo,
                uls ? result.GovernCaseName ?? "" : "",
                0,
                0,
                designCode ?? "",
                result.ProfileName ?? "",
                result.MaterialName ?? "",
                result.ResOfCalc.FromRobot(),
                uls ? result.Ratio : double.NaN,
                uls ? result.Lay : double.NaN,
                uls ? result.Laz : double.NaN,
                slsUy ? result.UY : double.NaN,
                slsUz ? result.UZ : double.NaN,
                slsUy ? result.RelLimit_uy : double.NaN,
                slsUz ? result.RelLimit_uz : double.NaN,
                slsUy ? result.GovernCaseName_uy ?? "" : "",
                slsUz ? result.GovernCaseName_uz ?? "" : "",
                slsVx ? result.vx : double.NaN,
                slsVy ? result.vy : double.NaN,
                slsVx ? result.RelLimit_vx : double.NaN,
                slsVy ? result.RelLimit_vy : double.NaN,
                slsVx ? result.GovernCaseName_vx ?? "" : "",
                slsVy ? result.GovernCaseName_vy ?? "" : "",
                codeResults);
        }

        /***************************************************/

        [Description("Converts the Robot member calculation outcome to a BHoM DesignCheckStatus.")]
        public static DesignCheckStatus FromRobot(this IRDimMembCalcRetValue value)
        {
            switch (value)
            {
                case IRDimMembCalcRetValue.I_DMCRV_CORRECT:
                    return DesignCheckStatus.Pass;
                case IRDimMembCalcRetValue.I_DMCRV_INCORRECT:
                    return DesignCheckStatus.Fail;
                case IRDimMembCalcRetValue.I_DMCRV_INSTABILITY:
                    return DesignCheckStatus.Unstable;
                case IRDimMembCalcRetValue.I_DMCRV_INCORRECTDATA:
                    return DesignCheckStatus.IncorrectData;
                default:
                    return DesignCheckStatus.Undefined;
            }
        }

        /***************************************************/
    }
}
