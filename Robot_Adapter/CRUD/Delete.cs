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

            if (type == typeof(Bar))
                return DeleteBars(ids);

            return 0;
        }

        public int DeleteNodes(IEnumerable<object> ids)
        {
            int sucess = 1;
            RobotSelection nodeSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            List<int> indicies = ids.Cast<int>().ToList();
            if (ids != null)
            {
                foreach (int ind in indicies)
                {
                    nodeSel.AddOne(ind);
                }
            }
            else
                nodeSel = m_RobotApplication.Project.Structure.Nodes.GetAll() as RobotSelection;


            if (nodeSel.Count == indicies.Count())
            {
                m_RobotApplication.Project.Structure.Nodes.DeleteMany(nodeSel);
                return sucess;
            }

            return 0;
        }

        public int DeleteBars(IEnumerable<object> ids)
        {
            int sucess = 1;
            RobotSelection barSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            List<int> indicies = ids.Cast<int>().ToList();
            if (ids != null)
            {
                foreach (int ind in indicies)
                {
                    barSel.AddOne(ind);
                }
            }
            else
                barSel = m_RobotApplication.Project.Structure.Bars.GetAll() as RobotSelection;


            if (barSel.Count == indicies.Count())
            {
                m_RobotApplication.Project.Structure.Bars.DeleteMany(barSel);
                return sucess;
            }

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

