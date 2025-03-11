/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using System;
using RobotOM;
using BH.oM.Structure.SectionProperties;
using BH.oM.Spatial.ShapeProfiles;
using System.Linq;
using BH.oM.Geometry;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void ToRobot(this IGeometricalSection section, IRobotBarSectionData secData)
        {
            Type type = section.GetType();

            if (!RobotAdapter.CheckNotNull(section.SectionProfile, oM.Base.Debugging.EventType.Warning, type) ||
                !ToRobotGeometricalSection(section.SectionProfile as dynamic, secData))
            {
                //If method returns false, no profile based data has been set. Fallback to explicitly setting properties.
                secData.SetValue(IRobotBarSectionDataValue.I_BSDV_AX, section.Area);
                secData.SetValue(IRobotBarSectionDataValue.I_BSDV_IX, section.J);
                secData.SetValue(IRobotBarSectionDataValue.I_BSDV_IY, section.Iy);
                secData.SetValue(IRobotBarSectionDataValue.I_BSDV_IZ, section.Iz);
                secData.SetValue(IRobotBarSectionDataValue.I_BSDV_VY, section.Vy);
                secData.SetValue(IRobotBarSectionDataValue.I_BSDV_VZ, section.Vz);
                secData.SetValue(IRobotBarSectionDataValue.I_BSDV_VPY, section.Vpy);
                secData.SetValue(IRobotBarSectionDataValue.I_BSDV_VPZ, section.Vpz);
                secData.SetValue(IRobotBarSectionDataValue.I_BSDV_ZY, section.Wply);
                secData.SetValue(IRobotBarSectionDataValue.I_BSDV_ZZ, section.Wplz);
                secData.CalcNonstdGeometry();
            }
        }

        /***************************************************/
        /****           Private Methods                  ****/
        /***************************************************/

        private static bool ToRobotGeometricalSection(this IProfile section, IRobotBarSectionData sectionData)
        {
            if (SetRobotTypeAndShapeType(section as dynamic, sectionData))
            {
                SetNonStandardSectionData(section as dynamic, sectionData, 0);
                sectionData.CalcNonstdGeometry();
                return true;
            }
            return false;
        }

        /***************************************************/

        private static bool ToRobotGeometricalSection(this TaperedProfile section, IRobotBarSectionData sectionData)
        {
            if (section.Profiles.Count == 1)
                return ToRobotGeometricalSection(section.Profiles.First().Value as dynamic, sectionData);

            IProfile startProfile, endProfile;

            if (section.Profiles.Count == 2 && section.Profiles.TryGetValue(0, out startProfile) && section.Profiles.TryGetValue(1, out endProfile))
            {
                if (startProfile.GetType() == endProfile.GetType())
                {
                    if (SetRobotTypeAndShapeType(startProfile as dynamic, sectionData))
                    {
                        bool success = SetNonStandardSectionData(startProfile as dynamic, sectionData, 0);
                        success &= SetNonStandardSectionData(endProfile as dynamic, sectionData, 1);
                        if(success)
                            sectionData.CalcNonstdGeometry();
                        return success;
                    }
                }
            }

            Engine.Base.Compute.RecordWarning($"The robot adapter currently only support tapered sections with two profiles of the same type. Section with name {sectionData.Name} set as explicit section with 0-properties.");
            return false;
        }

        /***************************************************/

        private static bool SetRobotTypeAndShapeType(this BoxProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_RECT;
            return true;
        }

        /***************************************************/

        private static bool SetRobotTypeAndShapeType(this FabricatedBoxProfile section, IRobotBarSectionData sectionData)
        {
            if (section.BotFlangeThickness > Tolerance.MicroDistance && (section.BotFlangeThickness - section.TopFlangeThickness) / (section.BotFlangeThickness + section.TopFlangeThickness) / 2 < 1e-6)   //If same flange thickness on both top and bot flange
            {
                sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX_2;
                sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX_2;
            }
            else
            {
                sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX_3;
                sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;
            }

            return true;
        }

        /***************************************************/

        private static bool SetRobotTypeAndShapeType(this FabricatedISectionProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_II;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
            return true;
        }

        /***************************************************/

        private static bool SetRobotTypeAndShapeType(this ISectionProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_BISYM;
            return true;
        }

        /***************************************************/

        private static bool SetRobotTypeAndShapeType(this TSectionProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_T;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE;
            return true;
        }

        /***************************************************/

        private static bool SetRobotTypeAndShapeType(this TubeProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_TUBE;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_TUBE;
            return true;
        }

        /***************************************************/

        private static bool SetRobotTypeAndShapeType(this RectangleProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_RECT_FILLED;
            return true;
        }

        /***************************************************/

        private static bool SetRobotTypeAndShapeType(this CircleProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_TUBE;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CIRC_FILLED;
            return true;
        }

        /***************************************************/

        //private static bool ToRobotGeometricalSection(this AngleProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        //{
        //    sectionData.Type = IRobotBarSectionType.I_BST_NS_L;
        //    sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_UUAP;
        //}

        /***************************************************/

        private static bool SetRobotTypeAndShapeType(this ChannelProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_C;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE;
            return true;
        }

        /***************************************************/

        private static bool SetRobotTypeAndShapeType(this GeneralisedFabricatedBoxProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX_3;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;
            return true;
        }

        /***************************************************/

        private static bool SetRobotTypeAndShapeType(this IProfile section, IRobotBarSectionData sectionData)
        {
            Engine.Base.Compute.RecordWarning("Profile of type " + section.GetType().Name + " is not yet fully supported for Steel sections. Section with name " + sectionData.Name + " set as explicit section.");
            return false;
        }

        /***************************************************/

        private static bool SetNonStandardSectionData(this BoxProfile section, IRobotBarSectionData sectionData, int position = 0)
        {
            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(position);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T, section.Thickness);
            return true;
        }

        /***************************************************/

        private static bool SetNonStandardSectionData(this FabricatedBoxProfile section, IRobotBarSectionData sectionData, int position = 0)
        {
            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(position);

            if (sectionData.ShapeType == IRobotBarSectionShapeType.I_BSST_USER_BOX_3) //Check assigned shapetype. Box3 for varying flange thickness, box 2 will be set if flange thickness the same
            {
                if (position != 0)  //Tapered sections only support box 2
                {
                    BH.Engine.Base.Compute.RecordWarning($"Robot only supports boxes with equal flanges for tapered sections. Section with name {sectionData.Name} set as explicit section with 0 properties.");
                    return false;
                }

                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B, section.Width);
                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1, section.Width - (2 * section.WebThickness));
                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2, section.Width);
                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H, section.Height - (section.TopFlangeThickness + section.BotFlangeThickness));
                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW, section.WebThickness);
                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF, section.TopFlangeThickness);
                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2, section.BotFlangeThickness);

            }
            else
            {
                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B, section.Width);
                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B1, section.Width - (2 * section.WebThickness));
                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_H, section.Height - (section.TopFlangeThickness + section.BotFlangeThickness));
                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TW, section.WebThickness);
                nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TF, section.TopFlangeThickness);
            }

            return true;
        }

        /***************************************************/

        private static bool SetNonStandardSectionData(this FabricatedISectionProfile section, IRobotBarSectionData sectionData, int position = 0)
        {
            RobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(position);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, section.TopFlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, section.BotFlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, section.Height - (section.BotFlangeThickness + section.TopFlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, section.TopFlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, section.BotFlangeThickness);
            return true;
        }

        /***************************************************/

        private static bool SetNonStandardSectionData(this ISectionProfile section, IRobotBarSectionData sectionData, int position = 0)
        {
            RobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(position);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, section.Height - (2 * section.FlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, section.FlangeThickness);
            return true;
        }

        /***************************************************/

        private static bool SetNonStandardSectionData(this TSectionProfile section, IRobotBarSectionData sectionData, int position = 0)
        {
            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(position);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_H, section.Height - section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TF, section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TW, section.WebThickness);
            return true;
        }

        /***************************************************/

        private static bool SetNonStandardSectionData(this TubeProfile section, IRobotBarSectionData sectionData, int position = 0)
        {
            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(position);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, section.Diameter);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T, section.Thickness);
            return true;
        }

        /***************************************************/

        private static bool SetNonStandardSectionData(this RectangleProfile section, IRobotBarSectionData sectionData, int position = 0)
        {
            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(position);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, section.Width);
            return true;
        }

        /***************************************************/

        private static bool SetNonStandardSectionData(this CircleProfile section, IRobotBarSectionData sectionData, int position = 0)
        {
            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(position);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, section.Diameter);
            return true;
        }

        /***************************************************/

        //private static bool ToRobotGeometricalSection(this AngleProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        //{
        //    sectionData.Type = IRobotBarSectionType.I_BST_NS_L;
        //    sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_UUAP;

        //    sectionData.MaterialName = material.DescriptionOrName();

        //    IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

        //    nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, section.Height - section.FlangeThickness);
        //    nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, section.Width);
        //    nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, section.FlangeThickness);
        //    nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, section.WebThickness);

        //    sectionData.CalcNonstdGeometry();
        //}

        /***************************************************/

        private static bool SetNonStandardSectionData(this ChannelProfile section, IRobotBarSectionData sectionData, int position = 0)
        {
            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(position);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_B, section.FlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_H, section.Height - (2 * section.FlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TF, section.FlangeThickness);

            return true;
        }

        /***************************************************/

        private static bool SetNonStandardSectionData(this GeneralisedFabricatedBoxProfile section, IRobotBarSectionData sectionData, int position = 0)
        {
            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(position);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B, section.Width + section.BotLeftCorbelWidth + section.BotRightCorbelWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1, section.Width - (2 * section.WebThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2, section.Width + section.TopLeftCorbelWidth + section.TopLeftCorbelWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H, section.Height - (section.TopFlangeThickness + section.BotFlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF, section.TopFlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2, section.BotFlangeThickness);
            return true;

        }

        /***************************************************/
    }
}





