/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.oM.Structure.MaterialFragments;
using RobotOM;
using BH.Engine.Adapter;
using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Offsets;
using BH.oM.Adapters.Robot;
using BH.Engine.Structure;
using BH.oM.Geometry;
using BH.Engine.Geometry;


namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static bool FromRobotCheckFlipNormal(Vector normal, bool flip)
        {
            double tolerance = 1e-16;

            if (Math.Abs(normal.Z) > tolerance)
            {
                if (normal.Z < 0)
                    flip = !flip;
            }
            else if (Math.Abs(normal.X) > tolerance)
            {
                if (normal.X < 0)
                    flip = !flip;
            }
            else
            {
                if (normal.Y < 0)
                    flip = !flip;
            }

            return flip;
        }

        /***************************************************/
    }

}

