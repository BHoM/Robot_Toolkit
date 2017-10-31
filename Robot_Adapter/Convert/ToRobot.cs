using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.Structural.Properties;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.Adapter.Robot
{    
    public static partial class Convert
    {
        /***************************************/

        #region Object Converters

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
                    List<ICurve> segments = (List<ICurve>)perimeter.IGetExploded();
                    for (int j = 0; j < segments.Count; j++)
                    {
                        if (segments[j] is Arc)
                        {
                            Arc bhomArc = segments[j] as Arc;
                            segment = robotapp.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_ARC);

                            RobotGeoArc arc = segment as RobotGeoArc;
                            arc.P1.Set(bhomArc.Start.X, bhomArc.Start.Y, bhomArc.Start.Z);
                            arc.P2.Set(bhomArc.Middle.X, bhomArc.Middle.Y, bhomArc.Middle.Z);
                            arc.P3.Set(bhomArc.End.X, bhomArc.End.Y, bhomArc.End.Z);
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

        #endregion 

        #region Property Converters

        public static IRobotBarSectionType FromBHoMEnum(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Angle:
                    return IRobotBarSectionType.I_BST_NS_L;
                case ShapeType.Box:
                    return IRobotBarSectionType.I_BST_NS_BOX;
                case ShapeType.Channel:
                    return IRobotBarSectionType.I_BST_NS_C;
                case ShapeType.Circle:
                    return IRobotBarSectionType.I_BST_NS_C;
                case ShapeType.DoubleAngle:
                    return IRobotBarSectionType.I_BST_NS_LP;
                case ShapeType.ISection:
                    return IRobotBarSectionType.I_BST_NS_I;
                case ShapeType.Rectangle:
                    return IRobotBarSectionType.I_BST_NS_RECT;
                case ShapeType.Tee:
                    return IRobotBarSectionType.I_BST_NS_T;
                case ShapeType.Tube:
                case ShapeType.Cable:
                    return IRobotBarSectionType.I_BST_NS_TUBE;
                case ShapeType.Polygon:
                    return IRobotBarSectionType.I_BST_NS_POLYGONAL;
                default:
                    return IRobotBarSectionType.I_BST_NS_RECT;

            }
        }
        #endregion

        #region List Converters

        internal static string FromSelectionList(List<int> numbers)
        {
            string selection = "";
            for (int i = 0; i < numbers.Count; i++)
            {
                selection += numbers[i] + " ";
            }
            return selection.Trim();
            
        }

        internal static string FromSelectionList(List<string> ids)
        {
            string selection = "";
            for (int i = 0; i < ids.Count; i++)
            {
                selection += ids[i] + " ";
            }
            return selection.Trim();
        }

        internal static string FromSelectionList<T>(IEnumerable<T> objects) where T : IObject
        {
            string selection = "";
            foreach (T obj in objects)
            {
                object objNumber = null;
                obj.CustomData.TryGetValue("Robot Number", out objNumber);                
                selection += objNumber.ToString() + " ";
            }
            return selection.Trim();
        }

        #endregion


    }

}
