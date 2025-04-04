/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Structure.Design;
using BH.oM.Adapter;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /****           Adapter Methods                 ****/
        /***************************************************/

        protected override int IDelete(Type type, IEnumerable<object> ids, ActionConfig actionConfig = null)
        {
            int success = 0;
            if (type == typeof(DesignGroup))
                success = DeleteSteelDesignGroups(ids as List<int>);
            else if (type == typeof(Node))
                success = DeleteNodes(ids);
            else if (type == typeof(Bar))
                success = DeleteBars(ids);
            else if (type == typeof(Panel))
                success = DeletePanels(ids);
            else if (type == typeof(BH.oM.Structure.Loads.Loadcase))
                success = DeleteLoadcases(ids);
            else if (type == typeof(BH.oM.Structure.Loads.LoadCombination))
                success = DeleteLoadCombinations(ids);
            else if (type == null)
                success = DeleteAll();
            else
                return base.IDelete(type, ids, actionConfig);

            UpdateView();
            return success;
        }

        /***************************************************/

        public int DeleteNodes(IEnumerable<object> ids)
        {
            int sucess = 1;
            string nodeIds = "";
            RobotSelection nodeSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            Dictionary<int, HashSet<string>> nodeTags = GetTypeTags(typeof(Node));
            if (ids != null)
            {
                List<int> indicies = ids.Cast<int>().ToList();
                foreach (int ind in indicies)
                {
                    nodeIds += ind.ToString() + ",";
                    nodeTags.Remove(ind);
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
                    nodeTags.Remove(i);
                }
                nodeIds.TrimEnd(',');
                nodeSel.FromText(nodeIds);
            }
            m_RobotApplication.Project.Structure.Nodes.DeleteMany(nodeSel);
            m_tags[typeof(Node)] = nodeTags;
            return sucess;
        }

        /***************************************************/

        public int DeleteBars(IEnumerable<object> ids)
        {
            int sucess = 1;
            string barIds = "";
            RobotSelection barSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            Dictionary<int, HashSet<string>> barTags = GetTypeTags(typeof(Bar));
            if (ids != null)
            {
                List<int> indicies = ids.Cast<int>().ToList();
                foreach (int ind in indicies)
                {
                    barIds += ind.ToString() + ",";
                    barTags.Remove(ind);
                }
                barIds.TrimEnd(',');
                barSel.FromText(barIds);
                //if (barSel.Count != indicies.Count())
                //{
                //    return 0;
                //}
            }
            else
            {
                int maxNodeIndex = m_RobotApplication.Project.Structure.Bars.FreeNumber;
                for (int i = 1; i < maxNodeIndex; i++)
                {
                    barIds += i.ToString() + ",";
                    barTags.Remove(i);
                }
                barIds.TrimEnd(',');
                barSel.FromText(barIds);
            }
            m_RobotApplication.Project.Structure.Bars.DeleteMany(barSel);
            m_tags[typeof(Bar)] = barTags;
            return sucess;
        }

        /***************************************************/

        public int DeletePanels(IEnumerable<object> ids)
        {
            int sucess = 1;
            string barIds = "";
            RobotSelection panelSections = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);
            Dictionary<int, HashSet<string>> panelTags = GetTypeTags(typeof(Panel));
            if (ids != null)
            {
                List<int> indicies = ids.Cast<int>().ToList();
                foreach (int ind in indicies)
                {
                    barIds += ind.ToString() + ",";
                    panelTags.Remove(ind);
                }
                barIds.TrimEnd(',');
                panelSections.FromText(barIds);
                //if (barSel.Count != indicies.Count())
                //{
                //    return 0;
                //}
            }
            else
            {
                int maxNodeIndex = m_RobotApplication.Project.Structure.Objects.FreeNumber;
                for (int i = 1; i < maxNodeIndex; i++)
                {
                    barIds += i.ToString() + ",";
                    panelTags.Remove(i);
                }
                barIds.TrimEnd(',');
                panelSections.FromText(barIds);
            }
            m_RobotApplication.Project.Structure.Objects.DeleteMany(panelSections);
            m_tags[typeof(Panel)] = panelTags;
            return sucess;
        }

        /***************************************************/

        public int DeleteLoadcases(IEnumerable<object> ids)
        {
            int sucess = 1;
            string caseIds = "";
            RobotSelection caseSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            Dictionary<int, HashSet<string>> caseTags = GetTypeTags(typeof(BH.oM.Structure.Loads.Loadcase));
            if (ids != null)
            {
                List<int> indicies = ids.Cast<int>().ToList();
                foreach (int ind in indicies)
                {
                    caseIds += ind.ToString() + ",";
                    caseTags.Remove(ind);
                }
                caseIds.TrimEnd(',');
                caseSel.FromText(caseIds);
            }
            else
            {
                caseSel = m_RobotApplication.Project.Structure.Selections.CreatePredefined(IRobotPredefinedSelection.I_PS_CASE_SIMPLE_CASES);               
            }            
            m_RobotApplication.Project.Structure.Cases.DeleteMany(caseSel);
            m_tags[typeof(BH.oM.Structure.Loads.Loadcase)] = caseTags;
            if(ids == null || ids.Count() != 0) BH.Engine.Base.Compute.RecordWarning("Note that when all cases are deleted in Robot, combinations are deleted also");
            return sucess;
        }

        /***************************************************/

        public int DeleteLoadCombinations(IEnumerable<object> ids)
        {
            int sucess = 1;
            string caseIds = "";
            RobotSelection caseSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            Dictionary<int, HashSet<string>> caseTags = GetTypeTags(typeof(BH.oM.Structure.Loads.LoadCombination));
            if (ids != null)
            {
                List<int> indicies = ids.Cast<int>().ToList();
                foreach (int ind in indicies)
                {
                    caseIds += ind.ToString() + ",";
                    caseTags.Remove(ind);
                }
                caseIds.TrimEnd(',');
                caseSel.FromText(caseIds);
            }
            else
            {
                caseSel = m_RobotApplication.Project.Structure.Selections.CreatePredefined(IRobotPredefinedSelection.I_PS_CASE_COMBINATIONS);
            }
            m_RobotApplication.Project.Structure.Cases.DeleteMany(caseSel);
            m_tags[typeof(BH.oM.Structure.Loads.LoadCombination)] = caseTags;
            return sucess;
        }

        /***************************************************/

        public int DeleteAll()
        {
            int success = 1;
            m_RobotApplication.Project.Structure.Clear();
            return success;
        }

        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        public int DeleteSteelDesignGroups(List<int> designGroupNumbers = null)
        {
            RobotApplication robot = m_RobotApplication;
            RDimServer RDServer = m_RobotApplication.Kernel.GetExtension("RDimServer");
            RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
            RDimGroups RDGroups = RDServer.GroupsService;

            if (designGroupNumbers != null)
            {
                foreach (int designGroupNumber in designGroupNumbers)
                {
                    DeleteSteelDesignGroup(designGroupNumber);
                }
            }
            else
            {
                for (int i = 0; i <= RDGroups.Count - 1; i++)
                {
                    int designGroupNumber = RDGroups.GetUserNo(i);
                    DeleteSteelDesignGroup(designGroupNumber);

                }
            }
            int success = 1;
            return success;
        }

        /***************************************************/

        private void DeleteSteelDesignGroup(int designGroupNumber)
        {
            RobotApplication robot = m_RobotApplication;
            RDimServer RDServer = m_RobotApplication.Kernel.GetExtension("RDimServer");
            RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
            RDimGroups RDGroups = RDServer.GroupsService;
            RDGroups.Delete(designGroupNumber);
        }

        /***************************************************/

    }

}







