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

using RobotOM;
using System.Collections.Generic;
using BH.oM.Structure.Constraints;


namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static Constraint6DOF ToBHoMObject(this RobotNodeSupport robotSupport)
        {            
            return Convert.ToBHoMObject(robotSupport.Data, robotSupport.Name);
        }

        /***************************************************/

        public static Constraint6DOF ToBHoMObject(this RobotNodeSupportData robotSupportData, string name = "")
        {
            List<bool> fixity = new List<bool>();
            fixity.Add(robotSupportData.UX != 0);
            fixity.Add(robotSupportData.UY != 0);
            fixity.Add(robotSupportData.UZ != 0);
            fixity.Add(robotSupportData.RX != 0);
            fixity.Add(robotSupportData.RY != 0);
            fixity.Add(robotSupportData.RZ != 0);

            List<double> stiffness = new List<double>();
            stiffness.Add(robotSupportData.KX);
            stiffness.Add(robotSupportData.KY);
            stiffness.Add(robotSupportData.KZ);
            stiffness.Add(robotSupportData.HX);
            stiffness.Add(robotSupportData.HY);
            stiffness.Add(robotSupportData.HZ);
            
            Constraint6DOF constraint6DOF = BH.Engine.Structure.Create.Constraint6DOF(name, fixity, stiffness);
            return constraint6DOF;
        }

        /***************************************************/

    }
}

