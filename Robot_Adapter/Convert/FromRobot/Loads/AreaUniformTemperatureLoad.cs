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

        public static AreaUniformTemperatureLoad FromRobotAreaTempLoad(this IRobotLoadRecord loadRecord)
        {
            if (loadRecord == null)
                return null;

            try
            {
                double t1 = loadRecord.GetValue((short)IRobotThermalIn3PointsRecordValues.I_3PRV_TX1);
                double t2 = loadRecord.GetValue((short)IRobotThermalIn3PointsRecordValues.I_3PRV_TX2);
                double t3 = loadRecord.GetValue((short)IRobotThermalIn3PointsRecordValues.I_3PRV_TX3);

                double g1 = loadRecord.GetValue((short)IRobotThermalIn3PointsRecordValues.I_3PRV_TZ1);
                double g2 = loadRecord.GetValue((short)IRobotThermalIn3PointsRecordValues.I_3PRV_TZ2);
                double g3 = loadRecord.GetValue((short)IRobotThermalIn3PointsRecordValues.I_3PRV_TZ3);

                if (t2 != 0 || t2 != 0 || t3 != 0 || g1 != 0 || g2 != 0 || g3 != 0)
                {
                    Engine.Base.Compute.RecordWarning("The robot adapter currently only supports uniform temprature loads with no gradients. Area temprature load not pulled!");
                    return null;
                }

                return new AreaUniformTemperatureLoad
                {
                    TemperatureChange = t1,
                };
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /***************************************************/


    }
}



