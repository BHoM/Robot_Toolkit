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

using BH.oM.Physical.Materials;
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Structure;
using RobotOM;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void RobotMaterial(IRobotMaterialData materialData, Material material)
        {
            materialData.Type = GetMaterialType(material);
            materialData.Name = material.Name;

            if (!material.IsStructural())
            {
                Engine.Reflection.Compute.RecordWarning("Material with name " + material.Name + " is does not contain structural properties. Please check the material");
                return;
            }

            if (material.IsIsotropic())
            {
                materialData.E = material.YoungsModulusIsotropic();
                materialData.NU = material.PoissonsRatioIsotropic();
                materialData.RO = material.Density * Engine.Robot.Query.RobotGravityConstant;
                materialData.LX = material.ThermalExpansionCoeffIsotropic();
                materialData.Kirchoff = material.ShearModulusIsotropic();
                materialData.DumpCoef = material.DampingRatio();
            }
            else
            {
                Engine.Reflection.Compute.RecordWarning("Robot_Toolkit does currently only suport Isotropic material. No structural properties for material with name " + material.Name + " have been pushed");
                return;
            }
        }

        /***************************************************/

        public static IRobotMaterialType GetMaterialType(Material mat)
        {
            switch (mat.MaterialType())
            {
                case MaterialType.Aluminium:
                    return IRobotMaterialType.I_MT_ALUMINIUM;
                case MaterialType.Steel:
                    return IRobotMaterialType.I_MT_STEEL;
                case MaterialType.Concrete:
                    return IRobotMaterialType.I_MT_CONCRETE;
                case MaterialType.Timber:
                    return IRobotMaterialType.I_MT_TIMBER;
                case MaterialType.Rebar:
                case MaterialType.Tendon:
                case MaterialType.Glass:
                case MaterialType.Cable:
                case MaterialType.Undefined:
                    return IRobotMaterialType.I_MT_OTHER;
                default:
                    return IRobotMaterialType.I_MT_OTHER;
            }
        }

        /***************************************************/

        public static MaterialType GetMaterialType(IRobotMaterialType mType)
        {
            switch (mType)
            {
                case IRobotMaterialType.I_MT_CONCRETE:
                    return MaterialType.Concrete;
                case IRobotMaterialType.I_MT_STEEL:
                    return MaterialType.Steel;
                case IRobotMaterialType.I_MT_TIMBER:
                    return MaterialType.Timber;
                case IRobotMaterialType.I_MT_ALUMINIUM:
                    return MaterialType.Aluminium;
                default:
                    return MaterialType.Steel;
            }
        }

        /***************************************************/
    }
}
