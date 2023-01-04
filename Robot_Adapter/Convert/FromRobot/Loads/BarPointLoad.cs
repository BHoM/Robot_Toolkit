/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

        public static BarPointLoad FromRobotBarPtLoad(this IRobotLoadRecord loadRecord)
        {
            if (loadRecord == null)
                return null;

            try
            {
                double fx = loadRecord.GetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FX);
                double fy = loadRecord.GetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FY);
                double fz = loadRecord.GetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FZ);
                double mx = loadRecord.GetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CX);
                double my = loadRecord.GetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CY);
                double mz = loadRecord.GetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CZ);
                double distA = loadRecord.GetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_X);
                double rel = loadRecord.GetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_REL);
                double local = loadRecord.GetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_LOC);

                if (rel != 0)
                {
                    Engine.Base.Compute.RecordWarning("Currently no support for BarPointLoads with relative distance from ends");
                    return null;
                }

                return new BarPointLoad
                {
                    Force = new Vector { X = fx, Y = fy, Z = fz },
                    Moment = new Vector { X = mx, Y = my, Z = mz },
                    DistanceFromA = distA,
                    Axis = local.FromRobotLoadAxis()
                };
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /***************************************************/
    }
}




