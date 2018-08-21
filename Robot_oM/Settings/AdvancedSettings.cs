using BH.oM.Base;

namespace BH.oM.Adapters.Robot
{
    public class AdvancedSettings : BHoMObject
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        public bool readBarsByQuery { get; set; } = false;
        public bool readNodesByQuery { get; set; } = false;

        /***************************************************/

    }
}
