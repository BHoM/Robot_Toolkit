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

        private bool CheckNotNull<T>(T obj, EventType errorLevel = EventType.Error, Type owningType = null)
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

        private bool CheckHasAdapterId<T>(T obj, EventType errorLevel = EventType.Error, Type owningType = null, bool isUpdate = false) where T : BHoMObject
        {
            if (obj == null || obj.CustomData == null || !obj.CustomData.ContainsKey(AdapterIdName))
            {
                if (owningType == null)
                    InputTypeHasNoIdMessage(typeof(T), errorLevel, isUpdate);
                else
                    PropertyTypeHasNoIdMessage(owningType, typeof(T), errorLevel, isUpdate);

                return false;
            }

            return true;
        }

        /***************************************************/

        private bool CheckInputObject<T>(T obj, EventType errorLevel = EventType.Error, Type owningType = null, bool isUpdate = false) where T : BHoMObject
        {
            return CheckNotNull(obj, errorLevel, owningType) && CheckHasAdapterId(obj, errorLevel, owningType, isUpdate);
        }

        /***************************************************/

        private bool CheckInputObjectAndExtractAdapterIdInt<T>(T obj, out int id, EventType errorLevel = EventType.Error, Type owningType = null, bool isUpdate = false) where T : BHoMObject
        {
            if (!CheckInputObject(obj, errorLevel, owningType, isUpdate))
            {
                id = -1;
                return false;
            }

            object idObject;
            if (obj.CustomData.TryGetValue(AdapterIdName, out idObject) && idObject != null)
            {
                if (idObject is int)
                {
                    id = (int)idObject;
                    return true;
                }

                return int.TryParse(idObject.ToString(), out id);
            }

            if (owningType == null)
                AdapterIdNotCorrectTypeMessage(typeof(T), EventType.Error, isUpdate);
            else
                PropertyAdapterIdNotCorrectTypeMessage(owningType, typeof(T), errorLevel, isUpdate);

            id = -1;
            return false;
        }

        /***************************************************/
    }
}
