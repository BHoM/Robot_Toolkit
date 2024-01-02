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

using BH.Engine.Geometry;
using BH.oM.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static RobotGeoCircle ToRobot(Circle circle, RobotApplication robotApplication)
        {
            if (!RobotAdapter.CheckNotNull(circle.Centre, oM.Base.Debugging.EventType.Error, typeof(Circle)) ||
                !RobotAdapter.CheckNotNull(circle.Normal, oM.Base.Debugging.EventType.Error, typeof(Circle)))
                return null;

            RobotGeoCircle robotCircle = robotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CIRCLE);
            Point point1 = circle.IPointAtParameter(0);
            Point point2 = circle.IPointAtParameter(0.33);
            Point point3 = circle.IPointAtParameter(0.66);
            robotCircle.P1.Set(point1.X, point1.Y, point1.Z);
            robotCircle.P2.Set(point2.X, point2.Y, point2.Z);
            robotCircle.P3.Set(point3.X, point3.Y, point3.Z);
            return robotCircle;
        }

        /***************************************************/

    }

}





