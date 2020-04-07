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
using BH.Engine.External.Robot;
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
                Engine.Reflection.Compute.RecordWarning("Zero distributed forces are not pushed to Robot");
                return;
            }

            if (load.MomentA.Length() != 0 && load.MomentB.Length() != 0)
            {
                Engine.Reflection.Compute.RecordError("Varying distributed moments are not supported in Robot.");
                return;
            }

            int counter = 0;
            //Group bars by length within 1 mm. Robot applies loads differently from BHoM (both points referencing the start point while BHoM references the start for A and end for B)
            foreach (var lengthGroupedBars in load.Objects.Elements.GroupBarsByLength(0.001))
            {
                double dist2 = lengthGroupedBars.Key - load.DistanceFromB;
                IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_TRAPEZOIDALE);
                loadRecord.Objects.FromText(lengthGroupedBars.Value.ToRobotSelectionString());
                loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX1, load.ForceA.X);
                loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY1, load.ForceA.Y);
                loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ1, load.ForceA.Z);
                loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_X1, load.DistanceFromA);
                loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX2, load.ForceB.X);
                loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY2, load.ForceB.Y);
                loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ2, load.ForceB.Z);
                loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_X2, dist2);

                if (load.Axis == LoadAxis.Local)
                    loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_LOCAL, 1);

                if (load.Projected)
                    loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PROJECTION, 1);

                counter++;
            }

            if (counter > 1)
                Engine.Reflection.Compute.RecordNote("Varying BarLoads in BHoM measures distance from start for the first point and from end for the second point, whilst Robot measures only from start node.To accommodate this, multiple loads have been generated in Robot, grouped by the length of the Bars the load is applied to.");


        }

        /***************************************************/
    }
}

