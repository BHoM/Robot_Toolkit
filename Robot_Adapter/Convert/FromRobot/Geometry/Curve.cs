/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
            RobotGeoSegment segment1, segment2;
            
            for (int j = 1; j <= contour.Segments.Count; j++)
            {
                segment1 = contour.Segments.Get(j) as RobotGeoSegment;
                segment2 = contour.Segments.Get(j % contour.Segments.Count + 1) as RobotGeoSegment;
                if (segment1.Type == IRobotGeoSegmentType.I_GST_ARC)
                {
                    RobotGeoSegmentArc arcSeg = segment1 as RobotGeoSegmentArc;

                    BH.oM.Geometry.Arc bhomArc = GeometryEngine.Create.Arc(arcSeg.P1.FromRobot(), arcSeg.P2.FromRobot(), segment2.P1.FromRobot());
                    polycurve.Curves.Add(bhomArc);
                }
                else
                {
                    polycurve.Curves.Add(new Line { Start = FromRobot(segment1.P1), End = FromRobot(segment2.P1) });
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

            Engine.Reflection.Compute.RecordError($"Can not convert object of typ {geoObject.GetType().Name} to a Curve.");
            return null;
        }

        /***************************************************/
    }
}


