using System.Collections.Generic;
using BH.oM.Structure.Loads;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        /***************************************************/

        private bool CreateCollection(IEnumerable<ILoad> loads)
        {
            RobotCaseServer caseServer = m_RobotApplication.Project.Structure.Cases;
            RobotGroupServer rGroupServer = m_RobotApplication.Project.Structure.Groups;

            foreach (ILoad load in loads)
            {
                IRobotCase rCase = caseServer.Get(load.Loadcase.Number);
                RobotSimpleCase sCase = rCase as RobotSimpleCase;
                BH.Engine.Robot.Convert.IRobotLoad(load, sCase, rGroupServer);
            }

            return true;
        }

        /***************************************************/

    }

}

