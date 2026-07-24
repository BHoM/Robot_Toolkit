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
using BH.oM.Base;
using BH.oM.Structure.Loads;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected bool Update(IEnumerable<ILoad> loads)
        {
            RobotCaseServer caseServer = m_RobotApplication.Project.Structure.Cases;

            foreach (ILoad load in loads)
            {
                BHoMObject bhomLoad = load as BHoMObject;
                int loadRecordId;

                if (!CheckInputObjectAndExtractAdapterIdInt(bhomLoad, out loadRecordId, oM.Base.Debugging.EventType.Error, null, true))
                    continue;

                if (!CheckNotNull(load.Loadcase, oM.Base.Debugging.EventType.Error, load.GetType()))
                    continue;

                RobotSimpleCase sCase = caseServer.Get(load.Loadcase.Number) as RobotSimpleCase;
                if (sCase == null)
                {
                    Engine.Base.Compute.RecordWarning($"Could not find a loadcase with number {load.Loadcase.Number} in Robot. Load could not be updated.");
                    continue;
                }

                IRobotLoadRecord loadRecord = sCase.Records.Get(loadRecordId);
                if (loadRecord == null)
                {
                    Engine.Base.Compute.RecordWarning($"Could not find a load record with id {loadRecordId} in loadcase {load.Loadcase.Number}. Load could not be updated.");
                    continue;
                }

                Convert.UpdateLoadValue(load as dynamic, loadRecord);
            }

            return true;
        }

        /***************************************************/
    }
}
