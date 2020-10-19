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

        protected bool Update(IEnumerable<Panel> panels)
        {
            Dictionary<int, HashSet<string>> panelTags = GetTypeTags(typeof(Panel));
            RobotObjObjectServer robotObjectServer = m_RobotApplication.Project.Structure.Objects;
            foreach (Panel panel in panels)
            {
                if (panel == null)
                {
                    Engine.Reflection.Compute.RecordWarning("At least one provided panel is null and is not updated!");
                    continue;
                }

                int panelId;
                object panelIdObj;
                if (!panel.CustomData.TryGetValue(AdapterIdName, out panelIdObj) || panelIdObj == null || !(panelIdObj is int))
                {
                    string panelName = string.IsNullOrWhiteSpace(panel.Name) ? "no name" : "name " + panel.Name;
                    Engine.Reflection.Compute.RecordWarning("Panel with " + panelName + " did not contain any Robot id in a suitable format. To update panels they need this information. For this operation to work, try using a Panel that has first been pulled from Robot");
                    continue;
                }
                else
                    panelId = (int)panelIdObj;

                RobotObjObject robotPanel = robotObjectServer.Get(panelId) as RobotObjObject;
                if (robotPanel == null)
                {
                    Engine.Reflection.Compute.RecordWarning("Could not find a panel with the Id " + panel.CustomData[AdapterIdName].ToString() + " in Robot. Panel could not be updated!");
                    continue;
                }
                robotObjectServer.DeleteMany(robotPanel.GetHostedObjects());
                List<Edge> panelSubEdges = new List<Edge>();
                RobotGeoObject contour = CreateRobotContour(panel.ExternalEdges, out panelSubEdges);

                if (contour == null)
                    continue;

                robotPanel.Main.Geometry = contour;
                robotPanel.Main.Attribs.Meshed = 1;
                robotPanel.Initialize();
                robotPanel.Update();
                SetRobotPanelEdgeConstraints(robotPanel, panelSubEdges);

                if (CheckNotNull(panel.Property, oM.Reflection.Debugging.EventType.Warning, typeof(Panel)))
                {
                    if (panel.Property is LoadingPanelProperty)
                        robotPanel.SetLabel(IRobotLabelType.I_LT_CLADDING, panel.Property.Name);
                    else
                        robotPanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, panel.Property.Name);
                }

                RobotSelection robotOpenings = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_OBJECT);
                int freeObjectNumber = robotObjectServer.FreeNumber;
                robotObjectServer.BeginMultiOperation();
                foreach (Opening opening in panel.Openings)
                {
                    List<Edge> openingSubEdges = new List<Edge>();
                    RobotObjObject robotPanelOpening = robotObjectServer.Create(freeObjectNumber);
                    freeObjectNumber++;
                    opening.CustomData[AdapterIdName] = robotPanelOpening.Number.ToString();
                    RobotGeoObject openingContour = CreateRobotContour(opening.Edges, out openingSubEdges);

                    if (openingContour == null)
                        continue;

                    robotPanelOpening.Main.Geometry = openingContour;

                    robotPanelOpening.Initialize();
                    robotPanelOpening.Update();
                    SetRobotPanelEdgeConstraints(robotPanelOpening, openingSubEdges);
                    robotOpenings.AddOne(robotPanelOpening.Number);
                }
                robotObjectServer.EndMultiOperation();
                robotPanel.SetHostedObjects(robotOpenings);
                robotPanel.Initialize();
                robotPanel.Update();
            }
            m_tags[typeof(Panel)] = panelTags;
            return true;
        }

        /***************************************************/

    }
}

