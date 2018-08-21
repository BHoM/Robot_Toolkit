using System.Collections.Generic;
using BH.oM.Structure.Properties;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

         private bool CreateCollection(IEnumerable<BarRelease> releases)
        {
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel lable = labelServer.Create(IRobotLabelType.I_LT_BAR_RELEASE, "");
            IRobotBarReleaseData rData = lable.Data;

            foreach (BarRelease bRelease in releases)
            {
                BH.Engine.Robot.Convert.RobotRelease(rData.StartNode, bRelease.StartRelease);
                BH.Engine.Robot.Convert.RobotRelease(rData.EndNode, bRelease.EndRelease);
                labelServer.StoreWithName(lable, bRelease.Name);
            }
            return true;
        }

        /***************************************************/

    }

}

