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

using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Physical.Materials;
using BH.oM.Geometry;
using System.Collections;
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Spatial;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using BH.oM.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Panel> ReadPanels(IList ids = null)
        {
            Dictionary<string, ISurfaceProperty> surfaceProperties = new Dictionary<string, ISurfaceProperty>();
            Dictionary<string, IMaterialFragment> materials = new Dictionary<string, IMaterialFragment>();
            List<Panel> panels = new List<Panel>();
            IRobotStructure robotStructureServer = m_RobotApplication.Project.Structure;
            IRobotObjObjectServer robotPanelServer = m_RobotApplication.Project.Structure.Objects;
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;

            List<int> panelIds = CheckAndGetIds<Panel>(ids);

            RobotSelection robotPanelSelection = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_PANEL);
            RobotSelection robotCladdingPanelSelection = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_GEOMETRY);
            if (panelIds == null || panelIds.Count == 0)
            {
                robotPanelSelection.FromText("all");
                robotCladdingPanelSelection.FromText("all");
            }
            else
            {
                robotPanelSelection.FromText(Convert.ToRobotSelectionString(panelIds));
                robotCladdingPanelSelection.FromText(Convert.ToRobotSelectionString(panelIds));
            }

            IRobotCollection robotPanels = robotPanelServer.GetMany(robotPanelSelection);
            IRobotCollection robotCladdingPanels = robotPanelServer.GetMany(robotCladdingPanelSelection);

            for (int i = 1; i <= robotPanels.Count; i++)
            {
                RobotObjObject robotPanel = (RobotObjObject)robotPanels.Get(i);
                Panel panel = null;

                if (robotPanel.Main.Attribs.Meshed == 1)
                {
                    ICurve outline = Convert.IFromRobot(robotPanel.Main.GetGeometry());
                    List<Opening> openings = new List<Opening>();

                    IRobotCollection robotOpenings = robotPanelServer.GetMany(robotPanel.GetHostedObjects());
                    for (int j = 1; j <= robotOpenings.Count; j++)
                    {
                        Opening opening;
                        RobotObjObject robotOpening = (RobotObjObject)robotOpenings.Get(j);
                        ICurve openingOutline = Convert.IFromRobot(robotOpening.Main.GetGeometry());

                        if (openingOutline != null)
                            opening = BH.Engine.Structure.Create.Opening(openingOutline);
                        else
                        {
                            Engine.Reflection.Compute.RecordError($"Failed to extract the outline geometry for Opening with id {robotOpening.Number} on Panel with id {robotPanel.Number}.");
                            opening = new Opening();
                        }

                        SetAdapterId(opening, robotOpening.Number);
                        openings.Add(opening);
                    }
                    try
                    {
                        if (outline != null)
                            panel = BH.Engine.Structure.Create.Panel(outline, openings);
                        else
                        {
                            BH.Engine.Reflection.Compute.RecordError($"Failed to extract the outline geometry for Panel with id {robotPanel.Number}.");
                            panel = new Panel { Openings = openings };
                        }
                    }
                    catch
                    {
                        BH.Engine.Reflection.Compute.RecordWarning($"Geometry for Panel with id {robotPanel.Number} not supported.");
                        continue;
                    }

                    SetAdapterId(panel, robotPanel.Number);

                    PanelFiniteElementIds feIds = new PanelFiniteElementIds
                    {
                        FiniteElementIds = robotPanel.FiniteElems,
                        NodeIds = robotPanel.Nodes
                    };

                    panel.Fragments.Add(feIds);

                    try
                    {
                        //Get the coordinate system for the panel
                        double x, y, z; robotPanel.Main.Attribs.GetDirX(out x, out y, out z);
                        Vector coordXAxis = BH.Engine.Geometry.Create.Vector(x, y, z);
                        Vector coordZAxis = panel.Normal();

                        if (coordZAxis != null)
                        {
                            bool flip = robotPanel.Main.Attribs.DirZ == 1;
                            flip = Convert.FromRobotCheckFlipNormal(coordZAxis, flip);

                            if (flip)
                            {
                                coordZAxis = coordZAxis.Reverse();
                                FlipOutline(panel);
                            }

                            //Set local orientation
                            panel.OrientationAngle = Engine.Structure.Compute.OrientationAngleAreaElement(coordZAxis, coordXAxis);
                        }
                        else
                        {
                            Engine.Reflection.Compute.RecordWarning("Failed to extract local Orientations for at least one Panel.");
                        }
                    }
                    catch (Exception e)
                    {
                        string message = "Failed to get local orientations for a Panel. Exception message: " + e.Message;

                        if (!string.IsNullOrEmpty(e.InnerException?.Message))
                        {
                            message += "\nInnerException: " + e.InnerException.Message;
                        }

                        Engine.Reflection.Compute.RecordWarning(message);
                    }

                    if (robotPanel.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) != 0)
                    {
                        string propName = robotPanel.GetLabelName(IRobotLabelType.I_LT_PANEL_THICKNESS);
                        if (!surfaceProperties.ContainsKey(propName))
                        {
                            ISurfaceProperty surfaceProperty = ReadSurfacePropertyFromPanel(robotPanel, materials);
                            if (surfaceProperty != null && surfaceProperty.Material == null)
                            {
                                surfaceProperty.Material = ReadMaterialFromPanel(robotPanel);
                                if (surfaceProperty.Material != null && !string.IsNullOrEmpty(surfaceProperty.Material.Name) && !materials.ContainsKey(surfaceProperty.Name))
                                {
                                    materials.Add(surfaceProperty.Material.Name, surfaceProperty.Material);
                                }
                            }
                            surfaceProperties.Add(propName, surfaceProperty);
                        }
                        panel.Property = surfaceProperties[propName];
                    }
                    panels.Add(panel);
                }
            }

            for (int i = 1; i <= robotCladdingPanels.Count; i++)
            {
                Panel claddingPanel = null;
                RobotObjObject robotCladdingPanel = (RobotObjObject)robotCladdingPanels.Get(i);
                if (robotCladdingPanel.HasLabel(IRobotLabelType.I_LT_CLADDING) != 0)
                {
                    List<Opening> claddingOpenings = new List<Opening>();
                    IRobotCollection robotCladdingOpenings = robotPanelServer.GetMany(robotCladdingPanel.GetHostedObjects());
                    for (int j = 1; j <= robotCladdingOpenings.Count; j++)
                    {
                        RobotObjObject robotOpening = (RobotObjObject)robotCladdingOpenings.Get(j);
                        claddingOpenings.Add(BH.Engine.Structure.Create.Opening(Convert.FromRobot(robotOpening.Main.GetGeometry() as RobotGeoContour)));
                    }
                    if (robotCladdingPanel.Main.Attribs.Meshed == 1)
                    {
                        ICurve outline = Convert.FromRobot(robotCladdingPanel.Main.GetGeometry() as dynamic);
                        claddingPanel = BH.Engine.Structure.Create.Panel(outline, claddingOpenings);
                    }
                    if (claddingPanel != null)
                    {
                        SetAdapterId(claddingPanel, robotCladdingPanel.Number);
                        string propName = robotCladdingPanel.GetLabelName(IRobotLabelType.I_LT_CLADDING);
                        if (propName != "")
                        {
                            if (!surfaceProperties.ContainsKey(propName))
                            {
                                ISurfaceProperty SurfaceProperty = ReadSurfacePropertyFromPanel(robotCladdingPanel, materials, true);
                                surfaceProperties.Add(propName, SurfaceProperty);
                            }
                            claddingPanel.Property = surfaceProperties[propName];
                        }
                        panels.Add(claddingPanel);
                    }
                }
            }
            return panels;
        }

        /***************************************************/

        //Method put here temporary until available in Spatial_Engine
        private void FlipOutline(Panel panel)
        {
            foreach (Edge e in panel.ExternalEdges)
            {
                e.Curve = e.Curve.IFlip();
            }
            panel.ExternalEdges.Reverse();
        }

        /***************************************************/

        //Method is stripped dpwn to only return data needed for extracting mesh results
        private List<Panel> ReadPanelsLight(IList ids = null)
        {
            IRobotStructure robotStructureServer = m_RobotApplication.Project.Structure;
            IRobotObjObjectServer robotPanelServer = m_RobotApplication.Project.Structure.Objects;
            List<Panel> BHoMPanels = new List<Panel>();
            Panel panel = null;

            List<int> panelIds = CheckAndGetIds<Panel>(ids);
            RobotSelection rPanSelect = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_PANEL);
            if (panelIds == null)
            {
                rPanSelect.FromText("all");
            }
            else if (panelIds.Count == 0)
            {
                rPanSelect.FromText("all");
            }
            else
            {
                rPanSelect.FromText(Convert.ToRobotSelectionString(panelIds));
            }

            IRobotCollection rPanels = robotPanelServer.GetMany(rPanSelect);

            for (int i = 1; i <= rPanels.Count; i++)
            {
                RobotObjObject robotPanel = (RobotObjObject)rPanels.Get(i);
                panel = null;

                if (robotPanel.Main.Attribs.Meshed == 1)
                {
                    ICurve outline = Convert.FromRobot(robotPanel.Main.GetGeometry() as dynamic);

                    panel = Engine.Structure.Create.Panel(outline, new List<ICurve>());
                    SetAdapterId(panel, robotPanel.Number);

                    PanelFiniteElementIds feIds = new PanelFiniteElementIds
                    {
                        FiniteElementIds = robotPanel.FiniteElems,
                        NodeIds = robotPanel.Nodes
                    };

                    panel.Fragments.Add(feIds);

                    //Get the coordinate system for the panel
                    double x, y, z; robotPanel.Main.Attribs.GetDirX(out x, out y, out z);
                    Vector coordXAxis = BH.Engine.Geometry.Create.Vector(x, y, z);
                    Vector coordZAxis = panel.Normal();


                    bool flip = robotPanel.Main.Attribs.DirZ == 1;
                    flip = Convert.FromRobotCheckFlipNormal(coordZAxis, flip);

                    if (flip)
                    {
                        coordZAxis *= -1;
                        FlipOutline(panel);
                    }

                    //Set local orientation
                    panel.OrientationAngle = Engine.Structure.Compute.OrientationAngleAreaElement(coordZAxis, coordXAxis);

                }
                BHoMPanels.Add(panel);
            }
            return BHoMPanels;
        }

        /***************************************************/
    }
}

