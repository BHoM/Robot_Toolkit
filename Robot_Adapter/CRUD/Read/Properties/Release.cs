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
using RobotOM;
using BH.oM.Structure.Constraints;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<BarRelease> ReadBarRelease(List<string> ids = null)
        {
            IRobotCollection releaseCollection = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_RELEASE);
            List<BarRelease> bhomReleases = new List<BarRelease>();

            for (int i = 1; i <= releaseCollection.Count; i++)
            {
                IRobotLabel rReleaseLabel = releaseCollection.Get(i);
                IRobotBarReleaseData rReleaseData = rReleaseLabel.Data as IRobotBarReleaseData;
                BarRelease bhomMBarRelease = new BarRelease
                {
                    Name = rReleaseLabel.Name,
                    StartRelease = BH.Engine.Robot.Convert.BHoMRelease(rReleaseData.StartNode),
                    EndRelease = BH.Engine.Robot.Convert.BHoMRelease(rReleaseData.EndNode)
                };
                bhomMBarRelease.CustomData.Add(AdapterIdName, rReleaseLabel.Name);
                bhomReleases.Add(bhomMBarRelease);
            }
            return bhomReleases;
        }

        /***************************************************/        
 
    }

}
