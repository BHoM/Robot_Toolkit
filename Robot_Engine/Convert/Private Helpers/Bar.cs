using BH.oM.Geometry;
using BH.oM.Common.Materials;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        public static void SetFEAType(RobotBar rBar, Bar bhomBar)
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
    }
}