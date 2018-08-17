using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Adapters.Robot;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Query
    {
        public static string GetStringFromEnum(DesignCode_Steel designCode)
        {
            switch (designCode)
            {
                case DesignCode_Steel.BS5950:
                    return "BS5950";
                case DesignCode_Steel.BS5950_2000:
                    return "BS 5950:2000";
                case DesignCode_Steel.BS_EN_1993_1_2005_NA_2008_A1_2014:
                    return "BS-EN 1993-1:2005/NA:2008/A1:2014";
                default:
                    return "BS-EN 1993-1:2005/NA:2008/A1:2014";
            }
        }

        public static string GetStringFromEnum(MaterialDB materialDB)
        {
            switch (materialDB)
            {
                case MaterialDB.American:
                    return "American";
                case MaterialDB.British:
                    return "British";
                default:
                    return "British";
            }
        }

        public static string GetStringFromEnum(SectionDB sectionDB)
        {
            switch (sectionDB)
            {
                case SectionDB.UKST:
                    return "UKST";
                case SectionDB.AISC:
                    return "AISC";
                default:
                    return "UKST";
            }
        }

    }
}
