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

using RobotOM;
using BH.oM.Structure.Constraints;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
       
        public static Constraint4DOF FromRobot(this IRobotLabel robotLabel, IRobotLinearReleaseData robotLinearReleaseData)
        {
            if (robotLabel == null || robotLinearReleaseData == null)
            {
                Engine.Base.Compute.RecordWarning("Failed to extract at least one Constraint4DOF from Robot.");
                return null;
            }

            Constraint4DOF linearRelease = new Constraint4DOF();
            linearRelease.Name = robotLabel.Name;
            linearRelease.TranslationX = GetLinearReleaseType(robotLinearReleaseData.UX);
            linearRelease.TranslationY = GetLinearReleaseType(robotLinearReleaseData.UY);
            linearRelease.TranslationZ = GetLinearReleaseType(robotLinearReleaseData.UZ);
            linearRelease.RotationX = GetLinearReleaseType(robotLinearReleaseData.RX);

            linearRelease.TranslationalStiffnessX = robotLinearReleaseData.KX;
            linearRelease.TranslationalStiffnessY = robotLinearReleaseData.KY;
            linearRelease.TranslationalStiffnessZ = robotLinearReleaseData.KZ;
            linearRelease.RotationalStiffnessX = robotLinearReleaseData.HX;

            return linearRelease;
        }

        /***************************************************/                    

        public static DOFType GetLinearReleaseType(this IRobotLinearReleaseDefinitionType linearReleases)
        {
            switch (linearReleases)
            {
                case IRobotLinearReleaseDefinitionType.I_LRDT_MINUS:
                    return DOFType.FixedNegative;
                case IRobotLinearReleaseDefinitionType.I_LRDT_PLUS:
                    return DOFType.FixedPositive;
                case IRobotLinearReleaseDefinitionType.I_LRDT_RELEASED:
                    return DOFType.Free;
                case IRobotLinearReleaseDefinitionType.I_LRDT_NONE:
                    return DOFType.Fixed;
                default:
                    return DOFType.Fixed;
            }
        }

        /***************************************************/
    }
}





