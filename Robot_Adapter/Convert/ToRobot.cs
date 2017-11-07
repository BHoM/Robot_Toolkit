using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Design;
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
            
        public static List<RobotNode> FromBHoMObjects(RobotAdapter robotAdapter, List<Node> bhomNodes)
        {
            List<RobotNode> robotNodes = new List<RobotNode>();
            RobotApplication robot = robotAdapter.RobotApplication;
            RobotStructureCache rcache = robot.Project.Structure.CreateCache();
            RobotSelection nodeSel = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            foreach (Node bhomNode in bhomNodes)
            {
                int nodeNum = 0;
                int.TryParse(bhomNode.CustomData[robotAdapter.AdapterId].ToString(), out nodeNum);
                rcache.AddNode(nodeNum, bhomNode.Point.X, bhomNode.Point.Y, bhomNode.Point.Z);
                bhomNode.CustomData[robotAdapter.AdapterId] = nodeNum;
                nodeSel.AddText(nodeNum.ToString());
            }
            robot.Project.Structure.ApplyCache(rcache);
            IRobotCollection robotNodeCol = robot.Project.Structure.Nodes.GetMany(nodeSel);
            for(int i = 1; i <= robotNodeCol.Count; i++)
            {
                robotNodes.Add(robotNodeCol.Get(i));
            }
            return robotNodes;
         }

        public static List<RobotBar> FromBHoMObjects(RobotAdapter robotAdapter, List<Bar> bhomBars)
        {
            List<RobotBar> robotBars = new List<RobotBar>();
            RobotApplication robot = robotAdapter.RobotApplication;
            RobotStructureCache rcache = robot.Project.Structure.CreateCache();
            RobotSelection barSel = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            string key = robotAdapter.AdapterId;
            foreach (Bar bhomBar in bhomBars)
            {
                
                int barNum = 0;
                int.TryParse(bhomBar.CustomData[key].ToString(), out barNum);
                rcache.AddBar(barNum, 
                              System.Convert.ToInt32(bhomBar.StartNode.CustomData[key]), 
                              System.Convert.ToInt32(bhomBar.EndNode.CustomData[key]),
                              "UC 305x305x97",
                              //bhomBar.SectionProperty.Name, 
                              "STEEL",
                              //bhomBar.SectionProperty.Material.Name, 
                              bhomBar.OrientationAngle);
                bhomBar.CustomData[robotAdapter.AdapterId] = barNum;
                barSel.AddText(barNum.ToString());
            }
            robot.Project.Structure.ApplyCache(rcache);
            IRobotCollection robotBarCol = robot.Project.Structure.Bars.GetMany(barSel);
            for (int i = 1; i <= robotBarCol.Count; i++)
            {
                robotBars.Add(robotBarCol.Get(i));
            }

            return robotBars;
        }

        public static List<RDimGroup> FromBHoMObjects(RobotAdapter robotAdapter, List<DesignGroup> bhomdesignGroups)
        {
            List<RDimGroup> robotSteelDesignGroups = new List<RDimGroup>();
            foreach (DesignGroup bhomdesignGroup in bhomdesignGroups)
            {
                RobotApplication robot = robotAdapter.RobotApplication;
                RDimServer RDServer = robotAdapter.RobotApplication.Kernel.GetExtension("RDimServer");
                RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
                RDimStream RDStream = RDServer.Connection.GetStream();
                RDimGroups RDGroups = RDServer.GroupsService;
                RDimGrpProfs RDGroupProfs = RDServer.Connection.GetGrpProfs();
                RDimGroup designGroup = RDGroups.New(0, bhomdesignGroup.Number);
                designGroup.Name = bhomdesignGroup.Name;
                designGroup.Material = bhomdesignGroup.MaterialName;
                RDStream.Clear();
                RDStream.WriteText(Convert.FromSelectionList(bhomdesignGroup.MemberIds));
                designGroup.SetMembList(RDStream);
                designGroup.SetProfs(RDGroupProfs);
                RDGroups.Save(designGroup);
                robotSteelDesignGroups.Add(designGroup);
            }
            return robotSteelDesignGroups;
        }

        #endregion

        #region Geometry Converters

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
