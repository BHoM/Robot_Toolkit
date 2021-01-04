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
             
        public static void ToRobot(this PointDisplacement load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            //Replace null vectors with empty vector
            load.Translation = load.Translation ?? new oM.Geometry.Vector();
            load.Rotation = load.Rotation ?? new oM.Geometry.Vector();

            if (load.Translation.Length() == 0 && load.Rotation.Length() == 0)
            {
                Engine.Reflection.Compute.RecordWarning("Zero point displacements are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_DISPLACEMENT);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UX, load.Translation.X);
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UY, load.Translation.Y);
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UZ, load.Translation.Z);
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RX, load.Rotation.X);
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RY, load.Rotation.Y);
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RZ, load.Rotation.Z);
        }

        /***************************************************/
    }
}


