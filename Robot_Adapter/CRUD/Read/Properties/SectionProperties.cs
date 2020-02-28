/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
            IRobotCollection secProps = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_SECTION);
            List<ISectionProperty> bhomSectionProps = new List<ISectionProperty>();
            Dictionary<string, IMaterialFragment> materials = ReadMaterials().ToDictionary(x => x.Name);

            for (int i = 1; i <= secProps.Count; i++)
            {
                ISectionProperty bhomSec = null;
                IMaterialFragment bhomMat = null;
                IRobotLabel rSection = secProps.Get(i);
                IRobotBarSectionData secData = rSection.Data as IRobotBarSectionData;
                
                //If material not yet read out, read it by label and stor
                if (!materials.TryGetValue(secData.MaterialName, out bhomMat))
                {
                    bhomMat = ReadMaterialByLabelName(secData.MaterialName);

                    if (bhomMat == null)
                    {
                        //TODO: find a sensible way to get out the type of material that the section in expecting.
                        //For now resorting to checking for concrete, if not true assume steal.
                        string materialType = "";
                        if (secData.IsConcrete)
                        {
                            bhomMat = new Concrete() { Name = secData.MaterialName };
                            materialType = "Concrete";
                        }
                        else
                        {
                            bhomMat = new Steel() { Name = secData.MaterialName };
                            materialType = "Steel";
                        }

                        Engine.Reflection.Compute.RecordWarning("Unable to extract material with label " + secData.MaterialName + ". An empty " + materialType + " with the same name has been created in its place");
                    }

                    materials[bhomMat.Name] = bhomMat;
                }

                bhomSec = Convert.FromRobot(secData, materials[secData.MaterialName]);

                if (bhomSec != null)
                {
                    bhomSec.Material = bhomMat;
                    bhomSec.Name = rSection.Name;
                    bhomSec.CustomData[AdapterIdName] = rSection.Name;
                    bhomSectionProps.Add(bhomSec);
                }
                else
                {
                    Engine.Reflection.Compute.RecordWarning("Unable to convert section named " + rSection.Name);
                }
            }
            return bhomSectionProps;
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


