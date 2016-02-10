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
    /// General tools
    /// </summary>
    public class General
    {
        /// <summary>
        /// Refreshes the Robot view
        /// </summary>
        /// <param name="FilePath"></param>
        public static void RefreshView(string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            robot.Project.ViewMngr.Refresh();
        }

        /// <summary>
        /// Deletes all elements from a Robot structure
        /// </summary>
        /// <param name="FilePath"></param>
        public static void ClearStructure(string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();
            robot.Project.Structure.Clear();
        }

        /// <summary>
        /// Get free node, bar, object numbers from a Robot model (the next highest number)
        /// </summary>
        /// <param name="freeNodeNum"></param>
        /// <param name="freeBarNum"></param>
        /// <param name="freeObjNum"></param>
        /// <param name="FilePath"></param>
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
