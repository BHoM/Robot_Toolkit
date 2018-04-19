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
        public static void SectionProperty(this SteelSection section, Material material, IRobotBarSectionData secData)
        {
                ISteelSection(section.SectionProfile, material, secData);
        }

        public static void ISteelSection(this IProfile section, Material material, IRobotBarSectionData secData)
        {
            SteelSection(section as dynamic, material, secData);
        }

        private static void SteelSection(this BoxProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_RECT;

            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T, section.Thickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void SteelSection(this FabricatedBoxProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;
            sectionData.MaterialName = material.Name;

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

        /***************************************************/

        private static void SteelSection(this FabricatedISectionProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_II;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
            sectionData.MaterialName = material.Name;

            RobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, section.BotFlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, section.TopFlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, section.BotFlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, section.TopFlangeThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void SteelSection(this ISectionProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_BISYM;
            sectionData.MaterialName = material.Name;

            RobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, section.FlangeThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void SteelSection(this TSectionProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_T;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE;

            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TF, section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TW, section.WebThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void SteelSection(this TubeProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_TUBE;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_TUBE;

            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, section.Diameter);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T, section.Thickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void SteelSection(this RectangleProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_RECT_FILLED;
            sectionData.CalcNonstdGeometry();

            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;

            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, section.Width);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void SteelSection(this CircleProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_TUBE;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_TUBE;

            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, section.Diameter);
            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void SteelSection(this AngleProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_L;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_UUAP;

            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, section.WebThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void SteelSection(this ChannelProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_C;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE;

            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_B, section.FlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TF, section.FlangeThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void SteelSection(this FreeFormProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.MaterialName = material.Name;
            SteelSection steelSection =  BH.Engine.Structure.Create.SteelSectionFromProfile(section, material);

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

        public static ISectionProperty BHoMSteelSection(IRobotBarSectionData secData)
        {
            IProfile sectionProfile;

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
                        sectionProfile = BH.Engine.Structure.Create.ISectionProfile(d, bf, Tw, Tf, ri, r);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_USER_RECT:
                        sectionProfile = BH.Engine.Structure.Create.BoxProfile(d, bf, Tf, 0, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_RECT_FILLED:
                        sectionProfile = BH.Engine.Structure.Create.RectangleProfile(d, bf, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE:
                        sectionProfile = BH.Engine.Structure.Create.TSectionProfile(d, bf, Tw, Tf, 0, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_USER_TUBE:
                        sectionProfile = BH.Engine.Structure.Create.TubeProfile(d, Tf);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

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
                        sectionProfile = BH.Engine.Structure.Create.TubeProfile(D, T);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_USER_RECT:
                        T = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T);
                        sectionProfile = BH.Engine.Structure.Create.BoxProfile(H, B, T, 0, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_USER_I_BISYM:
                        T = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T);
                        sectionProfile = BH.Engine.Structure.Create.ISectionProfile(H, B, TW, TF, 0, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_USER_BOX_3:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H);
                        TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW);
                        TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF);
                        double TF2 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2);
                        sectionProfile = BH.Engine.Structure.Create.FabricatedBoxProfile(H, B, TW, TF, TF2, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    default:
                        return null;
                }
            }
        }
    }
}
