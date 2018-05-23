using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.Engine.Serialiser;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Base;
using BH.oM.Common.Materials;
using BH.oM.Structural.Design;
using BH.oM.Adapters.Robot;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {         
        
        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/
               
        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/
             
        /***************************************************/

        public List<Material> ReadMaterial(List<string> ids = null)
        {
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotCollection rMaterials = labelServer.GetMany(IRobotLabelType.I_LT_MATERIAL);
            List<Material> bhomMaterials = new List<Material>();

            for (int i = 1; i <= rMaterials.Count; i++)
            {
                IRobotLabel rMatLable = rMaterials.Get(i);
                IRobotMaterialData mData = rMatLable.Data as IRobotMaterialData;
                MaterialType bhomMatType = BH.Engine.Robot.Convert.GetMaterialType(mData.Type);
                Material bhomMat = BH.Engine.Common.Create.Material(mData.Name, bhomMatType, mData.E, mData.NU, mData.LX, mData.RO);
                bhomMat.CustomData.Add(AdapterId, mData.Name);
                bhomMaterials.Add(bhomMat);
            }
            return bhomMaterials;
        }

        /***************************************************/      
        

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

