using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Modify
    {

        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<oM.Structure.Loads.ContourLoad> UpgradeVersion(this List<oM.Adapters.Robot.ContourLoad> loads)
        {
            List<oM.Structure.Loads.ContourLoad> upgradedLoads = new List<oM.Structure.Loads.ContourLoad>();

            foreach (oM.Adapters.Robot.ContourLoad load in loads)
                upgradedLoads.Add(load.UpgradeVersion());

            return upgradedLoads;
        }

        /***************************************************/

        public static oM.Structure.Loads.ContourLoad UpgradeVersion(this oM.Adapters.Robot.ContourLoad load)
        {
            return new oM.Structure.Loads.ContourLoad
            {
                Name = load.Name,
                Axis = load.Axis,
                Loadcase = load.Loadcase,
                Projected = load.Projected,
                Force = load.Force,
                Contour = load.Contour,
            };
        }

        /***************************************************/

        public static List<oM.Structure.Loads.GeometricalLineLoad> UpgradeVersion(this List<oM.Adapters.Robot.GeometricalLineLoad> loads)
        {
            List<oM.Structure.Loads.GeometricalLineLoad> upgradedLoads = new List<oM.Structure.Loads.GeometricalLineLoad>();

            foreach (oM.Adapters.Robot.GeometricalLineLoad load in loads)
                upgradedLoads.Add(load.UpgradeVersion());

            return upgradedLoads;
        }

        /***************************************************/

        public static oM.Structure.Loads.GeometricalLineLoad UpgradeVersion(this oM.Adapters.Robot.GeometricalLineLoad load)
        {
            return new oM.Structure.Loads.GeometricalLineLoad
            {
                Name = load.Name,
                Axis = load.Axis,
                Loadcase = load.Loadcase,
                Projected = load.Projected,
                ForceA = load.ForceA,
                ForceB = load.ForceB,
                MomentA = load.MomentA,
                MomentB = load.MomentB,
                Location = load.Location,
            };
        }

        /***************************************************/

    }
}
