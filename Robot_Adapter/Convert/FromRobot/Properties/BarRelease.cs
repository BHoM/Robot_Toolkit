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

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        
        public static BarRelease FromRobot(this IRobotLabel robotLabel, IRobotBarReleaseData robotBarReleaseData, string name = "")
        {
            if (robotLabel.Name != "")
                name = robotLabel.Name;

            BarRelease release = new BarRelease
            {
                Name = name,
                StartRelease = FromRobot(robotBarReleaseData.StartNode),
                EndRelease = FromRobot(robotBarReleaseData.EndNode)
            };

            return release;
        }

        /***************************************************/

        public static Constraint6DOF FromRobot(this IRobotBarEndReleaseData barEndReleaseData)
        {
            if (barEndReleaseData == null)
            {
                Engine.Reflection.Compute.RecordWarning("At least one constraint on at least on BarRelease could not be accessed from Robot and is returned as null.");
                return null;
            }

            Constraint6DOF endRelease = new Constraint6DOF();
            endRelease.TranslationX = FromRobot(barEndReleaseData.UX);
            endRelease.TranslationY = FromRobot(barEndReleaseData.UY);
            endRelease.TranslationZ = FromRobot(barEndReleaseData.UZ);
            endRelease.RotationX = FromRobot(barEndReleaseData.RX);
            endRelease.RotationY = FromRobot(barEndReleaseData.RY);
            endRelease.RotationZ = FromRobot(barEndReleaseData.RZ);

            endRelease.TranslationalStiffnessX = barEndReleaseData.KX;
            endRelease.TranslationalStiffnessY = barEndReleaseData.KY;
            endRelease.TranslationalStiffnessZ = barEndReleaseData.KZ;
            endRelease.RotationalStiffnessX = barEndReleaseData.HX;
            endRelease.RotationalStiffnessY = barEndReleaseData.HY;
            endRelease.RotationalStiffnessZ = barEndReleaseData.HZ;

            return endRelease;
        }

        /***************************************************/

        public static DOFType FromRobot(IRobotBarEndReleaseValue endRelease)
        {
            switch (endRelease)
            {
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC:
                    return DOFType.Spring;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_MINUS:
                    return DOFType.SpringNegative;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_PLUS:
                    return DOFType.SpringPositive;
                case IRobotBarEndReleaseValue.I_BERV_NONE:
                    return DOFType.Fixed;
                case IRobotBarEndReleaseValue.I_BERV_MINUS:
                    return DOFType.FixedNegative;
                case IRobotBarEndReleaseValue.I_BERV_PLUS:
                    return DOFType.FixedPositive;
                case IRobotBarEndReleaseValue.I_BERV_STD:
                    return DOFType.Free;
                case IRobotBarEndReleaseValue.I_BERV_NONLINEAR:
                    return DOFType.NonLinear;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED:
                    return DOFType.SpringRelative;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_MINUS:
                    return DOFType.SpringRelativeNegative;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_PLUS:
                    return DOFType.SpringRelativePositive;
                default:
                    return DOFType.Fixed;
            }
        }

        /***************************************************/
      
    }
}

