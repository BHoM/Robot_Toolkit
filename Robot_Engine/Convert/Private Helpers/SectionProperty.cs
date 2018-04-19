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
        public static void ISectionProperty(this ISectionProperty section, IRobotBarSectionData secData)
        {
            SectionProperty(section as dynamic, section.Material, secData);
        }

        /***************************************************/

        public static void SectionProperty(this ExplicitSection section, Material material, IRobotBarSectionData secData)
        {
            
        }

        /***************************************************/

        public static void SectionProperty(this CableSection section, Material material, IRobotBarSectionData secData)
        {
            
        }

        /***************************************************/

        //private static void SectionShapeType(this IList<ICurve> edges, IRobotBarSectionData secData)
        //{
        //    throw new NotImplementedException();
        //}

        /***************************************************/

        public static ISectionProperty IBHoMSection(IRobotBarSectionData secData, Material material)
        {
            switch (material.Type)
            {
                case MaterialType.Aluminium:
                    return null;
                case MaterialType.Steel:
                    return BHoMSteelSection(secData);
                case MaterialType.Concrete:
                    return BHoMConcreteSection(secData);
                case MaterialType.Timber:
                    return null;
                case MaterialType.Rebar:
                    return null;
                case MaterialType.Tendon:
                    return null;
                case MaterialType.Glass:
                    return null;
                case MaterialType.Cable:
                    return null;
                default:
                    return null;
            }
        }
    }
}
