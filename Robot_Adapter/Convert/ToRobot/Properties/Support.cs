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
using System.Collections.Generic;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void ToRobot(IRobotNodeSupportData suppData, Constraint6DOF constraint)
        {

            switch (constraint.TranslationX)
            {
                case DOFType.Fixed:
                    suppData.UX = 1;
                    break;
                case DOFType.FixedPositive:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_UX, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_PLUS);
                    break;
                case DOFType.FixedNegative:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_UX, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_MINUS);
                    break;
                default:
                    suppData.UX = 0;
                    break;
            }

            switch (constraint.TranslationY)
            {
                case DOFType.Fixed:
                    suppData.UY = 1;
                    break;
                case DOFType.FixedPositive:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_UY, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_PLUS);
                    break;
                case DOFType.FixedNegative:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_UY, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_MINUS);
                    break;
                default:
                    suppData.UY = 0;
                    break;
            }

            switch (constraint.TranslationZ)
            {
                case DOFType.Fixed:
                    suppData.UZ = 1;
                    break;
                case DOFType.FixedPositive:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_UZ, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_PLUS);
                    break;
                case DOFType.FixedNegative:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_UZ, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_MINUS);
                    break;
                default:
                    suppData.UZ = 0;
                    break;
            }


            switch (constraint.RotationX)
            {
                case DOFType.Fixed:
                    suppData.RX = 1;
                    break;
                case DOFType.FixedPositive:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_RX, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_PLUS);
                    break;
                case DOFType.FixedNegative:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_RX, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_MINUS);
                    break;
                default:
                    suppData.RX = 0;
                    break;
            }

            switch (constraint.RotationY)
            {
                case DOFType.Fixed:
                    suppData.RY = 1;
                    break;
                case DOFType.FixedPositive:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_RY, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_PLUS);
                    break;
                case DOFType.FixedNegative:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_RY, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_MINUS);
                    break;
                default:
                    suppData.RY = 0;
                    break;
            }

            switch (constraint.RotationZ)
            {
                case DOFType.Fixed:
                    suppData.UZ = 1;
                    break;
                case DOFType.FixedPositive:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_RZ, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_PLUS);
                    break;
                case DOFType.FixedNegative:
                    suppData.SetOneDir(IRobotNodeSupportFixingDirection.I_NSFD_RZ, IRobotNodeSupportOneDirectionFixingType.I_NSODFT_MINUS);
                    break;
                default:
                    suppData.RZ = 0;
                    break;
            }
          
        }
       

    }
    /***************************************************/
}





