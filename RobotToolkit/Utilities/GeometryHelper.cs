using BHoM.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotToolkit
{
    public class GeometryHelper
    {
        internal static Point CPoint(RobotGeoPoint3D point)
        {
            return new Point(point.X, point.Y, point.Z);
        }

        internal static Point CPoint(RobotNode point)
        {
            return new Point(point.X, point.Y, point.Z);
        }

        internal static void SetNode(RobotNode node, Point p)
        {
            node.X = Math.Round(p.X, 3);
            node.Y = Math.Round(p.Y, 3);
            node.Z = Math.Round(p.Z, 3);
        }

        internal static void SetGeoPoint(IRobotGeoPoint3D point, Point p)
        {
            point.Set(Math.Round(p.X, 3), Math.Round(p.Y, 3), Math.Round(p.Z, 3));
        }

        internal static Arc CArc(RobotGeoArc rArc)
        {
            return new Arc(CPoint(rArc.P1), CPoint(rArc.P2), CPoint(rArc.P3));
        }

        internal static Arc CArc(RobotGeoPoint3D p1, RobotGeoPoint3D p2, RobotGeoPoint3D p3)
        {
            return new Arc(CPoint(p1), CPoint(p2), CPoint(p3));
        }

        internal static Line CLine(RobotGeoPoint3D p1, RobotGeoPoint3D p2)
        {
            return new Line(CPoint(p1), CPoint(p2));
        }

        internal static Line CLine(RobotNode p1, RobotNode p2)
        {
            return new Line(CPoint(p1), CPoint(p2));
        }

        internal static Circle CCircle(RobotGeoPoint3D p1, RobotGeoPoint3D p2, RobotGeoPoint3D p3)
        {
            return new Circle(CPoint(p1), CPoint(p2), CPoint(p3));
        }

        internal static GeometryBase CGeoObject(RobotObjObject obj)
        {
            switch (obj.Main.Geometry.Type)
            {
                case IRobotGeoObjectType.I_GOT_ARC:
                    RobotGeoArc rArc = (obj.Main.GetGeometry() as RobotGeoArc);
                    return CArc(rArc);
                case IRobotGeoObjectType.I_GOT_CIRCLE:
                    RobotGeoCircle rCircle = obj.Main.GetGeometry() as RobotGeoCircle;
                    return CCircle(rCircle.P1, rCircle.P2, rCircle.P3);
                case IRobotGeoObjectType.I_GOT_CONTOUR:
                    RobotGeoSegmentCollection segments
                        = (obj.Main.GetGeometry() as RobotGeoContour).Segments as RobotGeoSegmentCollection;
                    RobotGeoSegment segment1;
                    RobotGeoSegment segment2;
                    List<Curve> curves = new List<Curve>();

                    for (int j = 1; j <= segments.Count; j++)
                    {
                        segment1 = segments.Get(j) as RobotGeoSegment;
                        segment2 = segments.Get(j % segments.Count + 1) as RobotGeoSegment;

                        if (segment1.Type == IRobotGeoSegmentType.I_GST_ARC)
                        {
                            RobotGeoPoint3D pOnArc = (segment1 as RobotGeoSegmentArc).P2;

                            Curve c = CArc(segment1.P1, pOnArc, segment2.P1);
                            curves.Add(c);
                        }
                        else
                        {
                            curves.Add(CLine(segment1.P1, segment2.P1));
                        }
                    }
                    return new Group<Curve>(curves);

                case IRobotGeoObjectType.I_GOT_POLYLINE:
                    segments = (obj.Main.GetGeometry() as RobotGeoPolyline).Segments as RobotGeoSegmentCollection;

                    List<Curve> polyline = new List<Curve>();

                    for (int j = 1; j <= segments.Count; j++)
                    {
                        segment1 = segments.Get(j) as RobotGeoSegment;
                        segment2 = segments.Get((j + 1) % segments.Count) as RobotGeoSegment;

                        if (segment1.Type == IRobotGeoSegmentType.I_GST_ARC)
                        {
                            RobotGeoPoint3D pOnArc = (segment1 as RobotGeoSegmentArc).P2;

                            Curve c = CArc(segment1.P1, pOnArc, segment2.P1);
                            polyline.Add(c);
                        }
                        else
                        {
                            polyline.Add(CLine(segment1.P1, segment2.P1));
                        }
                    }
                    return new Group<Curve>(polyline);
                case IRobotGeoObjectType.I_GOT_INTERSECTION:
                default:
                    return null;
            }
        }

        internal static RobotGeoContour CreateContour(RobotApplication robot, List<Curve> segments)
        {           
            RobotGeoContour contour = robot.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CONTOUR);
                         
            if (contour != null)
            {
                RobotGeoSegment segment = null;
                for (int j = 0; j < segments.Count; j++)
                {
                    if (segments[j] is Arc)
                    {
                        Arc bhomArc = segments[j] as Arc;
                        segment = robot.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_ARC);
                        RobotGeoArc arc = segment as RobotGeoArc;
                        arc.P1.Set(segments[j].StartPoint.X, segments[j].StartPoint.Y, segments[j].StartPoint.Z);
                        arc.P2.Set(bhomArc.MiddlePoint.X, bhomArc.MiddlePoint.Y, bhomArc.MiddlePoint.Z);
                        arc.P3.Set(segments[j].EndPoint.X, segments[j].EndPoint.Y, segments[j].EndPoint.Z);
                    }
                    else
                    {
                        segment = robot.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_SEGMENT_LINE);
                        segment.P1.Set(segments[j].StartPoint.X, segments[j].StartPoint.Y, segments[j].StartPoint.Z);                        
                    }
                    contour.Add(segment);            
                   
                }
                int counter = segments.Count;            
            }
            contour.Initialize();
            return contour;
        }    
    }
}
