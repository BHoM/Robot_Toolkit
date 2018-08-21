using RobotOM;
using BH.oM.Structural.Elements;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void SetFEAType(IRobotBar rBar, Bar bhomBar)
        {
            if (bhomBar.FEAType == BarFEAType.CompressionOnly)
            {
                rBar.TensionCompression = IRobotBarTensionCompression.I_BTC_COMPRESSION_ONLY;
            }
            if (bhomBar.FEAType == BarFEAType.TensionOnly)
            {
                rBar.TensionCompression = IRobotBarTensionCompression.I_BTC_TENSION_ONLY;
            }
            if (bhomBar.FEAType == BarFEAType.Axial)
            {
                rBar.TrussBar = true;
            }
        }

        /***************************************************/

    }
}