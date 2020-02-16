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

using RobotOM;
using BH.oM.Structure.Constraints;
using System.Collections.Generic;
using System;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.MaterialFragments;

namespace BH.Engine.Robot
{
    public class ConcreteMaterialComparer : IEqualityComparer<Concrete>
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public bool Equals(Concrete material1, Concrete material2)
        {
            if(material1.Name == material2.Name &&
               material1.Density == material2.Density &&
               material1.YoungsModulus == material2.YoungsModulus &&
               material1.PoissonsRatio == material2.PoissonsRatio &&
               material1.ThermalExpansionCoeff == material2.ThermalExpansionCoeff &&
               material1.DampingRatio == material2.DampingRatio &&
               material1.CubeStrength == material2.CubeStrength &&
               material1.CylinderStrength == material2.CylinderStrength)           
               return true;
            else
                return false;
        }

        /***************************************************/

        public int GetHashCode(Concrete obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;

            return obj.Name == null ? 0 : obj.Name.GetHashCode();
        }

        /***************************************************/
       
    }
}

