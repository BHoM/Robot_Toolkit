/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Loads;
using BH.oM.Physical.Materials;
using BH.oM.Adapter;
using BH.Engine.Base;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected bool Update(IEnumerable<LoadCombination> loadCombinations)
        {
            bool success = true;
            
            m_RobotApplication.Project.Structure.Cases.BeginMultiOperation();
            
            foreach (LoadCombination lComb in loadCombinations)
            {
                //Check combination itself is not null
                if (!CheckNotNull(lComb))
                    continue;

                // Use the Number property directly (consistent with Create method)
                int combinationId = lComb.Number;
                
                // Check if the combination exists in Robot
                if (m_RobotApplication.Project.Structure.Cases.Exist(combinationId) == -1)
                {
                    Engine.Base.Compute.RecordWarning("Load combination with number " + combinationId.ToString() + " does not exist in Robot. Load combination could not be updated!");
                    success = false;
                    continue;
                }

                // Get the existing combination from Robot
                IRobotCase robotCase = m_RobotApplication.Project.Structure.Cases.Get(combinationId) as IRobotCase;
                if (robotCase == null || robotCase.Type != IRobotCaseType.I_CT_COMBINATION)
                {
                    Engine.Base.Compute.RecordWarning("Case with number " + combinationId.ToString() + " is not a load combination in Robot. Load combination could not be updated!");
                    success = false;
                    continue;
                }

                RobotCaseCombination rCaseCombination = robotCase as RobotCaseCombination;
                if (rCaseCombination == null)
                {
                    Engine.Base.Compute.RecordWarning("Failed to cast case with number " + combinationId.ToString() + " to RobotCaseCombination. Load combination could not be updated!");
                    success = false;
                    continue;
                }

                // Update the combination name if provided
                if (!string.IsNullOrWhiteSpace(lComb.Name))
                    rCaseCombination.Name = lComb.Name;

                // Clear existing case factors by deleting them individually
                // Note: Robot API requires deleting case factors in reverse order to avoid index shifting
                for (int i = rCaseCombination.CaseFactors.Count; i >= 1; i--)
                {
                    rCaseCombination.CaseFactors.Delete(i);
                }

                // Add new case factors from the BHoM LoadCombination
                if (lComb.LoadCases != null && lComb.LoadCases.Count > 0)
                {
                    for (int i = 0; i < lComb.LoadCases.Count; i++)
                    {
                        //Check tuple as well as case not null
                        if (CheckNotNull(lComb.LoadCases[i], oM.Base.Debugging.EventType.Error, typeof(LoadCombination)) &&
                            CheckNotNull(lComb.LoadCases[i].Item2, oM.Base.Debugging.EventType.Error, typeof(LoadCombination)))
                        {
                            System.Tuple<double, ICase> loadcase = lComb.LoadCases[i];
                            rCaseCombination.CaseFactors.New(lComb.LoadCases[i].Item2.Number, lComb.LoadCases[i].Item1);
                        }
                    }
                }
                else
                {
                    Engine.Base.Compute.RecordWarning("Load combination with number " + combinationId.ToString() + " has no load cases. The combination has been cleared of all case factors.");
                }

                // Set the adapter ID to maintain the connection between BHoM and Robot objects
                this.SetAdapterId(lComb, lComb.Number);
            }
            
            m_RobotApplication.Project.Structure.Cases.EndMultiOperation();
            
            return success;
        }

        /***************************************************/

     }
}






