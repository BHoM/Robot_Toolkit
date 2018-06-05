﻿using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.DataManipulation.Queries;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Loads;
using RobotOM;
using System.Diagnostics;
using BH.oM.Adapters.Robot;
using BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public RobotConfig RobotConfig { get; set; } = new RobotConfig();               
               
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public RobotAdapter(string filePath = "", RobotConfig robotConfig = null, bool Active = false)
        {
            if (Active)
            {
                AdapterId = Engine.Robot.Convert.AdapterID;

                Config.SeparateProperties = true;
                Config.MergeWithComparer = true;
                Config.ProcessInMemory = false;

                if (robotConfig != null)
                    RobotConfig = robotConfig;

                if (IsApplicationRunning())
                {
                    m_RobotApplication = new RobotApplication();
                }
                else
                {
                    try
                    {
                        m_RobotApplication.Visible = 1;
                        m_RobotApplication.Interactive = 1;
                        m_RobotApplication.Project.New(IRobotProjectType.I_PT_SHELL);
                        m_RobotApplication = new RobotApplication();
                    }
                    catch
                    {
                        Console.WriteLine("Cannot load Robot, check that Robot is installed and a license is available");
                    }
                }

                ReadMaterialNamesFromDB(RobotConfig.DatabaseSettings.MaterialDatabase.ToString());
                ReadSecPropNamesFromDB(RobotConfig.DatabaseSettings.SectionDatabase.ToString());

                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    m_RobotApplication.Project.Open(filePath);
                }

                SetProjectPreferences(m_RobotApplication, robotConfig);
            }
        }

        /***************************************************/

        /***************************************************/
        /**** Public  Fields                           ****/
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

        public static bool SetProjectPreferences(RobotApplication robotApp, RobotConfig robotConfig)
        {
            robotApp.Project.Preferences.SetActiveCode(IRobotCodeType.I_CT_STEEL_STRUCTURES, Query.GetStringFromEnum(robotConfig.DatabaseSettings.SteelDesignCode));
            robotApp.Project.Preferences.SetCurrentDatabase(IRobotDatabaseType.I_DT_SECTIONS, Query.GetStringFromEnum(robotConfig.DatabaseSettings.SectionDatabase));
            return true;
        }
        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private RobotApplication m_RobotApplication;
        private Dictionary<Type, Dictionary<int, HashSet<string>>> m_tags = new Dictionary<Type, Dictionary<int, HashSet<string>>>();
        private List<string> m_dbMaterialNames = new List<string>();
        private List<string> m_dbSecPropNames = new List<string>();
        private RobotConfig m_robotConfig = new RobotConfig();
        //private Dictionary<int, string> m_NodeTaggs = new Dictionary<int, string>();
        //private Dictionary<string, string> m_MaterialTaggs = new Dictionary<string, string>();
        //private Dictionary<string, string> m_SectionPropertyTaggs = new Dictionary<string, string>();
        //private Dictionary<string, string> m_SupportTaggs = new Dictionary<string, string>();


        /***************************************************/
        /**** Private Helper Methods                    ****/
        /***************************************************/

        
    }
}