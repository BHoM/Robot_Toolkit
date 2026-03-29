/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

        [Description("Creates a DesignGroup defining a set of elements to be designed together in Robot.")]
        [Input("number", "The number identifier for the design group in Robot.")]
        [Input("materialName", "The name of the material assigned to the design group.")]
        [Input("elementIds", "The list of element identifiers belonging to this design group.")]
        [Output("designGroup", "The DesignGroup object for use with the Robot adapter.")]
        public static DesignGroup DesignGroup(int number = 0, string materialName = "", List<int> elementIds = null)
        {
            DesignGroup designGroup = new DesignGroup();
            designGroup.Number = number;
            designGroup.MaterialName = materialName;
            if (elementIds != null)
                designGroup.MemberIds = elementIds;

            return designGroup;
        }

        /***************************************************/
    }
}





