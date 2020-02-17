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

using BH.oM.Geometry;
using BH.Engine.Geometry;
using RobotOM;
using System.Collections.Generic;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static RobotGeoPoint3D ToRobot(RobotApplication robotapp, Point point)
        {
            RobotGeoPoint3D robotPoint = robotapp.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_POINT_3D);
            robotPoint.Set(point.X, point.Y, point.Z);
            return robotPoint;                        
        }

        /***************************************************/

        public static RobotPointsArray ToRobot(Polyline segments)
        {
            RobotPointsArray contour = new RobotPointsArray();
            List<Point> pts = segments.ControlPoints;
            contour.SetSize(pts.Count);
            for (int i = 1; i <= pts.Count; i++)
            {
                contour.Set(i, pts[i - 1].X, pts[i - 1].Y, pts[i - 1].Z);
            }    
            return contour;
        }

        /***************************************************/

        public static RobotGeoContour ToRobot(RobotApplication robotapp, ICurve perimeter)
        {           
            RobotGeoContour contour = robotapp.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CONTOUR);
                         
            if (contour != null)
            {
                RobotGeoSegment segment = null;
                {
                    List<ICurve> segments = (List<ICurve>)perimeter.ISubParts();
                    for (int j = 0; j < segments.Count; j++)
                    {
                        if (segments[j] is Arc)
                        {
                            Arc bhomArc = segments[j] as Arc;
                            segment = robotapp.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_ARC);

                            RobotGeoArc arc = segment as RobotGeoArc;
                            Point start = bhomArc.StartPoint();
                            Point middle = bhomArc.PointAtParameter(0.5);
                            Point end = bhomArc.EndPoint();

                            arc.P1.Set(start.X, start.Y, start.Z);
                            arc.P2.Set(middle.X, middle.Y, middle.Z);
                            arc.P3.Set(end.X, end.Y, end.Z);
                            contour.Add(segment);
                        }
                        else
                        {
                            Line bhomLine = segments[j] as Line;
                            segment = robotapp.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_SEGMENT_LINE);
                            segment.P1.Set(bhomLine.Start.X, bhomLine.Start.Y, bhomLine.Start.Z);
                            contour.Add(segment);
                        }
                    }
                }
                                                            
            }
            return contour;
        }

        /***************************************************/

    }

}

