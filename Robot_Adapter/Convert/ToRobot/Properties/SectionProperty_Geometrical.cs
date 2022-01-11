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

using System;
using RobotOM;
using BH.oM.Structure.SectionProperties;
using BH.oM.Spatial.ShapeProfiles;

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

        private static bool ToRobotGeometricalSection(this BoxProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_RECT;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T, section.Thickness);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotGeometricalSection(this FabricatedBoxProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX_3;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);


            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1, section.Width - (2 * section.WebThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H, section.Height - (section.TopFlangeThickness + section.BotFlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF, section.TopFlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2, section.BotFlangeThickness);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotGeometricalSection(this FabricatedISectionProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_II;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;

            RobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, section.TopFlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, section.BotFlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, section.Height - (section.BotFlangeThickness + section.TopFlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, section.TopFlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, section.BotFlangeThickness);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotGeometricalSection(this ISectionProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_BISYM;

            RobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, section.Height - (2 * section.FlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, section.FlangeThickness);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotGeometricalSection(this TSectionProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_T;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_H, section.Height - section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TF, section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TW, section.WebThickness);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotGeometricalSection(this TubeProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_TUBE;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_TUBE;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, section.Diameter);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T, section.Thickness);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotGeometricalSection(this RectangleProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_RECT_FILLED;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, section.Width);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotGeometricalSection(this CircleProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_TUBE;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CIRC_FILLED;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, section.Diameter);
            sectionData.CalcNonstdGeometry();
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

        private static bool ToRobotGeometricalSection(this ChannelProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_C;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_B, section.FlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_H, section.Height - (2 * section.FlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TF, section.FlangeThickness);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotGeometricalSection(this GeneralisedFabricatedBoxProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX_3;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);


            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B, section.Width + section.BotLeftCorbelWidth + section.BotRightCorbelWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1, section.Width - (2 * section.WebThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2, section.Width + section.TopLeftCorbelWidth + section.TopLeftCorbelWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H, section.Height - (section.TopFlangeThickness + section.BotFlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF, section.TopFlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2, section.BotFlangeThickness);

            sectionData.CalcNonstdGeometry();
            return true;

        }

        /***************************************************/

        private static bool ToRobotGeometricalSection(this IProfile section, IRobotBarSectionData sectionData)
        {
            BH.Engine.Base.Compute.RecordWarning("Profile of type " + section.GetType().Name + " is not yet fully supported for Steel sections. Section with name " + sectionData.Name + " set as explicit section");
            return false;
        }

        /***************************************************/
    }
}


