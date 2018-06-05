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
        public static DatabaseSettings DatabaseSettings(MaterialDB materialDB = 0, 
                                                        SectionDB sectionDB = 0)
        {
            DatabaseSettings databaseSettings = new DatabaseSettings();
            databaseSettings.materialDatabase = materialDB;
            databaseSettings.sectionDatabase = sectionDB;
                      
            return databaseSettings;
        }

        public static DatabaseSettings DatabaseSettings(string materialDB = "British", 
                                                        string sectionDB = "UKST")
        {
            DatabaseSettings databaseSettings = new DatabaseSettings();

            MaterialDB mat_enum = 0;
            if (Enum.TryParse(materialDB, true, out mat_enum))
                databaseSettings.materialDatabase = (MaterialDB)mat_enum;

            SectionDB sec_enum = 0;
            if (Enum.TryParse(sectionDB, true, out sec_enum))
                databaseSettings.sectionDatabase = (SectionDB)sec_enum;

            return databaseSettings;
        }
    }
}
