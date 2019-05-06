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
using BH.oM.Physical.Materials;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Material> ReadMaterial(List<string> ids = null)
        {
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotCollection rMaterials = labelServer.GetMany(IRobotLabelType.I_LT_MATERIAL);
            IRobotMaterialData mData;
            Material bhomMat = null;
            List<Material> bhomMaterials = new List<Material>();
            int counter = 0;
            bool refresh = false;

            //foreach (string matName in m_dbMaterialNames)
            //{
            //    IRobotLabel label = labelServer.Create(IRobotLabelType.I_LT_MATERIAL, "");
            //    mData = label.Data;
            //    mData.LoadFromDBase(matName);
            //    MaterialType bhomMatType = BH.Engine.Robot.Convert.GetMaterialType(mData.Type);
            //    bhomMat = BH.Engine.Common.Create.Material(matName, bhomMatType, mData.E, mData.NU, mData.LX, mData.RO / Engine.Robot.Query.RobotGravityConstant);
            //    if (m_indexDict.ContainsKey(bhomMat.GetType()) && counter == 0)
            //        m_indexDict[bhomMat.GetType()] = 0;
            //    bhomMat.CustomData.Add(AdapterId, NextId(bhomMat.GetType(), refresh));

            //    //if (refresh)
            //    //    refresh = false;

            //    if (bhomMat != null)
            //        bhomMaterials.Add(bhomMat);
            //    counter++;
            //}

            for (int i = 1; i <= rMaterials.Count; i++)
            {
                IRobotLabel rMatLable = rMaterials.Get(i);

                bhomMat = MaterialFromLabel(rMatLable);

                if (bhomMat != null)
                    bhomMaterials.Add(bhomMat);
                //}
            }

            return bhomMaterials;
        }

        /***************************************************/

        private Material ReadMaterialByLabelName(string labelName)
        {
            IRobotLabel materialLabel = m_RobotApplication.Project.Structure.Labels.Get(IRobotLabelType.I_LT_MATERIAL, labelName);
            return MaterialFromLabel(materialLabel);
        }

        /***************************************************/

        private Material MaterialFromLabel(IRobotLabel materialLabel)
        {
            IRobotMaterialData mData = materialLabel.Data as IRobotMaterialData;

            Material bhomMat;

            switch (mData.Type)
            {
                case IRobotMaterialType.I_MT_STEEL:
                    bhomMat = Engine.Structure.Create.SteelMaterial(mData.Name, mData.E, mData.NU, mData.LX, mData.RO / Engine.Robot.Query.RobotGravityConstant, mData.DumpCoef, mData.RE, mData.RT);
                    break;
                case IRobotMaterialType.I_MT_CONCRETE:
                    bhomMat = Engine.Structure.Create.ConcreteMaterial(mData.Name, mData.E, mData.NU, mData.LX, mData.RO / Engine.Robot.Query.RobotGravityConstant, mData.DumpCoef, 0, 0);
                    break;
                case IRobotMaterialType.I_MT_OTHER:
                case IRobotMaterialType.I_MT_TIMBER:
                case IRobotMaterialType.I_MT_ALUMINIUM:
                case IRobotMaterialType.I_MT_ALL:
                default:
                    bhomMat = new Material() { Density = mData.RO / Engine.Robot.Query.RobotGravityConstant, Name = mData.Name };
                    Engine.Reflection.Compute.RecordWarning("Material of Robot type " + mData.Type + " not yet suported. Empty material will be provided");
                    break;
            }
           
            bhomMat.CustomData.Add(AdapterId, NextId(bhomMat.GetType(), false));
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

    }
}

