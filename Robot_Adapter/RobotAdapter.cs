using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Adapter.Queries;
using BH.oM.Materials;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.Adapter.Structural;
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
        public bool UseBarQueryMethod = false;
        public bool UseNodeQueryMethod = false;
        
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public RobotAdapter()
        {
            bool robot_active = (Process.GetProcessesByName("robot").Length >0)? true: false;
            if (robot_active)
            {
               RobotApplication = new RobotApplication();
                AdapterId = ID;
           }
        }

        /***************************************************/

        public RobotAdapter(string filePath) : this()
        {
            if (!string.IsNullOrWhiteSpace(filePath))
                RobotApplication.Project.Open(filePath);
            else
                RobotApplication.Project.New(IRobotProjectType.I_PT_SHELL);
            AdapterId = ID;
        }

        public int Update(FilterQuery filter, string property, object newValue, Dictionary<string, string> config = null)
        {
            throw new NotImplementedException();
        }

        protected override object GetNextId(Type type, bool refresh)
        {
            if (type == typeof(BH.oM.Structural.Design.DesignGroup))
            {
                List<int> groupNumbers = new List<int>();
                foreach(BH.oM.Structural.Design.DesignGroup designGroup in ReadDesignGroups())
                {
                    groupNumbers.Add(designGroup.Number);
                }
                groupNumbers.Sort();
                return groupNumbers.Count > 0? groupNumbers.Last() + 1 : 1;
            }
            else
            {
                return null;
            }
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
    }
}