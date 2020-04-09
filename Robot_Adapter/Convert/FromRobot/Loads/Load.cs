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

using System.Collections.Generic;
using System.Linq;
using RobotOM;
using BH.oM.Structure.Loads;
using BH.oM.Adapters.Robot;
using BH.oM.Geometry;
using BH.Engine.Geometry;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static LoadNature FromRobot(IRobotCaseNature nature)
        {
            switch (nature)
            {
                case IRobotCaseNature.I_CN_PERMANENT:
                    return LoadNature.Dead;
                case IRobotCaseNature.I_CN_EXPLOATATION:
                    return LoadNature.Live;
                case IRobotCaseNature.I_CN_SEISMIC:
                    return LoadNature.Seismic;
                case IRobotCaseNature.I_CN_SNOW:
                    return LoadNature.Snow;
                case IRobotCaseNature.I_CN_TEMPERATURE:
                    return LoadNature.Temperature;
                case IRobotCaseNature.I_CN_WIND:
                    return LoadNature.Wind;
                default:
                    return LoadNature.Other;
            }
        }

        /***************************************************/

        public static LoadAxis FromRobotLoadAxis(this double axisValue)
        {
            return axisValue == 1 ? LoadAxis.Local : LoadAxis.Global;
        }

        /***************************************************/

        public static bool FromRobotProjected(this double isProjectedValue)
        {
            return isProjectedValue == 1;
        }

        /***************************************************/
    }
}

