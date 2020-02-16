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
    public class MaterialComparer : IEqualityComparer<IMaterialFragment>
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public bool Equals(IMaterialFragment material1, IMaterialFragment material2)
        {
            if (material1.GetType() == typeof(Steel) && material2.GetType() == typeof(Steel))
            {
                SteelMaterialComparer steelMaterialComparer = new SteelMaterialComparer();
                return steelMaterialComparer.Equals(material1 as Steel, material2 as Steel);
            }
            else if (material1.GetType() == typeof(Concrete) && material2.GetType() == typeof(Concrete))
            {
                ConcreteMaterialComparer concreteMaterialComparer = new ConcreteMaterialComparer();
                return concreteMaterialComparer.Equals(material1 as Concrete, material2 as Concrete);
            }
            else
                return false;
        }

        /***************************************************/

        public int GetHashCode(IMaterialFragment obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;

            return obj.Name == null ? 0 : obj.Name.GetHashCode();
        }

        /***************************************************/
       
    }
}

