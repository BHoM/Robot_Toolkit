using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Global;
using BHoM.Structural;
using RobotOM;
using BHoME = BHoM.Structural.Elements;
using BHoMP = BHoM.Structural.Properties;
using BHoML = BHoM.Structural.Loads;

namespace Robot_Adapter.Structural.Interface
{
    public partial class RobotAdapter : BHoM.Structural.Interface.IElementAdapter
    {
        private RobotApplication Robot;
        private string Settings;

        public string Filename
        {
            get
            {
                return Robot.Project.FileName;
            }
        }

        public RobotAdapter()
        {
            Robot = new RobotApplication();
        }

        /// <summary>
        /// Gets the bars in the robot model base on the input seleciton
        /// </summary>
        /// <param name="bars">output bar list</param>
        /// <param name="option"></param>
        /// <returns>true is successful</returns>
        public bool GetBars(out List<BHoME.Bar> bars, string option = "all")
        {
            Structural.Elements.BarIO.GetBars(Robot, out bars);
            return true;
        }

        /// <summary>
        /// Sets the bars in robot
        /// </summary>
        /// <param name="bars"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool CreateBars(List<BHoME.Bar> bars, out List<string> ids)
        {
            Structural.Elements.BarIO.CreateBars(Robot, bars, out ids);
            return true;
        }

        public bool GetLoadcases(out List<BHoML.ICase> cases)
        {
            throw new NotImplementedException();
        }

        public bool GetLoads(out List<BHoML.ILoad> loads, string option = "")
        {
            throw new NotImplementedException();
        }

        public bool GetNodes(out List<BHoME.Node> nodes, string option = "all")
        {
            Structural.Elements.NodeIO.GetNodes(Robot, out nodes, option);
            return true;
        }

        public bool GetPanels(out List<BHoME.Panel> panels, string option = "all")
        {
            return Structural.Elements.PanelIO.GetPanels(Robot, out panels);
        }


        public bool GetLevels(out List<BHoME.Storey> levels, string options = "")
        {
            throw new NotImplementedException();
        }

        public bool GetOpenings(out List<BHoME.Opening> opening, string option = "")
        {
            throw new NotImplementedException();
        }

        public bool SetNodes(List<BHoME.Node> nodes, out List<string> ids, string option = "")
        {
            Robot.Interactive = 0;
            Structural.Elements.NodeIO.CreateNodes(Robot, nodes, out ids);
            Robot.Interactive = 1;
            return true;
        }

        public bool SetBars(List<BHoME.Bar> bars, out List<string> ids, string option = "")
        {
            return Structural.Elements.BarIO.CreateBars(Robot, bars, out ids);
        }

        public bool SetPanels(List<BHoME.Panel> panels, out List<string> ids, string option = "")
        {
            return Structural.Elements.PanelIO.CreatePanels(Robot, panels, out ids);
        }

        public bool SetOpenings(List<BHoME.Opening> opening, out List<string> ids, string option = "")
        {
            return Structural.Elements.PanelIO.CreateOpenings(Robot, opening, out ids);
        }

        public bool SetLevels(List<BHoME.Storey> stories, out List<string> ids, string option = "")
        {
            ids = new List<string>();
            if (stories.Count > 0)
            {
                stories.Sort(delegate (BHoME.Storey s1, BHoME.Storey s2)
                {
                    return s1.Elevation.CompareTo(s2.Elevation);
                });

                double height = stories[1].Elevation - stories[0].Elevation;

                for (int i = 0; i < stories.Count; i++)
                {
                    Robot.Project.Structure.Storeys.Create2(stories[i].Name, stories[i].Elevation);
                    ids.Add(stories[i].Name);
                    // if (i < stories.Count - 1) height = stories[i + 1].Elevation - stories[i].Elevation;
                }
            }
            return true;
        }

        public bool SetLoads(List<BHoML.ILoad> loads, string option = "")
        {
            return Loads.LoadIO.SetLoads(Robot, loads);
        }

        public bool SetLoadcases(List<BHoML.ICase> cases)
        {
            return Loads.LoadIO.SetLoadcases(Robot, cases);
        }

        public bool GetGrids(out List<BHoME.Grid> grids, string options = "")
        {
            throw new NotImplementedException();
        }

        public bool SetGrids(List<BHoME.Grid> grid, out List<string> ids, string option = "")
        {
            RobotStructuralAxisGridMngr gm;
            IRobotStructuralAxisGrid sag;
            RobotStructuralAxisGridCartesian cartesianGrid = null;

            gm = Robot.Project.AxisMngr;

            if (gm.Count == 0)
            {
                gm.Create(IRobotStructuralAxisGridType.I_SAGT_CARTESIAN, "Structural Cartesian Axis");
            }

            sag = gm.Get(1);
            if (sag.Type == IRobotStructuralAxisGridType.I_SAGT_CARTESIAN)
            {
                cartesianGrid = gm.Get(1) as RobotStructuralAxisGridCartesian;
            }
            int xCounter = 1;
            int yCounter = 1;
            double xPrevious = 0;
            double yPrevious = 0;
            string xLabel = "";
            string yLabel = "";
            grid.Sort(delegate (BHoME.Grid g1, BHoME.Grid g2)
            {
                double d1 = g1.Plane.Normal.X * 1000 + g1.Plane.Origin.X + g1.Plane.Normal.Y + g1.Plane.Origin.Y;
                double d2 = g2.Plane.Normal.X * 1000 + g2.Plane.Origin.X + g2.Plane.Normal.Y + g2.Plane.Origin.Y;
                return d1.CompareTo(d2);
            });
            for (int i = 0; i < grid.Count; i++)
            {
                if (Math.Abs(grid[i].Plane.Normal.X) >= 0.99)
                {
                    if (xCounter == 1)
                    {
                        cartesianGrid.X.StartPosition = grid[i].Plane.Origin.X;
                        xPrevious = grid[i].Plane.Origin.X;
                        xCounter++;
                        xLabel = grid[i].Name;
                    }
                    else
                    {
                        cartesianGrid.X.AddSequence(grid[i].Plane.Origin.X - xPrevious, 1);
                        cartesianGrid.X.SetAxisLabel(xCounter++, grid[i].Name);
                        xPrevious = grid[i].Plane.Origin.X;
                    }
                }
                else if (Math.Abs(grid[i].Plane.Normal.Y) >= 0.99)
                {
                    if (yCounter == 1)
                    {
                        cartesianGrid.Y.StartPosition = grid[i].Plane.Origin.Y;
                        yPrevious = grid[i].Plane.Origin.Y;
                        yCounter++;
                        yLabel = grid[i].Name;
                    }
                    else
                    {
                        cartesianGrid.Y.AddSequence(grid[i].Plane.Origin.Y - yPrevious, 1);
                        cartesianGrid.Y.SetAxisLabel(yCounter++, grid[i].Name);
                        yPrevious = grid[i].Plane.Origin.Y;
                    }

                }
            }
            if (grid.Count > 0)
            {
                cartesianGrid.X.SetAxisLabel(1, xLabel);
                cartesianGrid.Y.SetAxisLabel(1, yLabel);
                cartesianGrid.Save();
            }

            ids = null;
            return true;
        }
    }
}
