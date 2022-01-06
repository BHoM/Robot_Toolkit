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
       
        public static void ToRobot(this AreaUniformlyDistributedLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            //Replace null vector with empty vector
            load.Pressure = load.Pressure ?? new oM.Geometry.Vector();

            if (load.Pressure.Length() == 0)
            {
                Engine.Reflection.Compute.RecordWarning("Zero pressures are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_UNIFORM);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PX, load.Pressure.X);
            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PY, load.Pressure.Y);
            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PZ, load.Pressure.Z);

            if (load.Axis == LoadAxis.Local)
                loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, 1);

            if(load.Projected)
                loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED, 1);
        }

        /***************************************************/
    }
}



