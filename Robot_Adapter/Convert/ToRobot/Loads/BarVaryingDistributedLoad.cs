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
using BH.Engine.Adapters.Robot;
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
            //Replace null vectors with empty vector
            load.ForceAtStart = load.ForceAtStart ?? new oM.Geometry.Vector();
            load.ForceAtEnd = load.ForceAtEnd ?? new oM.Geometry.Vector();
            load.MomentAtStart = load.MomentAtStart ?? new oM.Geometry.Vector();
            load.MomentAtEnd = load.MomentAtEnd ?? new oM.Geometry.Vector();

            if (load.ForceAtStart.Length() == 0 && load.ForceAtEnd.Length() == 0)
            {
                Engine.Base.Compute.RecordWarning("Zero distributed forces are not pushed to Robot");
                return;
            }

            if (load.MomentAtStart.Length() != 0 || load.MomentAtEnd.Length() != 0)
            {
                Engine.Base.Compute.RecordError("Varying distributed moments are not supported in Robot and will not be pushed.");
            }

            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_TRAPEZOIDALE);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX1, load.ForceAtStart.X);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY1, load.ForceAtStart.Y);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ1, load.ForceAtStart.Z);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_X1, load.StartPosition);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX2, load.ForceAtEnd.X);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY2, load.ForceAtEnd.Y);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ2, load.ForceAtEnd.Z);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_X2, load.EndPosition);

            if (load.Axis == LoadAxis.Local)
                loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_LOCAL, 1);

            if (load.Projected)
                loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PROJECTION, 1);

            if (load.RelativePositions)
                loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_RELATIVE, 1);

        }

        /***************************************************/
    }
}





