/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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

using BH.oM.Common.Materials;
using RobotOM;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Section.ShapeProfiles;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void SectionProperty(this SteelSection section, Material material, IRobotBarSectionData secData)
        {
            ISteelSection(section.SectionProfile, material, secData);
        }

        /***************************************************/

        public static void ISteelSection(this IProfile section, Material material, IRobotBarSectionData secData)
        {
            SteelSection(section as dynamic, material, secData);
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

                switch (secData.ShapeType)
                {
                    case IRobotBarSectionShapeType.I_BSST_USER_I_BISYM:
                    case IRobotBarSectionShapeType.I_BSST_IPE:
                    case IRobotBarSectionShapeType.I_BSST_HEA:
                        sectionProfile = BH.Engine.Structure.Create.ISectionProfile(d, bf, Tw, Tf, r, ri);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_USER_RECT:
                    case IRobotBarSectionShapeType.I_BSST_TREC:
                    case IRobotBarSectionShapeType.I_BSST_TRON:
                    case IRobotBarSectionShapeType.I_BSST_TCAR:
                        if (r != 0)
                        {
                            sectionProfile = BH.Engine.Structure.Create.BoxProfile(d, bf, Tf, r + Tf, r);
                        }
                        else
                        {
                            sectionProfile = BH.Engine.Structure.Create.BoxProfile(d, bf, Tf, 0, 0);
                        }
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
                double B1 = 0;
                double B2 = 0;
                double H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H);
                double TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW);
                double TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF);
                double TF2 = 0;


                switch (secData.ShapeType)
                {
                    case IRobotBarSectionShapeType.I_BSST_USER_TUBE:
                        T = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T);
                        D = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D);
                        sectionProfile = BH.Engine.Structure.Create.TubeProfile(D, T);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_USER_RECT:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H);
                        sectionProfile = BH.Engine.Structure.Create.RectangleProfile(H, B, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_USER_I_BISYM:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H);
                        TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW);
                        TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF);
                        sectionProfile = BH.Engine.Structure.Create.ISectionProfile(H + (2 * TF), B, TW, TF, 0, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_USER_BOX_3:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B);
                        B1 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1);
                        B2 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H);
                        TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW);
                        TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF);
                        TF2 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2);

                        sectionProfile = BH.Engine.Structure.Create.FabricatedBoxProfile(H + TF + TF2, B1 + (2 * TW), TW, TF2, TF, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_BOX:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_B);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_H);
                        TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TW);
                        TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TF);

                        sectionProfile = BH.Engine.Structure.Create.FabricatedBoxProfile(H, B, TW, TF, TF, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);

                    case IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H);
                        TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW);
                        TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1);
                        sectionProfile = BH.Engine.Structure.Create.ISectionProfile(H + (2 * TF), B, TW, TF, 0, 0);
                        return BH.Engine.Structure.Create.SteelSectionFromProfile(sectionProfile);


                    default:
                        return null;
                }
            }
        }

        /***************************************************/

        /***************************************************/
        /****           Private Methods                  ****/
        /***************************************************/

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
            sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX_3;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;
            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);


            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1, section.Width - (2 * section.WebThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H, section.Height - (section.TopFlangeThickness + section.BotFlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW, section.WebThickness);
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

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, section.TopFlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, section.BotFlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, section.Height - (section.BotFlangeThickness + section.TopFlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, section.TopFlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, section.BotFlangeThickness);

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
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, section.Height - (2 * section.FlangeThickness));
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
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_H, section.Height - section.FlangeThickness);
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
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_RECT_FILLED;

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
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CIRC_FILLED;

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

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, section.Height - section.FlangeThickness);
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
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_H, section.Height - (2 * section.FlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TF, section.FlangeThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void SteelSection(this FreeFormProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.MaterialName = material.Name;
            SteelSection steelSection = BH.Engine.Structure.Create.SteelSectionFromProfile(section, material);

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

        private static void SteelSection(this GeneralisedFabricatedBoxProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX_3;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;

            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);


            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B, section.Width + section.BotLeftCorbelWidth + section.BotRightCorbelWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1, section.Width - (2 * section.WebThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H, section.Height - (section.TopFlangeThickness + section.BotFlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF, section.TopFlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2, section.BotFlangeThickness);

            sectionData.CalcNonstdGeometry();

        }

        /***************************************************/

        private static void SteelSection(this KiteProfile section, Material material, IRobotBarSectionData sectionData)
        {
            sectionData.MaterialName = material.Name;
            SteelSection steelSection = BH.Engine.Structure.Create.SteelSectionFromProfile(section, material);

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
    }
}