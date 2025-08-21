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

using BH.oM.Adapters.Robot;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        public static LateralTorsionalBucklingParameters LateralTorsionalBucklingParameters(bool lateralBucklingEnabled = false,
                                                                                           double lateralBucklingLengthCoefficient = 1.0,
                                                                                           bool loadLevelUpperFlangeAuto = true,
                                                                                           double loadLevelUpperFlangeValue = 1.0,
                                                                                           bool loadLevelLowerFlangeAuto = true,
                                                                                           double loadLevelLowerFlangeValue = 1.0,
                                                                                           bool criticalMomentAuto = true,
                                                                                           double criticalMomentValue = 1.0,
                                                                                           string lateralBucklingCurve = "auto",
                                                                                           int lateralBucklingMethodType = 0,
                                                                                           double lambdaLT0 = 0.4,
                                                                                           double beta = 0.75,
                                                                                           double kfl = 1.0)
        {
            LateralTorsionalBucklingParameters ltbParams = new LateralTorsionalBucklingParameters();
            ltbParams.LateralBucklingEnabled = lateralBucklingEnabled;
            ltbParams.LateralBucklingLengthCoefficient = lateralBucklingLengthCoefficient;
            ltbParams.LoadLevelUpperFlangeAuto = loadLevelUpperFlangeAuto;
            ltbParams.LoadLevelUpperFlangeValue = loadLevelUpperFlangeValue;
            ltbParams.LoadLevelLowerFlangeAuto = loadLevelLowerFlangeAuto;
            ltbParams.LoadLevelLowerFlangeValue = loadLevelLowerFlangeValue;
            ltbParams.CriticalMomentAuto = criticalMomentAuto;
            ltbParams.CriticalMomentValue = criticalMomentValue;
            ltbParams.LateralBucklingCurve = lateralBucklingCurve;
            ltbParams.LateralBucklingMethodType = lateralBucklingMethodType;
            ltbParams.LambdaLT0 = lambdaLT0;
            ltbParams.Beta = beta;
            ltbParams.Kfl = kfl;

            return ltbParams;
        }

        /***************************************************/
    }
}