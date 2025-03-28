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
       
        public static void ToRobot(this BarPointLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            //Replace null vectors with empty vector
            load.Force = load.Force ?? new oM.Geometry.Vector();
            load.Moment = load.Moment ?? new oM.Geometry.Vector();

            if (load.Force.Length() == 0 && load.Moment.Length() == 0)
            {
                Engine.Base.Compute.RecordWarning("Zero forces and moments are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_FORCE_CONCENTRATED);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FX, load.Force.X);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FY, load.Force.Y);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FZ, load.Force.Z);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CX, load.Moment.X);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CY, load.Moment.Y);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CZ, load.Moment.Z);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_X, load.DistanceFromA);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_REL, 0);

            if (load.Axis == LoadAxis.Local)
                loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_LOC, 1);

        }

        /***************************************************/
    }
}






