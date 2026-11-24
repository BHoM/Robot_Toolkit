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

namespace BH.oM.Adapters.Robot
{
    /***************************************************/
    /****               Public Enums                ****/
    /***************************************************/

    [Description("Outcome of a Robot steel member verification, as reported by the Robot design module.")]
    public enum DesignCheckStatus
    {
        [Description("The status could not be determined.")]
        Undefined,
        [Description("The member satisfies the code checks.")]
        Pass,
        [Description("The member does not satisfy the code checks.")]
        Fail,
        [Description("The member is unstable.")]
        Unstable,
        [Description("The design data for the member is incorrect or incomplete, so the check could not be carried out.")]
        IncorrectData
    }

    /***************************************************/

}
