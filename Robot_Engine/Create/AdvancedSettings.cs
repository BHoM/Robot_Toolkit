using BH.oM.Adapters.Robot;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        public static AdvancedSettings AdvancedSettings(bool readNodesByQuery = false, bool readBarsByQuery = false)
        {
            AdvancedSettings advancedSettings = new AdvancedSettings();

            advancedSettings.readBarsByQuery = readBarsByQuery;
            advancedSettings.readNodesByQuery = readNodesByQuery;

            return advancedSettings;
        }

        /***************************************************/
    }
}
