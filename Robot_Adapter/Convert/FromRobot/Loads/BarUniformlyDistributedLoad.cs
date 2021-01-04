/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

using BH.oM.Geometry;
using BH.oM.Structure.Loads;
using RobotOM;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static BarUniformlyDistributedLoad FromRobotBarUDLForce(this IRobotLoadRecord loadRecord)
        {
            if (loadRecord == null)
                return null;

            try
            {
                double fx = loadRecord.GetValue((short)IRobotBarUniformRecordValues.I_BURV_PX);
                double fy = loadRecord.GetValue((short)IRobotBarUniformRecordValues.I_BURV_PY);
                double fz = loadRecord.GetValue((short)IRobotBarUniformRecordValues.I_BURV_PZ);
                double local = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM);
                double proj = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED);

                return new BarUniformlyDistributedLoad
                {
                    Force = new Vector { X = fx, Y = fy, Z = fz },
                    Projected = proj.FromRobotProjected(),
                    Axis = local.FromRobotLoadAxis()
                };
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /***************************************************/

        public static BarUniformlyDistributedLoad FromRobotBarUDLMoment(this IRobotLoadRecord loadRecord)
        {
            if (loadRecord == null)
                return null;

            try
            {
                double mx = loadRecord.GetValue((short)IRobotBarMomentDistributedRecordValues.I_BMDRV_MX);
                double my = loadRecord.GetValue((short)IRobotBarMomentDistributedRecordValues.I_BMDRV_MY);
                double mz = loadRecord.GetValue((short)IRobotBarMomentDistributedRecordValues.I_BMDRV_MZ);
                double local = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM);
                double proj = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED);

                return new BarUniformlyDistributedLoad
                {
                    Moment = new Vector { X = mx, Y = my, Z = mz },
                    Projected = proj.FromRobotProjected(),
                    Axis = local.FromRobotLoadAxis()
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


