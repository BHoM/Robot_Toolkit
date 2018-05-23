using BH.oM.Geometry;
using BH.oM.Common.Materials;
using BH.Engine.Reflection;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.Engine.Reflection;
using BH.oM.Reflection.Debuging;
using BH.oM.Adapters.Robot;


namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************/

        public static Point ToBHoMGeometry(RobotGeoPoint3D point)
        {
            return new Point { X = point.X, Y = point.Y, Z = point.Z };
        }

        public static Circle ToBHoMGeometry(RobotGeoCircle circle)
        {
            return (GeometryEngine.Create.Circle(ToBHoMGeometry(circle.P1 as dynamic), ToBHoMGeometry(circle.P2 as dynamic), ToBHoMGeometry(circle.P3 as dynamic)));
        }

        public static Arc ToBHoMGeometry(this RobotGeoArc arc)
        {
            return new Arc { Start = ToBHoMGeometry(arc.P1 as dynamic), Middle = ToBHoMGeometry(arc.P2 as dynamic), End = ToBHoMGeometry(arc.P3 as dynamic) };
        }

        public static Polyline ToBHoMGeometry(this RobotGeoPolyline polyline)
        {
            Polyline bhomPolyline = new Polyline();

            for (int j = 1; j <= polyline.Segments.Count; j++)
            {
                bhomPolyline.ControlPoints.Add(ToBHoMGeometry(polyline.Segments.Get(j).P1 as dynamic));
            }
            bhomPolyline.ControlPoints.Add(ToBHoMGeometry(polyline.Segments.Get(1).P1));

            return bhomPolyline;
        }

        public static PolyCurve ToBHoMGeometry(this RobotGeoContour contour)
        {            
            PolyCurve polycurve = new PolyCurve();
            RobotGeoSegment segment1, segment2;
            
            for (int j = 1; j <= contour.Segments.Count; j++)
            {
                segment1 = contour.Segments.Get(j) as RobotGeoSegment;
                segment2 = contour.Segments.Get(j % contour.Segments.Count + 1) as RobotGeoSegment;

                if (segment1.Type == IRobotGeoSegmentType.I_GST_ARC)
                {
                    polycurve.Curves.Add(ToBHoMGeometry(segment1 as dynamic));
                }
                else
                {
                    polycurve.Curves.Add(new Line { Start = ToBHoMGeometry(segment1.P1 as dynamic), End = ToBHoMGeometry(segment2.P1 as dynamic) });
                }
            }
            return polycurve;
        }     

    }
}
