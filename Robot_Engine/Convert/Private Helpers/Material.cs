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
    }
}
