/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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

using BH.oM.Common.Materials;
using RobotOM;
using BH.oM.Structure.Properties.Section;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void ISectionProperty(this ISectionProperty section, IRobotBarSectionData secData)
        {
            SectionProperty(section as dynamic, section.Material, secData);
        }

        /***************************************************/

        public static void SectionProperty(this ExplicitSection section, Material material, IRobotBarSectionData secData)
        {
            
        }

        /***************************************************/

        public static void SectionProperty(this CableSection section, Material material, IRobotBarSectionData secData)
        {
            
        }

        /***************************************************/

        //private static void SectionShapeType(this IList<ICurve> edges, IRobotBarSectionData secData)
        //{
        //    throw new NotImplementedException();
        //}

        /***************************************************/

        public static ISectionProperty IBHoMSection(IRobotBarSectionData secData, Material material)
        {
            switch (material.Type)
            {
                case MaterialType.Aluminium:
                    return null;
                case MaterialType.Steel:
                    return BHoMSteelSection(secData);
                case MaterialType.Concrete:
                    return BHoMConcreteSection(secData);
                case MaterialType.Timber:
                    return null;
                case MaterialType.Rebar:
                    return null;
                case MaterialType.Tendon:
                    return null;
                case MaterialType.Glass:
                    return null;
                case MaterialType.Cable:
                    return null;
                default:
                    return null;
            }
        }

        /***************************************************/

    }
}
