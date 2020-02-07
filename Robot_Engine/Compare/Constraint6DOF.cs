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

namespace BH.Engine.Robot
{
    public class Constraint6DOFComparer : IEqualityComparer<Constraint6DOF>
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public bool Equals(Constraint6DOF support_1, Constraint6DOF support_2)
        {
            if (
                support_1.Name == support_2.Name &&
                support_1.TranslationX == support_2.TranslationX &&
                support_1.TranslationY == support_2.TranslationY &&
                support_1.TranslationZ == support_2.TranslationZ &&
                support_1.RotationX == support_2.RotationX &&
                support_1.RotationY == support_2.RotationY &&
                support_1.RotationZ == support_2.RotationZ &&
                support_1.TranslationalStiffnessX == support_2.TranslationalStiffnessX &&
                support_1.TranslationalStiffnessY == support_2.TranslationalStiffnessY &&
                support_1.TranslationalStiffnessZ == support_2.TranslationalStiffnessZ &&
                support_1.RotationalStiffnessX == support_2.RotationalStiffnessX &&
                support_1.RotationalStiffnessY == support_2.RotationalStiffnessY &&
                support_1.RotationalStiffnessZ == support_2.RotationalStiffnessZ)

            {
                return true;
            }
            else
                return false;
        }
        /***************************************************/

        public int GetHashCode(Constraint6DOF obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;

            return obj.Name == null ? 0 : obj.Name.GetHashCode();
        }

        /***************************************************/
    }
}

