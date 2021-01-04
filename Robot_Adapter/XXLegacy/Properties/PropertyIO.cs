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

//using System;
//using BH.oM.Structure;
//using RobotOM;
//using BHoM.Materials;
//using BHoMP = BH.oM.Structure.Properties;
//using BHD = BH.oM.Structure.Databases;

//namespace Robot_Adapter
//{
//    /// <summary>
//    /// Section property class, the parent abstract class for all structural 
//    /// sections (RC, steel, PT beams, columns, bracing). Properties defined in this 
//    /// parent class are those that would populate a multi category section database only
//    /// </summary>

//    public class PropertyIO
//    {
//        /// <summary>
//        /// Gets the BHoM shape type from the robot section shape
//        /// </summary>
//        /// <param name="sType"></param>
//        /// <returns></returns>
//        public static BHoMP.ShapeType GetShapeType(IRobotBarSectionShapeType sType)
//        {
//            switch (sType)
//            {
//                case IRobotBarSectionShapeType.I_BSST_TRND:
//                case IRobotBarSectionShapeType.I_BSST_USER_BOX:
//                case IRobotBarSectionShapeType.I_BSST_USER_BOX_2:
//                case IRobotBarSectionShapeType.I_BSST_USER_BOX_3:
//                    return BHoMP.ShapeType.Box;
//                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_R:
//                case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT:
//                    return BHoMP.ShapeType.Rectangle;
//                case IRobotBarSectionShapeType.I_BSST_HEA:
//                case IRobotBarSectionShapeType.I_BSST_HEB:
//                case IRobotBarSectionShapeType.I_BSST_IPE:
//                case IRobotBarSectionShapeType.I_BSST_UUPN:
//                case IRobotBarSectionShapeType.I_BSST_DCIP:
//                case IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_HEXAGONAL_OPENINGS:
//                case IRobotBarSectionShapeType.I_BSST_USER_I_BISYM:
//                    return BHoMP.ShapeType.ISection;
//                case IRobotBarSectionShapeType.I_BSST_UUAP:
//                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_L:
//                    return BHoMP.ShapeType.Angle;
//                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_T:
//                case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T:
//                case IRobotBarSectionShapeType.I_BSST_MHEA:
//                case IRobotBarSectionShapeType.I_BSST_MHEB:
//                case IRobotBarSectionShapeType.I_BSST_MHEM:
//                case IRobotBarSectionShapeType.I_BSST_MIPE:
//                case IRobotBarSectionShapeType.I_BSST_DCED:
//                case IRobotBarSectionShapeType.I_BSST_IPN:
//                case IRobotBarSectionShapeType.I_BSST_DCIG:
//                case IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE:
//                    return BHoMP.ShapeType.Tee;
//                case IRobotBarSectionShapeType.I_BSST_TRON:
//                case IRobotBarSectionShapeType.I_BSST_USER_TUBE:
//                    return BHoMP.ShapeType.Tube;
//                case IRobotBarSectionShapeType.I_BSST_CAE:
//                case IRobotBarSectionShapeType.I_BSST_CAEP:
//                case IRobotBarSectionShapeType.I_BSST_CAI:
//                case IRobotBarSectionShapeType.I_BSST_CAIP:
//                case IRobotBarSectionShapeType.I_BSST_UPN:
//                case IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE:
//                    return BHoMP.ShapeType.Channel;
//                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_C:
//                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_CQ:
//                    return BHoMP.ShapeType.Circle;
//            }
//            //MessageBox.Show(bType.ToString());
//            return BHoMP.ShapeType.Rectangle;
//        }



//        internal static IRobotBarSectionType GetShapeType(BHoMP.ShapeType type)
//        {
//            switch (type)
//            {
//                case BHoMP.ShapeType.Angle:
//                    return IRobotBarSectionType.I_BST_NS_L;
//                case BHoMP.ShapeType.Box:
//                    return IRobotBarSectionType.I_BST_NS_BOX;
//                case BHoMP.ShapeType.Channel:
//                    return IRobotBarSectionType.I_BST_NS_C;
//                case BHoMP.ShapeType.Circle:
//                    return IRobotBarSectionType.I_BST_NS_C;
//                case BHoMP.ShapeType.DoubleAngle:
//                    return IRobotBarSectionType.I_BST_NS_LP;
//                case BHoMP.ShapeType.ISection:
//                    return IRobotBarSectionType.I_BST_NS_I;
//                case BHoMP.ShapeType.Rectangle:
//                    return IRobotBarSectionType.I_BST_NS_RECT;
//                case BHoMP.ShapeType.Tee:
//                    return IRobotBarSectionType.I_BST_NS_T;
//                case BHoMP.ShapeType.Tube:
//                case BHoMP.ShapeType.Cable:
//                    return IRobotBarSectionType.I_BST_NS_TUBE;
//                case BHoMP.ShapeType.Polygon:
//                    return IRobotBarSectionType.I_BST_NS_POLYGONAL;
//                default:
//                    return IRobotBarSectionType.I_BST_NS_RECT;

//            }
//        }

//        public static BHoMP.PanelType GetPanelType(IRobotObjectStructuralType robotSType)
//        {
//            switch (robotSType)
//            {
//                case IRobotObjectStructuralType.I_OST_SLAB:
//                    return BHoMP.PanelType.Slab;
//                case IRobotObjectStructuralType.I_OST_WALL:
//                    return BH.oM.Structure.Properties.PanelType.Wall;
//                default:
//                    return BH.oM.Structure.Properties.PanelType.Undefined;
//            }

//        }

//        internal static BHoMP.PanelProperty GetThickness(IRobotLabel rLabel, BHoMP.PanelType type)
//        {
//            IRobotThicknessData data = rLabel.Data;
//            BHoMP.PanelProperty thicknessProp = null;
//            switch (data.ThicknessType)
//            {
//                case IRobotThicknessType.I_TT_HOMOGENEOUS:
//                    IRobotThicknessHomoData homoData = data.Data;
//                    switch (homoData.Type)
//                    {
//                        case IRobotThicknessHomoType.I_THT_CONSTANT:
//                        case IRobotThicknessHomoType.I_THT_VARIABLE_ON_PLANE:
//                        case IRobotThicknessHomoType.I_THT_VARIABLE_ALONG_LINE:
//                            thicknessProp = new BHoMP.ConstantThickness(rLabel.Name);
//                            thicknessProp.Thickness = homoData.ThickConst;
//                            thicknessProp.Type = type;
//                            break;
//                    }
//                    break;
//                case IRobotThicknessType.I_TT_ORTHOTROPIC:
//                    IRobotThicknessOrthoData orthoData = data.Data;
//                    double[] modifiers = null;

//                    switch (orthoData.Type)
//                    {
//                        case IRobotThicknessOrthoType.I_TOT_ONE_SIDED_UNIDIR_RIBS:
//                            thicknessProp = new BHoMP.Ribbed(rLabel.Name, orthoData.H, orthoData.HA, orthoData.A1, orthoData.A, orthoData.DirType == IRobotThicknessOrthoDirType.I_TODT_DIR_X ? BHoMP.PanelDirection.X : BHoMP.PanelDirection.Y);
//                            break;
//                        case IRobotThicknessOrthoType.I_TOT_ONE_SIDED_BIDIR_RIBS:
//                            thicknessProp = new BHoMP.Waffle(rLabel.Name, orthoData.H, orthoData.HA, orthoData.HB, orthoData.A1, orthoData.B1, orthoData.A, orthoData.B);
//                            break;
//                        case IRobotThicknessOrthoType.I_TOT_MATERIAL:
//                            thicknessProp = new BHoMP.ConstantThickness(rLabel.Name, orthoData.H, type);
//                            double n1 = orthoData.N1;
//                            double n2 = orthoData.N2;
//                            modifiers = thicknessProp.Modifiers;
//                            modifiers[(int)BHoMP.PanelModifier.f11] = n1;
//                            modifiers[(int)BHoMP.PanelModifier.f12] = Math.Sqrt(n1 + n2);
//                            modifiers[(int)BHoMP.PanelModifier.f22] = n2;
//                            modifiers[(int)BHoMP.PanelModifier.m11] = n1;
//                            modifiers[(int)BHoMP.PanelModifier.m12] = Math.Sqrt(n1 + n2);
//                            modifiers[(int)BHoMP.PanelModifier.m22] = n2;
//                            modifiers[(int)BHoMP.PanelModifier.v13] = n1;
//                            modifiers[(int)BHoMP.PanelModifier.v23] = n2;
//                            break;
//                        case IRobotThicknessOrthoType.I_TOT_UNIDIR_BOX_FLOOR:
//                        case IRobotThicknessOrthoType.I_TOT_SLAB_ON_TRAPEZOID_PLATE:
//                        case IRobotThicknessOrthoType.I_TOT_GRILLAGE:
//                        case IRobotThicknessOrthoType.I_TOT_BIDIR_BOX_FLOOR:
//                        case IRobotThicknessOrthoType.I_TOT_HOLLOW_CORE_SLAB:
//                        case IRobotThicknessOrthoType.I_TOT_USER:
//                            break;
//                        default:
//                            thicknessProp = new BHoMP.ConstantThickness(rLabel.Name, orthoData.H, type);
//                            modifiers = thicknessProp.Modifiers;
//                            modifiers[(int)BHoMP.PanelModifier.f11] = orthoData.HA;
//                            modifiers[(int)BHoMP.PanelModifier.f12] = orthoData.H0;
//                            modifiers[(int)BHoMP.PanelModifier.f22] = orthoData.HB;
//                            modifiers[(int)BHoMP.PanelModifier.m11] = orthoData.HC;
//                            modifiers[(int)BHoMP.PanelModifier.m12] = orthoData.A;
//                            modifiers[(int)BHoMP.PanelModifier.m22] = orthoData.A1;
//                            modifiers[(int)BHoMP.PanelModifier.v13] = orthoData.A2;
//                            modifiers[(int)BHoMP.PanelModifier.v23] = orthoData.B;
//                            modifiers[(int)BHoMP.PanelModifier.Weight] = orthoData.B1;
//                            break;
//                    }
//                    break;
//            }
//            return thicknessProp;
//        }

//        internal static void CreateThicknessProperty(RobotApplication robot, BHoMP.PanelProperty thicknessProperty)
//        {
//            RobotLabelServer labelServer = robot.Project.Structure.Labels;
//            if (!labelServer.IsUsed(IRobotLabelType.I_LT_PANEL_THICKNESS, thicknessProperty.Name))
//            {
//                if (thicknessProperty.Material != null)
//                {
//                    PropertyIO.CreateMaterial(robot, thicknessProperty.Material);
//                }
//                RobotLabel sectionLabel = labelServer.Create(IRobotLabelType.I_LT_PANEL_THICKNESS, thicknessProperty.Name) as RobotLabel;
//                RobotThicknessData data = sectionLabel.Data;
//                data.MaterialName = thicknessProperty.Material != null ? thicknessProperty.Material.Name : "";
//                RobotThicknessOrthoData orthoData = null;
//                if (thicknessProperty is BHoMP.ConstantThickness)
//                {
//                    if (thicknessProperty.HasModifiers())
//                    {
//                        data.ThicknessType = IRobotThicknessType.I_TT_ORTHOTROPIC;
//                        orthoData = data.Data;
//                        orthoData.Type = (IRobotThicknessOrthoType)14;
//                        orthoData.H = thicknessProperty.Thickness;
//                        double[] modifiers = thicknessProperty.Modifiers;
//                        orthoData.HA = modifiers[(int)BHoMP.PanelModifier.f11];
//                        orthoData.H0 = modifiers[(int)BHoMP.PanelModifier.f12];
//                        orthoData.HB = modifiers[(int)BHoMP.PanelModifier.f22];
//                        orthoData.HC = modifiers[(int)BHoMP.PanelModifier.m11];
//                        orthoData.A = modifiers[(int)BHoMP.PanelModifier.m12];
//                        orthoData.A1 = modifiers[(int)BHoMP.PanelModifier.m22];
//                        orthoData.A2 = modifiers[(int)BHoMP.PanelModifier.v13];
//                        orthoData.B = modifiers[(int)BHoMP.PanelModifier.v23];
//                        orthoData.B1 = modifiers[(int)BHoMP.PanelModifier.Weight];
//                    }
//                    else
//                    {
//                        data.ThicknessType = IRobotThicknessType.I_TT_HOMOGENEOUS;
//                        (data.Data as RobotThicknessHomoData).ThickConst = thicknessProperty.Thickness;
//                    }
//                }
//                else if (thicknessProperty is BHoMP.Waffle)
//                {
//                    BHoMP.Waffle waffle = thicknessProperty as BHoMP.Waffle;
//                    data.ThicknessType = IRobotThicknessType.I_TT_ORTHOTROPIC;
//                    orthoData = data.Data;
//                    orthoData.Type = (IRobotThicknessOrthoType)14;
//                    orthoData.H = thicknessProperty.Thickness;

//                    orthoData.HA = waffle.TotalDepthX;
//                    orthoData.HB = waffle.TotalDepthY;
//                    orthoData.A = waffle.SpacingX;
//                    orthoData.A1 = waffle.StemWidthX;
//                    orthoData.B = waffle.SpacingY;
//                    orthoData.B1 = waffle.StemWidthY;
//                }
//                else
//                {
//                    BHoMP.Ribbed ribbed = thicknessProperty as BHoMP.Ribbed;
//                    data.ThicknessType = IRobotThicknessType.I_TT_ORTHOTROPIC;
//                    orthoData = data.Data;
//                    orthoData.Type = (IRobotThicknessOrthoType)14;
//                    orthoData.H = thicknessProperty.Thickness;

//                    orthoData.HA = ribbed.TotalDepth;
//                    orthoData.A = ribbed.Spacing;
//                    orthoData.A1 = ribbed.StemWidth;
//                }
//                labelServer.Store(sectionLabel);
//            }

//        }

//        /// <summary>Set the BHoM section shape type</summary>
//        public static BHoMP.SectionProperty GetSection(IRobotLabel sec_label)
//        {
//            IRobotBarSectionData sec_data = sec_label.Data;
//            BHoMP.SectionProperty property = null;
//            if (sec_data.IsConcrete)
//            {
//                double b = 0;
//                double h = 0;
//                double T1 = 0;
//                double T2 = 0;
//                double T3 = 0;
//                double b1 = 0;
//                double b2 = 0;
//                double b3 = 0;
//                RobotBarSectionConcreteData concMember = sec_data.Concrete;
//                switch (sec_data.ShapeType)
//                {
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT:
//                        h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_H);
//                        b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_B);
//                        property = BHoMP.SectionProperty.CreateRectangularSection(MaterialType.Concrete, h, b);
//                        break;
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I:
//                        b1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1);
//                        b2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2);
//                        T2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1);
//                        T3 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2);
//                        h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H);
//                        b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B);
//                        property = BHoMP.SectionProperty.CreateISection(MaterialType.Concrete, b1, b2, h, T2, T3, b, 0, 0);
//                        break;
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T:
//                        h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_H);
//                        b1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_B);
//                        T2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_HF);
//                        b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_BF);
//                        property = BHoMP.SectionProperty.CreateTeeSection(MaterialType.Concrete, h, b, T2, b1);
//                        break;
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM:
//                        return null;
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_COL_C:
//                        b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_DE);
//                        h = b;
//                        property = BHoMP.SectionProperty.CreateCircularSection(MaterialType.Concrete, b);
//                        break;
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_COL_R:
//                        b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
//                        h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
//                        property = BHoMP.SectionProperty.CreateRectangularSection(MaterialType.Concrete, h, b);
//                        break;
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_COL_CQ:
//                        //quarter Cirlce de dimeter
//                        return null;
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_COL_CH:
//                        //Half circle de dimeter total height
//                        return null;
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_COL_L:
//                        b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
//                        h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
//                        T1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1);
//                        T2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1);
//                        property = BHoMP.SectionProperty.CreateAngleSection(MaterialType.Concrete, h, b, h - T2, b - T1, 0, 0);
//                        break;
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_COL_P:
//                        //equal sided polygon de total height n angle
//                        break;
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_COL_T:
//                        b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
//                        h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
//                        T1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1);
//                        T2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1);
//                        property = BHoMP.SectionProperty.CreateTeeSection(MaterialType.Concrete, h, b, h - T2, b - 2 * T1);
//                        property.Orientation = 180;
//                        break;
//                    case IRobotBarSectionShapeType.I_BSST_CONCR_COL_Z:
//                        return null;
//                }
//            }
//            else
//            {
//                double d = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
//                double b = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
//                double Tf = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
//                double Tw = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
//                double r = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_RA);
//                double ri = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_RI);
//                double s = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_S);
//                double mass = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WEIGHT);
//                property = new BHoMP.SteelSection(GetShapeType(sec_data.ShapeType), d, b, Tw, Tf, r, ri, mass, s);
//            }

//            property.Orientation += sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_GAMMA);
//            return property;
//            #region BarTypes
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_R;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_T;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_L;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_Z;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_P;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_C;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_CH;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_CQ;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UNKNOWN;

//            //        ///<summary>Equal Angle (xy axis, parallel to legs)</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAE;

//            //        ///<summary>Equal Angle (uv main axis)</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAEP;

//            //        ///<summary>Unequal Angles (xy axis, parallel to legs)</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAI;

//            //        ///<summary>Unequal Angle (uv main axis) </summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAIP;

//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCEC;

//            //        ///<summary>Compound Equal Angles Legs Back to Back</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCED;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCEP;

//            //        ///<summary>Compound Unequal Angles Long Legs Back to Back</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCIG;

//            //        ///<summary>Compound Unequal Angles Short Legs Back to Back</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCIP;

//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEAA;

//            //        ///<summary>Universal bearing pile</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEB;

//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEC;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEM;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HER;


//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HHEA;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HHEB;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HHEM;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IIPE;



//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPEA;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPEO;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPER;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPEV;

//            //        ///<summary>Rolled Steel Joists</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPN;

//            //        ///<summary>Structural Tees cut from Universal Columns</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MHEA;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MHEB;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MHEM;

//            //        ///<summary>Structural Tees cut from Universal Beams</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MIPE;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_PRS;

//            //        ///<summary>Square hollow section</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TCAR;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TEAE;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TEAI;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_THEX;

//            //        ///<summary>Rectangular hollow section</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TREC;

//            //        ///<summary>Circular Hollow Section</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TRON;

//            //        ///<summary>Parallel flange channel</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UAP;

//            //        ///<summary>Rolled steel channel</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UPN;

//            //        ///<summary>Two Parallel Flange Channel Back to Back</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UUAP;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UUPN;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_FRTG;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WOOD_RECT;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UPAF;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_RECT_FILLED;

//            //        ///<summary>Solid circular section</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CIRC_FILLED;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CCL;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_URND;

//            //        ///<summary>Square and Rectangular Hollow Sections</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TRND;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CUAP;

//            //        ///<summary>Castellated beam</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_HEXAGONAL_OPENINGS;

//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_ROUND_OPENINGS;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_HEXAGONAL_OPENINGS_SHIFTED;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_SFB;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_IFBA;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_IFBB;


//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_BOX;

//            //        ///<summary>Rectangular section (solid or hollow)</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_RECT;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_RECT;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TUBE;

//            //        ///<summary>User defined circular hollow section</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_TUBE;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_ISYM;

//            //        ///<summary>I section with bi symmetry (flanges similar)</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_I_BISYM;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_INSYM;

//            //        ///<summary>I section with mono symmetry (flanges different)</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TUSER;

//            //        ///<summary>User defined tee shape</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE;

//            //        ///<summary>User defined channel</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CUSER;

//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TBETC;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DRECT;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WOOD_DRECT;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WOOD_CIRC;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_CIRC_FILLED;

//            //        ///<summary>User defined fabricated box with offset webs and bi symmetry (flanges similar)</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_BOX_2;

//            //        ///<summary>User defined polygonal</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_POLYGONAL;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CORRUGATED_WEB;

//            //        ///<summary>User defined fabricated box with offset webs and mono symmetry (flanges different)</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;

//            //        ///<summary>Cruciform section with flanges</summary>
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WELD_CROSS;

//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_CROSS;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_K;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_LH;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_KCS;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_DLH;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_SLH;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_G;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_VG;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_BG;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA1;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA2;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_ZED1;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_U;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_CE1;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_ANGL;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_OMEGA;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SO1;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_RIVE1;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_C_PLUS;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA_SL;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_Z;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_L_LIPS;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_Z_ROT;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_FACE;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_BACK;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2I;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2LI;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_FACE;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_BACK;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_SHORT;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_LONG;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_CROSS;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_SHORT;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_LONG;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI_BACK;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_FACE_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_BACK_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2I_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2LI_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_FACE_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_BACK_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_SHORT_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_LONG_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_CROSS_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_SHORT_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_LONG_WELD;
//            //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI_BACK_WELD;
//            //    }

//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_AX);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_AY);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_AZ);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IX);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IY);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IZ);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VY);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VPY);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VZ);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VPZ);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_SURFACE);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WEIGHT);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_RA);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_RI);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_S);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ZY);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ZZ);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WX);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WY);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WZ);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_GAMMA);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IOMEGA);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P1_LENGTH);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P1_THICKNESS);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P2_LENGTH);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P2_THICKNESS);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P3_LENGTH);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P3_THICKNESS);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P4_LENGTH);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P4_THICKNESS);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF2);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF2);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_DIM1);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_DIM2);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_DIM3);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ANGLE1);
//            //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ANGLE2);

//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B1);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TF);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TW);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TF);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TW);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P1_L);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P1_T);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P2_L);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P2_T);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P3_L);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P3_T);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P4_L);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P4_T);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TF);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TW);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_DRECT_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_DRECT_D);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_DRECT_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_X);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_Z);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_B1);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_TF);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_TW);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_TF);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_TW);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_D);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_D_IS_INT);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_N);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_T);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TF);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TW);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_H1);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_TF);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_TW);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_H1);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_TF);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_TW);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_B);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_H);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_TF);
//            //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_TW);

//            //}
//            #endregion
//        }

//        /// 
//        /// <summary>
//        /// Creates a robot bar property from a BHoM section property
//        /// </summary>
//        /// <param name="robot"></param>
//        /// <param name="sectionProperty"></param>
//        public static void CreateBarProperty(RobotApplication robot, BHoMP.SectionProperty sectionProperty, Material material, bool isColumn)
//        {
//            RobotLabelServer labelServer = robot.Project.Structure.Labels;
//            MaterialType matType = MaterialType.Steel;
//            if (material == null)
//            {
//                if (sectionProperty.Shape == BHoMP.ShapeType.Rectangle || sectionProperty.Shape == BHoMP.ShapeType.Circle)
//                {
//                    matType = MaterialType.Concrete;
//                }
//            }
//            else
//            {
//                matType = material.Type;
//            }

//            if (!labelServer.IsUsed(IRobotLabelType.I_LT_BAR_SECTION, sectionProperty.Name))
//            {
//                RobotLabel sectionLabel = labelServer.Create(IRobotLabelType.I_LT_BAR_SECTION, sectionProperty.Name) as RobotLabel;
//                RobotLabel cableLabel = labelServer.Create(IRobotLabelType.I_LT_BAR_CABLE, sectionProperty.Name) as RobotLabel;
//                RobotBarSectionData data = sectionLabel.Data;


//                RobotBarCableData cdata = cableLabel.Data;
//                double[] sectionData = sectionProperty.SectionData;

//                if (data.LoadFromDBase(sectionProperty.Name) == 1)
//                {
//                    labelServer.Store(sectionLabel);
//                }
//                else if (sectionProperty is BHoMP.ExplicitSectionProperty)
//                {
//                    BHoMP.ExplicitSectionProperty explicitSection = sectionProperty as BHoMP.ExplicitSectionProperty;
//                    if (material != null) data.MaterialName = material.Name;
//                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_AX, explicitSection.GrossArea);
//                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_IX, explicitSection.J);
//                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_IY, explicitSection.Iy);
//                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_IZ, explicitSection.Iz);
//                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_VY, explicitSection.Vy);
//                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_VZ, explicitSection.Vz);
//                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_VPY, explicitSection.Vpy);
//                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_VPZ, explicitSection.Vpz);

//                    labelServer.Store(sectionLabel);
//                }
//                else if (matType == MaterialType.Steel)
//                {
//                    if (sectionData != null)
//                    {
//                        data.Type = GetShapeType(sectionProperty.Shape);
//                        RobotBarSectionNonstdData nonStandard = data.CreateNonstd(0);
//                        switch (data.Type)
//                        {
//                            case IRobotBarSectionType.I_BST_NS_I:
//                            case IRobotBarSectionType.I_BST_NS_BOX:
//                            case IRobotBarSectionType.I_BST_NS_L:
//                            case IRobotBarSectionType.I_BST_NS_C:
//                            case IRobotBarSectionType.I_BST_NS_Z:
//                                nonStandard.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, sectionData[(int)BHD.SteelSectionData.Width]);
//                                nonStandard.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, sectionData[(int)BHD.SteelSectionData.Height] - sectionData[(int)BHD.SteelSectionData.TF1] - sectionData[(int)BHD.SteelSectionData.TF1]);
//                                nonStandard.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, sectionData[(int)BHD.SteelSectionData.TW]);
//                                nonStandard.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, sectionData[(int)BHD.SteelSectionData.TF1]);
//                                break;
//                            case IRobotBarSectionType.I_BST_NS_TUBE:
//                                nonStandard.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, sectionData[(int)BHD.SteelSectionData.Height]);
//                                nonStandard.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T, sectionData[(int)BHD.SteelSectionData.TF1]);
//                                break;
//                            case IRobotBarSectionType.I_BST_NS_RECT:
//                                nonStandard.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, sectionData[(int)BHD.SteelSectionData.Width]);
//                                nonStandard.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, sectionData[(int)BHD.SteelSectionData.Height]);
//                                //nonStandard.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T, sectionData[(int)SectionTableColumn.TF1 - 3]);
//                                break;
//                        }

//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_BF, sectionData[(int)SectionTableColumn.B1 - 3]);
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_BF2, sectionData[(int)SectionTableColumn.B2 - 3]);
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_TF, sectionData[(int)SectionTableColumn.TF1 - 3]);
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_TW, sectionData[(int)SectionTableColumn.TW - 3]);
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_TF2, sectionData[(int)SectionTableColumn.TF2 - 3]);
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_RA, sectionData[(int)SectionTableColumn.r1 - 3]);
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_RI, sectionData[(int)SectionTableColumn.r2 - 3]);
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_S, sectionData[(int)SectionTableColumn.Spacing - 3]);
//                        data.SetValue(IRobotBarSectionDataValue.I_BSDV_GAMMA, sectionProperty.Orientation);
//                        data.CalcNonstdGeometry();
//                    }

//                    labelServer.Store(sectionLabel);
//                }
//                else if (matType == MaterialType.Concrete && !isColumn)
//                {
//                    RobotBarSectionConcreteData concreteData;//= data.Concrete;
//                    switch (sectionProperty.Shape)
//                    {
//                        case BHoMP.ShapeType.Rectangle:
//                            data.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT;
//                            concreteData = data.Concrete;
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_H, sectionProperty.TotalDepth);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_B, sectionProperty.TotalWidth);
//                            concreteData.CalcGeometry();
//                            break;
//                        case BHoMP.ShapeType.ISection:
//                            data.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I;
//                            concreteData = data.Concrete;
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1, sectionData[(int)BHD.SteelSectionData.B1]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2, sectionData[(int)BHD.SteelSectionData.B2]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1, sectionData[(int)BHD.SteelSectionData.TF1]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2, sectionData[(int)BHD.SteelSectionData.TF2]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H, sectionData[(int)BHD.SteelSectionData.Height]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B, sectionData[(int)BHD.SteelSectionData.TW]);
//                            concreteData.CalcGeometry();
//                            break;
//                        case BHoMP.ShapeType.Tee:
//                            data.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T;
//                            concreteData = data.Concrete;
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H, sectionData[(int)BHD.SteelSectionData.Height]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B, sectionData[(int)BHD.SteelSectionData.TW]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_HF, sectionData[(int)BHD.SteelSectionData.TF1]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_BF, sectionData[(int)BHD.SteelSectionData.Width]);
//                            concreteData.CalcGeometry();
//                            break;
//                    }

//                    labelServer.Store(sectionLabel);
//                }
//                else if (matType == MaterialType.Concrete && isColumn)
//                {
//                    RobotBarSectionConcreteData concreteData;
//                    switch (sectionProperty.Shape)
//                    {
//                        case BHoMP.ShapeType.Circle:
//                            data.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_COL_C;
//                            concreteData = data.Concrete;
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_DE, sectionProperty.TotalDepth);
//                            concreteData.CalcGeometry();
//                            break;
//                        case BHoMP.ShapeType.Rectangle:
//                            data.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_COL_R;
//                            concreteData = data.Concrete;
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B, sectionProperty.TotalWidth);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H, sectionProperty.TotalDepth);
//                            concreteData.CalcGeometry();
//                            break;

//                        case BHoMP.ShapeType.Angle:
//                            data.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_COL_L;
//                            concreteData = data.Concrete;
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B, sectionData[(int)BHD.SteelSectionData.Width]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H, sectionData[(int)BHD.SteelSectionData.Height]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1, sectionData[(int)BHD.SteelSectionData.Width] - sectionData[(int)BHD.SteelSectionData.TW]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1, sectionData[(int)BHD.SteelSectionData.Height] - sectionData[(int)BHD.SteelSectionData.TF1]);
//                            concreteData.CalcGeometry();
//                            break;
//                        case BHoMP.ShapeType.Tee:
//                            data.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_COL_T;
//                            concreteData = data.Concrete;
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B, sectionData[(int)BHD.SteelSectionData.Width]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H, sectionData[(int)BHD.SteelSectionData.Height]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1, (sectionData[(int)BHD.SteelSectionData.Width] - sectionData[(int)BHD.SteelSectionData.TW]) / 2);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1, sectionData[(int)BHD.SteelSectionData.Height] - sectionData[(int)BHD.SteelSectionData.TF1]);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L2, (sectionData[(int)BHD.SteelSectionData.Width] - sectionData[(int)BHD.SteelSectionData.TW]) / 2);
//                            concreteData.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H2, sectionData[(int)BHD.SteelSectionData.Height] - sectionData[(int)BHD.SteelSectionData.TF1]);
//                            concreteData.CalcGeometry();
//                            break;
//                        case BHoMP.ShapeType.Zed:
//                            break;
//                    }
//                    labelServer.Store(sectionLabel);
//                }
//                else if (matType == MaterialType.Timber)
//                {
//                    data.ShapeType = IRobotBarSectionShapeType.I_BSST_WOOD_RECT;
//                    data.Type = IRobotBarSectionType.I_BST_NS_RECT;
//                    RobotBarSectionNonstdData nonStandard = data.CreateNonstd(0);
//                    nonStandard.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, sectionProperty.TotalWidth);
//                    nonStandard.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, sectionProperty.TotalDepth);
//                    data.CalcNonstdGeometry();
//                    labelServer.Store(sectionLabel);
//                }
//                else if (matType == MaterialType.Cable)
//                {
//                    if (sectionProperty.Shape == BHoMP.ShapeType.Cable)
//                    {
//                        cdata.MaterialName = matType.ToString();
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_AX, sectionData[(int)BHD.CableSectionData.A]);
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_VPY, 1);
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_VPZ, 1);
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_VY, 1);
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_VZ, 1);
//                        //data.SetValue(IRobotBarSectionDataValue.)
//                        //data.SetValue(IRobotBarSectionDataValue.I_BSDV_AX, 21);
//                        //IRobotBarCableData cabledata;
//                        cdata.SectionAX = sectionData[(int)BHD.CableSectionData.A];
//                        //cabledata.SectionAX
//                        //cabledata.AssemblingParamValue(IRobotBarCableAssemblingParamType.I_BCAPT_FORCE_FO
//                    }
//                    labelServer.Store(cableLabel);
//                }
//                else
//                {
//                    labelServer.Store(sectionLabel);
//                }
//            }
//        }

//        /// <summary>
//        /// Converts the BHoM material type to a Robot material type
//        /// </summary>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        public static IRobotMaterialType GetMaterialType(MaterialType type)
//        {
//            switch (type)
//            {
//                case MaterialType.Aluminium:
//                    return IRobotMaterialType.I_MT_ALUMINIUM;
//                case MaterialType.Concrete:
//                    return IRobotMaterialType.I_MT_CONCRETE;
//                case MaterialType.Steel:
//                    return IRobotMaterialType.I_MT_STEEL;
//                case MaterialType.Timber:
//                    return IRobotMaterialType.I_MT_TIMBER;
//                default:
//                    return IRobotMaterialType.I_MT_ALL;
//            }
//        }

//        /// <summary>
//        /// Converts the Robot material type to a BHoM material type
//        /// </summary>
//        /// <param name="mType"></param>
//        /// <returns></returns>
//        public static MaterialType GetMaterialType(IRobotMaterialType mType)
//        {
//            switch (mType)
//            {
//                case IRobotMaterialType.I_MT_CONCRETE:
//                    return MaterialType.Concrete;
//                case IRobotMaterialType.I_MT_STEEL:
//                    return MaterialType.Steel;
//                case IRobotMaterialType.I_MT_TIMBER:
//                    return MaterialType.Timber;
//                case IRobotMaterialType.I_MT_ALUMINIUM:
//                    return MaterialType.Aluminium;
//                default:
//                    return MaterialType.Steel;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="material"></param>
//        /// <returns></returns>
//        public static Material GetMaterial(IRobotLabel material)
//        {
//            IRobotMaterialData data = null;
//            data = material.Data as IRobotMaterialData;

//            double E = data.E;
//            double G = data.Kirchoff;
//            double tempCoeff = data.LX;
//            double v = data.NU;
//            double density = data.RO;
//            IRobotMaterialType type = data.Type;

//            return new Material(material.Name, GetMaterialType(type), E, v, tempCoeff, G, density);
//        }

//        /// <summary>
//        /// Creates a Robot material
//        /// </summary>
//        /// <param name="robot"></param>
//        /// <param name="m"></param>
//        public static void CreateMaterial(RobotApplication robot, Material m)
//        {
//            if (m != null && !robot.Project.Structure.Labels.IsUsed(IRobotLabelType.I_LT_MATERIAL, m.Name))
//            {
//                IRobotLabel material = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_MATERIAL, m.Name);
//                IRobotMaterialData materialData = material.Data;
//                IRobotMaterialData data = null;
//                data = material.Data as IRobotMaterialData;

//                double E = data.E;
//                double G = data.Kirchoff;
//                double tempCoeff = data.LX;
//                double v = data.NU;
//                double density = data.RO;
//                IRobotMaterialType type = data.Type;
//                materialData.E = m.YoungsModulus;
//                materialData.Kirchoff = m.ShearModulus;
//                materialData.LX = m.CoeffThermalExpansion;
//                materialData.RO = m.Weight * 1000;
//                materialData.NU = m.PoissonsRatio;
//                materialData.Type = GetMaterialType(m.Type);
//                robot.Project.Structure.Labels.Store(material);
//            }
//        }
//    }
//}

