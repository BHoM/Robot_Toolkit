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
using BH.oM.Structure.MaterialFragments;
using RobotOM;
using BH.oM.Structure.SectionProperties;
using BH.Engine.Structure;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static ISectionProperty FromRobot(this IRobotBarSectionData secData, IMaterialFragment material, string robotLabelName)
        {

            ISectionProperty prop = null;
            IProfile profile = null;
            
            try
            {
                // Check for cellular/castellated beams by shape type (Robot may report them as I_BST_STANDARD or I_BST_COMPLEX)
                // These return CellularSection (ISectionProperty) directly, not IProfile
                if (secData.ShapeType == IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_ROUND_OPENINGS ||
                    secData.ShapeType == IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_HEXAGONAL_OPENINGS ||
                    secData.ShapeType == IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_HEXAGONAL_OPENINGS_SHIFTED)
                {
                    // Cellular beams - extract from standard section data with DIM values
                    prop = FromRobotCellularProfile(secData);
                    
                    // Set material on the cellular section
                    if (prop != null && material != null)
                    {
                        prop.Material = material;
                    }
                    
                    if (prop == null)
                    {
                        Engine.Base.Compute.RecordWarning($"Failed to convert cellular/castellated beam named {robotLabelName}. Shape type: {secData.ShapeType}, Section type: {secData.Type}");
                    }
                    
                    return prop;
                }
                
                // Check for cellular/castellated beams with I_BST_SPECIAL type (alternative path)
                if (secData.Type == IRobotBarSectionType.I_BST_SPECIAL)
                {
                    IRobotBarSectionSpecialData secSpecData = secData.Special;
                    if (secSpecData != null)
                    {
                        prop = FromRobotSpecialProfile(secSpecData, secData);
                        // Set material on the cellular section
                        if (prop != null && material != null)
                        {
                            prop.Material = material;
                        }
                        
                        if (prop == null)
                        {
                            Engine.Base.Compute.RecordWarning($"Failed to convert special section (cellular/castellated beam) named {robotLabelName}. Section type: {secData.ShapeType}");
                        }
                        
                        return prop;
                    }
                    else
                    {
                        Engine.Base.Compute.RecordWarning($"Special section data not available for section {robotLabelName} of type {secData.Type}");
                    }
                }
                
                // Handle standard profiles (concrete and other geometrical sections)
                if (secData.IsConcrete)
                    profile = FromRobotConcreteProfile(secData);
                else
                    profile = FromRobotGeneralProfile(secData);
            }
            catch (System.Exception ex)
            {
                Engine.Base.Compute.RecordWarning($"Exception converting section {robotLabelName}: {ex.Message}");
            }

            if (profile != null)
                prop = Create.SectionPropertyFromProfile(profile, material, secData.Name);            
            else
            {

                string message = "Failed to convert the section named " + robotLabelName + " to a geometric section.";

                ExplicitSection exp = new ExplicitSection() { Name = robotLabelName, Material = material };

                try
                {
                    exp.Area = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_AX);
                    exp.J = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_IX);
                    exp.Iy = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_IY);
                    exp.Iz = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_IZ);
                    exp.Vy = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_VY);
                    exp.Vz = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_VZ);
                    exp.Vpy = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_VPY);
                    exp.Vpz = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_VPZ);
                    exp.Wply = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_ZY);
                    exp.Wplz = secData.GetValue(IRobotBarSectionDataValue.I_BSDV_ZZ);

                    message += " Section has been returned as explicit with main analytical properties set.";
                }
                catch (System.Exception)
                {
                    message += " Section has been returned as an empty explicit section";
                }
                Engine.Base.Compute.RecordWarning(message);

                prop = exp;
            }

            return prop;           
        }

        /***************************************************/
    }
}






