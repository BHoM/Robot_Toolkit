using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit
{
    public class General
    {
        public static void RefreshView(string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            robot.Project.ViewMngr.Refresh();
        }

        public static void ClearStructure(string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            robot.Project.Structure.Clear();
        }

        public static void GetFreeNumbers(out int freeNodeNum, out int freeBarNum, out int freeObjNum, string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            freeNodeNum = robot.Project.Structure.Nodes.FreeNumber;
            freeBarNum = robot.Project.Structure.Bars.FreeNumber;
            freeObjNum = robot.Project.Structure.Objects.FreeNumber;
        }

    }
}
