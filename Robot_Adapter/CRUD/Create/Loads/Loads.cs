/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Elements;

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
                if (!CheckNotNull(load.Loadcase, oM.Base.Debugging.EventType.Error, load.GetType()))
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
            //Dynamic dispatching should be working, and was working up to a point where it all of a sudden stopped.
            //SHould try commenting out this line of code, and/or make a bigger investigation as to why dynamic dispatching is causing an issue in Robot toolkit
            //Code further down as a fix for now

            //return CheckLoad(load as dynamic);

            //This _should_ not be needed. as dynamic call above _should_ work and _was_ working.
            if(load is IElementLoad<Bar>)
                return CheckLoad(load as IElementLoad<Bar>);
            if(load is IElementLoad<Node>)
                return CheckLoad(load as IElementLoad<Node>);
            if(load is IElementLoad<IAreaElement>)
                return CheckLoad(load as IElementLoad<IAreaElement>);
            if(load is IElementLoad<BHoMObject>)
                return CheckLoad(load as IElementLoad<BHoMObject>);
            if (load is IElementLoad<IBHoMObject>)
                return CheckLoad(load as IElementLoad<IBHoMObject>);
            if (load is IElementLoad<Panel>)
                return CheckLoad(load as IElementLoad<Panel>);
            if(load is IElementLoad<FEMesh>)
                return CheckLoad(load as IElementLoad<FEMesh>);

            return CheckLoad(load);
        }

        /***************************************************/

        private bool CheckLoad<T>(IElementLoad<T> load) where T : IBHoMObject
        {
            if (!CheckNotNull(load))
                return false;

            Type type = load.GetType();
            if (load.Objects.Elements.Any(x => !CheckNotNull(x, oM.Base.Debugging.EventType.Error, type)))
                return false;

            if (load.HasAssignedObjectIds(AdapterIdFragmentType))
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






