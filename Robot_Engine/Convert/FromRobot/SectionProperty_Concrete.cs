﻿using BH.oM.Geometry;
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

        public static void SectionProperty(this ConcreteSection section, Material material, IRobotBarSectionData secData)
        {
                IConcreteSection(section.SectionProfile, material, secData);
        }

        /***************************************************/

        public static void IConcreteSection(this IProfile section, Material material, IRobotBarSectionData secData)
        {
            ConcreteSection(section as dynamic, material, secData);
        }

        /***************************************************/

        private static void ConcreteSection(this RectangleProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT;

            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_H, section.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_B, section.Width);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ConcreteSection(this TSectionProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_T;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T;

            sectionData.MaterialName = material.Name;

            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_BF, section.Width);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_H, section.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_HF, section.FlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_B, section.WebThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ConcreteSection(this FabricatedISectionProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I;
            sectionData.MaterialName = material.Name;

            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1, section.TopFlangeWidth);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2, section.BotFlangeWidth);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1, section.TopFlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2, section.BotFlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H, section.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B, section.WebThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ConcreteSection(this CircleProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_C;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_COL_C;

            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_DE, section.Diameter);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ConcreteSection(this ISectionProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I;
            sectionData.MaterialName = material.Name;

            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1, section.Width);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2, section.Width);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1, section.FlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2, section.FlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H, section.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B, section.WebThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ConcreteSection(this FreeFormProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.MaterialName = material.Name;
            ConcreteSection steelSection = BH.Engine.Structure.Create.ConcreteSectionFromProfile(section, material);

            sectionData.SetValue(IRobotBarSectionDataValue.I_BSDV_AX, steelSection.Area);
            sectionData.SetValue(IRobotBarSectionDataValue.I_BSDV_IX, steelSection.J);
            sectionData.SetValue(IRobotBarSectionDataValue.I_BSDV_IY, steelSection.Iy);
            sectionData.SetValue(IRobotBarSectionDataValue.I_BSDV_IZ, steelSection.Iz);
            sectionData.SetValue(IRobotBarSectionDataValue.I_BSDV_VY, steelSection.Vy);
            sectionData.SetValue(IRobotBarSectionDataValue.I_BSDV_VZ, steelSection.Vz);
            sectionData.SetValue(IRobotBarSectionDataValue.I_BSDV_VPY, steelSection.Vpy);
            sectionData.SetValue(IRobotBarSectionDataValue.I_BSDV_VPZ, steelSection.Vpz);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        public static ISectionProperty BHoMConcreteSection(IRobotBarSectionData secData)
        {
            double b = 0;
            double h = 0;
            double HF = 0;
            double HF1 = 0;
            double HF2 = 0;
            double b1 = 0;
            double b2 = 0;
            double b3 = 0;
            double h1 = 0;
            double h2 = 0;
            double l1 = 0;
            double l2 = 0;

            RobotBarSectionConcreteData concMember = secData.Concrete;
            IProfile sectionProfile;

            switch (secData.ShapeType)
            {
                case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_B);
                    sectionProfile = BH.Engine.Structure.Create.RectangleProfile(b, h, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromProfile(sectionProfile);

                case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I:
                    b1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1);
                    b2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2);
                    HF1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1);
                    HF2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2);
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B);
                    sectionProfile = BH.Engine.Structure.Create.FabricatedISectionProfile(h, b1, b2, b, HF1, HF2, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromProfile(sectionProfile);

                case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_H);
                    b1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_B);
                    HF = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_HF);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_BF);
                    sectionProfile = BH.Engine.Structure.Create.TSectionProfile(h, b, b1, HF, 0, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromProfile(sectionProfile);

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_R:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
                    sectionProfile = BH.Engine.Structure.Create.RectangleProfile(b, h, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromProfile(sectionProfile);

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_T:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
                    h1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1);
                    h2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H2);
                    l1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1);
                    l2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L2);
                    sectionProfile = BH.Engine.Structure.Create.TSectionProfile(h, b, b - l1 - l2, h - h1, 0, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromProfile(sectionProfile);

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_C:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_DE);
                    sectionProfile = BH.Engine.Structure.Create.CircleProfile(h);
                    return BH.Engine.Structure.Create.ConcreteSectionFromProfile(sectionProfile);

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_L:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
                    h1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1);
                    l1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1);
                    sectionProfile = BH.Engine.Structure.Create.AngleProfile(h, b, b - l1, h - h1, 0, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromProfile(sectionProfile);

                default:
                    return null;
            }
        }
    }
}