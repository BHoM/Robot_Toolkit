using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using RobotOM;

using BH.Engine.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/
             
        private bool CreateCollection<T>(IEnumerable<BH.oM.Base.BHoMGroup<T>> groups) where T : BH.oM.Base.IBHoMObject
        {
            RobotGroupServer rGroupServer = m_RobotApplication.Project.Structure.Groups;
            foreach (BHoMGroup<T> group in groups)
            {
                IRobotObjectType rType = BH.Engine.Robot.Convert.RobotObjectType(typeof(T));
                string members = group.Elements.Select(x => int.Parse(x.CustomData[BH.Engine.Robot.Convert.AdapterID].ToString())).GeterateIdString();
                rGroupServer.Create(rType, group.Name, members);
            }
            return true;
        }

        /***************************************************/

    }

}

