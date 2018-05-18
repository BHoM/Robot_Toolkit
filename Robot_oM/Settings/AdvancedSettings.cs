using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.oM.Adapters.Robot
{
    public class AdvancedSettings : BHoMObject
    {        
        public bool readBarsByQuery { get; set; } = false;
        public bool readNodesByQuery { get; set; } = false;
    }
}
