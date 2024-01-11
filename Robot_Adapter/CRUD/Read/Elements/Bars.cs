/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Offsets;
using BH.oM.Base;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.Design;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;
using BH.Engine.Base.Objects;
using BH.Engine.Adapters.Robot;
using BH.Engine.Base;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Bar> ReadBars(IList ids = null)
        {
            List<int> barIds = CheckAndGetIds<Bar>(ids);
            Dictionary<int, HashSet<string>> barTags = GetTypeTags(typeof(Bar));
            List<Bar> bhomBars = new List<Bar>();
            HashSet<string> tags = new HashSet<string>();

            m_RobotApplication.Project.Structure.Bars.BeginMultiOperation();

            IRobotCollection robotBars;

            if (barIds == null || barIds.Count == 0)
                robotBars = m_RobotApplication.Project.Structure.Bars.GetAll();
            else
            {
                RobotSelection barSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
                barSelection.FromText(Convert.ToRobotSelectionString(barIds));
                robotBars = m_RobotApplication.Project.Structure.Bars.GetMany(barSelection);
            }

            for (int i = 1; i <= robotBars.Count; i++)
            {
                RobotBar robotBar = robotBars.Get(i);

                if (robotBar == null)
                {
                    Engine.Base.Compute.RecordError("At least one Bar failed to get extracted from the Robot API.");
                    continue;
                }

                if (!robotBar.IsSuperBar)
                {
                    Bar bhomBar = Convert.FromRobot(robotBar);

                    if (bhomBar == null)
                    {
                        Engine.Base.Compute.RecordError($"Failed convert Bar with number {robotBar.Number}. This bar in not extracted from the model.");
                        continue;
                    }

                    SetAdapterId(bhomBar, robotBar.Number);
                    if (barTags != null && barTags.TryGetValue(robotBar.Number, out tags))
                        bhomBar.Tags = tags;
                    bhomBars.Add(bhomBar);
                }
                else
                {
                    SuperBarWarning();
                }
            }

            m_RobotApplication.Project.Structure.Bars.EndMultiOperation();

            List<int> nodeIds = bhomBars.SelectMany(x => new int[] { int.Parse(x.Start.Name), int.Parse(x.End.Name) }).Distinct().ToList();
            Dictionary<int, Node> bhomNodes = GetCachedOrReadAsDictionary<int, Node>(nodeIds);
            List<string> releaseIds = bhomBars.Select(x => x.Release?.Name).Where(x => x != null).Distinct().ToList();
            Dictionary<string, BarRelease> bhombarReleases = releaseIds.Count == 0 ? new Dictionary<string, BarRelease>() : GetCachedOrReadAsDictionary<string, BarRelease>(releaseIds);
            List<string> materialIds = bhomBars.Select(x => x.SectionProperty?.Material?.Name).Where(x => x != null).Distinct().ToList();
            Dictionary<string, IMaterialFragment> bhomMaterials = materialIds.Count == 0 ? new Dictionary<string, IMaterialFragment>() : GetCachedOrReadAsDictionary<string, IMaterialFragment>(materialIds);
            List<string> sectionIds = bhomBars.Select(x => x.SectionProperty?.Name).Where(x => x != null).Distinct().ToList();
            Dictionary<string, ISectionProperty> bhomSections = sectionIds.Count == 0 ? new Dictionary<string, ISectionProperty>() : GetCachedOrReadAsDictionary<string, ISectionProperty>(sectionIds);
            List<string> offsetIds = bhomBars.Select(x => x.Offset?.Name).Where(x => x != null).Distinct().ToList();
            Dictionary<string, Offset> offsets = offsetIds.Count == 0 ? new Dictionary<string, Offset>() : GetCachedOrReadAsDictionary<string, Offset>(offsetIds);
            List<string> framingElemIds = bhomBars.Select(x => x.FindFragment<FramingElementDesignProperties>()?.Name).Where(x => x != null).Distinct().ToList();
            Dictionary<string, FramingElementDesignProperties> bhomFramEleDesProps = framingElemIds.Count == 0 ? new Dictionary<string, FramingElementDesignProperties>() : GetCachedOrReadAsDictionary<string, FramingElementDesignProperties>(framingElemIds);
            Dictionary<string, Dictionary<string, ISectionProperty>> sectionWithMaterial = new Dictionary<string, Dictionary<string, ISectionProperty>>();  //Used to store sections where the material differs from the default

            foreach (Bar bar in bhomBars)
            {
                bool nodesExtracted = true;
                Node startNode;
                if (bhomNodes.TryGetValue(int.Parse(bar.Start.Name), out startNode))
                    bar.Start = startNode;
                else
                {
                    nodesExtracted = false;
                    Engine.Base.Compute.RecordError($"Failed to extract the {nameof(Bar.Start)} for Bar {this.GetAdapterId(bar)}");
                }

                Node endNode;
                if (bhomNodes.TryGetValue(int.Parse(bar.End.Name), out endNode))
                    bar.End = endNode;
                else
                {
                    nodesExtracted = false;
                    Engine.Base.Compute.RecordError($"Failed to extract the {nameof(Bar.End)} for Bar {this.GetAdapterId(bar)}");
                }

                if(nodesExtracted)
                    bar.OrientationAngle = bar.FromRobotOrientationAngle(bar.OrientationAngle); //Checks for verticality of element

                if (bar.SectionProperty != null)
                {
                    string secName = bar.SectionProperty.Name;
                    ISectionProperty secProp;

                    if (bar.SectionProperty.Material != null)
                    {
                        //Get material name and material
                        string matName = bar.SectionProperty.Material.Name;

                        IMaterialFragment barMaterial = null;
                        if (!bhomMaterials.TryGetValue(matName, out barMaterial))
                        {
                            BH.Engine.Base.Compute.RecordWarning($"Could not extract material with name {matName}. null material will be provided for the crossection for bars with this material.");
                        }
                        Dictionary<string, ISectionProperty> innerDict;
                        //Check if a section of the specified type has allready been pulled
                        if (!sectionWithMaterial.TryGetValue(secName, out innerDict))
                        {
                            innerDict = new Dictionary<string, ISectionProperty>();
                        }

                        //Check if a section of the specified type with the material has allready been added
                        if (!innerDict.TryGetValue(matName, out secProp))
                        {
                            //If not, get out the section from the basic dictionary
                            if (bhomSections.TryGetValue(secName, out secProp))
                            {
                                //Construct and store a copy of the section, with new material
                                secProp = secProp.ShallowClone(true);
                                secProp.Material = barMaterial;
                                innerDict[matName] = secProp;
                                sectionWithMaterial[secName] = innerDict;
                            }
                            else
                            {
                                BH.Engine.Base.Compute.RecordEvent("Section property type " + secName + " is not supported", oM.Base.Debugging.EventType.Warning);
                            }
                        }
                    }
                    else
                    {
                        //No material label appended to the bar. The default section is used, if found
                        if (bhomSections.TryGetValue(secName, out secProp))
                        {
                            Dictionary<string, ISectionProperty> innerDict;
                            if (!sectionWithMaterial.TryGetValue(secName, out innerDict))
                            {
                                innerDict = new Dictionary<string, ISectionProperty>();
                            }
                            if (!innerDict.ContainsKey(secProp.Material.Name))
                            {
                                innerDict[secProp.Material.Name] = secProp;
                                sectionWithMaterial[secName] = innerDict;
                            }
                        }
                        else
                        {
                            BH.Engine.Base.Compute.RecordEvent("Section property type " + secName + " is not supported", oM.Base.Debugging.EventType.Warning);
                        }
                    }

                    bar.SectionProperty = secProp;
                }


                if (bar.Offset != null)
                {
                    Offset offset;
                    if (offsets.TryGetValue(bar.Offset.Name, out offset))
                        bar.Offset = offset;
                    else
                        Engine.Base.Compute.RecordWarning($"Failed to extract the {nameof(Bar.Offset)} for Bar {this.GetAdapterId(bar)}");

                }

                if (bar.Release != null)
                {
                    BarRelease release;
                    if (bhombarReleases.TryGetValue(bar.Release.Name, out release))
                        bar.Release = release;
                    else
                        Engine.Base.Compute.RecordNote("Bars with auto-generated releases in Robot will be pulled with null releases in BHoM.");

                }

                FramingElementDesignProperties dummyFrameProp = bar.FindFragment<FramingElementDesignProperties>();
                if (dummyFrameProp != null)
                {
                    FramingElementDesignProperties frameProp;
                    if (bhomFramEleDesProps.TryGetValue(dummyFrameProp.Name, out frameProp))
                        bar.Fragments.AddOrReplace(frameProp);
                    else
                        Engine.Base.Compute.RecordWarning($"Failed to extract the {nameof(FramingElementDesignProperties)} for Bar {this.GetAdapterId(bar)}");

                }


            }
            
            //Postprocess the used sections
            PostProcessBarSections(sectionWithMaterial);

            return bhomBars;
        }

        /***************************************************/


        private void SuperBarWarning()
        {
            Engine.Base.Compute.RecordWarning("Model contains 'Super Bars' which are not extracted. Reading Bars only extracts the individual bar segments.");
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
            Dictionary<int, Node> bhomNodes = bhomNodesList.ToDictionary(x => GetAdapterId<int>(x));

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

            List<int> barIds = CheckAndGetIds<Bar>(ids);

            if (barIds == null || barIds.Count == 0)
                bar_sel.FromText("all");
            else
                bar_sel.FromText(Convert.ToRobotSelectionString(barIds));

            int elemGamma_id = 20;
            int membGamma_id = 274;

            result_params.ResultIds.SetSize(7);
            result_params.ResultIds.Set(1, nod1_id);
            result_params.ResultIds.Set(2, nod2_id);
            result_params.ResultIds.Set(3, 269);
            result_params.ResultIds.Set(4, 270);
            result_params.ResultIds.Set(5, elemGamma_id); //Gamma angle
            result_params.ResultIds.Set(6, membGamma_id); //Member gamma angle. Without this the element gamma angle is not extract for whatever reason.
            result_params.ResultIds.Set(7, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX); //Seem to be needed to force some proeprties above to be extracted.

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

                    nod1 = (int)TryGetValue(result_row, nod1_id);
                    nod2 = (int)TryGetValue(result_row, nod2_id);

                    Node startNode = null;
                    if (!bhomNodes.TryGetValue(nod1, out startNode))
                        Engine.Base.Compute.RecordError($"Failed to extract the start node of the Bar with number {bar_num}.");

                    Node endNode = null;
                    if (!bhomNodes.TryGetValue(nod2, out endNode))
                        Engine.Base.Compute.RecordError($"Failed to extract the end node of the Bar with number {bar_num}.");

                    Bar bhomBar = new Bar { Start = startNode, End = endNode };

                    double gamma = TryGetValue(result_row, elemGamma_id);

                    gamma = double.IsNaN(gamma) ? 0 : gamma;

                    bhomBar.SectionProperty = null;
                    bhomBar.OrientationAngle = Convert.FromRobotOrientationAngle(bhomBar, gamma);
                    SetAdapterId(bhomBar, bar_num);
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





