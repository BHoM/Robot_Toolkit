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
        
        public static void ToRobot(this BarVaryingDistributedLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.ForceA.Length() == 0 && load.ForceB.Length() == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero distributed forces are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_TRAPEZOIDALE);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX1, load.ForceA.X);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY1, load.ForceA.Y);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ1, load.ForceA.Z);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_X1, load.DistanceFromA);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX2, load.ForceB.X);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY2, load.ForceB.Y);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ2, load.ForceB.Z);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_X2, load.DistanceFromB);
        }

        /***************************************************/
    }
}
