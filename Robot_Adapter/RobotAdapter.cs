using System;
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

        public RobotAdapter(string filePath = "", string dataBase = "British")
        {

            AdapterId = Engine.Robot.Convert.AdapterID;

            Config.SeparateProperties = true;
            Config.MergeWithComparer = true;
            Config.ProcessInMemory = false;

            m_dataBaseName = dataBase;
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

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                m_RobotApplication.Project.Open(filePath);
            }
        }

        /***************************************************/

        //public RobotAdapter(string filePath = null) : this()
        //{
        //    if (!string.IsNullOrWhiteSpace(filePath))
        //    {
        //        m_RobotApplication.Project.Open(filePath);
        //    }
        //    else if (IsApplicationRunning())
        //    {
        //        m_RobotApplication = new RobotApplication();
        //    }
        //    //else
        //    //{
        //    //    m_RobotApplication = new RobotApplication();
        //    //    m_RobotApplication.Visible = 1;
        //    //    m_RobotApplication.Interactive = 1;
        //    //    m_RobotApplication.Project.New(IRobotProjectType.I_PT_SHELL);
        //    //}
        //}

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

        private Dictionary<int, HashSet<string>> GetTypeTags(Type t)
        {
            Dictionary<int, HashSet<string>> typeTags;

            if (!m_tags.TryGetValue(t, out typeTags))
                typeTags = new Dictionary<int, HashSet<string>>();

            m_tags[t] = typeTags;

            return typeTags;
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
        private Dictionary<Type, Dictionary<int, HashSet<string>>> m_tags = new Dictionary<Type, Dictionary<int, HashSet<string>>>();
        private string m_dataBaseName;
        //private Dictionary<int, string> m_NodeTaggs = new Dictionary<int, string>();
        //private Dictionary<string, string> m_MaterialTaggs = new Dictionary<string, string>();
        //private Dictionary<string, string> m_SectionPropertyTaggs = new Dictionary<string, string>();
        //private Dictionary<string, string> m_SupportTaggs = new Dictionary<string, string>();
    }
}