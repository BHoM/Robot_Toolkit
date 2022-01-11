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
using BH.Engine.Structure;
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

        public static void ToRobot(this ConcreteSection section, IRobotBarSectionData secData)
        {
            
            if (!RobotAdapter.CheckNotNull(section.SectionProfile, oM.Base.Debugging.EventType.Warning, typeof(ConcreteSection)) ||
                !ToRobotConcreteSection(section.SectionProfile as dynamic, secData))
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
        /****           Private Methods                 ****/
        /***************************************************/

        private static bool ToRobotConcreteSection(this RectangleProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_H, section.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_B, section.Width);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotConcreteSection(this TSectionProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_T;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T;

            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_BF, section.Width);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_H, section.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_HF, section.FlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_B, section.WebThickness);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotConcreteSection(this FabricatedISectionProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I;

            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1, section.TopFlangeWidth);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2, section.BotFlangeWidth);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1, section.TopFlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2, section.BotFlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H, section.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B, section.WebThickness);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotConcreteSection(this CircleProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_C;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_COL_C;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_DE, section.Diameter);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotConcreteSection(this ISectionProfile section, IRobotBarSectionData sectionData)
        {
            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I;

            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1, section.Width);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2, section.Width);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1, section.FlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2, section.FlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H, section.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B, section.WebThickness);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotConcreteSection(this IProfile section, IRobotBarSectionData sectionData)
        {
            BH.Engine.Base.Compute.RecordWarning("Profile of type " + section.GetType().Name + " is not yet fully supported for Concrete sections. Section with name " + sectionData.Name + " set as explicit section");
            return false;
        }

        /***************************************************/
    }
}



