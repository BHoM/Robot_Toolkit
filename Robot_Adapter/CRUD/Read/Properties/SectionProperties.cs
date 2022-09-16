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

using System.Collections.Generic;
using System.Linq;
using RobotOM;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.MaterialFragments;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            Dictionary<string, IMaterialFragment> materials = ReadMaterials().ToDictionary(x => x.Name);
            return ReadLabels(IRobotLabelType.I_LT_BAR_SECTION, ids, materials).Select(x => x as ISectionProperty).Where(x => x != null).ToList();
        }

        /***************************************************/

        private void ReadSecPropNamesFromDB(string dbName)
        {
            m_RobotApplication.Project.Preferences.SetCurrentDatabase(IRobotDatabaseType.I_DT_SECTIONS, dbName);
            RobotSectionDatabaseList dbList = m_RobotApplication.Project.Preferences.SectionsActive;
            List<string> secNames = new List<string>();
            RobotSectionDatabase rDataBase = null;

            for (int i = 1; i < dbList.Count; i++)
            {
                if (dbList.GetDatabase(i).Name == dbName)
                {
                    rDataBase = dbList.GetDatabase(i);
                    break;
                }
            }

            RobotNamesArray sections = rDataBase.GetAll();

            for (int i = 1; i < sections.Count; i++)
            {
                if (!m_dbSecPropNames.Contains(sections.Get(i)))
                    m_dbSecPropNames.Add(sections.Get(i));
            }
        }

        /***************************************************/

    }

}




