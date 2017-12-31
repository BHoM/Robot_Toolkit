using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;

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
                Engine.Robot.Convert.FromBHoMObjects(this.RobotApplication, objects.ToList() as dynamic);
            }
            this.RobotApplication.Project.ViewMngr.Refresh();
            return true;
        }
             
        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

