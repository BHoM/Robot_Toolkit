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

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter 
    {

        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void InputTypeIsNullMessage(Type type, EventType errorLevel = EventType.Error)
        {
            string message;
            if (type == null)
                message = "An input object is null and could not be pushed.";
            else
                message = $"An input object of type {type.ToString()} is null and could not be pushed.";

            Compute.RecordEvent(message, errorLevel);
        }

        /***************************************************/

        public static void InputTypeHasNoIdMessage(Type type, EventType errorLevel = EventType.Error)
        {
            string message;
            if (type == null)
                message = "An input object has no ID assigned and could not be pushed.";
            else
                message = $"An input object of type {type.ToString()} has no ID assigned and could not be pushed.";

            Compute.RecordEvent(message, errorLevel);
        }

        /***************************************************/

        public static void PropertyTypeIsNullMessage(Type owningType, Type propertyType, EventType errorLevel = EventType.Error)
        {
            string message = "A property ";
            if (propertyType != null)
                message += "of type " + propertyType.ToString();

            message += " of an input object ";
                
            if(owningType != null)
                message += "of type " + owningType.ToString();

            message += " is null and could not be pushed. Please check the input data to ensure everything has been correctly assigned.";
    
            Compute.RecordEvent(message, errorLevel);
        }

        /***************************************************/

        public static void PropertyTypeHasNoIdMessage(Type owningType, Type propertyType, EventType errorLevel = EventType.Error)
        {
            string message = "A property ";
            if (propertyType != null)
                message += "of type " + propertyType.ToString();

            message += " of an input object ";

            if (owningType != null)
                message += "of type " + owningType.ToString();

            message += " has no ID assigned and could not be pushed.";

            Compute.RecordEvent(message, errorLevel);
        }

        /***************************************************/
    }
}
