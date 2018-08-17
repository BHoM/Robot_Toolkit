using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.oM.Adapters.Robot
{
    public class RobotConfig : BHoMObject
    {
        public AdvancedSettings AdvancedSettings { get; set; } = new AdvancedSettings();
        public DatabaseSettings DatabaseSettings { get; set; } = new DatabaseSettings();
    }
}
