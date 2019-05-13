/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using RobotOM;
using BH.Engine.Serialiser;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Loads;
using BH.oM.Base;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.Design;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;
using BH.Engine.Base.Objects;
using BH.Engine.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Bar> ReadBars(IList ids = null)
        {
            List<int> barIds = CheckAndGetIds(ids);

            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionaryDistinctCheck(x => x.CustomData[AdapterId].ToString());
            Dictionary<string, BarRelease> bhombarReleases = ReadBarRelease().ToDictionaryDistinctCheck(x => x.Name.ToString());
            Dictionary<string, ISectionProperty> bhomSections = ReadSectionProperties().ToDictionaryDistinctCheck(x => x.Name.ToString());
            Dictionary<string, IStructuralMaterial> bhomMaterial = ReadMaterial().ToDictionaryDistinctCheck(x => x.Name.ToString());
            Dictionary<string, FramingElementDesignProperties> bhomFramEleDesProps = ReadFramingElementDesignProperties().ToDictionaryDistinctCheck(x => x.Name.ToString());
            Dictionary<int, HashSet<string>> barTags = GetTypeTags(typeof(Bar));
            Dictionary<string, Dictionary<string,ISectionProperty>> sectionWithMaterial = new Dictionary<string, Dictionary<string, ISectionProperty>>();  //Used to store sections where the material differs from the default
            HashSet<string> tags = new HashSet<string>();

            m_RobotApplication.Project.Structure.Bars.BeginMultiOperation();
            if (barIds == null || barIds.Count == 0)
            {
                IRobotCollection robotBars = m_RobotApplication.Project.Structure.Bars.GetAll();
                for (int i = 1; i <= robotBars.Count; i++)
                {
                    RobotBar robotBar = robotBars.Get(i);
                    Bar bhomBar = BH.Engine.Robot.Convert.ToBHoMObject( robotBar, 
                                                                        bhomNodes, 
                                                                        bhomSections, 
                                                                        bhomMaterial, 
                                                                        bhombarReleases,
                                                                        bhomFramEleDesProps,
                                                                        ref sectionWithMaterial);
                    bhomBar.CustomData[AdapterId] = robotBar.Number;
                    if (barTags != null && barTags.TryGetValue(robotBar.Number, out tags))
                        bhomBar.Tags = tags;
                    bhomBars.Add(bhomBar);
                }
            }
            else
            {
                for (int i = 0; i < barIds.Count; i++)
                {
                    RobotBar robotBar = m_RobotApplication.Project.Structure.Bars.Get(barIds[i]) as RobotBar;
                    Bar bhomBar = BH.Engine.Robot.Convert.ToBHoMObject( robotBar, 
                                                                        bhomNodes, 
                                                                        bhomSections, 
                                                                        bhomMaterial,
                                                                        bhombarReleases,
                                                                        bhomFramEleDesProps,
                                                                        ref sectionWithMaterial);
                    bhomBar.CustomData[AdapterId] = robotBar.Number;
                    if (barTags != null && barTags.TryGetValue(robotBar.Number, out tags))
                        bhomBar.Tags = tags;
                    bhomBars.Add(bhomBar);
                }
            }
            m_RobotApplication.Project.Structure.Bars.EndMultiOperation();

            //Postprocess the used sections
            PostProcessBarSections(sectionWithMaterial);

            return bhomBars;
        }

        /***************************************************/

        private void PostProcessBarSections(Dictionary<string, Dictionary<string, ISectionProperty>> sectionsWithMaterial)
        {
            //If more than one material is used for a specific section, append the material name to the
            //end of the name of the section
            foreach (KeyValuePair<string,Dictionary<string, ISectionProperty>> outerKvp in sectionsWithMaterial)
            {

                if (outerKvp.Value.Count == 1)
                    continue;

                foreach (KeyValuePair<string,ISectionProperty> innerKvp in outerKvp.Value)
                {
                    innerKvp.Value.Name = innerKvp.Value.Name + "-" + innerKvp.Value.Material.Name;
                }

            }
        }

        /***************************************************/

        //Fast query method - returns basic bar information, not full bar objects
        private List<Bar> ReadBarsQuery(List<string> ids = null)
        {
            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodesQuery();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.Name.ToString());

            RobotResultQueryParams result_params = default(RobotResultQueryParams);
            result_params = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
            RobotStructure rstructure = default(RobotStructure);
            rstructure = m_RobotApplication.Project.Structure;
            RobotSelection cas_sel = default(RobotSelection);
            RobotSelection bar_sel = default(RobotSelection);
            IRobotResultQueryReturnType query_return = default(IRobotResultQueryReturnType);

            bool ok = false;
            RobotResultRow result_row = default(RobotResultRow);
            int bar_num = 0;

            int nod1 = 0;
            int nod2 = 0;

            int nod1_id = 15;
            int nod2_id = 16;

            bar_sel = m_RobotApplication.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_BAR);
            cas_sel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            try
            {
                cas_sel.FromText(m_RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
            }
            catch
            {
                m_RobotApplication.Project.Structure.Cases.CreateSimple(1, "Dead Load", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                cas_sel.FromText(m_RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
            }

            result_params.ResultIds.SetSize(5);
            result_params.ResultIds.Set(1, nod1_id);
            result_params.ResultIds.Set(2, nod2_id);
            result_params.ResultIds.Set(3, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
            result_params.ResultIds.Set(4, 269);
            result_params.ResultIds.Set(5, 270);

            result_params.SetParam(IRobotResultParamType.I_RPT_BAR_RELATIVE_POINT, 0);
            result_params.Selection.Set(IRobotObjectType.I_OT_BAR, bar_sel);
            result_params.Selection.Set(IRobotObjectType.I_OT_CASE, cas_sel);
            result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            RobotResultRowSet row_set = new RobotResultRowSet();

            while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
            {
                query_return = rstructure.Results.Query(result_params, row_set);
                ok = row_set.MoveFirst();
                while (ok)
                {
                    result_row = row_set.CurrentRow;
                    bar_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_BAR);

                    nod1 = (int)result_row.GetValue(nod1_id);
                    nod2 = (int)result_row.GetValue(nod2_id);

                    Node startNode = null; bhomNodes.TryGetValue(nod1.ToString(), out startNode);
                    Node endNode = null; bhomNodes.TryGetValue(nod2.ToString(), out endNode);
                    Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode, Name = bar_num.ToString() };

                    bhomBar.SectionProperty = null;
                    //bhomBar.OrientationAngle = robotBar.Gamma * 180 / Math.PI;
                    bhomBar.Name = bar_num.ToString();

                    bhomBar.CustomData[AdapterId] = bar_num.ToString();
                    bhomBars.Add(bhomBar);

                    ok = row_set.MoveNext();
                }
                row_set.Clear();
            }
            result_params.Reset();
            return bhomBars;
        }

        /***************************************************/
 
    }

}

