using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Adapters.Robot;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Create
    {
        public static RobotConfig RobotConfig(AdvancedSettings advancedSettings = null, DatabaseSettings databaseSettings = null)
        {
            RobotConfig robotConfig = new oM.Adapters.Robot.RobotConfig();
            if (advancedSettings != null)
                robotConfig.AdvancedSettings = advancedSettings;
            if (databaseSettings != null)
                robotConfig.DatabaseSettings = databaseSettings;

            return robotConfig;
        }
    }
}
