/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
         
        public static void ToRobot(this PointAcceleration load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            //Replace null vectors with empty vector
            load.TranslationalAcceleration = load.TranslationalAcceleration ?? new oM.Geometry.Vector();
            load.RotationalAcceleration = load.RotationalAcceleration ?? new oM.Geometry.Vector();

            if (load.TranslationalAcceleration.Length() == 0)
            {
                Engine.Base.Compute.RecordWarning("Zero point accelerations are not pushed to Robot");
                return;
            }

            if (load.RotationalAcceleration.Length() != 0)
            {
                Engine.Base.Compute.RecordError("Rotational accelerations are not supported in Robot.");
                return;
            }

            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_ACCELERATION);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UX, load.TranslationalAcceleration.X);
            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UY, load.TranslationalAcceleration.Y);
            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UZ, load.TranslationalAcceleration.Z);

        }

        /***************************************************/
    }
}






