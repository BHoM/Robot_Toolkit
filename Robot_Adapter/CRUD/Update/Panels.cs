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

using BH.oM.Structure.Elements;
using BH.oM.Structure.SurfaceProperties;
using RobotOM;
using System.Collections.Generic;
using BH.oM.Adapter;
using System.Linq;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected override bool UpdateOnly<T>(IEnumerable<T> panels, string tag = "", ActionConfig actionConfig = null)
        {
            Dictionary<int, HashSet<string>> panelTags = GetTypeTags(typeof(Panel));
            RobotObjObjectServer robotObjectServer = m_RobotApplication.Project.Structure.Objects;
            foreach (Panel panel in panels.Select(x => x as Panel))
            {
                RobotObjObject robotPanel = robotObjectServer.Get((int)panel.CustomData[AdapterIdName]) as RobotObjObject;
                if (robotPanel == null)
                    return false;
                
                List<Edge> subEdges = new List<Edge>();

                robotPanel.Main.Geometry = CreateRobotContour(panel.ExternalEdges, out subEdges);
                robotPanel.Main.Attribs.Meshed = 1;
                robotPanel.Initialize();
                robotPanel.Update();
                if (HasEdgeSupports(subEdges)) SetRobotPanelEdgeSupports(robotPanel, subEdges);
                if (HasEdgeReleases(subEdges)) SetRobotPanelEdgeReleases(robotPanel, subEdges);

                if (panel.Property is LoadingPanelProperty)
                    robotPanel.SetLabel(IRobotLabelType.I_LT_CLADDING, panel.Property.Name);
                else
                    robotPanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, panel.Property.Name);

                robotObjectServer.DeleteMany(robotPanel.GetHostedObjects());

                RobotSelection rPanelOpenings = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_OBJECT);
                int freeObjectNumber = robotObjectServer.FreeNumber;
                robotObjectServer.BeginMultiOperation();
                foreach (Opening opening in panel.Openings)
                {
                    RobotObjObject robotPanelOpening = robotObjectServer.Create(freeObjectNumber);
                    freeObjectNumber++;
                    opening.CustomData[AdapterIdName] = robotPanelOpening.Number.ToString();
                    robotPanelOpening.Main.Geometry = CreateRobotContour(opening.Edges, out subEdges);
                    robotPanelOpening.Initialize();
                    robotPanelOpening.Update();
                    if (HasEdgeSupports(subEdges)) SetRobotPanelEdgeSupports(robotPanelOpening, subEdges);
                    if (HasEdgeReleases(subEdges)) SetRobotPanelEdgeReleases(robotPanelOpening, subEdges);
                    rPanelOpenings.AddOne(robotPanelOpening.Number);
                }
                robotObjectServer.EndMultiOperation();
                robotPanel.SetHostedObjects(rPanelOpenings);
            }
            m_tags[typeof(Panel)] = panelTags;
            return true;
        }

        /***************************************************/

    }
}

