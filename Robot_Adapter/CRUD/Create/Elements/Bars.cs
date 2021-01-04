/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Reflection.Debugging;
using BH.oM.Adapters.Robot;
using BH.Engine.Structure;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Base;
using BH.oM.Base;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/
   
        private bool CreateCollection(IEnumerable<Bar> bhomBars)
        {
            if (bhomBars != null)
            {
                IRobotStructure rStructure = m_RobotApplication.Project.Structure;
                IRobotBarServer barServer = rStructure.Bars;

                List<Bar> bars = bhomBars.ToList();
                IRobotStructureCache rcache = rStructure.CreateCache();
                RobotSelection rSelect = rStructure.Selections.Create(IRobotObjectType.I_OT_BAR);
                string tensionBars = "";
                string compressionBars = "";
                string axialBars = "";
                int barNum = 0;
                Dictionary<int, HashSet<string>> barTags = GetTypeTags(typeof(Bar));

                List<Bar> nonCacheBars = new List<Bar>();
                foreach (Bar bhomBar in bars)
                {
                    //Check bar itself is not null and correctly set up and extract ID information
                    if (!CheckInputObjectAndExtractAdapterIdInt(bhomBar, out barNum, EventType.Error))
                        continue;

                    int stNodeId, endNodeId;

                    //Check nodes are not null and correctly set up and extract id information
                    if (!CheckInputObjectAndExtractAdapterIdInt(bhomBar.StartNode, out stNodeId, EventType.Error, typeof(Bar)) ||
                        !CheckInputObjectAndExtractAdapterIdInt(bhomBar.EndNode, out endNodeId, EventType.Error, typeof(Bar)))
                        continue;

                    //Check positions of input nodes are not null
                    if (!CheckNotNull(bhomBar.StartNode.Position, EventType.Error, typeof(Node)) ||
                        !CheckNotNull(bhomBar.EndNode.Position, EventType.Error, typeof(Node)))
                        continue;

                    string sectionName = "";
                    string materialName = "";
                    if (CheckNotNull(bhomBar.SectionProperty, EventType.Warning, typeof(Bar)))
                    {
                        sectionName = Convert.Match(m_dbSecPropNames, bhomBar.SectionProperty);
                        if (sectionName == null)
                            sectionName = bhomBar.SectionProperty.DescriptionOrName();

                        if (CheckNotNull(bhomBar.SectionProperty.Material, EventType.Warning, typeof(Bar)))
                        {
                            materialName = Convert.Match(m_dbMaterialNames, bhomBar.SectionProperty.Material);

                            if (materialName == null)
                                materialName = bhomBar.SectionProperty.Material.DescriptionOrName();
                        }
                    }


                    double orientationAngle = bhomBar.ToRobotOrientationAngle();
                          
                    rcache.AddBar(barNum,
                                  stNodeId,
                                  endNodeId,
                                  sectionName,
                                  materialName,
                                  orientationAngle);

                    if (!string.IsNullOrWhiteSpace(bhomBar.Name))
                        rcache.SetBarName(barNum, bhomBar.Name);


                    if (bhomBar.Release != null)
                        rcache.SetBarLabel(barNum, IRobotLabelType.I_LT_BAR_RELEASE, bhomBar.Release.DescriptionOrName());
                    else
                        Engine.Reflection.Compute.RecordNote("Bars with no assigned releases will use defaults in Robot.");

                    if (bhomBar.Offset != null && bhomBar.Offset.Start != null && bhomBar.Offset.End != null && (bhomBar.Offset.Start.SquareLength() > 0 || bhomBar.Offset.End.SquareLength() > 0))
                        rcache.SetBarLabel(barNum, IRobotLabelType.I_LT_BAR_OFFSET, bhomBar.Offset.DescriptionOrName());


                    IFragment designFragment;
                    if (bhomBar.Fragments.TryGetValue(typeof(FramingElementDesignProperties), out designFragment))
                    {
                        FramingElementDesignProperties framEleDesProps = designFragment as FramingElementDesignProperties;
                        if (framEleDesProps != null)
                        {
                            if (m_RobotApplication.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name) != -1)
                            {
                                Create(framEleDesProps);
                            }
                            else
                            {
                                List<FramingElementDesignProperties> framEleDesPropsList = new List<FramingElementDesignProperties>();
                                framEleDesPropsList.Add(framEleDesProps);
                                Update(framEleDesPropsList);
                            }
                            rcache.SetBarLabel(barNum, IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name);
                        }
                    }                  


                    if (bhomBar.FEAType == BarFEAType.TensionOnly)
                        tensionBars = tensionBars + barNum.ToString() + ",";
                    else if (bhomBar.FEAType == BarFEAType.CompressionOnly)
                        compressionBars = compressionBars + barNum.ToString() + ",";
                    else if (bhomBar.FEAType == BarFEAType.Axial)
                        axialBars = axialBars + barNum.ToString() + ",";
                }
                
                tensionBars.TrimEnd(',');
                compressionBars.TrimEnd(',');
                axialBars.TrimEnd(',');

                m_RobotApplication.Project.Structure.ApplyCache(rcache as RobotStructureCache);

                rSelect.FromText(tensionBars);
                barServer.SetTensionCompression(rSelect, IRobotBarTensionCompression.I_BTC_TENSION_ONLY);
                rSelect.FromText(compressionBars);
                barServer.SetTensionCompression(rSelect, IRobotBarTensionCompression.I_BTC_COMPRESSION_ONLY);
                barServer.SetTrussBar(axialBars, true);
                
                

                foreach (Bar bhomBar in bars)
                {
                    try
                    {
                        barNum = GetAdapterId<int>(bhomBar);
                        RobotBar rBar = barServer.Get(barNum) as RobotBar;
                        barTags[barNum] = bhomBar.Tags;
                        Convert.SetFEAType(rBar, bhomBar);
                    }
                    catch
                    {
                        Engine.Reflection.Compute.RecordWarning("Failed to set FEA type for at least one bar.");
                    }
                }
                m_tags[typeof(Bar)] = barTags;

            }
            return true;
        }

        /***************************************************/

    }

}



