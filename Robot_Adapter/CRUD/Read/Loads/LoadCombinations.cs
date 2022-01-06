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

using BH.oM.Structure.Loads;
using RobotOM;
using System.Collections.Generic;
using System;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<LoadCombination> ReadLoadCombinations(List<string> ids = null)
        {
            m_RobotApplication.Project.Structure.Cases.BeginMultiOperation();
            List<LoadCombination> bhomLoadCombinations = new List<LoadCombination>();
            IRobotCaseCollection rLoadCollection = m_RobotApplication.Project.Structure.Cases.GetAll();

            for (int i = 1; i <= rLoadCollection.Count; i++)
            {
                try
                {
                IRobotCase rCase = rLoadCollection.Get(i) as IRobotCase;
                if (rCase.Type == IRobotCaseType.I_CT_COMBINATION) // No support for code combinations for now as it needs building support for components
                {
                    IRobotCaseCombination rCaseCombination = rCase as IRobotCaseCombination;
                    List<System.Tuple<double, ICase>> bCaseFactors = new List<System.Tuple<double, ICase>>();

                    for (int j = 1; j <= rCaseCombination.CaseFactors.Count; j++)
                    {
                        IRobotCase rCaseIn = rLoadCollection.Get(rCaseCombination.CaseFactors.Get(j).CaseNumber) as IRobotCase;
                        Loadcase lCase = BH.Engine.Structure.Create.Loadcase(rCaseIn.Name, rCaseIn.Number, Convert.FromRobot(rCaseIn.Nature));
                        SetAdapterId(lCase, rCaseCombination.Number);
                        bCaseFactors.Add(new System.Tuple<double, ICase>(rCaseCombination.CaseFactors.Get(j).Factor, lCase));
                    }

                    LoadCombination lCombination = BH.Engine.Structure.Create.LoadCombination(rCaseCombination.Name, rCaseCombination.Number, bCaseFactors);

                    SetAdapterId(lCombination, rCaseCombination.Number);
                    bhomLoadCombinations.Add(lCombination);
                }
                }
                catch (Exception e)
                {
                    string message = "Failed to extract a LoadCombination. Exception message: " + e.Message;

                    if (!string.IsNullOrEmpty(e.InnerException?.Message))
                    {
                        message += "\nInnerException: " + e.InnerException.Message;
                    }

                    Engine.Reflection.Compute.RecordError(message);
                }
            }
            m_RobotApplication.Project.Structure.Cases.EndMultiOperation();
            return bhomLoadCombinations;
        }

        /***************************************************/

    }

}

