/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
using BH.oM.Base;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Reflection.Attributes;

namespace BH.oM.Adapters.Robot
{
    [Deprecated("3.0", "Superseded by BH.oM.Structure.Loads.ContourLoad")]
    public class ContourLoad : BHoMObject, ILoad
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        public LoadAxis Axis { get; set; } = LoadAxis.Global;

        public Loadcase Loadcase { get; set; } = null;

        public bool Projected { get; set; } = false;

        public Vector Force { get; set; } = new Vector();

        public Polyline Contour { get; set; } = null;

        /***************************************************/
    }
}
