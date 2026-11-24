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

using System.ComponentModel;
using BH.oM.Base;
using BH.oM.Quantities.Attributes;

namespace BH.oM.Adapters.Robot
{
    [Description("EN 1993-1-1 values from a Robot steel member verification, at the governing point of the governing ultimate limit state case. \n" +
                 "Values Robot did not calculate or report for the member are NaN.")]
    public class EurocodeSteelCodeResults : BHoMObject, ISteelDesignCodeResults
    {
        /***************************************************/
        /**** Section and material                      ****/
        /***************************************************/

        [Description("Cross-section class (1 to 4) to EN 1993-1-1 clause 5.5.")]
        public virtual int SectionClass { get; set; } = 0;

        [Stress]
        [Description("Yield strength fy used in the verification.")]
        public virtual double YieldStrength { get; set; } = double.NaN;

        [Description("Partial factor gamma_M0 for cross-section resistance.")]
        public virtual double GammaM0 { get; set; } = double.NaN;

        [Description("Partial factor gamma_M1 for member resistance to instability.")]
        public virtual double GammaM1 { get; set; } = double.NaN;

        /***************************************************/
        /**** Cross-section resistances                 ****/
        /***************************************************/

        [Force]
        [Description("Design tension resistance Nt,Rd (6.2.3).")]
        public virtual double TensionResistance { get; set; } = double.NaN;

        [Force]
        [Description("Design compression resistance Nc,Rd (6.2.4).")]
        public virtual double CompressionResistance { get; set; } = double.NaN;

        [Moment]
        [Description("Design bending resistance about the local y axis Mc,y,Rd (6.2.5).")]
        public virtual double BendingResistanceY { get; set; } = double.NaN;

        [Moment]
        [Description("Design bending resistance about the local z axis Mc,z,Rd (6.2.5).")]
        public virtual double BendingResistanceZ { get; set; } = double.NaN;

        [Force]
        [Description("Design plastic shear resistance in the local y direction Vpl,y,Rd (6.2.6).")]
        public virtual double ShearResistanceY { get; set; } = double.NaN;

        [Force]
        [Description("Design plastic shear resistance in the local z direction Vpl,z,Rd (6.2.6).")]
        public virtual double ShearResistanceZ { get; set; } = double.NaN;

        /***************************************************/
        /**** Flexural buckling                         ****/
        /***************************************************/

        [Description("Buckling length coefficient about the local y axis (Robot 'ky').")]
        public virtual double BucklingLengthCoefficientY { get; set; } = double.NaN;

        [Description("Buckling length coefficient about the local z axis (Robot 'kz').")]
        public virtual double BucklingLengthCoefficientZ { get; set; } = double.NaN;

        [Force]
        [Description("Elastic critical force for flexural buckling about the local y axis Ncr,y.")]
        public virtual double CriticalForceY { get; set; } = double.NaN;

        [Force]
        [Description("Elastic critical force for flexural buckling about the local z axis Ncr,z.")]
        public virtual double CriticalForceZ { get; set; } = double.NaN;

        [Description("Non-dimensional slenderness for flexural buckling about the local y axis, lambda-bar y.")]
        public virtual double RelativeSlendernessY { get; set; } = double.NaN;

        [Description("Non-dimensional slenderness for flexural buckling about the local z axis, lambda-bar z.")]
        public virtual double RelativeSlendernessZ { get; set; } = double.NaN;

        [Description("Imperfection factor alpha for the buckling curve about the local y axis.")]
        public virtual double ImperfectionFactorY { get; set; } = double.NaN;

        [Description("Imperfection factor alpha for the buckling curve about the local z axis.")]
        public virtual double ImperfectionFactorZ { get; set; } = double.NaN;

        [Description("Reduction factor for flexural buckling about the local y axis, chi y (6.3.1.2).")]
        public virtual double BucklingReductionFactorY { get; set; } = double.NaN;

        [Description("Reduction factor for flexural buckling about the local z axis, chi z (6.3.1.2).")]
        public virtual double BucklingReductionFactorZ { get; set; } = double.NaN;

        [Force]
        [Description("Design buckling resistance about the local y axis Nb,y,Rd (6.3.1.1).")]
        public virtual double BucklingResistanceY { get; set; } = double.NaN;

        [Force]
        [Description("Design buckling resistance about the local z axis Nb,z,Rd (6.3.1.1).")]
        public virtual double BucklingResistanceZ { get; set; } = double.NaN;

        /***************************************************/
        /**** Lateral torsional buckling                ****/
        /***************************************************/

        [Moment]
        [Description("Elastic critical moment for lateral torsional buckling Mcr.")]
        public virtual double CriticalMoment { get; set; } = double.NaN;

        [Description("Moment distribution factor C1 used to calculate Mcr.")]
        public virtual double C1 { get; set; } = double.NaN;

        [Description("Non-dimensional slenderness for lateral torsional buckling, lambda-bar LT.")]
        public virtual double RelativeSlendernessLT { get; set; } = double.NaN;

        [Description("Reduction factor for lateral torsional buckling, chi LT (6.3.2.2 or 6.3.2.3).")]
        public virtual double LTBReductionFactor { get; set; } = double.NaN;

        [Description("Modified reduction factor for lateral torsional buckling, chi LT,mod (6.3.2.3(2)).")]
        public virtual double LTBReductionFactorModified { get; set; } = double.NaN;

        [Moment]
        [Description("Design buckling resistance moment Mb,Rd (6.3.2.1).")]
        public virtual double LTBResistance { get; set; } = double.NaN;

        /***************************************************/
        /**** Interaction factors (6.3.3)               ****/
        /***************************************************/

        [Description("Interaction factor kyy (Annex A or B).")]
        public virtual double Kyy { get; set; } = double.NaN;

        [Description("Interaction factor kyz (Annex A or B).")]
        public virtual double Kyz { get; set; } = double.NaN;

        [Description("Interaction factor kzy (Annex A or B).")]
        public virtual double Kzy { get; set; } = double.NaN;

        [Description("Interaction factor kzz (Annex A or B).")]
        public virtual double Kzz { get; set; } = double.NaN;

        /***************************************************/
        /**** Check ratios                              ****/
        /***************************************************/

        [Ratio]
        [Description("Cross-section check ratio for tension, NEd / Nt,Rd.")]
        public virtual double TensionRatio { get; set; } = double.NaN;

        [Ratio]
        [Description("Cross-section check ratio for compression, NEd / Nc,Rd.")]
        public virtual double CompressionRatio { get; set; } = double.NaN;

        [Ratio]
        [Description("Cross-section check ratio for bending about the local y axis, My,Ed / Mc,y,Rd.")]
        public virtual double BendingRatioY { get; set; } = double.NaN;

        [Ratio]
        [Description("Cross-section check ratio for bending about the local z axis, Mz,Ed / Mc,z,Rd.")]
        public virtual double BendingRatioZ { get; set; } = double.NaN;

        [Ratio]
        [Description("Cross-section check ratio for shear in the local y direction, Vy,Ed / Vpl,y,Rd.")]
        public virtual double ShearRatioY { get; set; } = double.NaN;

        [Ratio]
        [Description("Cross-section check ratio for shear in the local z direction, Vz,Ed / Vpl,z,Rd.")]
        public virtual double ShearRatioZ { get; set; } = double.NaN;

        [Ratio]
        [Description("Cross-section check ratio for combined axial force and biaxial bending (6.2.9).")]
        public virtual double CombinedSectionRatio { get; set; } = double.NaN;

        [Ratio]
        [Description("Member check ratio for flexural buckling, NEd / Nb,Rd.")]
        public virtual double FlexuralBucklingRatio { get; set; } = double.NaN;

        [Ratio]
        [Description("Member check ratio for lateral torsional buckling, My,Ed / Mb,Rd.")]
        public virtual double LTBRatio { get; set; } = double.NaN;

        [Ratio]
        [Description("Member check ratio for combined compression and bending, equation 6.61.")]
        public virtual double InteractionRatioY { get; set; } = double.NaN;

        [Ratio]
        [Description("Member check ratio for combined compression and bending, equation 6.62.")]
        public virtual double InteractionRatioZ { get; set; } = double.NaN;

        /***************************************************/
    }
}
