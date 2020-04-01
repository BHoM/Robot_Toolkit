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

using BH.oM.Geometry;
using BH.oM.Structure.Loads;
using RobotOM;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static BarVaryingDistributedLoad FromRobotBarVarDistLoad(this IRobotLoadRecord loadRecord)
        {
            double fax = loadRecord.GetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX1);
            double fay = loadRecord.GetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY1);
            double faz = loadRecord.GetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ1);
            double distA = loadRecord.GetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_X1);
            double fbx = loadRecord.GetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX2);
            double fby = loadRecord.GetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY2);
            double fbz = loadRecord.GetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ2);
            double distB = loadRecord.GetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_X2);

            double rel = loadRecord.GetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_RELATIVE);
            double axis = loadRecord.GetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_LOCAL);
            double proj = loadRecord.GetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PROJECTION);

            if (rel != 0)
            {
                Engine.Reflection.Compute.RecordWarning("Currently no support for BarVaryingDistributedLoad with relative distance from ends");
                return null;
            }

            return new BarVaryingDistributedLoad
            {
                ForceA = new Vector { X = fax, Y = fay, Z = faz },
                ForceB = new Vector { X = fbx, Y = fby, Z = fbz },
                DistanceFromA = distA,
                DistanceFromB = distB,
                Axis = axis.FromRobotLoadAxis(),
                Projected = proj.FromRobotProjected()
            };
        }

        /***************************************************/
    }
}

