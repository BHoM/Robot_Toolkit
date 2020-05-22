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
                if (timber.YoungsModulus.Y == timber.YoungsModulus.Z)
                {
                    materialData.E_Trans = timber.YoungsModulus.Y;
                }
                else{
                    materialData.E_Trans = Math.Abs((timber.YoungsModulus.Y + timber.YoungsModulus.Z) / 2);
                    Engine.Reflection.Compute.RecordWarning("Due to limitations ib the Naterial model for orthotripic Timber materials in Robot, Young's modulus has been taken as an average bwtween the y and z-component of the vector in the BHoM material.");
                }
                materialData.RO = timber.Density * Engine.Robot.Query.RobotGravityConstant;
                materialData.GMean = Math.Abs((timber.ShearModulus.X+ timber.ShearModulus.Y+ timber.ShearModulus.Z)/3);//Since shear modulus is expressed as shear stress over shear strain, longitudinal value is used as it is likely used for cross section analysis.
                materialData.LX = timber.ThermalExpansionCoeff.X;//Value in X axis. Longitudinal expansion for bar element is likely of interets.
                materialData.DumpCoef = timber.DampingRatio;
                Engine.Reflection.Compute.RecordWarning("Due to limitations in the Material model for orthotropic Timber materials in Robot, the following assumptions have been made when converting the BHoM material to Robot:\n" +
                                                        "- Youngs modulus has been set to the x-component of the vector in the BHoM material.\n" +
                                                        "- Youngs modulus transversal has been set to the average of the y and z-component of the vector in the BHoM material\n" +
                                                        "- The ShearModulus (G) has been set to the average value of all components of the vector in the BHoM material\n" +
                                                        "- The Thermal Expansion Coefficient has been set to the x-component of the vector in the BHoM material\n" +
                                                        "- Poissons Ratio is ignored for timber materials.");
            }
            else
            {
                IOrthotropic orthotropic = material as IOrthotropic;
                materialData.E = orthotropic.YoungsModulus.X;
                materialData.NU = orthotropic.PoissonsRatio.X;//Poisson ratio in X axis. Longitudinal defomation under axial loading is likely of interests.
                materialData.RO = orthotropic.Density * Engine.Robot.Query.RobotGravityConstant;
                materialData.LX = orthotropic.ThermalExpansionCoeff.X;//Value in X axis. Longitudinal expansion for bar element is likely of interets.
                materialData.Kirchoff = orthotropic.ShearModulus.X;//Since shear modulus is expressed as shear stress over shear strain, longitudinal value is used as it is likely used for cross section analysis.
                materialData.DumpCoef = orthotropic.DampingRatio;
                Engine.Reflection.Compute.RecordWarning("Robot does not support generic orthotropic materials. Material pushed will be treated as an isotropic material, only taking the x-component of the values into acount.\n" +
                                                        "This means the y and z-components of the vectors for YoungsModulus, ShearModulus, PoissonsRatio and ThermalExpansionCoeff will be ignored.");
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

