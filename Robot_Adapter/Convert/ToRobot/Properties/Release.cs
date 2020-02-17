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

        public static void ToRobot(IRobotBarEndReleaseData rData, Constraint6DOF bhomRelease)
        {
            rData.UX = ToRobot(bhomRelease.TranslationX);
            rData.UY = ToRobot(bhomRelease.TranslationY);
            rData.UZ = ToRobot(bhomRelease.TranslationZ);
            rData.RX = ToRobot(bhomRelease.RotationX);
            rData.RY = ToRobot(bhomRelease.RotationY);
            rData.RZ = ToRobot(bhomRelease.RotationZ);

            rData.KX = bhomRelease.TranslationalStiffnessX;
            rData.KY = bhomRelease.TranslationalStiffnessY;
            rData.KZ = bhomRelease.TranslationalStiffnessZ;
            rData.HX = bhomRelease.RotationalStiffnessX;
            rData.HY = bhomRelease.RotationalStiffnessY;
            rData.HZ = bhomRelease.RotationalStiffnessZ;
        }

        /***************************************************/

        public static Constraint6DOF FromRobot(IRobotBarEndReleaseData rData)
        {
            Constraint6DOF bhomEndRelease = new Constraint6DOF();
            bhomEndRelease.TranslationX = FromRobot(rData.UX);
            bhomEndRelease.TranslationY = FromRobot(rData.UY);
            bhomEndRelease.TranslationZ = FromRobot(rData.UZ);
            bhomEndRelease.RotationX = FromRobot(rData.RX);
            bhomEndRelease.RotationY = FromRobot(rData.RY);
            bhomEndRelease.RotationZ = FromRobot(rData.RZ);

            bhomEndRelease.TranslationalStiffnessX = rData.KX;
            bhomEndRelease.TranslationalStiffnessY = rData.KY;
            bhomEndRelease.TranslationalStiffnessZ = rData.KZ;
            bhomEndRelease.RotationalStiffnessX = rData.HX;
            bhomEndRelease.RotationalStiffnessY = rData.HY;
            bhomEndRelease.RotationalStiffnessZ = rData.HZ;

            return bhomEndRelease;
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

        public static IRobotBarEndReleaseValue ToRobot(DOFType endRelease)
        {
            switch (endRelease)
            {
                case DOFType.Spring:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC;
                case DOFType.SpringNegative:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_MINUS;
                case DOFType.SpringPositive:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_PLUS;
                case DOFType.Free:
                    return IRobotBarEndReleaseValue.I_BERV_STD;
                case DOFType.FixedNegative:
                    return IRobotBarEndReleaseValue.I_BERV_MINUS;
                case DOFType.FixedPositive:
                    return IRobotBarEndReleaseValue.I_BERV_PLUS;
                case DOFType.Fixed:
                    return IRobotBarEndReleaseValue.I_BERV_NONE;
                case DOFType.NonLinear:
                    return IRobotBarEndReleaseValue.I_BERV_NONLINEAR;
                case DOFType.SpringRelative:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED;
                case DOFType.SpringRelativeNegative:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_MINUS;
                case DOFType.SpringRelativePositive:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_PLUS;
                default:
                    return IRobotBarEndReleaseValue.I_BERV_STD;
            }
        }
     
        /***************************************************/
    }
}

