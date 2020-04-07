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
using System.Collections.Generic;
using BH.Engine.External.Robot;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using System;
using System.Linq;
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

        //Fixing the issue with Robot defining the second point in the BarVaryingLoad in relation to the start, while BHoM defines it in relation to the end
        public static List<BarVaryingDistributedLoad> FixVaryingLoadEndDistances(this BarVaryingDistributedLoad load)
        {
            List<BarVaryingDistributedLoad> loads = new List<BarVaryingDistributedLoad>();

            int counter = 0;
            foreach (var lengthGroup in load.Objects.Elements.GroupBarsByLength(0.001))
            {
                BarVaryingDistributedLoad clone = load.GetShallowClone() as BarVaryingDistributedLoad;
                clone.DistanceFromB = lengthGroup.Key - clone.DistanceFromB; //Set distance from B to be from end node instead of start
                clone.Objects = new BHoMGroup<Bar>() { Elements = lengthGroup.Value };
                loads.Add(clone);
                counter++;
            }

            if (counter > 1)
                Engine.Reflection.Compute.RecordNote("Varying BarLoads in BHoM measures distance from start for the first point and from end for the second point, whilst Robot measures only from start node. To accommodate this, load pulled from Robot has been split up in multiple loads, grouped by the length of the Bars the load is applied to.");

            return loads;
        }

        /***************************************************/
    }
}

