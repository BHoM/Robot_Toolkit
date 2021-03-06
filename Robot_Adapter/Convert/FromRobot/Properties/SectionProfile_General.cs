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

using BH.oM.Structure.MaterialFragments;
using BH.oM.Geometry;
using RobotOM;
using BH.oM.Structure.SectionProperties;
using BH.oM.Spatial.ShapeProfiles;
using System;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static IProfile FromRobotGeneralProfile(IRobotBarSectionData secData)
        {
            IProfile sectionProfile = null;

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
                    case IRobotBarSectionShapeType.I_BSST_HEAA:
                    case IRobotBarSectionShapeType.I_BSST_HEA:
                    case IRobotBarSectionShapeType.I_BSST_HEB:
                    case IRobotBarSectionShapeType.I_BSST_HEC:
                        sectionProfile = BH.Engine.Spatial.Create.ISectionProfile(d, bf, Tw, Tf, r, ri);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_USER_RECT:
                    case IRobotBarSectionShapeType.I_BSST_TREC:
                    case IRobotBarSectionShapeType.I_BSST_TCAR:
                    case IRobotBarSectionShapeType.I_BSST_TRND:
                        if (r != 0)
                        {
                            sectionProfile = BH.Engine.Spatial.Create.BoxProfile(d, bf, Tf, r + Tf, r);
                        }
                        else
                        {
                            sectionProfile = BH.Engine.Spatial.Create.BoxProfile(d, bf, Tf, 0, 0);
                        }
                        break;

                    case IRobotBarSectionShapeType.I_BSST_RECT_FILLED:
                        sectionProfile = BH.Engine.Spatial.Create.RectangleProfile(d, bf, 0);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE:
                        sectionProfile = BH.Engine.Spatial.Create.TSectionProfile(d, bf, Tw, Tf, 0, 0);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_USER_TUBE:
                    case IRobotBarSectionShapeType.I_BSST_TRON:
                        sectionProfile = BH.Engine.Spatial.Create.TubeProfile(d, Tf);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_UPN:
                        sectionProfile = BH.Engine.Spatial.Create.ChannelProfile(d, bf, Tw, Tf, r, ri);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_CAE:
                        sectionProfile = BH.Engine.Spatial.Create.AngleProfile(d, bf, Tw, Tf, r, ri);
                        break;
                    default:
                        return null;
                }
            }
            else if (secData.Type == IRobotBarSectionType.I_BST_COMPLEX)
            {
                BH.Engine.Reflection.Compute.RecordWarning("Complex sections can not currently be read from Robot.");
                return null;
            }
            else
            {
                IRobotBarSectionNonstdData nonStdData = secData.GetNonstd(1);
                if (nonStdData == null)
                    return null;

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
                        sectionProfile = BH.Engine.Spatial.Create.TubeProfile(D, T);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_RECT_FILLED:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H);
                        sectionProfile = BH.Engine.Spatial.Create.RectangleProfile(H, B, 0);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_USER_RECT:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H);
                        T = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T);

                        if(T == 0)
                            sectionProfile = BH.Engine.Spatial.Create.RectangleProfile(H, B, 0);
                        else
                            sectionProfile = BH.Engine.Spatial.Create.BoxProfile(H, B, T, 0, 0);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_USER_I_BISYM:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H);
                        TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW);
                        TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF);
                        sectionProfile = BH.Engine.Spatial.Create.ISectionProfile(H + (2 * TF), B, TW, TF, 0, 0);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_USER_BOX_2:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B);
                        B1 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B1);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_H);
                        TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TW);
                        TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TF);
                        double outstand = (B - B1 - (2 * TW)) / 2;
                        //Check if subtraction leads to negative "0", if so, set to zero to avoid profile method braking
                        if (outstand < 0 && Math.Abs(outstand) < Tolerance.MicroDistance)
                            outstand = 0;

                        sectionProfile = BH.Engine.Spatial.Create.GeneralisedFabricatedBoxProfile(H + (2 * TF), B1 + (2 * TW), TW, TF, TF, outstand, outstand);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_USER_BOX_3:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B);
                        B1 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1);
                        B2 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H);
                        TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW);
                        TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF);
                        TF2 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2);

                        double topOutstand = (B2 - B1 - (2 * TW)) / 2;
                        double botOutstand = (B - B1 - (2 * TW)) / 2;

                        //Check if subtraction leads to negative "0", if so, set to zero to avoid profile method braking
                        if (topOutstand < 0 && Math.Abs(topOutstand) < Tolerance.MicroDistance)
                            topOutstand = 0;

                        if (botOutstand < 0 && Math.Abs(botOutstand) < Tolerance.MicroDistance)
                            botOutstand = 0;

                        //If outstands are 0 (less than microdistance fraction of the width) return a standard fabricated box section.
                        if (B1 > Tolerance.Distance && topOutstand / B1 < Tolerance.MicroDistance && botOutstand / B1 < Tolerance.MicroDistance)
                        {
                            sectionProfile = BH.Engine.Spatial.Create.FabricatedBoxProfile(H + TF + TF2, B1 + 2 * TW, TW, TF, TF2, 0);
                        }
                        else
                        {
                            sectionProfile = BH.Engine.Spatial.Create.GeneralisedFabricatedBoxProfile(H + TF + TF2, B1 + (2 * TW), TW, TF, TF2, topOutstand, botOutstand);
                        }
                        break;

                    case IRobotBarSectionShapeType.I_BSST_BOX:
                        B = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_B);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_H);
                        TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TW);
                        TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TF);

                        sectionProfile = BH.Engine.Spatial.Create.FabricatedBoxProfile(H + (2 * TF), B, TW, TF, TF, 0);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM:
                        B1 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1);
                        B2 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2);
                        H = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H);
                        TW = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW);
                        TF = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1);
                        TF2 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2);
                        sectionProfile = BH.Engine.Spatial.Create.FabricatedISectionProfile(H + TF + TF2, B1, B2, TW, TF, TF2, 0);
                        break;

                    case IRobotBarSectionShapeType.I_BSST_USER_CIRC_FILLED:
                        sectionProfile = BH.Engine.Spatial.Create.CircleProfile(D);
                        break;

                    default:
                        return null;
                }
            }

            return sectionProfile;

        }

        /***************************************************/
    }
}

