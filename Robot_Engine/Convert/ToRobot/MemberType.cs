using BH.oM.Geometry;
using BH.oM.Common.Materials;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Adapters.Robot;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        public static void RobotMemberType(IRobotLabel robotMemberType, FramingElementDesignProperties framingElementDesignProperties)
        {
            
            //RobotEurocodeSteelDesignFactors mEuroCodeDesignFactors = robotMemberType.Data;
            //string name = robotMemberType.Name;
            //object rMemberTypeData = robotMemberType.Data;

            //IRDimMembDef memberDef = robotMemberType.Data;

            //double length = memberDef.Length;

            //IRDimMembParamsE32 memberDesignParams_EC3 = memberDef.CodeParams;
            //bool angle_conn = memberDesignParams_EC3.AngleConn;
            //double beta = memberDesignParams_EC3.Beta;
            //double boltsDiameter = memberDesignParams_EC3.BoltsDiam;
            //int numberOfBolts = memberDesignParams_EC3.BoltsNo;
            //bool isBracedInY = memberDesignParams_EC3.BracedY;
            //bool isBracedInZ = memberDesignParams_EC3.BracedZ;
            //double bucklingLengthCoefficientY = memberDesignParams_EC3.BuckLengthCoeffY;
            //double bucklingLengthCoefficientZ = memberDesignParams_EC3.BuckLengthCoeffZ;
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

            //FramingElementDesignProperties bhomDesignProps = BHE.Create.FramingElementDesignProperties(rMemberType.Name);

            //bhomDesignPropsList.Add(bhomDesignProps);

        }
    }
}
