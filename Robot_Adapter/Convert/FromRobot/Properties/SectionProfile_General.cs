/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
                BH.Engine.Base.Compute.RecordWarning("Complex sections can not currently be read from Robot.");
                return null;
            }
            else
            {
                //Get profile at start. FOr non tapered section this will be the general definition
                IRobotBarSectionNonstdData nonStdData = secData.GetNonstd(1);
                if (nonStdData == null)
                    return null;

                IProfile startProfile = NonStandardProfile(secData.ShapeType, nonStdData);

                //Check if a profile exists at the next position. 
                nonStdData = secData.GetNonstd(2);
                if (nonStdData == null)
                    sectionProfile = startProfile;  //If no profile at second position, the section is not tapered, and start profile can be used
                else
                {
                    //If profile exists, extract end position section and return section profile as tapered section
                    IProfile endProfile = NonStandardProfile(secData.ShapeType, nonStdData);
                    sectionProfile = Engine.Spatial.Create.TaperedProfile(startProfile, endProfile, 1);
                }


            }

            return sectionProfile;

        }

        /***************************************************/

        private static IProfile NonStandardProfile(IRobotBarSectionShapeType shapeType, IRobotBarSectionNonstdData nonStdData)
        {
            double d, t, b, b1, b2, h, tw, tf, tf2;
            IProfile sectionProfile;
            switch (shapeType)
            {
                case IRobotBarSectionShapeType.I_BSST_USER_TUBE:
                    t = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T);
                    d = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D);
                    sectionProfile = BH.Engine.Spatial.Create.TubeProfile(d, t);
                    break;

                case IRobotBarSectionShapeType.I_BSST_RECT_FILLED:
                    b = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B);
                    h = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H);
                    sectionProfile = BH.Engine.Spatial.Create.RectangleProfile(h, b, 0);
                    break;

                case IRobotBarSectionShapeType.I_BSST_USER_RECT:
                    b = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B);
                    h = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H);
                    t = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T);

                    if (t == 0)
                        sectionProfile = BH.Engine.Spatial.Create.RectangleProfile(h, b, 0);
                    else
                        sectionProfile = BH.Engine.Spatial.Create.BoxProfile(h, b, t, 0, 0);
                    break;

                case IRobotBarSectionShapeType.I_BSST_USER_I_BISYM:
                    b = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B);
                    h = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H);
                    tw = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW);
                    tf = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF);
                    sectionProfile = BH.Engine.Spatial.Create.ISectionProfile(h + (2 * tf), b, tw, tf, 0, 0);
                    break;

                case IRobotBarSectionShapeType.I_BSST_USER_BOX_2:
                    b = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B);
                    b1 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B1);
                    h = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_H);
                    tw = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TW);
                    tf = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TF);
                    double outstand = (b - b1 - (2 * tw)) / 2;
                    //Check if subtraction leads to negative "0", if so, set to zero to avoid profile method braking
                    if (outstand < 0 && Math.Abs(outstand) < Tolerance.MicroDistance)
                        outstand = 0;

                    //If outstands are 0 (less than microdistance fraction of the width) return a standard fabricated box section.
                    if (b1 > Tolerance.Distance && outstand / b1 < Tolerance.MicroDistance)
                        sectionProfile = BH.Engine.Spatial.Create.FabricatedBoxProfile(h + (2 * tf), b1 + 2 * tw, tw, tf, tf, 0);
                    else
                        sectionProfile = BH.Engine.Spatial.Create.GeneralisedFabricatedBoxProfile(h + (2 * tf), b1 + (2 * tw), tw, tf, tf, outstand, outstand);
                    break;

                case IRobotBarSectionShapeType.I_BSST_USER_BOX_3:
                    b = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B);
                    b1 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1);
                    b2 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2);
                    h = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H);
                    tw = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW);
                    tf = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF);
                    tf2 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2);

                    double topOutstand = (b2 - b1 - (2 * tw)) / 2;
                    double botOutstand = (b - b1 - (2 * tw)) / 2;

                    //Check if subtraction leads to negative "0", if so, set to zero to avoid profile method braking
                    if (topOutstand < 0 && Math.Abs(topOutstand) < Tolerance.MicroDistance)
                        topOutstand = 0;

                    if (botOutstand < 0 && Math.Abs(botOutstand) < Tolerance.MicroDistance)
                        botOutstand = 0;

                    //If outstands are 0 (less than microdistance fraction of the width) return a standard fabricated box section.
                    if (b1 > Tolerance.Distance && topOutstand / b1 < Tolerance.MicroDistance && botOutstand / b1 < Tolerance.MicroDistance)
                    {
                        sectionProfile = BH.Engine.Spatial.Create.FabricatedBoxProfile(h + tf + tf2, b1 + 2 * tw, tw, tf, tf2, 0);
                    }
                    else
                    {
                        sectionProfile = BH.Engine.Spatial.Create.GeneralisedFabricatedBoxProfile(h + tf + tf2, b1 + (2 * tw), tw, tf, tf2, topOutstand, botOutstand);
                    }
                    break;

                case IRobotBarSectionShapeType.I_BSST_BOX:
                    b = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_B);
                    h = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_H);
                    tw = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TW);
                    tf = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TF);

                    sectionProfile = BH.Engine.Spatial.Create.FabricatedBoxProfile(h + (2 * tf), b, tw, tf, tf, 0);
                    break;

                case IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM:
                    b1 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1);
                    b2 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2);
                    h = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H);
                    tw = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW);
                    tf = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1);
                    tf2 = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2);
                    sectionProfile = BH.Engine.Spatial.Create.FabricatedISectionProfile(h + tf + tf2, b1, b2, tw, tf, tf2, 0);
                    break;

                case IRobotBarSectionShapeType.I_BSST_USER_CIRC_FILLED:
                case IRobotBarSectionShapeType.I_BSST_CIRC_FILLED:
                    d = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D);
                    sectionProfile = BH.Engine.Spatial.Create.CircleProfile(d);
                    break;

                case IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE:
                    b = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_B);
                    h = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_H);
                    tf = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TF);
                    tw = nonStdData.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TW);
                    sectionProfile = BH.Engine.Spatial.Create.TSectionProfile(h, b, tw, tf);
                    break;

                default:
                    return null;
            }
            return sectionProfile;
        }

        /***************************************************/
    }
}


