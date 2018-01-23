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
using BH.oM.Common.Materials;
using Robot_Adapter.Structural.Elements;
using BH.oM.Structural.Design;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter : BHoMAdapter
    {

        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/

        protected override int Delete(Type type, IEnumerable<object> ids)
        {
            if (type == typeof(DesignGroup))
            {
                DeleteSteelDesignGroups(ids as List<int>);
            }
                return 0;
        }

        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        public void DeleteSteelDesignGroups(List<int> designGroupNumbers = null)
        {
            RobotApplication robot = this.RobotApplication;
            RDimServer RDServer = this.RobotApplication.Kernel.GetExtension("RDimServer");
            RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
            RDimGroups RDGroups = RDServer.GroupsService;

            if (designGroupNumbers != null)
            {
                foreach (int designGroupNumber in designGroupNumbers)
                {
                    _DeleteSteelDesignGroup(designGroupNumber);
                }
            }
            else
            {
                for (int i = 0; i <= RDGroups.Count - 1; i++)
                {
                    int designGroupNumber = RDGroups.GetUserNo(i);
                    _DeleteSteelDesignGroup(designGroupNumber);

                }
            }
        }

        private void _DeleteSteelDesignGroup(int designGroupNumber)
        {
            RobotApplication robot = this.RobotApplication;
            RDimServer RDServer = this.RobotApplication.Kernel.GetExtension("RDimServer");
            RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
            RDimGroups RDGroups = RDServer.GroupsService;
            RDGroups.Delete(designGroupNumber);            
        }



        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

