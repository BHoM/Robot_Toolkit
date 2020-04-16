/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BH.Engine.Structure;
using RobotOM;
using BH.oM.Structure.SectionProperties;
using BH.oM.Geometry.ShapeProfiles;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void ToRobot(this IGeometricalSection section, IMaterialFragment material, IRobotBarSectionData secData)
        {
            ToRobotGeometricalSection(section.SectionProfile as dynamic, material, secData);
        }

        /***************************************************/

        /***************************************************/
        /****           Private Methods                  ****/
        /***************************************************/

        private static void ToRobotGeometricalSection(this BoxProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_RECT;

            sectionData.MaterialName = material.DescriptionOrName();

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T, section.Thickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ToRobotGeometricalSection(this FabricatedBoxProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX_3;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;
            sectionData.MaterialName = material.DescriptionOrName();

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

        private static void ToRobotGeometricalSection(this FabricatedISectionProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_II;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
            sectionData.MaterialName = material.DescriptionOrName();

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

        private static void ToRobotGeometricalSection(this ISectionProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_BISYM;
            sectionData.MaterialName = material.DescriptionOrName();

            RobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, section.Height - (2 * section.FlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, section.FlangeThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ToRobotGeometricalSection(this TSectionProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_T;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE;

            sectionData.MaterialName = material.DescriptionOrName();

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_H, section.Height - section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TF, section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TW, section.WebThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ToRobotGeometricalSection(this TubeProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_TUBE;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_TUBE;

            sectionData.MaterialName = material.DescriptionOrName();

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, section.Diameter);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T, section.Thickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ToRobotGeometricalSection(this RectangleProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_RECT_FILLED;

            sectionData.MaterialName = material.DescriptionOrName();

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, section.Height);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, section.Width);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ToRobotGeometricalSection(this CircleProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_TUBE;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CIRC_FILLED;

            sectionData.MaterialName = material.DescriptionOrName();

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, section.Diameter);
            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ToRobotGeometricalSection(this AngleProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_L;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_UUAP;

            sectionData.MaterialName = material.DescriptionOrName();

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, section.Height - section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, section.Width);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, section.FlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, section.WebThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ToRobotGeometricalSection(this ChannelProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_C;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE;

            sectionData.MaterialName = material.DescriptionOrName();

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_B, section.FlangeWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_H, section.Height - (2 * section.FlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TF, section.FlangeThickness);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ToRobotGeometricalSection(this GeneralisedFabricatedBoxProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX_3;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;

            sectionData.MaterialName = material.DescriptionOrName();

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);


            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B, section.Width + section.BotLeftCorbelWidth + section.BotRightCorbelWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1, section.Width - (2 * section.WebThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2, section.Width + section.TopLeftCorbelWidth + section.TopLeftCorbelWidth);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H, section.Height - (section.TopFlangeThickness + section.BotFlangeThickness));
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW, section.WebThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF, section.TopFlangeThickness);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2, section.BotFlangeThickness);

            sectionData.CalcNonstdGeometry();

        }

        /***************************************************/

        private static void ToRobotGeometricalSection(this IProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            BH.Engine.Reflection.Compute.RecordWarning("Profile of type " + section.GetType().Name + "is not yet fully supported for Steel sections. Section with name " + sectionData.Name + " set as explicit section");

            sectionData.MaterialName = material.DescriptionOrName();
            IGeometricalSection steelSection = Create.SectionPropertyFromProfile(section, material as Steel);

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
