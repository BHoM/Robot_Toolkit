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
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Structure.Constraints;
using System.Collections;
using BH.Engine.Adapters.Robot;
using BH.oM.Spatial;
using BH.oM.Spatial.SettingOut;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Level> ReadLevels(IList ids = null)
        {
            List<int> levelIds = CheckAndGetIds<Level>(ids);
            List<Level> bhomLevels = new List<Level>();

            Dictionary<int, HashSet<string>> nodeTags = GetTypeTags(typeof(Level));
            HashSet<string> tags = new HashSet<string>();

            RobotStoreyMngr robotStoreys;

            robotStoreys = m_RobotApplication.Project.Structure.Storeys;

            for (int i = 1; i <= robotStoreys.Count; i++)
            {
                RobotStorey robotStorey = robotStoreys.Get(i);
                Level bhomLevel = new Level
                {
                    Name = robotStorey.Name,
                    Elevation = robotStorey.TopLevel
                };
                SetAdapterId(bhomLevel, i);
                bhomLevels.Add(bhomLevel);
            }

            return bhomLevels;
        }

        /***************************************************/              


    }

}







