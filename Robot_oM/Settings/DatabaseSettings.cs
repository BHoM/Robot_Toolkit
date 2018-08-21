using BH.oM.Base;

namespace BH.oM.Adapters.Robot
{
    public class DatabaseSettings : BHoMObject
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        public MaterialDB MaterialDatabase { get; set; } = MaterialDB.British;
        public SectionDB SectionDatabase { get; set; } = SectionDB.UKST;
        public DesignCode_Steel SteelDesignCode { get; set; } = DesignCode_Steel.Default;

        /***************************************************/

    }
}
