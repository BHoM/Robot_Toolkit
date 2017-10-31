using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
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
            bool success = true;
            if (typeof(BH.oM.Structural.Design.DesignGroup).IsAssignableFrom(typeof(T)))
            {
                foreach (T obj in objects)
                {
                    CreateSteelDesignGroup(obj as BH.oM.Structural.Design.DesignGroup);
                }
            }
            return success;
        }

        public void CreateSteelDesignGroup(BH.oM.Structural.Design.DesignGroup bhomdesignGroup)
        {
            RobotApplication robot = this.RobotApplication;
            RDimServer RDServer = this.RobotApplication.Kernel.GetExtension("RDimServer");
            RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
            RDimStream RDStream = RDServer.Connection.GetStream();
            RDimGroups RDGroups = RDServer.GroupsService;
            RDimGrpProfs RDGroupProfs = RDServer.Connection.GetGrpProfs();
            RDimGroup designGroup =  RDGroups.New(0, bhomdesignGroup.Number);
            designGroup.Name = bhomdesignGroup.Name;
            designGroup.Material = bhomdesignGroup.MaterialName;
            RDStream.Clear();
            RDStream.WriteText(Convert.FromSelectionList(bhomdesignGroup.MemberIds));
            designGroup.SetMembList(RDStream);      
            designGroup.SetProfs(RDGroupProfs);
            RDGroups.Save(designGroup);        
        }
     
        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

