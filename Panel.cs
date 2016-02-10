using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit
{
    /// <summary>
    /// Robot panel objects
    /// </summary>
    public class Panel
    {
        /// <summary>
        /// Get the first free panel number (by free object number)
        /// </summary>
        /// <param name="freePanelNum"></param>
        /// <param name="FilePath"></param>
        public static void GetFreePanelNumber(out int freePanelNum, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            freePanelNum = robot.Project.Structure.Objects.FreeNumber;
        }

        /// <summary>
        /// Get panel objects from Robot using the COM interface
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="coords"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool GetPanels(out int[] ids, out double[][,] coords, string FilePath = "LiveLink" )
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotSelection panel_sel = robot.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_PANEL);
            IRobotCollection panel_col = robot.Project.Structure.Objects.GetMany(panel_sel);
            panel_sel = null;
            List<int> _ids = new List<int>();
            List<double[,]> _coords = new List<double[,]>();
            List<RobotObjObject> _rpanels = new List<RobotObjObject>();

            robot.Project.Structure.Objects.BeginMultiOperation();

            for (int i = 1; i <= panel_col.Count; i++)
            {
                RobotObjObject rpanel = (RobotObjObject)panel_col.Get(i);
                _ids.Add(rpanel.Number);
                _rpanels.Add(rpanel);
                IRobotCollection edge_pnt_col = rpanel.Main.DefPoints;
                
                double[,] pointArray = new double[rpanel.Main.DefPoints.Count, 3];
                for (int j = 0; j < edge_pnt_col.Count; j++)
                {
                    RobotGeoPoint3D pnt = (RobotGeoPoint3D)edge_pnt_col.Get(j + 1);
                    pointArray[j, 0] = pnt.X;
                    pointArray[j, 1] = pnt.Y;
                    pointArray[j, 2] = pnt.Z;
                    pnt = null;
                }
                _coords.Add(pointArray);

                pointArray = null;
                edge_pnt_col = null;
                rpanel = null;
            }
            panel_col = null;
            
            robot.Project.Structure.Objects.EndMultiOperation();

            
            coords = _coords.ToArray();
            ids = _ids.ToArray();

            return true;
        }

        /// <summary>
        /// Get contour geometry from Robot using the COM interface
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="coords"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool GetContours(out int[] ids, out double[][,] coords, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotSelection selection = robot.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_GEOMETRY);
            IRobotCollection collection = robot.Project.Structure.Objects.GetMany(selection);

            List<int> _ids = new List<int>();
            List<double[,]> _coords = new List<double[,]>();
            List<RobotObjObject> _rcontours = new List<RobotObjObject>();

            robot.Project.Structure.Objects.BeginMultiOperation();

            for (int i = 0; i < collection.Count; i++)
            {
                RobotObjObject tempContour = (RobotObjObject)collection.Get(i + 1);

                if (tempContour.Main.Geometry.Type == IRobotGeoObjectType.I_GOT_CONTOUR)
                {
                    _ids.Add(tempContour.Number);
                    _rcontours.Add(tempContour);
                    IRobotCollection pnts = tempContour.Main.DefPoints;
                    double[,] pointArray = new double[pnts.Count, 3];
                    for (int j = 0; j < pnts.Count; j++)
                    {
                        RobotGeoPoint3D pnt = (RobotGeoPoint3D)pnts.Get(j + 1);
                        pointArray[j, 0] = pnt.X;
                        pointArray[j, 1] = pnt.Y;
                        pointArray[j, 2] = pnt.Z;
                    }
                    _coords.Add(pointArray);
                }
                else
                    continue;
            }
            robot.Project.Structure.Objects.EndMultiOperation();
            coords = _coords.ToArray();
            ids = _ids.ToArray();
            return true;
        }

        /// <summary>
        /// Create Robot panels
        /// </summary>
        /// <param name="PanelNumbers"></param>
        /// <param name="EdgePointCoords"></param>
        /// <param name="thicknessNames"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CreatePanels(List<int> PanelNumbers, List<double[,]> EdgePointCoords, List<string> thicknessNames, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            robot.Project.Structure.Objects.BeginMultiOperation();
            for (int i = 0; i < PanelNumbers.Count; i++)
            {
            RobotPointsArray ptArray = new RobotPointsArray();
            ptArray.SetSize(EdgePointCoords[i].GetLength(0));
            for (int j = 0; j < EdgePointCoords[i].GetLength(0); j++)
                ptArray.Set(j + 1, EdgePointCoords[i][j, 0], EdgePointCoords[i][j, 1], EdgePointCoords[i][j, 2]);
               
            robot.Project.Structure.Objects.CreateContour(PanelNumbers[i], ptArray);
            RobotSelection selection = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_OBJECT);

            selection.AddOne(PanelNumbers[i]);
            RobotObjObject rpanel = (RobotObjObject)robot.Project.Structure.Objects.Get(PanelNumbers[i]);

            rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_CALC_MODEL, "Shell");
            rpanel.Main.Attribs.Meshed = 1;
            rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, thicknessNames[i]);
                
            rpanel.StructuralType = IRobotObjectStructuralType.I_OST_UNDEFINED;

            rpanel.Initialize();
            }
            robot.Project.Structure.Objects.EndMultiOperation();
            return true;
        }

        /// <summary>
        /// Create Robot openings in panels
        /// </summary>
        /// <param name="PanelNumbers"></param>
        /// <param name="EdgePointCoords"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CreateOpenings(List<int> PanelNumbers, List<double[,]> EdgePointCoords, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            robot.Project.Structure.Objects.BeginMultiOperation();
            for (int i = 0; i < PanelNumbers.Count; i++)
            {
                RobotPointsArray ptArray = new RobotPointsArray();
                ptArray.SetSize(EdgePointCoords[i].GetLength(0));
                for (int j = 0; j < EdgePointCoords[i].GetLength(0); j++)
                    ptArray.Set(j + 1, EdgePointCoords[i][j, 0], EdgePointCoords[i][j, 1], EdgePointCoords[i][j, 2]);

                robot.Project.Structure.Objects.CreateContour(PanelNumbers[i], ptArray);
                RobotSelection selection = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_OBJECT);

                selection.AddOne(PanelNumbers[i]);
                RobotObjObject rcontour = (RobotObjObject)robot.Project.Structure.Objects.Get(PanelNumbers[i]);

                rcontour.Initialize();
            }
            robot.Project.Structure.Objects.EndMultiOperation();
            robot.Project.Structure.Objects.AutoRecalcHoles = true;
            return true;
        }

        /// <summary>
        /// Update panel geometry by redefinition of edge point coordinates
        /// </summary>
        /// <param name="PanelNumbers"></param>
        /// <param name="EdgePointCoords"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool UpdateGeometry(List<int> PanelNumbers, List<double[,]> EdgePointCoords, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            robot.Project.Structure.Objects.BeginMultiOperation();
            for (int i = 0; i < PanelNumbers.Count; i++)
            {
                RobotSelection selection = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_OBJECT);
                 selection.AddOne(PanelNumbers[i]);
                RobotObjObject rpanel = (RobotObjObject)robot.Project.Structure.Objects.Get(PanelNumbers[i]);

               // if (rpanel.Main.GetGeometry().Type == IRobotGeoObjectType.I_GOT_CONTOUR)
                //{
                    IRobotGeoContour contourGeometry = (IRobotGeoContour)rpanel.Main.GetGeometry();
                    contourGeometry.Clear();
                    
                    for (int j = 0; j < EdgePointCoords[i].GetLength(0); j++)
                    {
                        RobotGeoSegment segment = new RobotGeoSegment();
                        segment.P1.Set(EdgePointCoords[i][j, 0], EdgePointCoords[i][j, 1], EdgePointCoords[i][j, 2]);
                        contourGeometry.Add(segment);
                    }
                    contourGeometry.Initialize();
                    rpanel.Initialize();
               // }
         
            }
            robot.Project.Structure.Objects.EndMultiOperation();
            return true;
        }

        /// <summary>
        /// Delete panels in a Robot model
        /// </summary>
        /// <param name="selString"></param>
        /// <param name="FilePath"></param>
        public static void DeletePanels(string selString, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotSelection sel = robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);
            sel.AddText(selString);

            robot.Project.Structure.Bars.DeleteMany(sel);
        }


    }
}
