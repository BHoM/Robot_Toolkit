using BH.oM.Geometry;
using BH.oM.Common.Materials;
using BH.Engine.Reflection;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.Engine.Reflection;
using BH.oM.Reflection.Debuging;
using BH.oM.Adapters.Robot.Properties;


namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************/

        public static Bar ToBHoMObject(this RobotBar robotBar, Dictionary<string,Node> bhomNodes, Dictionary<string, ISectionProperty> bhomSections, Dictionary<string, Material> bhomMaterials, Dictionary<string, BarRelease> barReleases)
        {
            Node startNode = null;  bhomNodes.TryGetValue(robotBar.StartNode.ToString(), out startNode);
            Node endNode = null; bhomNodes.TryGetValue(robotBar.EndNode.ToString(), out endNode);
            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode, Name = robotBar.Name };
            ISectionProperty secProp = null;
            Material barMaterial = null;
            BarRelease bhomBarRel = null;

            if (robotBar.HasLabel(IRobotLabelType.I_LT_BAR_SECTION) == -1)
            {
                string secName = robotBar.GetLabelName(IRobotLabelType.I_LT_BAR_SECTION);
                if (!bhomSections.TryGetValue(secName, out secProp))
                    BH.Engine.Reflection.Compute.RecordEvent("Section property type is not supported", oM.Reflection.Debuging.EventType.Warning);
            }

            if (robotBar.HasLabel(IRobotLabelType.I_LT_MATERIAL) == -1)
            {
                string matName = robotBar.GetLabelName(IRobotLabelType.I_LT_MATERIAL);
                if (secProp != null)
                {
                    if (bhomMaterials.TryGetValue(matName, out barMaterial))
                        secProp.Material = barMaterial;
                    else
                        BH.Engine.Reflection.Compute.RecordEvent("Section property has no material assigned", oM.Reflection.Debuging.EventType.Warning);
                }
           }

            if (robotBar.HasLabel(IRobotLabelType.I_LT_BAR_RELEASE) == -1)
            {

                string releaseName = robotBar.GetLabelName(IRobotLabelType.I_LT_BAR_RELEASE);
                if (!barReleases.TryGetValue(releaseName, out bhomBarRel))
                    BH.Engine.Reflection.Compute.RecordEvent("Bars with auto-generated releases will not have releases", oM.Reflection.Debuging.EventType.Warning);
            }

            bhomBar.SectionProperty = secProp;
            bhomBar.OrientationAngle = robotBar.Gamma * Math.PI / 180;
            bhomBar.CustomData[AdapterID] = robotBar.Number;

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

            //Custom properties inserted into the custom property dictionary
            if (robotBar.HasLabel(IRobotLabelType.I_LT_MEMBER_TYPE) == -1)
            {
                string memberTypeName = robotBar.GetLabelName(IRobotLabelType.I_LT_MEMBER_TYPE);
                try
                {
                    //bhomBar.CustomData.Add("FramingElementDesignProperties", framingElementDesignProperties[memberTypeName]);
                }
                catch
                {
                    Compute.RecordEvent("Member type not present in the BHoM list", EventType.Error);
                }
            }


            return bhomBar;       
        }       
        }        
    
}
