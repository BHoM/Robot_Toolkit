/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;
using BH.oM.Structure.Elements;
using BH.Engine.Structure;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [PreviousVersion("3.2", "BH.Engine.External.Robot.Compute.GroupBarsByLength(System.Collections.Generic.IEnumerable<BH.oM.Structure.Elements.Bar>, System.Double)")]
        [Description("Groups bars by length, within a tolerance.")]
        [Input("bars", "The bars to group.")]
        [Input("tolerance", "Acceptable difference in length for each group")]
        [Output("barGroup", "The bars grouped, as a dictionary, with the key being the length and the value being the corresponding bars.")]
        public static Dictionary<double, List<Bar>> GroupBarsByLength(this IEnumerable<Bar> bars, double tolerance)
        {
            Dictionary<double, List<Bar>> dict = new Dictionary<double, List<Bar>>();
            foreach (var group in bars.GroupBy(x => (int)Math.Round(x.Length() / tolerance)))
            {
                dict[group.Key*tolerance] = group.ToList();
            }
            return dict;
        }

        /***************************************************/
    }
}
