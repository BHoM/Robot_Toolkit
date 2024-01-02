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

        public static RobotGeoSegment ToRobot(Arc arc, RobotApplication robotApplication)
        {
            Type arcType = typeof(Arc);
            //Check inner properties are not null
            if (!RobotAdapter.CheckNotNull(arc.CoordinateSystem, oM.Base.Debugging.EventType.Error, arcType) ||
                !RobotAdapter.CheckNotNull(arc.CoordinateSystem.Origin, oM.Base.Debugging.EventType.Error, arcType) ||
                !RobotAdapter.CheckNotNull(arc.CoordinateSystem.X, oM.Base.Debugging.EventType.Error, arcType) ||
                !RobotAdapter.CheckNotNull(arc.CoordinateSystem.Y, oM.Base.Debugging.EventType.Error, arcType) ||
                !RobotAdapter.CheckNotNull(arc.CoordinateSystem.Z, oM.Base.Debugging.EventType.Error, arcType))
                return null;

            RobotGeoSegmentArc robotArc = robotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_SEGMENT_ARC);
            Point start = arc.StartPoint();
            Point middle = arc.PointAtParameter(0.5);
            robotArc.P1.Set(start.X, start.Y, start.Z);
            robotArc.P2.Set(middle.X, middle.Y, middle.Z);
            return robotArc as RobotGeoSegment;
        }

        /***************************************************/
    }

}





