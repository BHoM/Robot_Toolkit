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

using RobotOM;
using BH.oM.Structure.Elements;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using System;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static int ToRobotFlipPanelZ(this Vector normal)
        {
            //Tolerance is lower than any geometry tolerance used in the BHoM, hence defined here
            double tolerance = 1e-16;
            if (Math.Abs(normal.Z) > tolerance)
            {
                if (normal.Z < 0)
                    return 1;
            }
            else if (Math.Abs(normal.X) > tolerance)
            {
                if (normal.X < 0)
                    return 1;
            }
            else
            {
                if (normal.Y < 0)
                    return 1;
            }

            return 0;
        }

        /***************************************************/

    }
}
