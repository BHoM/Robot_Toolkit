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

using BH.Engine.Geometry;
using BH.oM.Geometry;
using RobotOM;
using BH.Engine.Adapter;
using System;
using System.Collections.Generic;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static RobotGeoObject ToRobot(List<ICurve> curves, RobotApplication robotApplication)
        {
            if (curves.Count == 1 && curves[0] is Circle)
            {
                RobotGeoObject circleContour = Convert.ToRobot(curves[0] as Circle, robotApplication) as RobotGeoObject;
                circleContour.Initialize();
                return circleContour;
            }
            else
            {
                RobotGeoContour contour = robotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CONTOUR);
                foreach (ICurve curve in curves)
                {
                    if (curve is Line)
                        contour.Add(Convert.ToRobot(curve as Line, robotApplication) as RobotGeoSegment);
                    else if (curve is Arc)
                        contour.Add(Convert.ToRobot(curve as Arc, robotApplication) as RobotGeoSegment);
                    else
                        BH.Engine.Reflection.Compute.RecordError("Only line, arc and circle curve geometry is supported for contours in Robot");
                }
                contour.Initialize();
                return contour as RobotGeoObject;
            }
        }

        /***************************************************/
    }
}

