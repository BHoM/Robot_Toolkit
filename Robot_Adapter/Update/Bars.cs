using System.Collections.Generic;
using System;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.oM.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

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

        /***************************************************/

    }
}
