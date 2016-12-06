using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace Robot_Adapter
{
    /// <summary>
    /// Application class for Robot application controls
    /// </summary>
    public class App
    {
        /// <summary>
        /// Runs the Robot calculations engine
        /// </summary>
        /// <param name="FilePath"></param>
        public static void RunCalculations(string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication(); 
            robot.Visible = 1;
            robot.UserControl = true;
            robot.Interactive = 1;
            robot.Project.CalcEngine.AnalysisParams.IgnoreWarnings = true;
            robot.Project.CalcEngine.AnalysisParams.AutoVerification = IRobotStructureAutoVerificationType.I_SAVT_NONE;
            robot.Project.CalcEngine.Calculate();
            robot.Project.CalcEngine.AutoFreezeResults = false;
        }

        /// <summary>
        /// Stops the Robot calculation engine
        /// </summary>
        /// <param name="FilePath"></param>
        public static void StopCalculations(string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            robot.Project.CalcEngine.StopCalculation();
        }
    }
}
