using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit
{
    public class App
    {
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

        public static void StopCalculations(string FilePath = "LiveLing")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            robot.Project.CalcEngine.StopCalculation();
        }
    }
}
