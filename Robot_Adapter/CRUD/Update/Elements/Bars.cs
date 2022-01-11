/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BH.Engine.Structure;
using BH.Engine.Geometry;
using RobotOM;
using BH.oM.Adapters.Robot;
using BH.oM.Base;
using BH.Engine.Base;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected bool Update(IEnumerable<Bar> bars)
        {
            RobotBarServer robotBarServer = m_RobotApplication.Project.Structure.Bars;
            Dictionary<int, HashSet<string>> barTags = GetTypeTags(typeof(Bar));
            foreach (Bar bar in bars)
            {
                int barId;

                if (!CheckInputObjectAndExtractAdapterIdInt(bar, out barId, oM.Base.Debugging.EventType.Error, null, true))
                    continue;

                RobotBar robotBar = robotBarServer.Get(barId) as RobotBar;
                if (robotBar == null)
                {
                    Engine.Base.Compute.RecordWarning("Could not find a bar with the Id " + barId.ToString() + " in Robot. Bar could not be updated!");
                    continue;
                }


                int stNodeId, endNodeId;

                //Check nodes are not null and correctly set up and extract id information
                if (!CheckInputObjectAndExtractAdapterIdInt(bar.StartNode, out stNodeId, oM.Base.Debugging.EventType.Error, typeof(Bar)) ||
                    !CheckInputObjectAndExtractAdapterIdInt(bar.EndNode, out endNodeId, oM.Base.Debugging.EventType.Error, typeof(Bar)))
                    continue;

                robotBar.StartNode = stNodeId;
                robotBar.EndNode = endNodeId;
                barTags[barId] = bar.Tags;

                if (!string.IsNullOrWhiteSpace(bar.Name))
                    robotBar.NameTemplate = bar.Name;
                
                if (CheckNotNull(bar.SectionProperty, oM.Base.Debugging.EventType.Warning, typeof(Bar)))
                    robotBar.SetSection(bar.SectionProperty.DescriptionOrName(), false);

                robotBar.Gamma = bar.ToRobotOrientationAngle();
                Convert.SetFEAType(robotBar, bar);

                if (bar.Release != null)
                    robotBar.SetLabel(IRobotLabelType.I_LT_BAR_RELEASE, bar.Release.DescriptionOrName());

                if (bar.Offset != null && bar.Offset.Start != null && bar.Offset.End != null && (bar.Offset.Start.SquareLength() > 0 || bar.Offset.End.SquareLength() > 0))
                    robotBar.SetLabel(IRobotLabelType.I_LT_BAR_OFFSET, bar.Offset.DescriptionOrName());

                IFragment designFragment;
                if (bar.Fragments.TryGetValue(typeof(FramingElementDesignProperties), out designFragment))
                {
                    FramingElementDesignProperties framEleDesProps = designFragment as FramingElementDesignProperties;
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



