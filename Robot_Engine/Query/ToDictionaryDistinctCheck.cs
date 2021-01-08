/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Query
    {
        /***************************************************/
        /****             Public Methods                ****/
        /***************************************************/

        public static Dictionary<string, T> ToDictionaryDistinctCheck<T>(this IEnumerable<T> list, Func<T, string> selector)
        {
            var group = list.Where(x => x != null && selector(x) != null).GroupBy(selector);

            foreach (var item in group.Where(x => x.Count() > 1))
            {
                Engine.Reflection.Compute.RecordWarning("Duplicate label name found for the " + typeof(T).ToString() + " with the label name " + item.Key + ". First of the extracted values will be used");
            }

            return group.ToDictionary(x => x.Key, y => y.First());
        }

        /***************************************************/
    }
}


