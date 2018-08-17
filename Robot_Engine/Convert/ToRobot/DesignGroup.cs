using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Design;
using RobotOM;
using System.Collections.Generic;
using BH.oM.Base;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************/

        public static IList<RDimGroup> FromBHoMObjects(RobotApplication robot, List<DesignGroup> bhomdesignGroups)
        {
            List<RDimGroup> robotSteelDesignGroups = new List<RDimGroup>();
            foreach (DesignGroup bhomdesignGroup in bhomdesignGroups)
            {
                RDimServer RDServer = robot.Kernel.GetExtension("RDimServer");
                RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
                RDimStream RDStream = RDServer.Connection.GetStream();
                RDimGroups RDGroups = RDServer.GroupsService;
                RDimGrpProfs RDGroupProfs = RDServer.Connection.GetGrpProfs();
                RDimGroup designGroup = RDGroups.New(0, bhomdesignGroup.Number);
                designGroup.Name = bhomdesignGroup.Name;
                designGroup.Material = bhomdesignGroup.MaterialName;
                RDStream.Clear();
                RDStream.WriteText(Convert.FromSelectionList(bhomdesignGroup.MemberIds));
                designGroup.SetMembList(RDStream);
                designGroup.SetProfs(RDGroupProfs);
                RDGroups.Save(designGroup);
                robotSteelDesignGroups.Add(designGroup);
            }
            return robotSteelDesignGroups;
        }        
    }

}
