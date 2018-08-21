using BH.oM.Base;

namespace BH.oM.Adapters.Robot
{
    public class RobotConfig : BHoMObject
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        public AdvancedSettings AdvancedSettings { get; set; } = new AdvancedSettings();
        public DatabaseSettings DatabaseSettings { get; set; } = new DatabaseSettings();

        /***************************************************/
    }
}
