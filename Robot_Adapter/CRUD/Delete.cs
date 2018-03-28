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

            int success = 0;

            if (type == typeof(Node))
                success = DeleteNodes(ids);

            if (type == typeof(Bar))
                success = DeleteBars(ids);

            updateview();
            return success;
        }

        public int DeleteNodes(IEnumerable<object> ids)
        {
            int sucess = 1;
            string nodeIds = "";
            RobotSelection nodeSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            if (ids != null)
            {
                List<int> indicies = ids.Cast<int>().ToList();
                foreach (int ind in indicies)
                {
                    nodeIds += ind.ToString() + ",";
                }
                nodeIds.TrimEnd(',');
                nodeSel.FromText(nodeIds);

                if (nodeSel.Count != indicies.Count())
                {
                    return 0;
                }
            }
            else
            {
                int maxNodeIndex = m_RobotApplication.Project.Structure.Nodes.FreeNumber;
                for (int i = 1; i < maxNodeIndex; i++)
                {
                    nodeIds += i.ToString() + ",";
                }
                nodeIds.TrimEnd(',');
                nodeSel.FromText(nodeIds);
            }
            m_RobotApplication.Project.Structure.Nodes.DeleteMany(nodeSel);
            return sucess;

        }

        public int DeleteBars(IEnumerable<object> ids)
        {
            int sucess = 1;
            string barIds = "";
            RobotSelection barSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            if (ids != null)
            {
                List<int> indicies = ids.Cast<int>().ToList();
                foreach (int ind in indicies)
                {
                    barIds += ind.ToString() + ",";
                }
                barIds.TrimEnd(',');
                barSel.FromText(barIds);
                if (barSel.Count != indicies.Count())
                {
                    return 0;
                }
            }
            else
            {
                int maxNodeIndex = m_RobotApplication.Project.Structure.Bars.FreeNumber;
                for (int i = 1; i < maxNodeIndex; i++)
                {
                    barIds += i.ToString() + ",";
                }
                barIds.TrimEnd(',');
                barSel.FromText(barIds);
            }
            m_RobotApplication.Project.Structure.Bars.DeleteMany(barSel);
            return sucess;
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

