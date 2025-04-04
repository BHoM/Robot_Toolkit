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
using System.Linq;
using BH.oM.Structure.Loads;
using RobotOM;
using BH.Engine.Base;
using BH.oM.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/
        
        private bool CreateCollection(IEnumerable<Loadcase> loadCase)
        {
            List<Loadcase> caseList = loadCase.ToList();
            IRobotCaseServer caseServer = m_RobotApplication.Project.Structure.Cases;

            m_RobotApplication.Project.Structure.Cases.BeginMultiOperation();
            for (int i = 0; i < caseList.Count; i++)
            {
                if (!CheckNotNull(caseList[i]))
                    continue;
                                
                int subNature;
                IRobotCaseNature rNature = Convert.ToRobotLoadcaseNature(caseList[i], out subNature);

                // Check if any loadcases havea zero
                if(caseList.Any(lc => lc.Number == 0 || lc.Number < 1))
                {
                    Compute.RecordError("One or more Loadcases have a number zero (or negative number) assigned and cannot be pushed.");
                    return false;
                }

                m_RobotApplication.Project.Structure.Cases.CreateSimple(caseList[i].Number, caseList[i].Name, rNature, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                IRobotSimpleCase sCase = caseServer.Get(caseList[i].Number) as IRobotSimpleCase;

                sCase.label = caseList[i].FindFragment<LoadCaseLabel>()?.Label ?? "";

                if (subNature >= 0)               
                    sCase.SetNatureExt(subNature);

                this.SetAdapterId(caseList[i], caseList[i].Number);
            }
            m_RobotApplication.Project.Structure.Cases.EndMultiOperation();
            return true;
        }

        /***************************************************/

    }

}







