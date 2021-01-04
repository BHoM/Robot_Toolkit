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

using BH.oM.Base;

namespace BH.oM.Adapters.Robot
{
    public class FramingElementDesignProperties : BHoMObject, IFragment
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        public virtual double EulerBucklingLengthCoefficientY { get; set; } = 1;
        public virtual double EulerBucklingLengthCoefficientZ { get; set; } = 1;
        public virtual double MemberLengthY { get; set; } = 1;
        public virtual double MemberLengthZ { get; set; } = 1;
        public virtual bool MemberLengthYIsRelative { get; set; } = true;
        public virtual bool MemberLengthZIsRelative { get; set; } = true;

        /***************************************************/


        //IRobotLabel rMemberType = memberTypes.Get(i);
        //RobotEurocodeSteelDesignFactors mEuroCodeDesignFactors = rMemberType.Data;
        //string name = rMemberType.Name;
        //object rMemberTypeData = rMemberType.Data;

        //IRDimMembDef memberDef = rMemberType.Data;
        //double length = memberDef.Length;

        //IRDimMembParamsE32 memberDesignParams_EC3 = memberDef.CodeParams;
        //bool angle_conn = memberDesignParams_EC3.AngleConn;
        //double beta = memberDesignParams_EC3.Beta;
        //double boltsDiameter = memberDesignParams_EC3.BoltsDiam;
        //int numberOfBolts = memberDesignParams_EC3.BoltsNo;
        //bool isBracedInY = memberDesignParams_EC3.BracedY;
        //bool isBracedInZ = memberDesignParams_EC3.BracedZ;
        //IRDimBucklingCurveE32 bucklingCurveY = memberDesignParams_EC3.BucklingCurveY;
        //IRDimBucklingCurveE32 bucklingCurveZ = memberDesignParams_EC3.BucklingCurveZ;
        //IRDimBuckDiagramE32 bucklingDiagramY = memberDesignParams_EC3.BucklingDiagramY;
        //IRDimBuckDiagramE32 bucklingDiagramZ = memberDesignParams_EC3.BucklingDiagramZ;
        //IRDimComplexSectE32 complexSection = memberDesignParams_EC3.ComplexSect;
        //double boltEdgeDistance_E2 = memberDesignParams_EC3.DistE2;
        //double boltSpacing_P1 = memberDesignParams_EC3.DistP1;
        //double shearParameter_ETA = memberDesignParams_EC3.Eta;
        //IRDimFireResistE32 fireResistance = memberDesignParams_EC3.FireResist;
        //bool isHotRolledPipe = memberDesignParams_EC3.HotRolledPipes;
        //double kfi = memberDesignParams_EC3.Kfl;
        //double lambda_LT0 = memberDesignParams_EC3.LamLT0;
        //IRDimLatBuckMethodTypeE32 lateralBucklingMethodType = memberDesignParams_EC3.LatBuckMethodType;
        //IRDimLatBuckCoeffDiagramE32 lateralBucklingCoefficientDiagram_LowerFlange = memberDesignParams_EC3.LatCoeffLowerFlange;
        //IRDimLatBuckCoeffDiagramE32 lateralBucklingCoefficientDiagram_UpperFlange = memberDesignParams_EC3.LatCoeffUpperFlange;
        //double lateralBucklingCoefficient_LowerFlange = memberDesignParams_EC3.LatCoeffLowerFlangeValue;
        //double lateralBucklingCoefficient_UpperFlange = memberDesignParams_EC3.LatCoeffUpperFlangeValue;
        //bool considerLeteralBuckling = memberDesignParams_EC3.LateralBuckling;
        //IRDimLoadLevelE32 loadLevel = memberDesignParams_EC3.LoadLevel;
        //double loadLevelValue = memberDesignParams_EC3.LoadLevelValue;
        //IRDimLoadTypeE32 loadTypeY = memberDesignParams_EC3.LoadTypeY;
        //IRDimLoadTypeE32 loadTypeZ = memberDesignParams_EC3.LoadTypeZ;
        //double materialCoefficient_Gamma0 = memberDesignParams_EC3.MaterCoeffGamma0;
        //double materialCoefficient_Gamma1 = memberDesignParams_EC3.MaterCoeffGamma1;
        //double materialCoefficient_Gamma2 = memberDesignParams_EC3.MaterCoeffGamma2;
        //double deflectionLimit_relativeY = memberDesignParams_EC3.RelLimitDeflUy;
        //double deflectionLimit_relativeZ = memberDesignParams_EC3.RelLimitdeflUz;
        //bool isSimplifiedParameters = memberDesignParams_EC3.Simplified;
        //double tensileAreaNetGrossRatio = memberDesignParams_EC3.TensAreaNetGros;
        //IRDimThinWalledE32 thinWalledProperties = memberDesignParams_EC3.ThinWalled;
        //bool considerTorsionalBuckling = memberDesignParams_EC3.TorsBuckOn;
        //bool tubeControl = memberDesignParams_EC3.TubeControl;
        //IRDimYieldStrengthTypeE32 yieldStrengthType = memberDesignParams_EC3.YieldStrengthType;
        //double yieldStrengthValue = memberDesignParams_EC3.YieldStrengthValue;

    }
}


