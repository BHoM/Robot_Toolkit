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
        public static void ISectionType(ISectionProperty section, IRobotBarSectionData secData)
        {
            SectionType(section as dynamic, section.Material.Name, secData);
        }

        public static IRobotBarSectionType SectionType(ExplicitSection section, string material, IRobotBarSectionData secData)
        {
            throw new NotImplementedException();
        }

        public static IRobotBarSectionType SectionType(CableSection section, string material, IRobotBarSectionData secData)
        {
            throw new NotImplementedException();
        }


        public static void SectionType(SteelSection section, string material, IRobotBarSectionData secData)
        {
            if (section.SectionDimensions.Shape == ShapeType.Polygon)
                SectionShapeType(section.Edges, secData);
            else
                ISectionShapeType(section.SectionDimensions, material, secData);
        }

        public static void SectionType(ConcreteSection section, string material, IRobotBarSectionData secData)
        {
            if (section.SectionDimension.Shape == ShapeType.Polygon)
                SectionShapeType(section.Edges, secData);
            else
                ISectionShapeType(section.SectionDimension, material, secData);
        }

        public static void ISectionShapeType(ISectionDimensions section, string material, IRobotBarSectionData secData)
        {
            SectionShapeType(section as dynamic, material, secData);
        }

        private static void SectionShapeType(StandardBoxDimensions section, string material, IRobotBarSectionData secData)
        {
            secData.Type = IRobotBarSectionType.I_BST_STANDARD;
            secData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_RECT;
            secData.MaterialName = material;

            RobotBarSectionData stdSecData = (RobotBarSectionData)secData;
            stdSecData.SetValue(IRobotBarSectionDataValue.I_BSDV_BF, section.Width);
            stdSecData.SetValue(IRobotBarSectionDataValue.I_BSDV_D, section.Height);
            stdSecData.SetValue(IRobotBarSectionDataValue.I_BSDV_TW, section.Thickness);
            stdSecData.SetValue(IRobotBarSectionDataValue.I_BSDV_TF, section.Thickness);
            stdSecData.SetValue(IRobotBarSectionDataValue.I_BSDV_RA, section.InnerRadius);
            stdSecData.SetValue(IRobotBarSectionDataValue.I_BSDV_RI, section.OuterRadius);
        }

        private static void SectionShapeType(StandardISectionDimensions section, string material, IRobotBarSectionData secData)
        {
            secData.Type = IRobotBarSectionType.I_BST_NS_I;
            secData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
            secData.MaterialName = material;

            RobotBarSectionNonstdData nonsStdData = secData.CreateNonstd(0);
            RobotBarSectionData stdSecData = (RobotBarSectionData)secData;
            stdSecData.SetValue(IRobotBarSectionDataValue.I_BSDV_BF, section.Width);

        }

        //private static IRobotBarSectionType SectionShapeType(StandardTeeSectionDimensions section, IRobotBarSectionData secData)
        //{
        //    return IRobotBarSectionType.I_BST_NS_T;
        //}

        //private static IRobotBarSectionType SectionShapeType(StandardChannelSectionDimensions section, IRobotBarSectionData secData)
        //{
        //    return IRobotBarSectionType.I_BST_NS_TUBE;
        //}

        //private static IRobotBarSectionType SectionShapeType(RectangleSectionDimensions section, IRobotBarSectionData secData)
        //{
        //    return IRobotBarSectionType.I_BST_NS_RECT;
        //}

        private static void SectionShapeType(IList<ICurve> edges, IRobotBarSectionData secData)
        {
            throw new NotImplementedException();
        }

    }
}
