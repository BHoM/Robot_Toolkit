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

using BH.oM.Physical.Materials;
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

        public static void ToRobot(this ISectionProperty section, IRobotBarSectionData secData)
        {
            ToRobot(section as dynamic, section.Material, secData);
        }

        /***************************************************/

        public static void ToRobot(this ExplicitSection section, IMaterialFragment material, IRobotBarSectionData secData)
        {
            secData.MaterialName = material.DescriptionOrName();

            secData.SetValue(IRobotBarSectionDataValue.I_BSDV_AX, section.Area);
            secData.SetValue(IRobotBarSectionDataValue.I_BSDV_IX, section.J);
            secData.SetValue(IRobotBarSectionDataValue.I_BSDV_IY, section.Iy);
            secData.SetValue(IRobotBarSectionDataValue.I_BSDV_IZ, section.Iz);
            secData.SetValue(IRobotBarSectionDataValue.I_BSDV_VY, section.Vy);
            secData.SetValue(IRobotBarSectionDataValue.I_BSDV_VZ, section.Vz);
            secData.SetValue(IRobotBarSectionDataValue.I_BSDV_VPY, section.Vpy);
            secData.SetValue(IRobotBarSectionDataValue.I_BSDV_VPZ, section.Vpz);

            secData.CalcNonstdGeometry();
        }

        /***************************************************/

        public static void ToRobot(this ISectionProperty section, IMaterialFragment material, IRobotBarSectionData secData)
        {
            secData.MaterialName = material.DescriptionOrName();
            Engine.Reflection.Compute.RecordWarning("Section of type " + section.GetType().Name + " is not yet supported in the Robot adapter. Section with name " + secData.Name + " will not have any properties set");
        }
        
        /***************************************************/

    }
}

