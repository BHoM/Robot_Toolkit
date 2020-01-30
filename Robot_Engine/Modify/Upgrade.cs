/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

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
