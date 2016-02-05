using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit
{
    public class Loadcase
    {
        public static bool CreateLoadcases(BHoM.Structural.Loads.Loadcase[] loadcases, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            RobotCaseServer caseServer = robot.Project.Structure.Cases;

            caseServer.BeginMultiOperation();

            foreach (BHoM.Structural.Loads.Loadcase loadcase in loadcases)
            {

                if (caseServer.Exist(loadcase.Number) == 0)
                {
                    caseServer.CreateSimple(loadcase.Number, loadcase.Name, IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                }
            }

            caseServer.EndMultiOperation();

            return true;
        }
    }
}

