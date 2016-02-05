using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit
{
    public class WindCFD
    {
        public static bool Generate(WindDirection windDir, WindParams windParams, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotSelection objSel = robot.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_PANEL);
            robot.Visible = 1;
            robot.UserControl = true;
            robot.Interactive = 1;

            IRobotWindLoadsSimulationParams robotWindParams = robot.Project.Structure.Cases.WindLoadsSimulationEngine.Params;
            robotWindParams.DirectionXNEnabled = windDir.x_negative;
            robotWindParams.DirectionXPEnabled = windDir.x_positive;
            robotWindParams.DirectionYNEnabled = windDir.y_negative;
            robotWindParams.DirectionYPEnabled = windDir.y_positive;
            robotWindParams.DirectionXNYNEnabled = windDir.quadrants;
            robotWindParams.DirectionXNYPEnabled = windDir.quadrants;
            robotWindParams.DirectionXPYNEnabled = windDir.quadrants;
            robotWindParams.DirectionXPYPEnabled = windDir.quadrants;
            robotWindParams.DeviationPercent = windParams.deviationPercent;
            robotWindParams.TerrainLevel = windParams.terrainLevel;
            robotWindParams.Velocity = windParams.velocity;
            robotWindParams.OpeningsClosed = windParams.openingsClosed;
            
            robotWindParams.Elements.Add(objSel);
            bool res = robot.Project.Structure.Cases.WindLoadsSimulationEngine.Generate();

            return true;
        }
    }

    public class WindDirection
    {
        public bool x_positive {get; private set;}
        public bool y_positive {get; private set;}
        public bool x_negative {get; private set;}
        public bool y_negative {get; private set;}
        public bool quadrants {get; private set;}
        
        public WindDirection()
        {
            this.SetDirection();
        }

        public void SetDirection(
            bool x_positive = true, 
            bool y_positive = true, 
            bool x_negative = false, 
            bool y_negative = false, 
            bool quadrants = false)
        {
            this.x_positive = x_positive;
            this.x_negative = x_negative;
            this.y_positive = y_positive;
            this.y_negative = y_negative;
            this.quadrants = quadrants;
        }
    }

    public class WindParams
    {
        public double deviationPercent { get; private set; }
        public double terrainLevel { get; private set; }
        public double velocity { get; private set; }
        public bool openingsClosed { get; private set; }

        public WindParams()
        {
            this.SetParameters();
        }

        public void SetParameters(
            double deviationPercent = 0.5, 
            double terrainLevel = 0, 
            double velocity = 24.385, 
            bool openingsClosed = true)
        {
            this.deviationPercent = deviationPercent;
            this.terrainLevel = terrainLevel;
            this.velocity = velocity;
            this.openingsClosed = openingsClosed;
        }
        }
    }
   
