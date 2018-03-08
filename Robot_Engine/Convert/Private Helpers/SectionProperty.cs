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
        public static void ISectionType(this ISectionProperty section, IRobotBarSectionData secData)
        {
            SectionType(section as dynamic, section.Material.Name, secData);
        }

        public static IRobotBarSectionType SectionType(this ExplicitSection section, string material, IRobotBarSectionData secData)
        {
            throw new NotImplementedException();
        }

        public static IRobotBarSectionType SectionType(this CableSection section, string material, IRobotBarSectionData secData)
        {
            throw new NotImplementedException();
        }


        public static void SectionType(this SteelSection section, string material, IRobotBarSectionData secData)
        {
            if (section.SectionDimensions.Shape == ShapeType.Polygon)
                SectionShapeType(section.Edges, secData);
            else
                ISectionShapeType(section.SectionDimensions, material, secData);
        }

        public static void SectionType(this ConcreteSection section, string material, IRobotBarSectionData secData)
        {
            if (section.SectionDimension.Shape == ShapeType.Polygon)
                SectionShapeType(section.Edges, secData);
            else
                ISectionShapeType(section.SectionDimension, material, secData);
        }

        public static void ISectionShapeType(this ISectionDimensions section, string material, IRobotBarSectionData secData)
        {
            SectionShapeType(section as dynamic, material, secData);
        }

        private static void SectionShapeType(this StandardBoxDimensions section, string material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_RECT;

            sectionData.MaterialName = material;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T, section.Thickness);

            sectionData.CalcNonstdGeometry();
        }

        private static void SectionShapeType(this FabricatedBoxDimensions section, string material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;
            sectionData.MaterialName = material;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF, section.TopFlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2, section.BotFlangeThickness);

            sectionData.CalcNonstdGeometry();
        }

        private static void SectionShapeType(this FabricatedISectionDimensions section, string material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_II;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
            sectionData.MaterialName = material;

            RobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, section.BotFlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, section.TopFlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, section.BotFlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, section.TopFlangeThickness);

            sectionData.CalcNonstdGeometry();
        }

        private static void SectionShapeType(this StandardISectionDimensions section, string material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_BISYM;
            sectionData.MaterialName = material;

            RobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, section.FlangeThickness);

            sectionData.CalcNonstdGeometry();
        }

        private static void SectionShapeType(this StandardTeeSectionDimensions section, string material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_T;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE;

            sectionData.MaterialName = material;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TF, section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TW, section.WebThickness);

            sectionData.CalcNonstdGeometry();
        }

        private static void SectionShapeType(this TubeDimensions section, string material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_TUBE;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_TUBE;

            sectionData.MaterialName = material;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, section.Diameter);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T, section.Thickness);

            sectionData.CalcNonstdGeometry();
        }

        private static void SectionShapeType(this RectangleSectionDimensions section, string material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_RECT_FILLED;

            sectionData.MaterialName = material;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, section.Width);

            sectionData.CalcNonstdGeometry();
        }

        private static void SectionShapeType(this StandardAngleSectionDimensions section, string material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_L;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_UUAP;

            sectionData.MaterialName = material;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, section.WebThickness);

            sectionData.CalcNonstdGeometry();
        }


        private static void SectionShapeType(this StandardChannelSectionDimensions section, string material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_C;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE;

            sectionData.MaterialName = material;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_B, section.FlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TF, section.FlangeThickness);

            sectionData.CalcNonstdGeometry();
        }

        //private static IRobotBarSectionType SectionShapeType(RectangleSectionDimensions section, IRobotBarSectionData secData)
        //{
        //    return IRobotBarSectionType.I_BST_NS_RECT;
        //}

        private static void SectionShapeType(this IList<ICurve> edges, IRobotBarSectionData secData)
        {
            throw new NotImplementedException();
        }


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
            ISectionDimensions secDim;

            switch (secData.ShapeType)
            {
                case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_B);
                    secDim = new RectangleSectionDimensions(b, h, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromDimensions(secDim);

                case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I:
                    b1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1);
                    b2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2);
                    HF1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1);
                    HF2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2);
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B);
                    secDim = new FabricatedISectionDimensions(h, b1, b2, b, HF1, HF2, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromDimensions(secDim);

                case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_H);
                    b1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_B);
                    HF = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_HF);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_BF);
                    secDim = new StandardTeeSectionDimensions(h, b, b1, HF, 0, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromDimensions(secDim);

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_R:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
                    secDim = new RectangleSectionDimensions(b, h, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromDimensions(secDim);

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_T:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
                    h1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1);
                    h2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H2);
                    l1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1);
                    l2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L2);
                    secDim = new StandardTeeSectionDimensions(h, b, b - l1 - l2, h - h1, 0, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromDimensions(secDim);

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_C:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_DE);
                    secDim = new CircleDimensions(h);
                    return BH.Engine.Structure.Create.ConcreteSectionFromDimensions(secDim);

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_L:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
                    h1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1);
                    l1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1);
                    secDim = new StandardAngleSectionDimensions(h, b, b - l1, h - h1, 0, 0);
                    return BH.Engine.Structure.Create.ConcreteSectionFromDimensions(secDim);

                default:
                    return null;
            }
        }


        public static ISectionProperty BHoMSteelSection(IRobotBarSectionData secData)
        {
            ISectionDimensions secDim;

            if (secData.Type == IRobotBarSectionType.I_BST_STANDARD)
            {
                double d = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                double bf = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
                double Tf = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
                double Tw = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
                double r = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_RA);
                double ri = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_RI);
                double s = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_S);
                double mass = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_WEIGHT);
                //double ass = secData.GetValue(IRobotBarSectionDataValue.);

                List<double> testVal = new List<double>();
                for (int i = 0; i < 41; i++)
                {
                    testVal.Add(secData.GetValue((IRobotBarSectionDataValue)i));
                }

                switch (secData.ShapeType)
                {
                    case IRobotBarSectionShapeType.I_BSST_USER_I_BISYM:
                        secDim = new StandardISectionDimensions(d, bf, Tw, Tf, ri, r);
                        return BH.Engine.Structure.Create.SteelSectionFromDimensions(secDim);

                    case IRobotBarSectionShapeType.I_BSST_USER_RECT:
                        secDim = new StandardBoxDimensions(d, bf, Tf, 0, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromDimensions(secDim);

                    case IRobotBarSectionShapeType.I_BSST_RECT_FILLED:
                        secDim = new RectangleSectionDimensions(d, bf, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromDimensions(secDim);

                    case IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE:
                        secDim = new StandardTeeSectionDimensions(d, bf, Tw, Tf, 0, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromDimensions(secDim);

                    case IRobotBarSectionShapeType.I_BSST_USER_TUBE:
                        secDim = new TubeDimensions(d, Tf);
                        return BH.Engine.Structure.Create.SteelSectionFromDimensions(secDim);

                    default:
                        return null;
                }
            }
            else
            {
                IRobotBarSectionNonstdData nonStdData = secData.GetNonstd(1);
                double D = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D);
                double T = 0;
                double B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B);
                double H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H);
                double TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW);
                double TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF);


                switch (secData.ShapeType)
                {
                    case IRobotBarSectionShapeType.I_BSST_USER_TUBE:
                        T = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T);
                        secDim = new TubeDimensions(D, T);
                        return BH.Engine.Structure.Create.SteelSectionFromDimensions(secDim);

                    case IRobotBarSectionShapeType.I_BSST_USER_RECT:
                        T = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T);
                        secDim = new StandardBoxDimensions(H, B, T, 0, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromDimensions(secDim);

                    case IRobotBarSectionShapeType.I_BSST_USER_I_BISYM:
                        T = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T);
                        secDim = new StandardISectionDimensions(H, B, TW, TF, 0, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromDimensions(secDim);

                    default:
                        return null;
                }
            }

            
        }


    }
}
