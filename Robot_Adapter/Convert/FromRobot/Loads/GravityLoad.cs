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

        public static GravityLoad FromRobotGravityLoad(this IRobotLoadRecord loadRecord)
        {
            if (loadRecord == null)
                return null;

            try
            {
                double dx = loadRecord.GetValue((short)IRobotDeadRecordValues.I_DRV_X);
                double dy = loadRecord.GetValue((short)IRobotDeadRecordValues.I_DRV_Y);
                double dz = loadRecord.GetValue((short)IRobotDeadRecordValues.I_DRV_Z);
                double coef = loadRecord.GetValue((short)IRobotDeadRecordValues.I_DRV_COEFF);

                return new GravityLoad
                {
                    GravityDirection = new Vector { X = dx * coef, Y = dy * coef, Z = dz * coef }
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





