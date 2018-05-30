using System.Collections.Generic;
using System.Linq;
using System;
using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using RobotOM;

using BH.Engine.Robot;
using BHEG = BH.Engine.Geometry;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Index Adapter Interface                   ****/
        /***************************************************/

        /***************************************************/
    
        public bool CreateCollection(IEnumerable<Bar> bhomBars)
        {
            if (bhomBars != null)
            {
                IRobotStructure rStructure = m_RobotApplication.Project.Structure;
                IRobotBarServer barServer = rStructure.Bars;
                m_RobotApplication.Interactive = 0;
                barServer.BeginMultiOperation();
                List<Bar> bars = bhomBars.ToList();
                IRobotStructureCache rcache = rStructure.CreateCache();
                RobotSelection rSelect = rStructure.Selections.Create(IRobotObjectType.I_OT_BAR);
                string tensionBars = "";
                string compressionBars = "";
                string axialBars = "";
                int barNum = 0;
                int freeNum = m_RobotApplication.Project.Structure.Bars.FreeNumber;
                Dictionary<int, HashSet<string>> barTags = GetTypeTags(typeof(Bar));

                List<Bar> nonCasheBars = new List<Bar>();
                foreach (Bar bhomBar in bars)
                {
                    barNum = System.Convert.ToInt32(bhomBar.CustomData[AdapterId]);

                    if (bhomBar.SectionProperty != null)
                    {
                        string sectionName = BH.Engine.Robot.Convert.Match(m_dbSecPropNames, bhomBar.SectionProperty);
                        string materialName = BH.Engine.Robot.Convert.Match(m_dbMaterialNames, bhomBar.SectionProperty.Material);
                        if (sectionName == null)
                            sectionName = bhomBar.SectionProperty.Name;
                        if (materialName == null)
                            materialName = bhomBar.SectionProperty.Material.Name;
                        
                        rcache.AddBar(barNum,
                                      System.Convert.ToInt32(bhomBar.StartNode.CustomData[AdapterId]),
                                      System.Convert.ToInt32(bhomBar.EndNode.CustomData[AdapterId]),
                                      sectionName,
                                      materialName,
                                      bhomBar.OrientationAngle * 180 / Math.PI);
                        rcache.SetBarLabel(barNum, IRobotLabelType.I_LT_BAR_RELEASE, bhomBar.Release.Name);
                    }

                    else
                    {
                        nonCasheBars.Add(bhomBar);
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

                foreach (Bar bhomBar in nonCasheBars)
                {
                    barServer.Create(System.Convert.ToInt32(bhomBar.CustomData[AdapterId]),
                                     System.Convert.ToInt32(bhomBar.StartNode.CustomData[AdapterId]),
                                     System.Convert.ToInt32(bhomBar.EndNode.CustomData[AdapterId]));
                }

                rSelect.FromText(tensionBars);
                barServer.SetTensionCompression(rSelect, IRobotBarTensionCompression.I_BTC_TENSION_ONLY);
                rSelect.FromText(compressionBars);
                barServer.SetTensionCompression(rSelect, IRobotBarTensionCompression.I_BTC_COMPRESSION_ONLY);
                barServer.SetTrussBar(axialBars, true);

                
                barServer.EndMultiOperation();
                m_RobotApplication.Interactive = 1;

                foreach (Bar bhomBar in bars)
                {
                    barNum = System.Convert.ToInt32(bhomBar.CustomData[AdapterId]);
                    RobotBar rBar = barServer.Get(barNum) as RobotBar;
                    barTags[barNum] = bhomBar.Tags;
                    BH.Engine.Robot.Convert.SetFEAType(rBar, bhomBar);
                }
                m_tags[typeof(Bar)] = barTags;
            }
            return true;
        }

        /***************************************************/
        
        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

