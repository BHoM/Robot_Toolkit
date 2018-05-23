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
using BH.oM.Adapters.Robot;

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

        public bool Create(FramingElementDesignProperties memberType)
        {
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel label = labelServer.CreateLike(IRobotLabelType.I_LT_MEMBER_TYPE, memberType.Name, labelServer.GetDefault(IRobotLabelType.I_LT_MEMBER_TYPE));

            labelServer.Store(label);
            return true;
        }

        /***************************************************/

        public bool CreateCollection(IEnumerable<FramingElementDesignProperties> memberTypes)
        {
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;

            foreach (FramingElementDesignProperties memberType in memberTypes)
            {
                IRobotLabel label = labelServer.CreateLike(IRobotLabelType.I_LT_MEMBER_TYPE, memberType.Name, labelServer.GetDefault(IRobotLabelType.I_LT_MEMBER_TYPE));

                labelServer.Store(label);
            }
            return true;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

