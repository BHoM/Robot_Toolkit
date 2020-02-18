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

using System.Collections.Generic;
using RobotOM;
using BH.oM.Structure.MaterialFragments;

namespace BH.Adapter.Robot
{
    public partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static IMaterialFragment FromRobot(this IRobotLabel robotLabel, IRobotMaterialData robotLabelData, string name = "")
        {
            if(robotLabel.Name != "") name = robotLabel.Name;
            IMaterialFragment material;
            switch (robotLabelData.Type)
            {
                case IRobotMaterialType.I_MT_STEEL:
                    material = Engine.Structure.Create.Steel(name, robotLabelData.E, robotLabelData.NU, robotLabelData.LX, robotLabelData.RO / Engine.Robot.Query.RobotGravityConstant, robotLabelData.DumpCoef, robotLabelData.RE, robotLabelData.RT);
                    break;
                case IRobotMaterialType.I_MT_CONCRETE:
                    material = Engine.Structure.Create.Concrete(name, robotLabelData.E, robotLabelData.NU, robotLabelData.LX, robotLabelData.RO / Engine.Robot.Query.RobotGravityConstant, robotLabelData.DumpCoef, 0, 0);
                    break;
                case IRobotMaterialType.I_MT_ALUMINIUM:
                    material = Engine.Structure.Create.Aluminium(name, robotLabelData.E, robotLabelData.NU, robotLabelData.LX, robotLabelData.RO / Engine.Robot.Query.RobotGravityConstant, robotLabelData.DumpCoef);
                    break;
                case IRobotMaterialType.I_MT_OTHER:
                case IRobotMaterialType.I_MT_TIMBER:
                case IRobotMaterialType.I_MT_ALL:
                default:
                    Engine.Reflection.Compute.RecordWarning("Material named '" + name + "' of Robot type " + robotLabelData.Type + " not yet suported. Empty material will be provided");
                    return null;
            }           
            return material;
        }

        /***************************************************/

    }
}


