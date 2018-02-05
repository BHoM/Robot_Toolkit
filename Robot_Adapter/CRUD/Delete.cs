using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.oM.Structural.Design;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/

        protected override int Delete(Type type, IEnumerable<object> ids)
        {
            //if (type == typeof(DesignGroup))
            //{
            //    DeleteSteelDesignGroups(ids as List<int>);
            //}

            if (type == typeof(Node))
                return DeleteNodes(ids);

            return 0;
        }

        public int DeleteNodes(IEnumerable<object> ids)
        {
            //int sucess = 1;
            //RobotSelection nodeSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            //List<int> indicies = ids.Cast<int>().ToList();

            //foreach (int ind in indicies)
            //{
            //    nodeSel.AddOne(ind);
            //}

            //if (nodeSel.Count == indicies.Count())
            //{
            //    m_RobotApplication.Project.Structure.Nodes.DeleteMany(nodeSel);
            //    return sucess;
            //}

            return 0;
        }

        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        //public void DeleteSteelDesignGroups(List<int> designGroupNumbers = null)
        //{
        //    RobotApplication robot = this.RobotApplication;
        //    RDimServer RDServer = this.RobotApplication.Kernel.GetExtension("RDimServer");
        //    RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
        //    RDimGroups RDGroups = RDServer.GroupsService;

        //    if (designGroupNumbers != null)
        //    {
        //        foreach (int designGroupNumber in designGroupNumbers)
        //        {
        //            _DeleteSteelDesignGroup(designGroupNumber);
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i <= RDGroups.Count - 1; i++)
        //        {
        //            int designGroupNumber = RDGroups.GetUserNo(i);
        //            _DeleteSteelDesignGroup(designGroupNumber);

        //        }
        //    }
        //}

        //private void _DeleteSteelDesignGroup(int designGroupNumber)
        //{
        //    RobotApplication robot = this.RobotApplication;
        //    RDimServer RDServer = this.RobotApplication.Kernel.GetExtension("RDimServer");
        //    RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
        //    RDimGroups RDGroups = RDServer.GroupsService;
        //    RDGroups.Delete(designGroupNumber);            
        //}



        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

