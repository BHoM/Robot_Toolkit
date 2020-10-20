/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Loads;
using BH.oM.Base;
using BH.Engine.Structure;
using System.Linq;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<ILoad> loads)
        {
            RobotCaseServer caseServer = m_RobotApplication.Project.Structure.Cases;
            RobotGroupServer rGroupServer = m_RobotApplication.Project.Structure.Groups;

            loads = loads.Where(x => ICheckLoad(x));

            foreach (ILoad load in loads)
            {
                if (!CheckNotNull(load.Loadcase, oM.Reflection.Debugging.EventType.Error, load.GetType()))
                    continue;

                IRobotCase rCase = caseServer.Get(load.Loadcase.Number);
                RobotSimpleCase sCase = rCase as RobotSimpleCase;
                Convert.ToRobot(load as dynamic, sCase, rGroupServer);               
            }
            
            return true;
        }

        /***************************************************/

        private bool ICheckLoad(ILoad load)
        {
            return CheckLoad(load as dynamic);
        }

        /***************************************************/

        private bool CheckLoad<T>(IElementLoad<T> load) where T : IBHoMObject
        {
            if (!CheckNotNull(load))
                return false;

            Type type = load.GetType();
            if (load.Objects.Elements.Any(x => !CheckNotNull(x, oM.Reflection.Debugging.EventType.Error, type)))
                return false;

            if (load.HasAssignedObjectIds(AdapterIdName))
                return true;
            else
            {
                BH.Adapter.Modules.Structure.ErrorMessages.LoadsWithoutObejctIdsAssignedError(load);
                return false;
            }
        }

        /***************************************************/

        private bool CheckLoad(ILoad load)
        {
            return CheckNotNull(load);
        }

        /***************************************************/

    }

}


