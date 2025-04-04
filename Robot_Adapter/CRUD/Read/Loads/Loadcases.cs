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

using System.Collections.Generic;
using RobotOM;
using BH.oM.Structure.Loads;
using System;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Loadcase> ReadLoadCase(List<string> ids = null)
        {
            RobotCaseCollection rLoadCases = m_RobotApplication.Project.Structure.Cases.GetAll();
            List<Loadcase> bhomLoadCases = new List<Loadcase>();

            for (int i = 1; i <= rLoadCases.Count; i++)
            {
                try
                {
                    IRobotCase rLoadCase = rLoadCases.Get(i) as IRobotCase;
                    if (rLoadCase.Type == IRobotCaseType.I_CT_SIMPLE)
                    {
                        Loadcase lCase = new Loadcase { Name = rLoadCase.Name, Number = rLoadCase.Number, Nature = Convert.FromRobot(rLoadCase.Nature) };
                        SetAdapterId(lCase, rLoadCase.Number);
                        bhomLoadCases.Add(lCase);
                    }
                }
                catch (Exception e)
                {
                    string message = "Failed to extract a Loadcase. Exception message: " + e.Message;

                    if (!string.IsNullOrEmpty(e.InnerException?.Message))
                    {
                        message += "\nInnerException: " + e.InnerException.Message;
                    }

                    Engine.Base.Compute.RecordError(message);
                }

            }
            return bhomLoadCases;
        }

        /***************************************************/

    }

}







