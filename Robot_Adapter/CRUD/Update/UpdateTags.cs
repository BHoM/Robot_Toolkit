/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Adapter;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****             Public Methods                ****/
        /***************************************************/

        protected override int IUpdateTags(Type type, IEnumerable<object> ids, IEnumerable<HashSet<string>> newTags, ActionConfig actionConfig = null)
        {
            List<int> indecies = ids.Select(x => (int)x).ToList();
            if (indecies.Count < 1)
                return 0;

            return UpdateTags(type, indecies, newTags.ToList());
        }

        /***************************************************/
        /****             Private Methods               ****/
        /***************************************************/

        private int UpdateTags(Type t, List<int> indecies, List<HashSet<string>> tags)
        {
            Dictionary<int, HashSet<string>> typeTags = this.GetTypeTags(t);// = m_tags[t];

            for (int i = 0; i < indecies.Count; i++)
            {
                typeTags[indecies[i]] = tags[i];
            }
            
            return indecies.Count;
        }

        /***************************************************/

    }
}




