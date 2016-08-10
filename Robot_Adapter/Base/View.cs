using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structure;

namespace RobotToolkit
{
    public class View
    {
        public static void RefreshView(RobotApplication robot)
        {
            robot.Project.ViewMngr.Refresh();
        }
    }
}
