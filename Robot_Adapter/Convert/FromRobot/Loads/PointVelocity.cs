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

        public static PointVelocity FromRobotPtVel(this IRobotLoadRecord loadRecord)
        {
            if (loadRecord == null)
                return null;

            try
            {
                double vx = loadRecord.GetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UX);
                double vy = loadRecord.GetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UY);
                double vz = loadRecord.GetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UZ);

                return new PointVelocity
                {
                    TranslationalVelocity = new Vector { X = vx, Y = vy, Z = vz }
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






