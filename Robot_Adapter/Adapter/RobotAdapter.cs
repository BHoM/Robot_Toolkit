/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using System;
using System.IO;
using System.Collections.Generic;
using RobotOM;
using System.Diagnostics;
using BH.oM.Adapters.Robot;
using BH.Engine.Adapters.Robot;
using BH.oM.Data.Requests;

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

        public RobotAdapter(string filePath = "", RobotConfig robotConfig = null, bool active = false)
        {
            //Initialise
            AdapterIdFragmentType = typeof(RobotId);
            SetupComparers();
            SetupDependencies();
            BH.Adapter.Modules.Structure.ModuleLoader.LoadModules(this);

            if (active)
            {

                RobotConfig = (robotConfig == null) ? new RobotConfig() : robotConfig;

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
                        Console.WriteLine("Cannot load Robot, check that Robot is installed and a license is available");
                    }
                }

                SetProjectPreferences(RobotConfig);

                ReadMaterialNamesFromDB(RobotConfig.DatabaseSettings.MaterialDatabase.ToString());
                ReadSecPropNamesFromDB(RobotConfig.DatabaseSettings.SectionDatabase.ToString());

                if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
                {
                    m_RobotApplication.Project.Open(filePath);
                }
            }
        }

        /***************************************************/
        /**** Public  Methods                           ****/
        /***************************************************/

        public static bool IsApplicationRunning()
        {
            return (Process.GetProcessesByName("robot").Length > 0) ? true : false;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private RobotApplication m_RobotApplication;
        private Dictionary<Type, Dictionary<int, HashSet<string>>> m_tags = new Dictionary<Type, Dictionary<int, HashSet<string>>>();
        private Dictionary<Type, HashSet<string>> m_exisitingGroups = new Dictionary<Type, HashSet<string>>();
        private List<string> m_dbMaterialNames = new List<string>();
        private Dictionary<string, oM.Physical.Materials.Material> m_dbMaterials = new Dictionary<string, oM.Physical.Materials.Material>();
        private List<string> m_dbSecPropNames = new List<string>();

        /***************************************************/
        /**** Private Helper Methods                    ****/
        /***************************************************/

        private void UpdateView()
        {
            m_RobotApplication.Project.ViewMngr.Refresh();
        }

        /***************************************************/

        private Dictionary<int, HashSet<string>> GetTypeTags(Type t)
        {
            Dictionary<int, HashSet<string>> typeTags;

            if (!m_tags.TryGetValue(t, out typeTags))
            {
                HashSet<string> existingGroups;
                typeTags = GetGroupTags(t, out existingGroups);
                m_exisitingGroups[t] = existingGroups;
                m_tags[t] = typeTags;
            }
            return typeTags;
        }

        /***************************************************/

        private void ClearCashedTags()
        {
            m_tags = new Dictionary<Type, Dictionary<int, HashSet<string>>>();
            m_exisitingGroups = new Dictionary<Type, HashSet<string>>();
        }

        private void SetProjectPreferences(RobotConfig robotConfig)
        {
            if (robotConfig.DatabaseSettings.SteelDesignCode != DesignCode_Steel.Default)
                m_RobotApplication.Project.Preferences.SetActiveCode(IRobotCodeType.I_CT_STEEL_STRUCTURES, Query.GetStringFromEnum(robotConfig.DatabaseSettings.SteelDesignCode));

            m_RobotApplication.Project.Preferences.SetCurrentDatabase(IRobotDatabaseType.I_DT_SECTIONS, Query.GetStringFromEnum(robotConfig.DatabaseSettings.SectionDatabase));
            m_RobotApplication.Project.Preferences.Materials.Load(Query.GetStringFromEnum(robotConfig.DatabaseSettings.MaterialDatabase));
        }

        /***************************************************/
    }
}





