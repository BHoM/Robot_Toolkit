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
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Structure;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        /***************************************************/

        private bool CreateCollection(IEnumerable<IMaterialFragment> materials, bool overwrite = true)
        {
            RobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel label = robotLabelServer.Create(IRobotLabelType.I_LT_MATERIAL, "");
            IRobotMaterialData matData = label.Data;
            foreach (IMaterialFragment material in materials)
            {
                if (!CheckNotNull(material, oM.Reflection.Debugging.EventType.Warning))
                    continue;

                string match = Convert.Match(m_dbMaterialNames, material);
                bool loadedFromDb = false;
                if (match != null)
                {
                    matData.LoadFromDBase(match);
                    if (matData != null && matData.Type == Convert.GetMaterialType(material)) //Check that the material type matches, if not, create a new one instead of loading from DB
                    {
                        if (robotLabelServer.Exist(IRobotLabelType.I_LT_MATERIAL, match) == -1)
                            MaterialExistsWarning(match);
                        m_RobotApplication.Project.Structure.Labels.StoreWithName(label, match);
                        loadedFromDb = true;
                    }
                }

                if(!loadedFromDb)
                {
                    string name = material.DescriptionOrName();
                    if (robotLabelServer.Exist(IRobotLabelType.I_LT_MATERIAL, name) == -1)
                        MaterialExistsWarning(name);
                    if(Convert.ToRobot(matData, material))
                        m_RobotApplication.Project.Structure.Labels.StoreWithName(label, name);
                }
            }
            return true;
        }

        /***************************************************/

        private void MaterialExistsWarning(string materialName)
        {
            BH.Engine.Reflection.Compute.RecordWarning("Material '" + materialName + "' already exists in the model, the properties have been overwritten");
        }

        /***************************************************/
    }
}


