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

        public static void ToRobot(this BarUniformlyDistributedLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            //Replace null vectors with empty vector
            load.Force = load.Force ?? new oM.Geometry.Vector();
            load.Moment = load.Moment ?? new oM.Geometry.Vector();

            if (load.Force.Length() == 0 && load.Moment.Length() == 0)
            {
                Engine.Reflection.Compute.RecordWarning("Zero forces and moments are not pushed to Robot");
                return;
            }

            if (load.Force.Length() != 0)
            {
                IRobotLoadRecord loadRecordForce = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM);
                loadRecordForce.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
                loadRecordForce.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PX, load.Force.X);
                loadRecordForce.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PY, load.Force.Y);
                loadRecordForce.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PZ, load.Force.Z);

                if (load.Axis == LoadAxis.Local)
                    loadRecordForce.SetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, 1);

                if (load.Projected)
                    loadRecordForce.SetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED, 1);
            }

            if (load.Moment.Length() != 0)
            {
                IRobotLoadRecord loadRecordMoment = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_MOMENT_DISTRIBUTED);
                loadRecordMoment.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
                loadRecordMoment.SetValue((short)IRobotBarMomentDistributedRecordValues.I_BMDRV_MX, load.Moment.X);
                loadRecordMoment.SetValue((short)IRobotBarMomentDistributedRecordValues.I_BMDRV_MY, load.Moment.Y);
                loadRecordMoment.SetValue((short)IRobotBarMomentDistributedRecordValues.I_BMDRV_MZ, load.Moment.Z);

                if (load.Axis == LoadAxis.Local)
                    loadRecordMoment.SetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, 1);

                if (load.Projected)
                    loadRecordMoment.SetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED, 1);
            }                      

        }

        /***************************************************/
    }
}



