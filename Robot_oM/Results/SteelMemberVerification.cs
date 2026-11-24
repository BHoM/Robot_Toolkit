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
using BH.oM.Analytical.Results;
using BH.oM.Base;
using BH.oM.Quantities.Attributes;
using BH.oM.Structure.Design;

namespace BH.oM.Adapters.Robot
{
    [Description("Result of a Robot steel member verification for a single member. ResultCase is the name of the governing ultimate limit state case. \n" +
                 "Values for a limit state that was not checked are NaN.")]
    public class SteelMemberVerification : DesignResult, IResultItem, IImmutable
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        [Description("Full name of the design code used for the verification, as reported by Robot.")]
        public virtual string DesignCode { get; }

        [Description("Name of the section the member was verified with.")]
        public virtual string SectionName { get; }

        [Description("Name of the material the member was verified with.")]
        public virtual string MaterialName { get; }

        [Description("Outcome of the verification as reported by Robot.")]
        public virtual DesignCheckStatus Status { get; }

        [Ratio]
        [Description("Governing ultimate limit state utilisation ratio for the member (Robot 'Ratio').")]
        public override double Utilisation { get; }

        [Ratio]
        [Description("Slenderness about the local y axis for the ultimate limit state (Robot 'Lay').")]
        public virtual double SlendernessY { get; }

        [Ratio]
        [Description("Slenderness about the local z axis for the ultimate limit state (Robot 'Laz').")]
        public virtual double SlendernessZ { get; }

        [Length]
        [Description("Serviceability deflection of the member in its local y direction (Robot 'uy').")]
        public virtual double DeflectionY { get; }

        [Length]
        [Description("Serviceability deflection of the member in its local z direction (Robot 'uz').")]
        public virtual double DeflectionZ { get; }

        [Description("Deflection limit in the local y direction as reported by Robot (Robot 'RelLimit_uy').")]
        public virtual double DeflectionLimitY { get; }

        [Description("Deflection limit in the local z direction as reported by Robot (Robot 'RelLimit_uz').")]
        public virtual double DeflectionLimitZ { get; }

        [Description("Name of the case governing the local y deflection.")]
        public virtual string DeflectionGoverningCaseY { get; }

        [Description("Name of the case governing the local z deflection.")]
        public virtual string DeflectionGoverningCaseZ { get; }

        [Length]
        [Description("Serviceability node displacement in the global X direction (Robot 'vx').")]
        public virtual double DisplacementX { get; }

        [Length]
        [Description("Serviceability node displacement in the global Y direction (Robot 'vy').")]
        public virtual double DisplacementY { get; }

        [Description("Displacement limit in the global X direction as reported by Robot (Robot 'RelLimit_vx').")]
        public virtual double DisplacementLimitX { get; }

        [Description("Displacement limit in the global Y direction as reported by Robot (Robot 'RelLimit_vy').")]
        public virtual double DisplacementLimitY { get; }

        [Description("Name of the case governing the global X displacement.")]
        public virtual string DisplacementGoverningCaseX { get; }

        [Description("Name of the case governing the global Y displacement.")]
        public virtual string DisplacementGoverningCaseY { get; }

        [Description("Design code specific values for the governing ultimate limit state, such as resistances, buckling factors and the individual check ratios. \n" +
                     "Null if they were not requested, or if the design code is not supported yet. Currently only EN 1993-1-1 is supported.")]
        public virtual ISteelDesignCodeResults CodeResults { get; }

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public SteelMemberVerification(IComparable objectId, IComparable resultCase, int modeNumber, double timeStep,
                                       string designCode,
                                       string sectionName,
                                       string materialName,
                                       DesignCheckStatus status,
                                       double utilisation,
                                       double slendernessY,
                                       double slendernessZ,
                                       double deflectionY,
                                       double deflectionZ,
                                       double deflectionLimitY,
                                       double deflectionLimitZ,
                                       string deflectionGoverningCaseY,
                                       string deflectionGoverningCaseZ,
                                       double displacementX,
                                       double displacementY,
                                       double displacementLimitX,
                                       double displacementLimitY,
                                       string displacementGoverningCaseX,
                                       string displacementGoverningCaseY,
                                       ISteelDesignCodeResults codeResults)
            : base(objectId, resultCase, modeNumber, timeStep)
        {
            DesignCode = designCode;
            SectionName = sectionName;
            MaterialName = materialName;
            Status = status;
            Utilisation = utilisation;
            SlendernessY = slendernessY;
            SlendernessZ = slendernessZ;
            DeflectionY = deflectionY;
            DeflectionZ = deflectionZ;
            DeflectionLimitY = deflectionLimitY;
            DeflectionLimitZ = deflectionLimitZ;
            DeflectionGoverningCaseY = deflectionGoverningCaseY;
            DeflectionGoverningCaseZ = deflectionGoverningCaseZ;
            DisplacementX = displacementX;
            DisplacementY = displacementY;
            DisplacementLimitX = displacementLimitX;
            DisplacementLimitY = displacementLimitY;
            DisplacementGoverningCaseX = displacementGoverningCaseX;
            DisplacementGoverningCaseY = displacementGoverningCaseY;
            CodeResults = codeResults;
        }

        /***************************************************/
    }
}
