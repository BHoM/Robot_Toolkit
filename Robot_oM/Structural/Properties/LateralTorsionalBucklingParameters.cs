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

using BH.oM.Base;

namespace BH.oM.Adapters.Robot
{
    public class LateralTorsionalBucklingParameters : BHoMObject
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        /// <summary>
        /// Enables or disables lateral torsional buckling consideration
        /// </summary>
        public virtual bool LateralBucklingEnabled { get; set; } = false;

        /// <summary>
        /// Lateral buckling length coefficient
        /// </summary>
        public virtual double LateralBucklingLengthCoefficient { get; set; } = 1.0;

        /// <summary>
        /// Load level for upper flange (Auto = true, User = false)
        /// </summary>
        public virtual bool LoadLevelUpperFlangeAuto { get; set; } = true;

        /// <summary>
        /// Load level value for upper flange (when LoadLevelUpperFlangeAuto = false)
        /// </summary>
        public virtual double LoadLevelUpperFlangeValue { get; set; } = 1.0;

        /// <summary>
        /// Load level for lower flange (Auto = true, User = false)
        /// </summary>
        public virtual bool LoadLevelLowerFlangeAuto { get; set; } = true;

        /// <summary>
        /// Load level value for lower flange (when LoadLevelLowerFlangeAuto = false)
        /// </summary>
        public virtual double LoadLevelLowerFlangeValue { get; set; } = 1.0;

        /// <summary>
        /// Critical moment calculation method (Auto = true, User = false)
        /// </summary>
        public virtual bool CriticalMomentAuto { get; set; } = true;

        /// <summary>
        /// Critical moment value (when CriticalMomentAuto = false)
        /// </summary>
        public virtual double CriticalMomentValue { get; set; } = 1.0;

        /// <summary>
        /// Lateral buckling curve (auto calculated based on section)
        /// </summary>
        public virtual string LateralBucklingCurve { get; set; } = "auto";

        /// <summary>
        /// Lateral buckling method type (General = 0, Detailed = 1, Simplified = 2)
        /// </summary>
        public virtual int LateralBucklingMethodType { get; set; } = 0;

        /// <summary>
        /// Lambda LT,0 parameter for detailed method
        /// </summary>
        public virtual double LambdaLT0 { get; set; } = 0.4;

        /// <summary>
        /// Beta parameter for detailed method
        /// </summary>
        public virtual double Beta { get; set; } = 0.75;

        /// <summary>
        /// kfl parameter for detailed method
        /// </summary>
        public virtual double Kfl { get; set; } = 1.0;

        /***************************************************/
    }
}