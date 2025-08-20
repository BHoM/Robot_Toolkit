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

        public static ServiceLimitStateParameters ServiceLimitStateParameters(bool deflectionLimitEnabled = false,
                                                                              double relativeDeflectionLimitY = 250.0,
                                                                              double relativeDeflectionLimitZ = 250.0,
                                                                              double absoluteDeflectionLimitY = 0.0,
                                                                              double absoluteDeflectionLimitZ = 0.0,
                                                                              bool useRelativeLimitY = true,
                                                                              bool useRelativeLimitZ = true)
        {
            ServiceLimitStateParameters slsParams = new ServiceLimitStateParameters();
            slsParams.DeflectionLimitEnabled = deflectionLimitEnabled;
            slsParams.RelativeDeflectionLimitY = relativeDeflectionLimitY;
            slsParams.RelativeDeflectionLimitZ = relativeDeflectionLimitZ;
            slsParams.AbsoluteDeflectionLimitY = absoluteDeflectionLimitY;
            slsParams.AbsoluteDeflectionLimitZ = absoluteDeflectionLimitZ;
            slsParams.UseRelativeLimitY = useRelativeLimitY;
            slsParams.UseRelativeLimitZ = useRelativeLimitZ;

            return slsParams;
        }

        /***************************************************/
    }
}