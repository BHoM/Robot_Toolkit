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
        public static DatabaseSettings DatabaseSettings(MaterialDB materialDB = MaterialDB.British, 
                                                        SectionDB sectionDB = SectionDB.UKST)
        {
            DatabaseSettings databaseSettings = new DatabaseSettings();
            databaseSettings.MaterialDatabase = materialDB;
            databaseSettings.SectionDatabase = sectionDB;
                      
            return databaseSettings;
        }

        public static DatabaseSettings DatabaseSettings(string materialDB = "British", 
                                                        string sectionDB = "UKST")
        {
            DatabaseSettings databaseSettings = new DatabaseSettings();

            MaterialDB mat_enum = 0;
            if (Enum.TryParse(materialDB, true, out mat_enum))
                databaseSettings.MaterialDatabase = (MaterialDB)mat_enum;

            SectionDB sec_enum = 0;
            if (Enum.TryParse(sectionDB, true, out sec_enum))
                databaseSettings.SectionDatabase = (SectionDB)sec_enum;

            return databaseSettings;
        }
    }
}
