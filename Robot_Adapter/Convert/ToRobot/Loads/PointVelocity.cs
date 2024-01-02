/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
         
        public static void ToRobot(this PointVelocity load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            //Replace null vectors with empty vector
            load.TranslationalVelocity = load.TranslationalVelocity ?? new oM.Geometry.Vector();
            load.RotationalVelocity = load.RotationalVelocity ?? new oM.Geometry.Vector();

            if (load.TranslationalVelocity.Length() == 0)
            {
                Engine.Base.Compute.RecordWarning("Zero point velocities are not pushed to Robot");
                return;
            }

            if (load.RotationalVelocity.Length() != 0)
            {
                Engine.Base.Compute.RecordError("Rotational velocities are not yet supported in the Robot Adapter.");
                return;
            }

            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_VELOCITY);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UX, load.TranslationalVelocity.X);
            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UY, load.TranslationalVelocity.Y);
            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UZ, load.TranslationalVelocity.Z);
        }

        /***************************************************/
    }
}





