using System.Collections.Generic;
using System.Linq;
using System;
using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using RobotOM;

using BH.Engine.Robot;
using BHEG = BH.Engine.Geometry;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Index Adapter Interface                   ****/
        /***************************************************/
        
        /***************************************************/
        
        public bool CreateCollection(IEnumerable<BarRelease> releases)
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

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

