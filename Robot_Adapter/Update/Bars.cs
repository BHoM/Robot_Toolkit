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

using System.Collections.Generic;
using System;
using BH.oM.Structure.Elements;
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
                    if (framEleDesProps != null)
                    {
                        if (m_RobotApplication.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name) != -1)
                            Create(framEleDesProps);
                        robotBar.SetLabel(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name);
                    }
                }

            }
            m_tags[typeof(Bar)] = barTags;
            return true;
        }

        /***************************************************/

    }
}
