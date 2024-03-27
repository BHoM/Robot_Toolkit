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

        public static PointDisplacement FromRobotPtDisp(this IRobotLoadRecord loadRecord)
        {
            if (loadRecord == null)
                return null;

            try
            {
                double ux = loadRecord.GetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UX);
                double uy = loadRecord.GetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UY);
                double uz = loadRecord.GetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UZ);
                double rx = loadRecord.GetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RX);
                double ry = loadRecord.GetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RY);
                double rz = loadRecord.GetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RZ);

                return new PointDisplacement
                {
                    Translation = new Vector { X = ux, Y = uy, Z = uz },
                    Rotation = new Vector { X = rx, Y = ry, Z = rz }
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





