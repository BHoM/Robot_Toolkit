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

using System.Collections.Generic;
using BH.oM.Structure.Loads;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<LoadCombination> lComabinations)
        {
            foreach (LoadCombination lComb in lComabinations)
            {
                if (m_RobotApplication.Project.Structure.Cases.Exist(lComb.Number)!=-1)
                {
                    RobotCaseCombination rCaseCombination = m_RobotApplication.Project.Structure.Cases.CreateCombination(lComb.Number, lComb.Name, IRobotCombinationType.I_CBT_ULS, IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_COMB);
                    for (int i = 0; i < lComb.LoadCases.Count; i++)
                    {
                        rCaseCombination.CaseFactors.New(lComb.LoadCases[i].Item2.Number, lComb.LoadCases[i].Item1);
                    }
                    updateview();
                }
            }
            return true;
        }

        /***************************************************/

    }

}

