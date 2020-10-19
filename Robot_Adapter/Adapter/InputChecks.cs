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
using BH.oM.Reflection.Debugging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Reflection;
using BH.oM.Base;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public bool CheckNotNull<T>(T obj, EventType errorLevel = EventType.Error, Type owningType = null)
        {
            if (obj == null)
            {
                if (owningType == null)
                    InputTypeIsNullMessage(typeof(T), errorLevel);
                else
                    PropertyTypeIsNullMessage(owningType, typeof(T), errorLevel);

                return false;
            }

            return true;
        }

        /***************************************************/

        public bool CheckHasAdapterId<T>(T obj, EventType errorLevel = EventType.Error, Type owningType = null) where T : BHoMObject
        {
            if (obj == null || obj.CustomData == null || !obj.CustomData.ContainsKey(AdapterIdName))
            {
                if (owningType == null)
                    InputTypeIsNullMessage(typeof(T), errorLevel);
                else
                    PropertyTypeHasNoIdMessage(owningType, typeof(T), errorLevel);

                return false;
            }

            return true;
        }

        /***************************************************/

        public bool CheckInputObject<T>(T obj, EventType errorLevel = EventType.Error, Type owningType = null) where T : BHoMObject
        {
            return CheckNotNull(obj, errorLevel, owningType) && CheckHasAdapterId(obj, errorLevel, owningType);
        }

        /***************************************************/
    }
}
