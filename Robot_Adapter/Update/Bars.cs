using System.Collections.Generic;
using System;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Serialiser;
using RobotOM;
using BH.Engine.Robot;
using BHEG = BH.Engine.Geometry;
using BH.oM.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {       
        protected bool Update(IEnumerable<Bar> bars)
        {
            Dictionary<int, HashSet<string>> barTags = GetTypeTags(typeof(Bar));
            foreach (Bar bar in bars)
            {
                RobotBar robotBar = m_RobotApplication.Project.Structure.Bars.Get((int)bar.CustomData[AdapterId]) as RobotBar;
                if (robotBar == null)
                    return false;

                robotBar.StartNode = System.Convert.ToInt32(bar.StartNode.CustomData[AdapterId]);
                robotBar.EndNode = System.Convert.ToInt32(bar.EndNode.CustomData[AdapterId]);
                barTags[System.Convert.ToInt32(bar.CustomData[AdapterId])] = bar.Tags;


                if (bar.SectionProperty != null && !string.IsNullOrWhiteSpace(bar.SectionProperty.Name))
                    robotBar.SetSection(bar.SectionProperty.Name, false);

                robotBar.Gamma = bar.OrientationAngle * Math.PI / 180;
                BH.Engine.Robot.Convert.SetFEAType(robotBar, bar);

                if (bar.CustomData.ContainsKey("FramingElementDesignProperties"))
                {
                    FramingElementDesignProperties framEleDesProps = bar.CustomData["FramingElementDesignProperties"] as FramingElementDesignProperties;
                    if (m_RobotApplication.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name) != -1)
                        Create(framEleDesProps);
                    robotBar.SetLabel(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name);
                }

            }
            m_tags[typeof(Bar)] = barTags;
            return true;
        }        
    }
}
