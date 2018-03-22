using BH.oM.Geometry;
using BH.oM.Common.Materials;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {

        public static void RobotMaterial(IRobotMaterialData materialData, Material material)
        {
            materialData.Type = GetMaterialType(material);
            materialData.Name = material.Name;
            materialData.E = material.YoungsModulus;
            materialData.NU = material.PoissonsRatio;
            materialData.RO = material.Density;
            materialData.LX = material.CoeffThermalExpansion;
            materialData.Kirchoff = BH.Engine.Common.Query.ShearModulus(material);
            materialData.DumpCoef = material.DampingRatio;
        }

        public static IRobotMaterialType GetMaterialType(Material mat)
        {
            switch (mat.Type)
            {
                case MaterialType.Aluminium:
                    return IRobotMaterialType.I_MT_ALUMINIUM;
                case MaterialType.Steel:
                    return IRobotMaterialType.I_MT_STEEL;
                case MaterialType.Concrete:
                    return IRobotMaterialType.I_MT_CONCRETE;
                case MaterialType.Timber:
                    return IRobotMaterialType.I_MT_TIMBER;
                case MaterialType.Rebar:
                case MaterialType.Tendon:
                case MaterialType.Glass:
                case MaterialType.Cable:
                    return IRobotMaterialType.I_MT_OTHER;
                default:
                    return IRobotMaterialType.I_MT_OTHER;
            }
        }

        public static MaterialType GetMaterialType(IRobotMaterialType mType)
        {
            switch (mType)
            {
                case IRobotMaterialType.I_MT_CONCRETE:
                    return MaterialType.Concrete;
                case IRobotMaterialType.I_MT_STEEL:
                    return MaterialType.Steel;
                case IRobotMaterialType.I_MT_TIMBER:
                    return MaterialType.Timber;
                case IRobotMaterialType.I_MT_ALUMINIUM:
                    return MaterialType.Aluminium;
                default:
                    return MaterialType.Steel;
            }
        }
    }
}
