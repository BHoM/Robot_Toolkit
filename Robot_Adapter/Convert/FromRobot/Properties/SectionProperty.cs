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
                if (secData.IsConcrete)
                    profile = FromRobotConcreteProfile(secData);
                else
                    profile = FromRobotGeneralProfile(secData);
            }
            catch (System.Exception)
            {

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
                Engine.Reflection.Compute.RecordWarning(message);

                prop = exp;
            }

            return prop;           
        }

        /***************************************************/
    }
}

