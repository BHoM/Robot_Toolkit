using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.Engine.Serialiser;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Base;
using BH.oM.Common.Materials;
using BH.oM.Structural.Design;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {         
        
        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/
        
        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/
             
        /***************************************************/
    
        public List<BH.oM.Adapters.Robot.DesignGroup> ReadDesignGroups()
        {
            RobotApplication robot = m_RobotApplication;
            RDimServer RDServer = m_RobotApplication.Kernel.GetExtension("RDimServer");
            RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
            RDimStream RDStream = RDServer.Connection.GetStream();
            RDimGroups RDGroups = RDServer.GroupsService;
            RDimGrpProfs RDGroupProfs = RDServer.Connection.GetGrpProfs();
            List<BH.oM.Adapters.Robot.DesignGroup> designGroupList = new List<BH.oM.Adapters.Robot.DesignGroup>();

            for (int i = 0; i <= RDGroups.Count - 1; i++)
            {
                int designGroupNumber = RDGroups.GetUserNo(i);
                RDimGroup designGroup = RDGroups.Get(designGroupNumber);
                BH.oM.Adapters.Robot.DesignGroup bhomDesignGroup = new BH.oM.Adapters.Robot.DesignGroup();
                bhomDesignGroup.Name = designGroup.Name;
                bhomDesignGroup.Number = designGroup.UsrNo;
                bhomDesignGroup.CustomData[AdapterId] = designGroup.UsrNo;
                bhomDesignGroup.CustomData[Engine.Robot.Convert.AdapterName] = designGroup.Name;
                bhomDesignGroup.MaterialName = designGroup.Material;
                designGroup.GetMembList(RDStream);
                if (RDStream.Size(IRDimStreamType.I_DST_TEXT) > 0)
                    bhomDesignGroup.MemberIds = Engine.Robot.Convert.ToSelectionList(RDStream.ReadText());
                designGroupList.Add(bhomDesignGroup);
            }
            return designGroupList;
        }

        /***************************************************/
        

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

