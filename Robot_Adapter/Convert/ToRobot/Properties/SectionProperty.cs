/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using BH.Engine.Structure;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void IToRobot(this ISectionProperty section, IRobotBarSectionData secData)
        {
            Type type = section.GetType();
            //Check the material is not null
            if(RobotAdapter.CheckNotNull(section.Material, oM.Base.Debugging.EventType.Warning, type))
                secData.MaterialName = section.Material.DescriptionOrName(); //Set the material to the section

            ToRobot(section as dynamic, secData);
        }

        /***************************************************/

        public static void ToRobot(this ExplicitSection section, IRobotBarSectionData secData)
        {
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

        /***************************************************/

        public static void ToRobot(this ISectionProperty section, IRobotBarSectionData secData)
        {
            Engine.Base.Compute.RecordWarning("Section of type " + section.GetType().Name + " is not yet supported in the Robot adapter. Section with name " + secData.Name + " will not have any properties set");
        }
        
        /***************************************************/

    }
}





