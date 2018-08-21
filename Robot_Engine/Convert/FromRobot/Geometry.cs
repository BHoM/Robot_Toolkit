using BH.oM.Geometry;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;


namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static Point ToBHoMGeometry(this RobotGeoPoint3D point)
        {
            return new Point { X = point.X, Y = point.Y, Z = point.Z };
        }

        /***************************************************/

        public static Circle ToBHoMGeometry(RobotGeoCircle circle)
        {
            return (GeometryEngine.Create.Circle(ToBHoMGeometry(circle.P1 as dynamic), ToBHoMGeometry(circle.P2 as dynamic), ToBHoMGeometry(circle.P3 as dynamic)));
        }

        /***************************************************/

        public static Arc ToBHoMGeometry(this IRobotGeoArc arc)
        {
            return GeometryEngine.Create.Arc(ToBHoMGeometry(arc.P1 as dynamic), ToBHoMGeometry(arc.P2 as dynamic), ToBHoMGeometry(arc.P3 as dynamic));
        }

        /***************************************************/

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

        /***************************************************/

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
                    RobotGeoSegmentArc arcSeg = segment1 as RobotGeoSegmentArc;

                    BH.oM.Geometry.Arc bhomArc = GeometryEngine.Create.Arc(arcSeg.P1.ToBHoMGeometry(), arcSeg.P2.ToBHoMGeometry(), segment2.P1.ToBHoMGeometry());
                    polycurve.Curves.Add(bhomArc);
                }
                else
                {
                    polycurve.Curves.Add(new Line { Start = ToBHoMGeometry(segment1.P1 as dynamic), End = ToBHoMGeometry(segment2.P1 as dynamic) });
                }
            }
            return polycurve;
        }

        /***************************************************/
    }
}
