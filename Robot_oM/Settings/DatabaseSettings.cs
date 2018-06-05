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
        public MaterialDB materialDatabase { get; set; } = MaterialDB.British;
        public SectionDB sectionDatabase { get; set; } = SectionDB.UKST;
        public DesignCode_SteelFramingElements designCode_SteelFramingElements { get; set; } = DesignCode_SteelFramingElements.BS_EN_1993_1_2005_NA_2008_A1_2014;
    }
}
