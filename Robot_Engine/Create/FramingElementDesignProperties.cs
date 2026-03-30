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
using BH.oM.Adapters.Robot;
using BH.oM.Base.Attributes;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        [Description("Creates a FramingElementDesignProperties object for use with Robot steel design.")]
        [Input("name", "The name of the framing element design properties.")]
        [Output("framingElementDesignProperties", "The FramingElementDesignProperties object for use with the Robot adapter.")]
        public static FramingElementDesignProperties FramingElementDesignProperties(string name)
        {
            FramingElementDesignProperties framingElementDesignProperties = new FramingElementDesignProperties();
            framingElementDesignProperties.Name = name;

            return framingElementDesignProperties;
        }

        /***************************************************/

        [Description("Creates a FramingElementDesignProperties object with Euler buckling length coefficients for use with Robot steel design.")]
        [Input("name", "The name of the framing element design properties.")]
        [Input("eulerBucklingLengthCoeffY", "The Euler buckling length coefficient about the local Y-axis.")]
        [Input("eulerBucklingLengthCoeffZ", "The Euler buckling length coefficient about the local Z-axis.")]
        [Output("framingElementDesignProperties", "The FramingElementDesignProperties object for use with the Robot adapter.")]
        public static FramingElementDesignProperties FramingElementDesignProperties(string name,
                                                                                    double eulerBucklingLengthCoeffY = 1,
                                                                                    double eulerBucklingLengthCoeffZ = 1)
        {
            FramingElementDesignProperties framEleDesignProps = new FramingElementDesignProperties();
            framEleDesignProps.Name = name;
            framEleDesignProps.EulerBucklingLengthCoefficientY = eulerBucklingLengthCoeffY;
            framEleDesignProps.EulerBucklingLengthCoefficientZ = eulerBucklingLengthCoeffZ;

            return framEleDesignProps;
        }

        /***************************************************/
    }
}





