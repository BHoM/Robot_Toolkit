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
using System;
using System.Collections.Generic;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static IRobotComponentType ToRobot(ICurve curve)
        {
            if (curve is Arc)
                return IRobotComponentType.I_CT_GEO_SEGMENT_ARC;

            if (curve is Line)
                return IRobotComponentType.I_CT_GEO_SEGMENT_LINE;

            else
                throw new Exception("Geometry is only valid for Line, Arc and Circle");
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

        public static void ToRobot(ICurve curve, RobotGeoObject contourGeo)
        {
            Circle bhomCircle = curve as Circle;
            RobotGeoCircle circle = contourGeo as RobotGeoCircle;
            Point bhomPoint1 = bhomCircle.IPointAtParameter(0);
            Point bhomPoint2 = bhomCircle.IPointAtParameter(0.33);
            Point bhomPoint3 = bhomCircle.IPointAtParameter(0.66);
            circle.P1.Set(bhomPoint1.X, bhomPoint1.Y, bhomPoint1.Z);
            circle.P2.Set(bhomPoint2.X, bhomPoint2.Y, bhomPoint2.Z);
            circle.P3.Set(bhomPoint3.X, bhomPoint3.Y, bhomPoint3.Z);
        }

        /***************************************************/

        public static RobotGeoSegment ToRobot(ICurve curve, RobotGeoSegment segment)
        {
            if (curve is Arc)
            {
                Arc bhomArc = curve as Arc;
                RobotGeoSegmentArc arc = segment as RobotGeoSegmentArc;
                Point start = bhomArc.StartPoint();
                Point middle = bhomArc.PointAtParameter(0.5);

                arc.P1.Set(start.X, start.Y, start.Z);
                arc.P2.Set(middle.X, middle.Y, middle.Z);
                return segment;
            }

            if (curve is Line)
            {
                Line bhomLine = curve as Line;
                segment.P1.Set(bhomLine.Start.X, bhomLine.Start.Y, bhomLine.Start.Z);
                return segment;
            }

            else
                return null;
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

