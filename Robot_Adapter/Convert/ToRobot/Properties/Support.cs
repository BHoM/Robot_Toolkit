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
using System.Collections.Generic;
using System;
using System.Windows.Forms.VisualStyles;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void ToRobot(IRobotNodeSupportData suppData, Constraint6DOF constraint)
        {
            // Springs first, because if someone tries to assign fixed + spring, Robot overrides the spring
            suppData.KX = constraint.TranslationalStiffnessX;
            suppData.KY = constraint.TranslationalStiffnessY;
            suppData.KZ = constraint.TranslationalStiffnessZ;
            suppData.HX = constraint.RotationalStiffnessX;
            suppData.HY = constraint.RotationalStiffnessY;
            suppData.HZ = constraint.RotationalStiffnessZ;

            // Translations and rotations
            SetDOF(suppData, constraint.TranslationX, IRobotNodeSupportFixingDirection.I_NSFD_UX);
            SetDOF(suppData, constraint.TranslationY, IRobotNodeSupportFixingDirection.I_NSFD_UY);
            SetDOF(suppData, constraint.TranslationZ, IRobotNodeSupportFixingDirection.I_NSFD_UZ);
            SetDOF(suppData, constraint.RotationX, IRobotNodeSupportFixingDirection.I_NSFD_RX);
            SetDOF(suppData, constraint.RotationY, IRobotNodeSupportFixingDirection.I_NSFD_RY);
            SetDOF(suppData, constraint.RotationZ, IRobotNodeSupportFixingDirection.I_NSFD_RZ);
        }

        private static void SetDOF(IRobotNodeSupportData suppData, DOFType dofType, IRobotNodeSupportFixingDirection dir)
        {
            switch (dofType)
            {
                case DOFType.Fixed:
                    suppData.SetFixed(dir,1); // Fixed
                    break;
                case DOFType.Free:
                case DOFType.Spring:
                    suppData.SetFixed(dir, 0);
                    suppData.SetOneDir(dir, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_NONE);
                    break;
                case DOFType.SpringPositive:
                case DOFType.FixedPositive:
                    suppData.SetOneDir(dir, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_PLUS);
                    break;
                case DOFType.SpringNegative:
                case DOFType.FixedNegative:
                    suppData.SetOneDir(dir, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_MINUS);
                    break;
                default:
                    suppData.SetFixed(dir, 0);
                    suppData.SetOneDir(dir, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_NONE);
                    Engine.Base.Compute.RecordError($"The support {dofType} is not supported and has been set to Free.");
                    break;
            }

        }
    }
    /***************************************************/
}






