using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Design;
using RobotOM;
using System.Collections.Generic;
using BH.oM.Base;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************/

        public static RobotGeoPoint3D FromBHoMGeometry(RobotApplication robotapp, Point point)
        {
            RobotGeoPoint3D robotPoint = robotapp.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_POINT_3D);
            robotPoint.Set(point.X, point.Y, point.Z);
            return robotPoint;                        
        }
        
        public static RobotPointsArray FromBHoMGeometry(Polyline segments)
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

        public static RobotGeoContour FromBHoMGeometry(RobotApplication robotapp, ICurve perimeter)
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
    }

}
