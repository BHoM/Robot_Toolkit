/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using BH.oM.Spatial.ShapeProfiles;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
        
        public static IProfile FromRobotConcreteProfile(IRobotBarSectionData secData)
        {
            double b = 0;
            double h = 0;
            double HF = 0;
            double HF1 = 0;
            double HF2 = 0;
            double b1 = 0;
            double b2 = 0;
            double h1 = 0;
            double h2 = 0;
            double l1 = 0;
            double l2 = 0;

            if (!secData.IsConcrete)
            {
                Engine.Base.Compute.RecordWarning("The section with the name " + secData.Name + " has a concrete material assigned but is not a concrete section type. The section will not be read, please check the material assignment");
                return null;
            }
            RobotBarSectionConcreteData concMember = secData.Concrete;
            IProfile sectionProfile;

            switch (secData.ShapeType)
            {
                case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_B);
                    sectionProfile = BH.Engine.Spatial.Create.RectangleProfile(h, b, 0);
                    break;

                case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I:
                    b1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1);
                    b2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2);
                    HF1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1);
                    HF2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2);
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B);
                    sectionProfile = BH.Engine.Spatial.Create.FabricatedISectionProfile(h, b1, b2, b, HF1, HF2, 0);
                    break;

                case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_H);
                    b1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_B);
                    HF = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_HF);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_BF);
                    sectionProfile = BH.Engine.Spatial.Create.TSectionProfile(h, b, b1, HF, 0, 0);
                    break;

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_R:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
                    sectionProfile = BH.Engine.Spatial.Create.RectangleProfile(b, h, 0);
                    break;

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_T:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
                    h1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1);
                    h2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H2);
                    l1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1);
                    l2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L2);
                    sectionProfile = BH.Engine.Spatial.Create.TSectionProfile(h, b, b - l1 - l2, h - h1, 0, 0);
                    break;

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_C:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_DE);
                    sectionProfile = BH.Engine.Spatial.Create.CircleProfile(h);
                    break;

                case IRobotBarSectionShapeType.I_BSST_CONCR_COL_L:
                    h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
                    b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
                    h1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1);
                    l1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1);
                    sectionProfile = BH.Engine.Spatial.Create.AngleProfile(h, b, b - l1, h - h1, 0, 0);
                    break;

                default:
                    return null;
            }

            return sectionProfile;

        }

        /***************************************************/
    }
}




