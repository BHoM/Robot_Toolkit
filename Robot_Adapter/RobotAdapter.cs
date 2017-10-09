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

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public fields                       ****/
        /***************************************************/

        public bool UseBarQueryMethod = false;
        public bool UseNodeQueryMethod = false;
        
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public RobotAdapter()
        {          
            RobotApplication = new RobotApplication();
        }

        /***************************************************/

        public RobotAdapter(string filePath) : this()
        {
            if (!string.IsNullOrWhiteSpace(filePath))
                RobotApplication.Project.Open(filePath);
            else
                RobotApplication.Project.New(IRobotProjectType.I_PT_SHELL);
        }
               
        public IEnumerable<object> Pull(IEnumerable<IQuery> query, Dictionary<string, string> config = null)
        {
            throw new NotImplementedException();
        }

        public bool Push(IEnumerable<object> objects, string tag = "", Dictionary<string, string> config = null)
        {
            throw new NotImplementedException();
        }

        public int Update(FilterQuery filter, string property, object newValue, Dictionary<string, string> config = null)
        {
            throw new NotImplementedException();
        }

        public object GetNextIndex(Type objectType, bool refresh = false)
        {
            throw new NotImplementedException();
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