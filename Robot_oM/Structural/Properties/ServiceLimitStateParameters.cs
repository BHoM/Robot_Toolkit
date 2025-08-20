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
    public class ServiceLimitStateParameters : BHoMObject
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        /// <summary>
        /// Enables or disables deflection and displacement limit checking
        /// </summary>
        public virtual bool DeflectionLimitEnabled { get; set; } = false;

        /// <summary>
        /// Relative deflection limit in Y direction (as a fraction of span length)
        /// </summary>
        public virtual double RelativeDeflectionLimitY { get; set; } = 250.0;

        /// <summary>
        /// Relative deflection limit in Z direction (as a fraction of span length)
        /// </summary>
        public virtual double RelativeDeflectionLimitZ { get; set; } = 250.0;

        /// <summary>
        /// Absolute deflection limit in Y direction (in model units)
        /// </summary>
        public virtual double AbsoluteDeflectionLimitY { get; set; } = 0.0;

        /// <summary>
        /// Absolute deflection limit in Z direction (in model units)
        /// </summary>
        public virtual double AbsoluteDeflectionLimitZ { get; set; } = 0.0;

        /// <summary>
        /// Whether to use relative deflection limits (true) or absolute limits (false) for Y direction
        /// </summary>
        public virtual bool UseRelativeLimitY { get; set; } = true;

        /// <summary>
        /// Whether to use relative deflection limits (true) or absolute limits (false) for Z direction
        /// </summary>
        public virtual bool UseRelativeLimitZ { get; set; } = true;

        /***************************************************/
    }
}