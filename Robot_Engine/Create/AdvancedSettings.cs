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
        public static AdvancedSettings AdvancedSettings(bool readNodesByQuery = false, bool readBarsByQuery = false)
        {
            AdvancedSettings advancedSettings = new AdvancedSettings();

            advancedSettings.readBarsByQuery = readBarsByQuery;
            advancedSettings.readNodesByQuery = readNodesByQuery;

            return advancedSettings;
        }     
    }
}
