
using RobotOM;

namespace RobotToolkit
{
    /// <summary>
    /// Wind CFD controller object
    /// </summary>
    public class WindCFD
    {
        /// <summary>
        /// Generate wind CFD analysis
        /// </summary>
        /// <param name="windDir"></param>
        /// <param name="windParams"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
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

    /// <summary>
    /// Wind directions
    /// </summary>
    public class WindDirection
    {
        /// <summary>Wind in positive X direction</summary>
        public bool x_positive {get; private set;}
        /// <summary>Wind in positive Y direction</summary>
        public bool y_positive {get; private set;}
        /// <summary>Wind in negative X direction</summary>
        public bool x_negative {get; private set;}
        /// <summary>Wind in negative Y direction</summary>
        public bool y_negative {get; private set;}
        /// <summary>Wind in quadrants direction</summary>
        public bool quadrants {get; private set;}
        
        /// <summary>
        /// Construct a wind directions object
        /// </summary>
        public WindDirection()
        {
            this.SetDirection();
        }

        /// <summary>
        /// Set wind directions
        /// </summary>
        /// <param name="x_positive"></param>
        /// <param name="y_positive"></param>
        /// <param name="x_negative"></param>
        /// <param name="y_negative"></param>
        /// <param name="quadrants"></param>
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

    /// <summary>
    /// Wind parameters for CFD analysis
    /// </summary>
    public class WindParams
    {
        /// <summary>Deviation of the solution</summary>
        public double deviationPercent { get; private set; }
        /// <summary>Terrain level for wind modelling</summary>
        public double terrainLevel { get; private set; }
        /// <summary>Wind velocity</summary>
        public double velocity { get; private set; }
        /// <summary>Panel openings assumed closed or open</summary>
        public bool openingsClosed { get; private set; }

        /// <summary>
        /// Wind parameters constructor
        /// </summary>
        public WindParams()
        {
            this.SetParameters();
        }

        /// <summary>
        /// Set wind parameters
        /// </summary>
        /// <param name="deviationPercent"></param>
        /// <param name="terrainLevel"></param>
        /// <param name="velocity"></param>
        /// <param name="openingsClosed"></param>
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
   
