/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Loads;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<LoadCombination> ReadLoadCombination(List<string> ids = null)
        {
            RobotCaseCollection rLoadCombinations = m_RobotApplication.Project.Structure.Cases.GetAll();
            List<LoadCombination> bhomLoadCombinations = new List<LoadCombination>();

            for (int i = 1; i <= rLoadCombinations.Count; i++)
            {
                IRobotCase rLoadCombination = rLoadCombinations.Get(i) as IRobotCase;
                LoadCombination lCombination = BH.Engine.Structure.Create.LoadCombination(rLoadCombination.Name, rLoadCombination.Number, BH.Engine.Robot.Convert.BHoMLoadNature(rLoadCombination.Nature));
                lCombination.CustomData[AdapterId] = rLoadCombination.Number;
                bhomLoadCombinations.Add(lCombination);
            }
            return bhomLoadCombinations;
        }

        /***************************************************/

    }

}

