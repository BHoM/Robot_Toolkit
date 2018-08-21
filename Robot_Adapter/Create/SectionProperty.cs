using System.Collections.Generic;
using BH.oM.Structure.Properties;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<ISectionProperty> secProp)
        {
            foreach (ISectionProperty p in secProp)
            {
                IRobotLabel label;
                IRobotBarSectionData secData;
                string match = BH.Engine.Robot.Convert.Match(m_dbSecPropNames, p);
                if (match != null)
                {
                    label = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, match);
                    secData = label.Data;
                    string matName = BH.Engine.Robot.Convert.Match(m_dbMaterialNames, p.Material);
                    if (matName == null)
                        matName = p.Material.Name;
                    
                    secData.LoadFromDBase(match);
                    secData.MaterialName = matName;
                    m_RobotApplication.Project.Structure.Labels.Store(label);
                }

                else
                {
                    label = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, p.Name);
                    secData = label.Data;
                    BH.Engine.Robot.Convert.ISectionProperty(p, secData);
                    m_RobotApplication.Project.Structure.Labels.Store(label);
                }
            }
            return true;
        }

        /***************************************************/
         
    }

}

