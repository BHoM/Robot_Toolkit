using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Design;
using RobotOM;
using BH.oM.Structural.Properties;
using BH.Adapter;
using BH.oM.Base;
using BH.oM.Materials;
using BH.Adapter.Queries;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter : BHoMAdapter
    {

        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/


        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            if (typeof(BHoMObject).IsAssignableFrom(typeof(T)))
            {
                Convert.FromBHoMObjects(this, objects.ToList() as dynamic);
            }
            this.RobotApplication.Project.ViewMngr.Refresh();
            return true;
        }
             
        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

