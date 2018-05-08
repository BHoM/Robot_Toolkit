using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.DataManipulation.Queries;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Loads;
using RobotOM;
using System.Diagnostics;
using BH.oM.Adapters.Robot;
using BH.Engine.Adapters.Robot;
using BH.oM.Reflection.Debuging;
using BH.Engine.Reflection;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public AdvancedSettings AdvancedSettings;
        public DatabaseSettings DatabaseSettings;

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public RobotAdapter(string FilePath = "", DatabaseSettings DatabaseSettings = null, AdvancedSettings AdvancedSettings = null, bool Active = false)
        {
            if (Active)
            {

                AdapterId = Engine.Robot.Convert.AdapterID;

                this.AdvancedSettings = (AdvancedSettings != null)? AdvancedSettings : new AdvancedSettings();
                this.DatabaseSettings = (DatabaseSettings != null)? DatabaseSettings: new DatabaseSettings();
                
                Config.SeparateProperties = true;
                Config.MergeWithComparer = true;
                Config.ProcessInMemory = false;

                m_matDataBaseName = this.DatabaseSettings.materialDatabase.ToString();
                m_secPropDataBaseName = this.DatabaseSettings.sectionDatabase.ToString();

                if (IsApplicationRunning())
                {
                    m_RobotApplication = new RobotApplication();
                }
                else
                {
                    try
                    {
                        m_RobotApplication = new RobotApplication();
                        m_RobotApplication.Visible = 1;
                        m_RobotApplication.Interactive = 1;
                        m_RobotApplication.Project.New(IRobotProjectType.I_PT_SHELL);
                    }
                    catch
                    {
                        Compute.RecordEvent("Cannot load Robot, check that Robot is installed and a license is available", EventType.Error);
                        Console.WriteLine("Cannot load Robot, check that Robot is installed and a license is available");
                    }
                }

                ReadMaterialNamesFromDB(m_matDataBaseName);
                ReadSecPropNamesFromDB(m_secPropDataBaseName);

                if (!string.IsNullOrWhiteSpace(FilePath))
                {
                    m_RobotApplication.Project.Open(FilePath);
                }
            }
        }

        /***************************************************/

        /***************************************************/
        /**** Public  Fields                            ****/
        /***************************************************/

        /***************************************************/
        /**** Public  Methods                           ****/
        /***************************************************/

        public static bool IsApplicationRunning()
        {
            return (Process.GetProcessesByName("robot").Length > 0) ? true : false;
        }

        private void updateview()
        {
            m_RobotApplication.Project.ViewMngr.Refresh();
        }

        private Dictionary<int, HashSet<string>> GetTypeTags(Type t)
        {
            Dictionary<int, HashSet<string>> typeTags;

            if (!m_tags.TryGetValue(t, out typeTags))
                typeTags = new Dictionary<int, HashSet<string>>();

            m_tags[t] = typeTags;

            return typeTags;
        }

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

        private void ReadSecPropNamesFromDB(string dbName)
        {
            m_RobotApplication.Project.Preferences.SetCurrentDatabase(IRobotDatabaseType.I_DT_SECTIONS, dbName);
            RobotSectionDatabaseList dbList = m_RobotApplication.Project.Preferences.SectionsActive;
            List<string> secNames = new List<string>();
            RobotSectionDatabase rDataBase = null;

            for (int i = 1; i < dbList.Count; i++)
            {
                if (dbList.GetDatabase(i).Name == "UKST")
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
        /**** Private Fields                            ****/
        /***************************************************/

        private RobotApplication m_RobotApplication;
        private Dictionary<Type, Dictionary<int, HashSet<string>>> m_tags = new Dictionary<Type, Dictionary<int, HashSet<string>>>();
        private string m_matDataBaseName;
        private string m_secPropDataBaseName;
        private List<string> m_dbMaterialNames = new List<string>();
        private List<string> m_dbSecPropNames = new List<string>();
        //private Dictionary<int, string> m_NodeTaggs = new Dictionary<int, string>();
        //private Dictionary<string, string> m_MaterialTaggs = new Dictionary<string, string>();
        //private Dictionary<string, string> m_SectionPropertyTaggs = new Dictionary<string, string>();
        //private Dictionary<string, string> m_SupportTaggs = new Dictionary<string, string>();
    }
}