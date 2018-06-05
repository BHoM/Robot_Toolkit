using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.oM.Adapters.Robot
{
    public class DatabaseSettings : BHoMObject
    {
        public MaterialDB MaterialDatabase { get; set; } = MaterialDB.British;
        public SectionDB SectionDatabase { get; set; } = SectionDB.UKST;
        public DesignCode_Steel SteelDesignCode { get; set; } 
    }
}
