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
            //    bhomMat = BH.Engine.Common.Create.Material(matName, bhomMatType, mData.E, mData.NU, mData.LX, mData.RO);
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
                if (!m_dbMaterialNames.Contains(mData.Name))
                {
                    MaterialType bhomMatType = BH.Engine.Robot.Convert.GetMaterialType(mData.Type);
                    bhomMat = BH.Engine.Common.Create.Material(mData.Name, bhomMatType, mData.E, mData.NU, mData.LX, mData.RO * 0.1);
                    bhomMat.CustomData.Add(AdapterId, NextId(bhomMat.GetType(), false));
                    if (bhomMat != null)
                        bhomMaterials.Add(bhomMat);
                }
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

