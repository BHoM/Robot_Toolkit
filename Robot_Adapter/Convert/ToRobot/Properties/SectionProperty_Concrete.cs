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

        public static void ToRobot(this ConcreteSection section, IMaterialFragment material, IRobotBarSectionData secData)
        {
            ToRobotConcreteSection(section.SectionProfile as dynamic, material, secData);
        }

        /***************************************************/

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private static void ToRobotConcreteSection(this RectangleProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
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

        private static void ToRobotConcreteSection(this TSectionProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
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

        private static void ToRobotConcreteSection(this FabricatedISectionProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
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

        private static void ToRobotConcreteSection(this CircleProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_C;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_COL_C;

            sectionData.MaterialName = material.Name;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_DE, section.Diameter);

            sectionData.CalcNonstdGeometry();
        }

        /***************************************************/

        private static void ToRobotConcreteSection(this ISectionProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
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

        private static void ToRobotConcreteSection(this IProfile section, IMaterialFragment material, IRobotBarSectionData sectionData)
        {
            BH.Engine.Reflection.Compute.RecordWarning("Profile of type " + section.GetType().Name + "is not yet fully supported for Concrete sections. Section with name " + sectionData.Name + " set as explicit section");

            sectionData.MaterialName = material.Name;
            ConcreteSection steelSection = BH.Engine.Structure.Create.ConcreteSectionFromProfile(section, material as Concrete);

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

