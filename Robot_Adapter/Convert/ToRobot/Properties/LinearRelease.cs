/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

        public static void ToRobot(IRobotLinearReleaseData releaseData, Constraint4DOF constraint)
        {
            releaseData.UX = GetLinearReleaseType(constraint.TranslationX);
            releaseData.UY = GetLinearReleaseType(constraint.TranslationY);
            releaseData.UZ = GetLinearReleaseType(constraint.TranslationZ);
            releaseData.RX = GetLinearReleaseType(constraint.RotationX);

            releaseData.KX = constraint.TranslationalStiffnessX;
            releaseData.KY = constraint.TranslationalStiffnessY;
            releaseData.KZ = constraint.TranslationalStiffnessZ;
            releaseData.HX = constraint.RotationalStiffnessX;
        }

        /***************************************************/

        public static IRobotLinearReleaseDefinitionType GetLinearReleaseType(DOFType linearReleases)
        {
            switch (linearReleases)
            {
                case DOFType.FixedNegative:
                    return IRobotLinearReleaseDefinitionType.I_LRDT_MINUS;
                case DOFType.FixedPositive:
                    return IRobotLinearReleaseDefinitionType.I_LRDT_PLUS;
                case DOFType.Free:
                    return IRobotLinearReleaseDefinitionType.I_LRDT_RELEASED;
                case DOFType.Fixed:
                    return IRobotLinearReleaseDefinitionType.I_LRDT_NONE;
                default:
                    return IRobotLinearReleaseDefinitionType.I_LRDT_NONE;
            }
        }

        /***************************************************/
    }
}






