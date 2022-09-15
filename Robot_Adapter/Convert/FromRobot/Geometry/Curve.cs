/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BH.oM.Geometry;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;


namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static PolyCurve FromRobot(this RobotGeoContour contour)
        {
            if (contour == null)
                return null;
                        
            PolyCurve polycurve = new PolyCurve();

            //Get out segment points. This is done first to ensure calls to get out contour as well as converting points is only done once.
            //Any call to the contour class is incredibly expensive, hence pre-processing to get out the points in order first saves a lot of computation power.
            List<List<Point>> segmentPoints = new List<List<Point>>();
            for (int j = 1; j <= contour.Segments.Count; j++)
            {
                RobotGeoSegment segment = contour.Segments.Get(j) as RobotGeoSegment;
                if (segment.Type == IRobotGeoSegmentType.I_GST_ARC)
                {
                    RobotGeoSegmentArc arcSeg = segment as RobotGeoSegmentArc;
                    segmentPoints.Add(new List<Point> { arcSeg.P1.FromRobot(), arcSeg.P2.FromRobot() });
                }
                else
                {
                    segmentPoints.Add(new List<Point> { segment.P1.FromRobot() });
                }
            }

            for (int j = 0; j < segmentPoints.Count; j++)
            {
                List<Point> currPts = segmentPoints[j];
                List<Point> nextPts = segmentPoints[(j + 1) % segmentPoints.Count];
                if (currPts.Count == 2) //Count of 2 for arcs, count of 1 for Lines
                {
                    ICurve segment;
                    try
                    {
                        segment = GeometryEngine.Create.Arc(currPts[0], currPts[1], nextPts[0]);    //Create arc from 2 point of current segment, and first point of next
                    }
                    catch (System.Exception)
                    {
                        segment = new Polyline { ControlPoints = new List<Point> { currPts[0], currPts[1], nextPts[0] } };
                        Engine.Base.Compute.RecordWarning("Failed to extract arc segment of polycurve as arc. Polyline segment returned in its place.");
                    }
                    polycurve.Curves.Add(segment);
                }
                else
                {
                    polycurve.Curves.Add(new Line { Start = currPts[0], End = nextPts[0] });    //Line from first of current points and last of next points
                }
            }
            return polycurve;
        }

        /***************************************************/

        public static Circle FromRobot(RobotGeoCircle circle)
        {
            return (GeometryEngine.Create.Circle(FromRobot(circle.P1), FromRobot(circle.P2), FromRobot(circle.P3)));
        }

        /***************************************************/

        public static Arc FromRobot(this IRobotGeoArc arc)
        {
            return GeometryEngine.Create.Arc(FromRobot(arc.P1), FromRobot(arc.P2), FromRobot(arc.P3));
        }

        /***************************************************/

        public static Polyline FromRobot(this RobotGeoPolyline polyline)
        {
            Polyline bhomPolyline = new Polyline();

            for (int j = 1; j <= polyline.Segments.Count; j++)
            {
                bhomPolyline.ControlPoints.Add(FromRobot(polyline.Segments.Get(j).P1));
            }
            bhomPolyline.ControlPoints.Add(FromRobot(polyline.Segments.Get(1).P1));

            return bhomPolyline;
        }

        /***************************************************/
        /**** Public Methods - Interface                ****/
        /***************************************************/

        public static ICurve IFromRobot(RobotGeoObject geoObject)
        {
            if (geoObject == null)
                return null;

            return FromRobot(geoObject as dynamic);
        }

        /***************************************************/
        private static ICurve FromRobot(object geoObject)
        {
            if (geoObject == null)
                return null;

            Engine.Base.Compute.RecordError($"Can not convert object of typ {geoObject.GetType().Name} to a Curve.");
            return null;
        }

        /***************************************************/
    }
}



