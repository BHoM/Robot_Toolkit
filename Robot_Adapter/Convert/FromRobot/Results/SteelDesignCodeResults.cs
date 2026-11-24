/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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
using System.ComponentModel;
using System.Reflection;
using BH.oM.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Converts the code specific results object of a Robot steel member verification (IRDimDetailedRes.CodeResults) to BHoM, based on the design code name. \n" +
                     "Returns null for design codes that are not supported yet.")]
        public static ISteelDesignCodeResults FromRobotSteelCodeResults(object codeResults, string designCode)
        {
            if (codeResults == null || string.IsNullOrEmpty(designCode))
                return null;

            if (designCode.Contains("1993"))
                return FromRobotEurocodeSteelCodeResults(codeResults);

            return null;
        }

        /***************************************************/

        [Description("Converts the code specific results object of a Robot steel member verification to EN 1993-1-1 results. \n" +
                     "Robot spreads these values over the IRDimCodeResE32_1 to IRDimCodeResE32_4 interfaces, so they are read by name through late binding. \n" +
                     "Values Robot does not expose are set to NaN.")]
        public static EurocodeSteelCodeResults FromRobotEurocodeSteelCodeResults(object codeResults)
        {
            if (codeResults == null)
                return null;

            double sectionClass = GetCodeValue(codeResults, "ClassOfSect");

            return new EurocodeSteelCodeResults
            {
                //Section and material
                SectionClass = double.IsNaN(sectionClass) ? 0 : (int)sectionClass,
                YieldStrength = GetCodeValue(codeResults, "MaterialCapacityFy"),
                GammaM0 = GetCodeValue(codeResults, "MaterCoeffGamma0"),
                GammaM1 = GetCodeValue(codeResults, "MaterCoeffGamma1"),

                //Cross-section resistances
                TensionResistance = GetCodeValue(codeResults, "PlastTensStrenNtrd"),
                CompressionResistance = GetCodeValue(codeResults, "PlastCompStrenNcrd"),
                BendingResistanceY = GetCodeValue(codeResults, "ReducMomStrenMcyrd"),
                BendingResistanceZ = GetCodeValue(codeResults, "ReducMomStrenMczrd"),
                ShearResistanceY = GetCodeValue(codeResults, "ShearStrenTplyrd"),
                ShearResistanceZ = GetCodeValue(codeResults, "ShearStrenTplzrd"),

                //Flexural buckling
                BucklingLengthCoefficientY = GetCodeValue(codeResults, "BuckParamKy"),
                BucklingLengthCoefficientZ = GetCodeValue(codeResults, "BuckParamKz"),
                CriticalForceY = GetCodeValue(codeResults, "BuckCritEulerForcNcry"),
                CriticalForceZ = GetCodeValue(codeResults, "BuckCritEulerForcNcrz"),
                RelativeSlendernessY = GetCodeValue(codeResults, "BuckRelSlendLaby"),
                RelativeSlendernessZ = GetCodeValue(codeResults, "BuckRelSlendLabz"),
                ImperfectionFactorY = GetCodeValue(codeResults, "BuckCurveCoeffAlfy"),
                ImperfectionFactorZ = GetCodeValue(codeResults, "BuckCurveCoeffAlfz"),
                BucklingReductionFactorY = GetCodeValue(codeResults, "BuckCoeffXy"),
                BucklingReductionFactorZ = GetCodeValue(codeResults, "BuckCoeffXz"),
                BucklingResistanceY = GetCodeValue(codeResults, "BuckStrenNbyrd"),
                BucklingResistanceZ = GetCodeValue(codeResults, "BuckStrenNbzrd"),

                //Lateral torsional buckling
                CriticalMoment = GetCodeValue(codeResults, "CritMomenMcr"),
                C1 = GetCodeValue(codeResults, "LatBuckParamC1"),
                RelativeSlendernessLT = GetCodeValue(codeResults, "LatBuckSlendLamlt"),
                LTBReductionFactor = GetCodeValue(codeResults, "LatBuckCoeffXlt"),
                LTBReductionFactorModified = GetCodeValue(codeResults, "LatBuckCoeffXltmod"),
                LTBResistance = GetCodeValue(codeResults, "LatBuckMomStrenmbrd"),

                //Interaction factors
                Kyy = GetCodeValue(codeResults, "OveralStabKyy"),
                Kyz = GetCodeValue(codeResults, "OveralStabKyz"),
                Kzy = GetCodeValue(codeResults, "OveralStabKzy"),
                Kzz = GetCodeValue(codeResults, "OveralStabKzz"),

                //Check ratios
                TensionRatio = GetCodeValue(codeResults, "SecCheckNt"),
                CompressionRatio = GetCodeValue(codeResults, "SecCheckNc"),
                BendingRatioY = GetCodeValue(codeResults, "SecCheckMy"),
                BendingRatioZ = GetCodeValue(codeResults, "SecCheckMz"),
                ShearRatioY = GetCodeValue(codeResults, "SecCheckVy"),
                ShearRatioZ = GetCodeValue(codeResults, "SecCheckVz"),
                CombinedSectionRatio = GetCodeValue(codeResults, "SecCheckNMyMz"),
                FlexuralBucklingRatio = GetCodeValue(codeResults, "BuckCheckNb"),
                LTBRatio = GetCodeValue(codeResults, "BuckCheckMy"),
                InteractionRatioY = GetCodeValue(codeResults, "BuckCheckNyMyMz"),
                InteractionRatioZ = GetCodeValue(codeResults, "BuckCheckNzMyMz"),
            };
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private static double GetCodeValue(object codeResults, string propertyName)
        {
            try
            {
                object value = codeResults.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, codeResults, null);
                return value == null ? double.NaN : System.Convert.ToDouble(value);
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /***************************************************/
    }
}
