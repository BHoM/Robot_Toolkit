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
using BH.oM.Structure.Loads;
using RobotOM;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/      
       
        public static void ToRobot(this GravityLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            int count = 0;
            if (load.GravityDirection.X != 0)
            {
                count++;
                IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_DEAD);
                loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
                loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_X, load.GravityDirection.X.Sign());
                loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_COEFF, Math.Abs(load.GravityDirection.X));
            }

            if (load.GravityDirection.Y != 0)
            {
                count++;
                IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_DEAD);
                loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
                loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Y, load.GravityDirection.Y.Sign());
                loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_COEFF, Math.Abs(load.GravityDirection.Y));
            }

            if (load.GravityDirection.Z != 0)
            {
                count++;
                IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_DEAD);
                loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
                loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Z, load.GravityDirection.Z.Sign());
                loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_COEFF, Math.Abs(load.GravityDirection.Z));
            }

            if (count > 1)
            {
                Engine.Reflection.Compute.RecordNote("Gravity load split into its components.");
            }
            else if (count == 0)
            {
                Engine.Reflection.Compute.RecordWarning("Gravity loads with no Gravity direction is not pushed to Robot.");
            }
        }

        /***************************************************/

        private static int Sign(this double d)
        {
            if (d < 0)
                return -1;
            else if (d > 0)
                return 1;
            else
                return 0;
        }

        /***************************************************/

    }
}

