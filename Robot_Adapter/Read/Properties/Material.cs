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
using BH.oM.Common.Materials;

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
                mData = rMatLable.Data as IRobotMaterialData;
                //if (!m_dbMaterialNames.Contains(mData.Name))
                //{
                MaterialType bhomMatType = BH.Engine.Robot.Convert.GetMaterialType(mData.Type);
                bhomMat = BH.Engine.Common.Create.Material(mData.Name, bhomMatType, mData.E, mData.NU, mData.LX, mData.RO / Engine.Robot.Query.RobotGravityConstant);
                bhomMat.CustomData.Add(AdapterId, NextId(bhomMat.GetType(), false));
                if (bhomMat != null)
                    bhomMaterials.Add(bhomMat);
                //}
            }

            return bhomMaterials;
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

