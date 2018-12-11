using System.Collections.Generic;
using System.Linq;
using RobotOM;
using BH.oM.Structure.Properties;
using BH.oM.Common.Materials;

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
            Dictionary<string, Material> materials = ReadMaterial().ToDictionary(x => x.Name);

            for (int i = 1; i <= secProps.Count; i++)
            {
                ISectionProperty bhomSec = null;
                Material bhomMat = null;
                IRobotLabel rSection = secProps.Get(i);
                IRobotBarSectionData secData = rSection.Data as IRobotBarSectionData;
                materials.TryGetValue(secData.MaterialName, out bhomMat);

                   bhomSec = BH.Engine.Robot.Convert.IBHoMSection(secData, bhomMat);

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

