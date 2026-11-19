/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Loads;
using BH.Engine.Base;
using BH.oM.Base;
using BH.oM.Structure;
using BH.oM.Adapters.Robot;
using BH.Engine.Adapter;
using BH.oM.Base.Debugging;
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
                if(!CheckNotNull(lComb))
{
                    Compute.RecordError("LoadCombination is null.");
                    return false;
                }

                // Use the Number property directly and try to get the combination
                int combinationId = lComb.Number;
                
                // Check if the RobotId matches the LoadCombination Number
                int robotId = Engine.Adapter.Query.HasAdapterIdFragment(lComb, typeof(RobotId)) ? GetAdapterId<int>(lComb) : 0;
                
                // If the LoadCombination doesn't have a RobotId, assign it from the Number
                if (robotId == 0)
                {
                    this.SetAdapterId(lComb, combinationId);
                    robotId = combinationId;
                    Compute.RecordWarning($"LoadCombination with number {combinationId} did not have a RobotId. RobotId has been set to the LoadCombination number.");
                }
                else if (robotId != combinationId)
                {
                    Compute.RecordWarning($"Load combination has mismatched IDs: RobotId = {robotId}, Number = {combinationId}. Using Number property for update.");
                }
                
                // Get the existing combination from Robot (following pattern from Loadcases Update method)
                RobotCaseCombination rCaseCombination = m_RobotApplication.Project.Structure.Cases.Get(combinationId) as RobotCaseCombination;
                if (rCaseCombination == null)
                {
                    Compute.RecordError("Load combination with number " + combinationId.ToString() + " does not exist in Robot. Load combination could not be updated!");
                    return false;
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
                    for (int i = rCaseCombination.CaseFactors.Count; i >= 1; i--)
                    {
                        rCaseCombination.CaseFactors.Delete(i);
                    }
                    for (int i = 0; i < lComb.LoadCases.Count; i++)
                    {
                        //Check tuple as well as case not null
                        if (CheckNotNull(lComb.LoadCases[i], EventType.Error, typeof(LoadCombination)) &&
                            CheckNotNull(lComb.LoadCases[i].Item2, EventType.Error, typeof(LoadCombination)))
                        {
                            System.Tuple<double, ICase> loadcase = lComb.LoadCases[i];
                            rCaseCombination.CaseFactors.New(lComb.LoadCases[i].Item2.Number, lComb.LoadCases[i].Item1);
                        }
                    }
                }
                else
                {
                    Compute.RecordWarning("Load combination with number " + combinationId.ToString() + " has no load cases.");
                }

            }
            
            m_RobotApplication.Project.Structure.Cases.EndMultiOperation();
            
            return success;
        }

        /***************************************************/

     }
}







