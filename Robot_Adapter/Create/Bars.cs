using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Structural.Elements;
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
                    barNum = System.Convert.ToInt32(bhomBar.CustomData[AdapterId]);

                    //if (bhomBar.SectionProperty != null)
                    //{
                    string sectionName = "";
                    string materialName = "";
                    if (bhomBar.SectionProperty != null)
                    {
                        sectionName = BH.Engine.Robot.Convert.Match(m_dbSecPropNames, bhomBar.SectionProperty);
                        materialName = BH.Engine.Robot.Convert.Match(m_dbMaterialNames, bhomBar.SectionProperty.Material);
                        if (sectionName == null)
                            sectionName = bhomBar.SectionProperty.Name;
                        if (materialName == null)
                            materialName = bhomBar.SectionProperty.Material.Name;
                    }
                    rcache.AddBar(barNum,
                                  System.Convert.ToInt32(bhomBar.StartNode.CustomData[AdapterId]),
                                  System.Convert.ToInt32(bhomBar.EndNode.CustomData[AdapterId]),
                                  sectionName,
                                  materialName,
                                  bhomBar.OrientationAngle * 180 / Math.PI);

                    rcache.SetBarLabel(barNum, IRobotLabelType.I_LT_BAR_RELEASE, bhomBar.Release.Name);

                    
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
                    barNum = System.Convert.ToInt32(bhomBar.CustomData[AdapterId]);
                    RobotBar rBar = barServer.Get(barNum) as RobotBar;
                    barTags[barNum] = bhomBar.Tags;
                    BH.Engine.Robot.Convert.SetFEAType(rBar, bhomBar);
                }
                m_tags[typeof(Bar)] = barTags;

                m_RobotApplication.Interactive = 1;

            }
            return true;
        }

        /***************************************************/

    }

}

