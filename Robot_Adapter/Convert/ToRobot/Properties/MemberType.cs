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
using BH.oM.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void ToRobot(IRobotLabel robotMemberType, FramingElementDesignProperties framingElementDesignProperties, DesignCode_Steel designCode)
        {
            if (robotMemberType == null || framingElementDesignProperties == null)
                return;

            IRDimMembDef memberDef = robotMemberType.Data;
            
            if (memberDef == null)
                return;

            // Set member lengths if specified
            if (framingElementDesignProperties.MemberLengthYIsRelative)
                memberDef.SetLengthYZUV(IRDimMembDefLengthDataType.I_DMDLDT_LENGTH_Y, -framingElementDesignProperties.MemberLengthY);
            else
                memberDef.SetLengthYZUV(IRDimMembDefLengthDataType.I_DMDLDT_LENGTH_Y, framingElementDesignProperties.MemberLengthY);

            if (framingElementDesignProperties.MemberLengthZIsRelative)
                memberDef.SetLengthYZUV(IRDimMembDefLengthDataType.I_DMDLDT_LENGTH_Z, -framingElementDesignProperties.MemberLengthZ);
            else
                memberDef.SetLengthYZUV(IRDimMembDefLengthDataType.I_DMDLDT_LENGTH_Z, framingElementDesignProperties.MemberLengthZ);

            // Set buckling length coefficients based on the specified design code
            if (designCode == DesignCode_Steel.BS_EN_1993_1_2005_NA_2008_A1_2014)
            {
                IRDimMembParamsE32 memberDesignParams_EC3 = memberDef.CodeParams;
                memberDesignParams_EC3.BuckLengthCoeffY = framingElementDesignProperties.EulerBucklingLengthCoefficientY;
                memberDesignParams_EC3.BuckLengthCoeffZ = framingElementDesignProperties.EulerBucklingLengthCoefficientZ;
                
                // Set lateral torsional buckling parameters if provided
                if (framingElementDesignProperties.LateralTorsionalBuckling != null)
                {
                    var ltb = framingElementDesignProperties.LateralTorsionalBuckling;
                    memberDesignParams_EC3.LateralBuckling = ltb.LateralBucklingEnabled;
                    
                    if (ltb.LateralBucklingEnabled)
                    {
                        memberDesignParams_EC3.LatCoeffUpperFlangeValue = ltb.LoadLevelUpperFlangeAuto ? 0 : ltb.LoadLevelUpperFlangeValue;
                        memberDesignParams_EC3.LatCoeffLowerFlangeValue = ltb.LoadLevelLowerFlangeAuto ? 0 : ltb.LoadLevelLowerFlangeValue;
                        memberDesignParams_EC3.LamLT0 = ltb.LambdaLT0;
                        memberDesignParams_EC3.Beta = ltb.Beta;
                        memberDesignParams_EC3.Kfl = ltb.Kfl;
                    }
                }
                
                // Set service limit state parameters if provided
                if (framingElementDesignProperties.ServiceLimitState != null)
                {
                    var sls = framingElementDesignProperties.ServiceLimitState;
                    if (sls.DeflectionLimitEnabled)
                    {
                        if (sls.UseRelativeLimitY)
                            memberDesignParams_EC3.RelLimitDeflUy = sls.RelativeDeflectionLimitY;
                        if (sls.UseRelativeLimitZ)
                            memberDesignParams_EC3.RelLimitdeflUz = sls.RelativeDeflectionLimitZ;
                    }
                }
                
                memberDef.CodeParams = memberDesignParams_EC3;
            }
            else if (designCode == DesignCode_Steel.BS5950)
            {
                IRDimMembParamsBS59 memberDesignParams_BS5950 = memberDef.CodeParams;
                memberDesignParams_BS5950.BuckLengthCoeffY = framingElementDesignProperties.EulerBucklingLengthCoefficientY;
                memberDesignParams_BS5950.BuckLengthCoeffZ = framingElementDesignProperties.EulerBucklingLengthCoefficientZ;
                
                // Set lateral torsional buckling parameters if provided
                if (framingElementDesignProperties.LateralTorsionalBuckling != null)
                {
                    var ltb = framingElementDesignProperties.LateralTorsionalBuckling;
                    // Note: BS5950 interface may not have direct LateralBuckling property
                    // Check Robot API documentation for available properties
                    // memberDesignParams_BS5950.LateralBuckling = ltb.LateralBucklingEnabled;
                }
                
                // Set service limit state parameters if provided
                if (framingElementDesignProperties.ServiceLimitState != null)
                {
                    var sls = framingElementDesignProperties.ServiceLimitState;
                    // Note: BS5950 may have different deflection limit properties
                    // Implementation would depend on available Robot API properties
                }
                
                memberDef.CodeParams = memberDesignParams_BS5950;
            }
            else if (designCode == DesignCode_Steel.BS5950_2000)
            {
                IRDimMembParamsBS59_2000 memberDesignParams_BS5950_2000 = memberDef.CodeParams;
                memberDesignParams_BS5950_2000.BuckLengthCoeffY = framingElementDesignProperties.EulerBucklingLengthCoefficientY;
                memberDesignParams_BS5950_2000.BuckLengthCoeffZ = framingElementDesignProperties.EulerBucklingLengthCoefficientZ;
                
                // Set lateral torsional buckling parameters if provided
                if (framingElementDesignProperties.LateralTorsionalBuckling != null)
                {
                    var ltb = framingElementDesignProperties.LateralTorsionalBuckling;
                    // Note: BS5950_2000 interface may not have direct LateralBuckling property
                    // Check Robot API documentation for available properties
                    // memberDesignParams_BS5950_2000.LateralBuckling = ltb.LateralBucklingEnabled;
                }
                
                // Set service limit state parameters if provided
                if (framingElementDesignProperties.ServiceLimitState != null)
                {
                    var sls = framingElementDesignProperties.ServiceLimitState;
                    // Note: BS5950_2000 may have different deflection limit properties
                    // Implementation would depend on available Robot API properties
                }
                
                memberDef.CodeParams = memberDesignParams_BS5950_2000;
            }
            else if (designCode == DesignCode_Steel.ANSI_AISC_360_10)
            {
                IRDimMembParamsANS memberDesignParams_AISC_360_10 = memberDef.CodeParams;
                memberDesignParams_AISC_360_10.BuckLenghtCoeffY = framingElementDesignProperties.EulerBucklingLengthCoefficientY;
                memberDesignParams_AISC_360_10.BuckLenghtCoeffZ = framingElementDesignProperties.EulerBucklingLengthCoefficientZ;
                
                // Set lateral torsional buckling parameters if provided
                if (framingElementDesignProperties.LateralTorsionalBuckling != null)
                {
                    var ltb = framingElementDesignProperties.LateralTorsionalBuckling;
                    // Note: ANSI_AISC_360_10 interface may not have direct LateralBuckling property
                    // Check Robot API documentation for available properties
                    // memberDesignParams_AISC_360_10.LateralBuckling = ltb.LateralBucklingEnabled;
                }
                
                // Set service limit state parameters if provided
                if (framingElementDesignProperties.ServiceLimitState != null)
                {
                    var sls = framingElementDesignProperties.ServiceLimitState;
                    // Note: ANSI_AISC_360_10 may have different deflection limit properties
                    // Implementation would depend on available Robot API properties
                }
                
                memberDef.CodeParams = memberDesignParams_AISC_360_10;
            }
        }

        /***************************************************/

    }
}






