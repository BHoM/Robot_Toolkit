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
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<BH.oM.Adapters.Robot.DesignGroup> bhomdesignGroups)
        {
            foreach (DesignGroup bhomdesignGroup in bhomdesignGroups)
            {
                RDimServer RDServer = m_RobotApplication.Kernel.GetExtension("RDimServer");
                RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
                RDimStream RDStream = RDServer.Connection.GetStream();
                RDimGroups RDGroups = RDServer.GroupsService;
                RDimGrpProfs RDGroupProfs = RDServer.Connection.GetGrpProfs();               
                RDimGroup designGroup = RDGroups.New(0, bhomdesignGroup.Number);
                designGroup.Name = bhomdesignGroup.Name;
                designGroup.Material = bhomdesignGroup.MaterialName;
                RDStream.Clear();
                RDStream.WriteText(BH.Engine.Robot.Convert.FromSelectionList(bhomdesignGroup.MemberIds));
                designGroup.SetMembList(RDStream);
                designGroup.SetProfs(RDGroupProfs);
                RDGroups.Save(designGroup);
            }
            return true;
        }
                
        /***************************************************/
    }

}


