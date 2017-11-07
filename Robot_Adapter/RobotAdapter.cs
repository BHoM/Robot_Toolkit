using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Adapter.Queries;
using BH.oM.Materials;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Design;
using BH.oM.Structural.Properties;
using RobotOM;
using System.Diagnostics;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public fields                       ****/
        /***************************************************/

        public const string ID = "Robot_id";
        public string AdapterName = "Robot_name";
        public bool UseBarQueryMethod = false;
        public bool UseNodeQueryMethod = false;
                  
                
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public RobotAdapter()
        {
            if (IsApplicationRunning())
            {
               RobotApplication = new RobotApplication();
               AdapterId = ID;
                Config.SeparateProperties = true;
                Config.MergeWithComparer = true;
            }
            else 
            {
                try
                {
                    RobotApplication.Project.New(IRobotProjectType.I_PT_SHELL);
                    AdapterId = ID;
                    Config.SeparateProperties = true;
                    Config.MergeWithComparer = true;
                }
                catch
                {
                    Console.WriteLine("Cannot load Robot, check that Robot is installed and a license is available");
                }
            }

        }

        /***************************************************/

        public RobotAdapter(string filePath) : this()
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                RobotApplication.Project.Open(filePath);
            }
            else if (IsApplicationRunning())
            {
                RobotApplication = new RobotApplication();
                AdapterId = ID;
            }
            else
            {
                RobotApplication.Project.New(IRobotProjectType.I_PT_SHELL);
                AdapterId = ID;
            }
        }

        public int Update(FilterQuery filter, string property, object newValue, Dictionary<string, string> config = null)
        {
            throw new NotImplementedException();
        }

        protected override object GetNextId(Type type, bool refresh)
        {
            int index = 1;
            if (!refresh && m_indexDict.TryGetValue(type, out index))
            {
                index++;
                m_indexDict[type] = index;
            }
            else
            {
                if (type == typeof(DesignGroup))
                {
                    List<int> groupNumbers = new List<int>();
                    foreach (DesignGroup designGroup in ReadDesignGroups())
                    {
                        groupNumbers.Add(designGroup.Number);
                    }
                    groupNumbers.Sort();
                    index = groupNumbers.Count > 0 ? groupNumbers.Last() + 1 : 1;
                }
                if (type == typeof(Bar))
                {
                    index = this.RobotApplication.Project.Structure.Bars.FreeNumber;
                }
                if (type == typeof(Node))
                {
                    index = this.RobotApplication.Project.Structure.Nodes.FreeNumber;
                }
                if (type == typeof(Panel))
                {
                    index = this.RobotApplication.Project.Structure.Objects.FreeNumber;
                }
                m_indexDict[type] = index;
            }
            return index;
        }

        public bool Create(IEnumerable<object> objects)
        {
            throw new NotImplementedException();
        }

        public bool UpdateTags(IEnumerable<object> objects)
        {
            throw new NotImplementedException();
        }

    
        /***************************************************/
        /**** Public  Fields                           ****/
        /***************************************************/

        public RobotApplication RobotApplication;

        /***************************************************/
        /**** Public  Methods                           ****/
        /***************************************************/

        public static void SetConfig(bool barQuery)
        {            
        }

        public static bool IsApplicationRunning()
        {
            return (Process.GetProcessesByName("robot").Length > 0) ? true : false;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private Dictionary<Type, int> m_indexDict = new Dictionary<Type, int>();

    }
}