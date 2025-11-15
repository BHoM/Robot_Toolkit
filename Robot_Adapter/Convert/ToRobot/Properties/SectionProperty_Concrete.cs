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

using BH.oM.Spatial.ShapeProfiles;
using BH.oM.Structure.SectionProperties;
using RobotOM;
using System;
using System.Linq;


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
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_RECT_H, section.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_RECT_B, section.Width);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool ToRobotConcreteSection(this TaperedProfile section, IRobotBarSectionData sectionData)
        {
            if (section.Profiles.Count == 1)
                return ToRobotGeometricalSection(section.Profiles.First().Value as dynamic, sectionData);

            IProfile startProfile, endProfile;
            if (section.Profiles.Count == 2 && section.Profiles.TryGetValue(0, out startProfile) && section.Profiles.TryGetValue(1, out endProfile))
            {
                if (startProfile.GetType() == endProfile.GetType())
                {
                    // RectangleProfile
                    if (startProfile is RectangleProfile)
                    {
                        return HandleRectangleTaper(
                            startProfile as RectangleProfile,
                            endProfile as RectangleProfile,
                            sectionData);
                    }

                    // TSectionProfile
                    if (startProfile is TSectionProfile)
                    {
                        return HandleTeeTaper(
                            startProfile as TSectionProfile,
                            endProfile as TSectionProfile,
                            sectionData);
                    }

                    // ISectionProfile
                    if (startProfile is ISectionProfile)
                    {
                        return HandleITaper(
                            startProfile as ISectionProfile,
                            endProfile as ISectionProfile,
                            sectionData);
                    }

                    // FabISectionProfile
                    if (startProfile is FabricatedISectionProfile)
                    {
                        return HandleFabITaper(
                            startProfile as FabricatedISectionProfile,
                            endProfile as FabricatedISectionProfile,
                            sectionData);
                    }
                }
                BH.Engine.Base.Compute.RecordWarning("Tapered section end profiles must match.");
                return false;
            }
            return false;
        }

        /***************************************************/

        private static bool HandleRectangleTaper(RectangleProfile startRect, RectangleProfile endRect, IRobotBarSectionData sectionData)
        {
            if (startRect == null || endRect == null)
                return false;

            // Rule: width must remain constant (based on original logic).
            if (!NearlyEqual(startRect.Width, endRect.Width))
            {
                BH.Engine.Base.Compute.RecordWarning("Concrete tapered rectangle requires constant width. Section " + sectionData.Name + " aborted.");
                return false;
            }

            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT;

            IRobotBarSectionNonstdData nonStd = sectionData.CreateNonstd(0);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_RECT_H, startRect.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_RECT_B, startRect.Width);
            sectionData.Concrete.SetTapered(endRect.Height);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        private static bool HandleTeeTaper(TSectionProfile startT, TSectionProfile endT, IRobotBarSectionData sectionData)
        {
            if (startT == null || endT == null)
                return false;

            // Assumed constants: FlangeWidth and WebThickness must match.
            if (!NearlyEqual(startT.Width, endT.Width) ||
                !NearlyEqual(startT.WebThickness, endT.WebThickness) || !NearlyEqual(startT.FlangeThickness, endT.FlangeThickness))
            {
                BH.Engine.Base.Compute.RecordWarning("T tapered section " + sectionData.Name +
                    " requires constant flange width, flange thickness and web thickness.");
                return false;
            }

            sectionData.Type = IRobotBarSectionType.I_BST_NS_T;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T;

            IRobotBarSectionNonstdData nonStd = sectionData.CreateNonstd(0);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_BF, startT.Width);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_H, startT.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_HF, startT.FlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_B, startT.WebThickness);

            // Taper depth
            sectionData.Concrete.SetTapered(endT.Height);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        private static bool HandleITaper(ISectionProfile startI, ISectionProfile endI, IRobotBarSectionData sectionData)
        {
            if (startI == null || endI == null)
                return false;

            // Assumed constants: flange widths & web thickness must match.
            if (!NearlyEqual(startI.Width, endI.Width) ||
                !NearlyEqual(startI.WebThickness, endI.WebThickness) || !NearlyEqual(startI.FlangeThickness, endI.FlangeThickness))
            {
                BH.Engine.Base.Compute.RecordWarning("I tapered section " + sectionData.Name +
                    " requires constant flange widths and web thickness.");
                return false;
            }

            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I;

            IRobotBarSectionNonstdData nonStd = sectionData.CreateNonstd(0);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H, startI.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1, startI.Width);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2, startI.Width);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1, startI.FlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2, startI.FlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B, startI.WebThickness);

            sectionData.Concrete.SetTapered(endI.Height);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        private static bool HandleFabITaper(FabricatedISectionProfile startI, FabricatedISectionProfile endI, IRobotBarSectionData sectionData)
        {
            if (startI == null || endI == null)
                return false;

            // Assumed constants: flange widths & web thickness must match.
            if (!NearlyEqual(startI.TopFlangeWidth, endI.TopFlangeWidth) || !NearlyEqual(startI.BotFlangeWidth, endI.BotFlangeWidth) || 
                !NearlyEqual(startI.WebThickness, endI.WebThickness) || !NearlyEqual(startI.TopFlangeThickness, endI.TopFlangeThickness) ||
                !NearlyEqual(startI.BotFlangeThickness, endI.BotFlangeThickness))
            {
                BH.Engine.Base.Compute.RecordWarning("I tapered section " + sectionData.Name +
                    " requires constant flange widths and web thickness.");
                return false;
            }

            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I;

            IRobotBarSectionNonstdData nonStd = sectionData.CreateNonstd(0);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H, startI.Height);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1, startI.TopFlangeWidth);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2, startI.BotFlangeWidth);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1, startI.TopFlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2, startI.BotFlangeThickness);
            sectionData.Concrete.SetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B, startI.WebThickness);

            sectionData.Concrete.SetTapered(endI.Height);

            sectionData.CalcNonstdGeometry();
            return true;
        }

        /***************************************************/

        private static bool NearlyEqual(double a, double b)
        {
            // Simple tolerance helper; adjust as needed.
            const double tol = 1e-6;
            return Math.Abs(a - b) < tol;
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






