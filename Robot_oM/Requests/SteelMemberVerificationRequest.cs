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

using System.Collections.Generic;
using System.ComponentModel;
using BH.oM.Structure.Requests;

namespace BH.oM.Adapters.Robot
{
    [Description("Runs Robot's steel member verification for the requested members and load cases, and returns a SteelMemberVerification result for each member. \n" +
                 "The model must have been analysed first. The members are checked with the design code and member type labels already set in Robot.")]
    public class SteelMemberVerificationRequest : IStructuralResultRequest
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        [Description("The members to verify. Can be pulled Bars or Robot member numbers. If empty, all members are verified.")]
        public virtual List<object> ObjectIds { get; set; } = new List<object>();

        [Description("The load cases and combinations to verify against. Can be Loadcases, LoadCombinations or Robot case numbers. If empty, all cases are used.")]
        public virtual List<object> Cases { get; set; } = new List<object>();

        [Description("Not used by the Robot design module. Present to satisfy the result request interface.")]
        public virtual List<string> Modes { get; set; } = new List<string>();

        [Description("If true, the ultimate limit state checks are carried out and the utilisation and slenderness values are populated.")]
        public virtual bool UltimateLimitState { get; set; } = true;

        [Description("If true, the serviceability limit state checks are carried out and the deflection and displacement values are populated. \n" +
                     "Only members whose Robot member type has deflection or displacement checks switched on return values.")]
        public virtual bool ServiceabilityLimitState { get; set; } = false;

        [Description("If true, design code specific values (resistances, buckling factors and the individual check ratios) are read for each member. \n" +
                     "Only EN 1993-1-1 is supported so far. Reading them makes the pull slower on large models.")]
        public virtual bool IncludeCodeResults { get; set; } = true;

        /***************************************************/
    }
}
