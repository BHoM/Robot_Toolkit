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
using RobotOM;
using BH.oM.Structure.MaterialFragments;
using System.Linq;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<IMaterialFragment> ReadMaterials(List<string> ids = null)
        {
            return ReadLabels(IRobotLabelType.I_LT_MATERIAL).Select(x => x as IMaterialFragment).ToList(); ;
        }

        /***************************************************/

        private IMaterialFragment ReadMaterialByLabelName(string labelName)
        {
            IRobotLabel materialLabel = m_RobotApplication.Project.Structure.Labels.Get(IRobotLabelType.I_LT_MATERIAL, labelName);
            if(materialLabel != null)
                return Convert.FromRobot(materialLabel, materialLabel.Data as RobotMaterialData);
            return null;
        }

        /***************************************************/

        private void ReadMaterialNamesFromDB(string dbName)
        {
            m_RobotApplication.Project.Preferences.Materials.Load(dbName);
            RobotNamesArray defaultMaterial = m_RobotApplication.Project.Preferences.Materials.GetAll();
            for (int i = 1; i <= defaultMaterial.Count; i++)
            {
                if (!m_dbMaterialNames.Contains(defaultMaterial.Get(i)))
                    m_dbMaterialNames.Add(defaultMaterial.Get(i));
            }
        }

        /***************************************************/

        private IMaterialFragment ReadMaterialFromPanel(IRobotObjObject robotPanel)
        {
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel thicknessLabel = robotPanel.GetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS);
            IRobotThicknessData thicknessData = thicknessLabel.Data;
            IRobotLabel robotLabel = robotLabelServer.Get(IRobotLabelType.I_LT_MATERIAL, thicknessData.MaterialName);
            return Convert.FromRobot(robotLabel, robotLabel.Data as RobotMaterialData);
        }
    }
}


