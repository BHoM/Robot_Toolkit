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

using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.oM.Structure.Results;
using BH.oM.Structure.Requests;
using BH.oM.Data.Requests;
using RobotOM;
using System;
using System.Collections;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /**** Protected Override Methods                ****/
        /***************************************************/

            

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private double TryGetValue(RobotResultRow row, int valuePosition)
        {
            return row.IsAvailable(valuePosition) ? row.GetValue(valuePosition) : double.NaN;
        }

        /***************************************************/

        private RobotSelection GetCaseSelection(IResultRequest request)
        {
            RobotSelection caseSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);

            if (request.Cases == null || request.Cases.Count == 0)
                caseSelection.FromText("all");
            else
                caseSelection.FromText(Convert.ToRobotSelectionString(GetCaseNumbers(request.Cases)));

            return caseSelection;
        }

        /***************************************************/
    }
}





