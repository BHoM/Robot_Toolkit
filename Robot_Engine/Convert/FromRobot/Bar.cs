using BH.oM.Common.Materials;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Constraint;
using BH.oM.Adapters.Robot;


namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static Bar ToBHoMObject( this RobotBar robotBar, 
                                        Dictionary<string,Node> bhomNodes, 
                                        Dictionary<string, ISectionProperty> bhomSections, 
                                        Dictionary<string, Material> bhomMaterials, 
                                        Dictionary<string, BarRelease> barReleases,
                                        Dictionary<string, FramingElementDesignProperties> bhomFramEleDesPropList)
        {
            Node startNode = null;  bhomNodes.TryGetValue(robotBar.StartNode.ToString(), out startNode);
            Node endNode = null; bhomNodes.TryGetValue(robotBar.EndNode.ToString(), out endNode);
            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode, Name = robotBar.Name };
            ISectionProperty secProp = null;
            Material barMaterial = null;
            BarRelease bhomBarRel = null;
            FramingElementDesignProperties bhomFramEleDesignProps = null;

            if (robotBar.HasLabel(IRobotLabelType.I_LT_BAR_SECTION) == -1)
            {
                string secName = robotBar.GetLabelName(IRobotLabelType.I_LT_BAR_SECTION);
                if (!bhomSections.TryGetValue(secName, out secProp))
                    BH.Engine.Reflection.Compute.RecordEvent("Section property type" + secName + "is not supported", oM.Reflection.Debugging.EventType.Warning);
            }

            if (robotBar.HasLabel(IRobotLabelType.I_LT_MATERIAL) == -1)
            {
                string matName = robotBar.GetLabelName(IRobotLabelType.I_LT_MATERIAL);
                if (secProp != null)
                {
                    if (bhomMaterials.TryGetValue(matName, out barMaterial))
                        secProp.Material = barMaterial;
                    else
                        BH.Engine.Reflection.Compute.RecordEvent("Section property has no material assigned", oM.Reflection.Debugging.EventType.Warning);
                }
           }

            if (robotBar.HasLabel(IRobotLabelType.I_LT_MEMBER_TYPE) == -1)
            {
                string framEleDesPropsName = robotBar.GetLabelName(IRobotLabelType.I_LT_MEMBER_TYPE);
                if (bhomFramEleDesPropList.TryGetValue(framEleDesPropsName, out bhomFramEleDesignProps))
                {
                    bhomBar.CustomData.Add("FramingElementDesignProperties", bhomFramEleDesignProps);
                }
                else
                { 
                    BH.Engine.Reflection.Compute.RecordEvent("Framing element design property" + framEleDesPropsName + "is not supported", oM.Reflection.Debugging.EventType.Warning);
                }
                    
            }

            if (robotBar.HasLabel(IRobotLabelType.I_LT_BAR_RELEASE) == -1)
            {

                string releaseName = robotBar.GetLabelName(IRobotLabelType.I_LT_BAR_RELEASE);
                if (barReleases.TryGetValue(releaseName, out bhomBarRel))
                    bhomBar.Release = bhomBarRel;
                else
                    BH.Engine.Reflection.Compute.RecordEvent("Bars with auto-generated releases will not have releases", oM.Reflection.Debugging.EventType.Warning);
            }

            bhomBar.SectionProperty = secProp;
            bhomBar.OrientationAngle = robotBar.Gamma * Math.PI / 180;
            bhomBar.CustomData[AdapterID] = robotBar.Number;
            bhomBar.CustomData["FramingElementDesignProperties"] = bhomFramEleDesignProps;

            if (robotBar.TensionCompression == IRobotBarTensionCompression.I_BTC_COMPRESSION_ONLY)
            {
                bhomBar.FEAType = BarFEAType.CompressionOnly;
            }
            if (robotBar.TensionCompression == IRobotBarTensionCompression.I_BTC_TENSION_ONLY)
            {
                bhomBar.FEAType = BarFEAType.TensionOnly;
            }
            if (robotBar.TrussBar == true)
            {
                bhomBar.FEAType = BarFEAType.Axial;
            }
            return bhomBar;       
        }

        /***************************************************/
    }

}
