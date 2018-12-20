/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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

using RobotOM;
using System.Collections.Generic;
using BH.oM.Structure.Properties.Constraint;


namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static Constraint6DOF ToBHoMObject(this RobotNodeSupport robSupport)
        {
            List<bool> fixity = new List<bool>();
            RobotNodeSupportData suppData = robSupport.Data;
            fixity.Add(suppData.UX != 0);
            fixity.Add(suppData.UY != 0);
            fixity.Add(suppData.UZ != 0);
            fixity.Add(suppData.RX != 0);
            fixity.Add(suppData.RY != 0);
            fixity.Add(suppData.RZ != 0);

            List<double> stiffness = new List<double>();
            stiffness.Add(suppData.KX);
            stiffness.Add(suppData.KY);
            stiffness.Add(suppData.KZ);
            stiffness.Add(suppData.HX);
            stiffness.Add(suppData.HY);
            stiffness.Add(suppData.HZ);

            Constraint6DOF const6DOF = BH.Engine.Structure.Create.Constraint6DOF(robSupport.Name, fixity, stiffness);
            return const6DOF;
        }

        /***************************************************/

    }
}
