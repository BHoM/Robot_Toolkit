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
using System.Collections.Generic;
using BH.oM.Structure.Constraints;


namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static Constraint6DOF FromRobot(this RobotNodeSupport robotSupport)
        {
            if (robotSupport?.Data == null)
            {
                Engine.Base.Compute.RecordWarning("Failed to extract at least one Constraint6DOF from Robot.");
                return null;
            }

            RobotNodeSupportData robotSupportData = robotSupport.Data;

            Constraint6DOF constraint6DOF = new Constraint6DOF();

            constraint6DOF.TranslationalStiffnessX = robotSupportData.KX;
            constraint6DOF.TranslationalStiffnessY = robotSupportData.KY;
            constraint6DOF.TranslationalStiffnessZ = robotSupportData.KZ;
            constraint6DOF.RotationalStiffnessX = robotSupportData.HX;
            constraint6DOF.RotationalStiffnessY = robotSupportData.HY;
            constraint6DOF.RotationalStiffnessZ = robotSupportData.HZ;

            constraint6DOF.Name = robotSupport.Name;
            constraint6DOF.TranslationX = GetDOF(robotSupportData, IRobotNodeSupportFixingDirection.I_NSFD_UX, constraint6DOF);
            constraint6DOF.TranslationY = GetDOF(robotSupportData, IRobotNodeSupportFixingDirection.I_NSFD_UY, constraint6DOF);
            constraint6DOF.TranslationZ = GetDOF(robotSupportData, IRobotNodeSupportFixingDirection.I_NSFD_UZ, constraint6DOF);
            constraint6DOF.RotationX = GetDOF(robotSupportData, IRobotNodeSupportFixingDirection.I_NSFD_RX, constraint6DOF);
            constraint6DOF.RotationY = GetDOF(robotSupportData, IRobotNodeSupportFixingDirection.I_NSFD_RY, constraint6DOF);
            constraint6DOF.RotationZ = GetDOF(robotSupportData, IRobotNodeSupportFixingDirection.I_NSFD_RZ, constraint6DOF);

            return constraint6DOF;

            /***************************************************/
        }

        private static DOFType GetDOF(IRobotNodeSupportData suppData, IRobotNodeSupportFixingDirection dir, Constraint6DOF constraint)
        {
            DOFType dofType = DOFType.Free;
            // In the Robot GUI, if it's fixed it overides any options for fixed positive and fixed negative

            bool spring = false;

            switch (dir)
            {
                case IRobotNodeSupportFixingDirection.I_NSFD_UX:
                    dofType = suppData.UX != 0 ? DOFType.Fixed : DOFType.Free;
                    spring = suppData.KX > 0 ? true : false;
                    break;
                case IRobotNodeSupportFixingDirection.I_NSFD_UY:
                    dofType = suppData.UY != 0 ? DOFType.Fixed : DOFType.Free;
                    spring = suppData.KY > 0 ? true : false;
                    break;
                case IRobotNodeSupportFixingDirection.I_NSFD_UZ:
                    dofType = suppData.UZ != 0 ? DOFType.Fixed : DOFType.Free;
                    spring = suppData.KZ > 0 ? true : false;
                    break;
                case IRobotNodeSupportFixingDirection.I_NSFD_RX:
                    dofType = suppData.RX != 0 ? DOFType.Fixed : DOFType.Free;
                    spring = suppData.HX > 0 ? true : false;
                    break;
                case IRobotNodeSupportFixingDirection.I_NSFD_RY:
                    dofType = suppData.RY != 0 ? DOFType.Fixed : DOFType.Free;
                    spring = suppData.HY > 0 ? true : false;
                    break;
                case IRobotNodeSupportFixingDirection.I_NSFD_RZ:
                    dofType = suppData.RZ != 0 ? DOFType.Fixed : DOFType.Free;
                    spring = suppData.HZ > 0 ? true : false;
                    break;
                default:
                    Engine.Base.Compute.RecordError("Invalid IRobotNodeSupportFixingDirection provided.");
                    break;
            }

            // If it's fixed, it overrides the spring element
            if (dofType == DOFType.Fixed)
                return dofType;
            else
            {
                IRobotNodeSupportOneDirectionFixingType fixity = suppData.GetOneDir(dir);
                switch (fixity)
                {
                    case IRobotNodeSupportOneDirectionFixingType.I_NSODFT_PLUS:
                        dofType = spring ? DOFType.SpringPositive : DOFType.FixedPositive;
                        break;
                    case IRobotNodeSupportOneDirectionFixingType.I_NSODFT_MINUS:
                        dofType = spring ? DOFType.SpringNegative : DOFType.FixedNegative;
                        break;
                    case IRobotNodeSupportOneDirectionFixingType.I_NSODFT_NONE:
                        dofType = spring ? DOFType.Spring : DOFType.Free;
                        break;
                }
            }

            return dofType;    
        }
    }
}






