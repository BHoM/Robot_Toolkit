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


using System;
using BH.oM.Physical.Materials;
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Structure;
using RobotOM;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void ToRobot(IRobotMaterialData materialData, IMaterialFragment material)
        {
            materialData.Type = GetMaterialType(material);
            materialData.Name = material.Name;

            if (material is IIsotropic)
            {
                IIsotropic isotropic = material as IIsotropic;
                materialData.E = isotropic.YoungsModulus;
                materialData.NU = isotropic.PoissonsRatio;
                materialData.RO = isotropic.Density * Engine.Robot.Query.RobotGravityConstant;
                materialData.LX = isotropic.ThermalExpansionCoeff;
                materialData.Kirchoff = isotropic.ShearModulus();
                materialData.DumpCoef = isotropic.DampingRatio;
            }
            else if (material is Timber)
            {
                Timber timber = material as Timber;
                materialData.E = timber.YoungsModulus.X;
                Engine.Reflection.Compute.RecordWarning("Young's modulus in Y and Z axis are ignored due to Robot definition limitation");
                materialData.E_Trans = Math.Abs((timber.YoungsModulus.Y+timber.YoungsModulus.Z)/2);
                Engine.Reflection.Compute.RecordWarning("Young transverse modulue (timber) is an average between Y and Z axis due to Robot definition limitation");
                materialData.RO = timber.Density * Engine.Robot.Query.RobotGravityConstant;
                materialData.Kirchoff = timber.ShearModulus.X;//Since shear modulus is expressed as shear stress over shear strain, longitudinal value is used as it is likely used for cross section analysis.
                Engine.Reflection.Compute.RecordWarning("Shear modulus in Y and Z axis are ignored due to Robot definition limitation");
                materialData.LX = timber.ThermalExpansionCoeff.X;//Value in X axis. Longitudinal expansion for bar element is likely of interets.
                Engine.Reflection.Compute.RecordWarning("Thermal Expansion Coefficient in Y and Z axis are ignored due to Robot definition limitation");
                materialData.NU = timber.PoissonsRatio.X;//Poisson ratio in X axis. Longitudinal defomation under axial loading is likely of interests.
                Engine.Reflection.Compute.RecordWarning("Poisson ratio in Y and Z axis are ignored due to Robot definition limitation");
                materialData.DumpCoef = timber.DampingRatio;

            }
            else
            {
                IOrthotropic orthotropic = material as IOrthotropic;
                materialData.E = orthotropic.YoungsModulus.X;
                Engine.Reflection.Compute.RecordWarning("Young's modulus in Y and Z axis are ignored due to Robot definition limitation");
                materialData.NU = orthotropic.PoissonsRatio.X;//Poisson ratio in X axis. Longitudinal defomation under axial loading is likely of interests.
                Engine.Reflection.Compute.RecordWarning("Poisson ratio in Y and Z axis are ignored due to Robot definition limitation");
                materialData.RO = orthotropic.Density * Engine.Robot.Query.RobotGravityConstant;
                materialData.LX = orthotropic.ThermalExpansionCoeff.X;//Value in X axis. Longitudinal expansion for bar element is likely of interets.
                Engine.Reflection.Compute.RecordWarning("Thermal Expansion Coefficient in Y and Z axis are ignored due to Robot definition limitation");
                materialData.Kirchoff = orthotropic.ShearModulus.X;//Since shear modulus is expressed as shear stress over shear strain, longitudinal value is used as it is likely used for cross section analysis.
                materialData.DumpCoef = orthotropic.DampingRatio;
                materialData.Type = IRobotMaterialType.I_MT_OTHER;
            }
        }

        /***************************************************/

        public static IRobotMaterialType GetMaterialType(IMaterialFragment mat)
        {
            if (mat is Steel)
                return IRobotMaterialType.I_MT_STEEL;
            else if (mat is Concrete)
                return IRobotMaterialType.I_MT_CONCRETE;
            else if (mat is Aluminium)
                return IRobotMaterialType.I_MT_ALUMINIUM;
            else if (mat is Timber)
                return IRobotMaterialType.I_MT_TIMBER;
            else
                return IRobotMaterialType.I_MT_OTHER;
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

