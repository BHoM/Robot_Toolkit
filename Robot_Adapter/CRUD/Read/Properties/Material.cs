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
using BH.oM.Structure.MaterialFragments;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<IMaterialFragment> ReadMaterial(List<string> ids = null)
        {
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotCollection rMaterials = labelServer.GetMany(IRobotLabelType.I_LT_MATERIAL);
            IMaterialFragment bhomMat = null;
            List<IMaterialFragment> bhomMaterials = new List<IMaterialFragment>();

            for (int i = 1; i <= rMaterials.Count; i++)
            {
                IRobotLabel robotMaterialLabel = rMaterials.Get(i);

                bhomMat = MaterialFromLabel(robotMaterialLabel);

                if (bhomMat != null)
                    bhomMaterials.Add(bhomMat);
            }

            return bhomMaterials;
        }

        /***************************************************/

        private IMaterialFragment ReadMaterialByLabelName(string labelName)
        {
            IRobotLabel materialLabel = m_RobotApplication.Project.Structure.Labels.Get(IRobotLabelType.I_LT_MATERIAL, labelName);
            if(materialLabel != null)
                return MaterialFromLabel(materialLabel);
            return null;
        }

        /***************************************************/

        private IMaterialFragment MaterialFromLabel(IRobotLabel robotMaterialLabel)
        {
            IRobotMaterialData mData = robotMaterialLabel.Data as IRobotMaterialData;
            IMaterialFragment bhomMat;
            switch (mData.Type)
            {
                case IRobotMaterialType.I_MT_STEEL:
                    bhomMat = Engine.Structure.Create.Steel(mData.Name, mData.E, mData.NU, mData.LX, mData.RO / Engine.Robot.Query.RobotGravityConstant, mData.DumpCoef, mData.RE, mData.RT);
                    break;
                case IRobotMaterialType.I_MT_CONCRETE:
                    bhomMat = Engine.Structure.Create.Concrete(mData.Name, mData.E, mData.NU, mData.LX, mData.RO / Engine.Robot.Query.RobotGravityConstant, mData.DumpCoef, 0, 0);
                    break;
                case IRobotMaterialType.I_MT_ALUMINIUM:
                    bhomMat = Engine.Structure.Create.Aluminium(mData.Name, mData.E, mData.NU, mData.LX, mData.RO / Engine.Robot.Query.RobotGravityConstant, mData.DumpCoef);
                    break;
                case IRobotMaterialType.I_MT_OTHER:
                case IRobotMaterialType.I_MT_TIMBER:
                case IRobotMaterialType.I_MT_ALL:
                default:
                    Engine.Reflection.Compute.RecordWarning("Material of Robot type " + mData.Type + " not yet suported. Empty material will be provided");
                    return null;
            }           
            bhomMat.CustomData.Add(AdapterIdName, NextFreeId(bhomMat.GetType(), false));
            return bhomMat;
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
            IMaterialFragment material = null;
            try
            {
                material = MaterialFromLabel(robotLabelServer.Get(IRobotLabelType.I_LT_MATERIAL, thicknessData.MaterialName));
            }
            catch
            {
                BH.Engine.Reflection.Compute.RecordWarning("No material is assigned for panel " + robotPanel.Number.ToString());
            }
            return material;
        }
    }
}

