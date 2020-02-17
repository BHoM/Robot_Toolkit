/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.Robot;

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

                m_RobotApplication.Interactive = 0;

                List<Bar> bars = bhomBars.ToList();
                IRobotStructureCache rcache = rStructure.CreateCache();
                RobotSelection rSelect = rStructure.Selections.Create(IRobotObjectType.I_OT_BAR);
                string tensionBars = "";
                string compressionBars = "";
                string axialBars = "";
                int barNum = 0;
                int freeNum = m_RobotApplication.Project.Structure.Bars.FreeNumber;
                Dictionary<int, HashSet<string>> barTags = GetTypeTags(typeof(Bar));

                List<Bar> nonCacheBars = new List<Bar>();
                foreach (Bar bhomBar in bars)
                {
                    barNum = System.Convert.ToInt32(bhomBar.CustomData[AdapterIdName]);

                    //if (bhomBar.SectionProperty != null)
                    //{
                    string sectionName = "";
                    string materialName = "";
                    if (bhomBar.SectionProperty != null)
                    {
                        sectionName = Convert.Match(m_dbSecPropNames, bhomBar.SectionProperty);
                        materialName = Convert.Match(m_dbMaterialNames, bhomBar.SectionProperty.Material);
                        if (sectionName == null)
                            sectionName = bhomBar.SectionProperty.Name;
                        if (materialName == null)
                            materialName = bhomBar.SectionProperty.Material.Name;
                    }
                    rcache.AddBar(barNum,
                                  System.Convert.ToInt32(bhomBar.StartNode.CustomData[AdapterIdName]),
                                  System.Convert.ToInt32(bhomBar.EndNode.CustomData[AdapterIdName]),
                                  sectionName,
                                  materialName,
                                  bhomBar.OrientationAngle * 180 / Math.PI);

                    if (bhomBar.Release != null)
                        rcache.SetBarLabel(barNum, IRobotLabelType.I_LT_BAR_RELEASE, bhomBar.Release.Name);
                    else
                        Engine.Reflection.Compute.RecordWarning("Bar with id " + barNum + " did not have any release assigned. Default in Robot will be used");

                    
                    if (bhomBar.CustomData.ContainsKey("FramingElementDesignProperties"))
                    {
                        FramingElementDesignProperties framEleDesProps = bhomBar.CustomData["FramingElementDesignProperties"] as FramingElementDesignProperties;
                        if (m_RobotApplication.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name)!=-1)
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
                        barNum = System.Convert.ToInt32(bhomBar.CustomData[AdapterIdName]);
                        RobotBar rBar = barServer.Get(barNum) as RobotBar;
                        barTags[barNum] = bhomBar.Tags;
                        Convert.ToRobot(rBar, bhomBar);
                    }
                    catch
                    {
                        Engine.Reflection.Compute.RecordWarning("Failed to set FEA type for at least one bar");
                    }
                }
                m_tags[typeof(Bar)] = barTags;

                m_RobotApplication.Interactive = 1;

            }
            return true;
        }

        /***************************************************/

    }

}


