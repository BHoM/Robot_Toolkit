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
     
        public List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            IRobotCollection secProps = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_SECTION);
            List<ISectionProperty> bhomSectionProps = new List<ISectionProperty>();
            Dictionary<string, Material> materials = ReadMaterial().ToDictionary(x => x.Name);

            for (int i = 1; i <= secProps.Count; i++)
            {
                ISectionProperty bhomSec = null;
                Material bhomMat = null;
                IRobotLabel rSection = secProps.Get(i);
                IRobotBarSectionData secData = rSection.Data as IRobotBarSectionData;

                if (materials.ContainsKey(secData.MaterialName))
                {
                   bhomSec = BH.Engine.Robot.Convert.IBHoMSection(secData, materials[secData.MaterialName]);
                   bhomMat = materials[secData.MaterialName];
                }

                else if(m_dbMaterialNames.Contains(secData.MaterialName))
                {
                    IRobotLabel label = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_MATERIAL, "");
                    IRobotMaterialData matData = label.Data;
                    matData.LoadFromDBase(secData.MaterialName);
                    MaterialType bhomMatType = BH.Engine.Robot.Convert.GetMaterialType(matData.Type);
                    bhomMat = BH.Engine.Common.Create.Material(secData.MaterialName, bhomMatType, matData.E, matData.NU, matData.LX, matData.RO);
                    bhomSec = BH.Engine.Robot.Convert.IBHoMSection(secData, bhomMat);
                }
                if (bhomSec != null)
                {
                    bhomSec.Material = bhomMat;
                    bhomSec.Name = rSection.Name;
                    bhomSec.CustomData.Add(AdapterId, rSection.Name);
                    bhomSectionProps.Add(bhomSec);
                }
            }
            return bhomSectionProps;
        }

        /***************************************************/        

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

