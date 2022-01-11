/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using BH.Engine.Geometry;
using BH.oM.Structure.Loads;
using RobotOM;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void ToRobot(this AreaUniformTemperatureLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.TemperatureChange == 0)
            {
                Engine.Base.Compute.RecordWarning("Zero temperature loads are not pushed to Robot");
                return;
            }
            IRobotLoadRecordThermalIn3Points loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_THERMAL_IN_3_POINTS) as IRobotLoadRecordThermalIn3Points;
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotThermalIn3PointsRecordValues.I_3PRV_TX1, load.TemperatureChange);
            
        }

        /***************************************************/
    }
}



