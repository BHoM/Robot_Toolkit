/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using BH.Engine.Adapters.Robot;

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

            Dictionary<int, ICase> pulledCases = new Dictionary<int, ICase>();  //Store all cases and combinations for later assignment to the combinations
            Dictionary<int, List<Tuple<int, double>>> comboCaseFactors = new Dictionary<int, List<Tuple<int, double>>>();   //Case factors for postprocessing of combinations
            for (int i = 1; i <= rLoadCollection.Count; i++)
            {
                try
                {
                    IRobotCase rCase = rLoadCollection.Get(i) as IRobotCase;
                    if (rCase.Type == IRobotCaseType.I_CT_SIMPLE)   //Pull simple cases to temporary dictionary to be used for assigning to combinations
                    {
                        Loadcase lCase = BH.Engine.Structure.Create.Loadcase(rCase.Name, rCase.Number, Convert.FromRobot(rCase.Nature));
                        SetAdapterId(lCase, rCase.Number);
                        pulledCases[lCase.Number] = lCase;
                    }
                    else if (rCase.Type == IRobotCaseType.I_CT_COMBINATION) // No support for code combinations for now as it needs building support for components
                    {
                        IRobotCaseCombination rCaseCombination = rCase as IRobotCaseCombination;
                        List<Tuple<int, double>> caseFactors = new List<Tuple<int, double>>();
                        for (int j = 1; j <= rCaseCombination.CaseFactors.Count; j++)
                        {
                            RobotCaseFactor rCaseFactor = rCaseCombination.CaseFactors.Get(j);
                            caseFactors.Add(new Tuple<int, double>(rCaseFactor.CaseNumber, rCaseFactor.Factor));    //Temporary storage of int-double, being casenumber-factor
                        }

                        comboCaseFactors[rCaseCombination.Number] = caseFactors;
                        LoadCombination lCombination = new LoadCombination { Name = rCaseCombination.Name, Number = rCaseCombination.Number };

                        SetAdapterId(lCombination, rCaseCombination.Number);
                        pulledCases[lCombination.Number] = lCombination;
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

                    Engine.Base.Compute.RecordError(message);
                }
            }

            m_RobotApplication.Project.Structure.Cases.EndMultiOperation();

            //Post process - Run through each combination, and make use of the comboCaseFactors and pulled cases to correctly assign inner cases/combinations to each combinations.
            foreach (LoadCombination combination in bhomLoadCombinations)
            {
                List<Tuple<int, double>> caseFactors;
                List<Tuple<double, ICase>> bCaseFactors = new List<Tuple<double, ICase>>();

                if (comboCaseFactors.TryGetValue(combination.Number, out caseFactors))
                {
                    foreach (Tuple<int, double> caseFactor in caseFactors)
                    {
                        ICase lCase;
                        if (!pulledCases.TryGetValue(caseFactor.Item1, out lCase))
                        {
                            lCase = new Loadcase { Number = caseFactor.Item1 };
                            SetAdapterId(lCase, caseFactor.Item1);
                            Engine.Base.Compute.RecordWarning($"Failed to extract case with number {lCase.Number} for combination {combination.Number}. A Loadcase with the correct number, but no name or nature set has been used in its place.");
                        }
                        bCaseFactors.Add(new Tuple<double, ICase>(caseFactor.Item2, lCase));
                    }
                }
                else
                    Engine.Base.Compute.RecordError($"Failed to extract case factors for LoadCombination with number {combination.Number}.");

                combination.LoadCases = bCaseFactors;
            }

            return bhomLoadCombinations;
        }

        /***************************************************/

    }

}


