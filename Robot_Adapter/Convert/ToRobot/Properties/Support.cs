/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void ToRobot(IRobotNodeSupportData suppData, Constraint6DOF constraint)
        {
            suppData.UX = constraint.TranslationX == DOFType.Fixed ? 1 : 0;
            suppData.UY = constraint.TranslationY == DOFType.Fixed ? 1 : 0;
            suppData.UZ = constraint.TranslationZ == DOFType.Fixed ? 1 : 0;
            suppData.RX = constraint.RotationX == DOFType.Fixed ? 1 : 0;
            suppData.RY = constraint.RotationY == DOFType.Fixed ? 1 : 0;
            suppData.RZ = constraint.RotationZ == DOFType.Fixed ? 1 : 0;
            suppData.KX = constraint.TranslationalStiffnessX;
            suppData.KY = constraint.TranslationalStiffnessY;
            suppData.KZ = constraint.TranslationalStiffnessZ;
            suppData.HX = constraint.RotationalStiffnessX;
            suppData.HY = constraint.RotationalStiffnessY;
            suppData.HZ = constraint.RotationalStiffnessZ;
        }

        /***************************************************/
    }
}


