/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using RobotOM;
using BH.oM.Structure.Constraints;
using System.Collections.Generic;
using System;
using BH.oM.Structure.SurfaceProperties;

namespace BH.Engine.Adapters.Robot
{
    public class LoadingPanelPropertyComparer : IEqualityComparer<LoadingPanelProperty>
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public bool Equals(LoadingPanelProperty property1, LoadingPanelProperty property2)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(property1, property2))
                return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(property1, null) || Object.ReferenceEquals(property2, null))
                return false;

            //Check if the GUIDs are the same
            if (property1.BHoM_Guid == property2.BHoM_Guid)
                return true;

            if (property1.Name == property2.Name &&
                 property1.Material.Name == property2.Material.Name &&
                 property1.LoadApplication == property2.LoadApplication)
                return true;
            return false;
        }

        /***************************************************/

        public int GetHashCode(LoadingPanelProperty obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;

            return obj.Name == null ? 0 : obj.Name.GetHashCode();
        }

        /***************************************************/
       
    }
}



