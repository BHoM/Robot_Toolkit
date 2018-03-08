﻿using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.DataManipulation.Queries;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Loads;
using RobotOM;
using System.Diagnostics;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public bool UseBarQueryMethod = false;
        public bool UseNodeQueryMethod = false;
                  
                
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public RobotAdapter()
        {
            AdapterId = Engine.Robot.Convert.AdapterID;

            Config.SeparateProperties = true;
            Config.MergeWithComparer = true;
            Config.ProcessInMemory = false;

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
        }

        /***************************************************/

        public RobotAdapter(string filePath = "") : this()
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                m_RobotApplication.Project.Open(filePath);
            }
            else if (IsApplicationRunning())
            {
                m_RobotApplication = new RobotApplication();
            }
            else
            {
                m_RobotApplication = new RobotApplication();
                m_RobotApplication.Visible = 1;
                m_RobotApplication.Interactive = 1;
                m_RobotApplication.Project.New(IRobotProjectType.I_PT_SHELL);
            }
        }

        //public int Update(FilterQuery filter, string property, object newValue, Dictionary<string, string> config = null)
        //{
        //    throw new NotImplementedException();
        //}

     

        //public bool UpdateTags(IEnumerable<object> objects)
        //{
        //    throw new NotImplementedException();
        //}


        /***************************************************/
        /**** Public  Fields                           ****/
        /***************************************************/

        /***************************************************/
        /**** Public  Methods                           ****/
        /***************************************************/

        //public static void SetConfig(bool barQuery)
        //{            
        //}

        public static bool IsApplicationRunning()
        {
            return (Process.GetProcessesByName("robot").Length > 0) ? true : false;
        }

        private void updateview()
        {
            m_RobotApplication.Project.ViewMngr.Refresh();
        }

        //~RobotAdapter()
        //{
        //    //m_RobotApplication.Project.SaveAs(@"C:\Users\phesari\Desktop\Structure.rtd");
        //    m_RobotApplication.Project.Save();
        //}

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private RobotApplication m_RobotApplication;

    }
}