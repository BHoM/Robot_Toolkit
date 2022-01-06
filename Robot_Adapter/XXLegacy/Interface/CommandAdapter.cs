/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Global;
using BHoM.Structural;
using RobotOM;
using BHoME = BHoM.Structural.Elements;
using BHoMP = BHoM.Structural.Properties;
using BHoML = BHoM.Structural.Loads;
using BHoM.Structural.Interface;
using BHoM.Base;
namespace Robot_Adapter.Structural.Interface
{
    public partial class RobotAdapter : BHoM.Structural.Interface.ICommandAdapter
    {
        public bool Analyse(List<string> cases = null)
        {
            Robot.UserControl = true;
            Robot.Interactive = 1;
            Robot.Project.CalcEngine.AnalysisParams.IgnoreWarnings = true;
            Robot.Project.CalcEngine.AnalysisParams.AutoVerification = IRobotStructureAutoVerificationType.I_SAVT_NONE;
            Robot.Project.CalcEngine.Calculate();
            Robot.Project.CalcEngine.AutoFreezeResults = false;
            return true;
        }

        public bool ClearResults()
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool Save(string fileName = null)
        {
            throw new NotImplementedException();
        }
    }
}



