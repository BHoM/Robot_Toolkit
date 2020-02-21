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

using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Physical.Materials;
using BH.oM.Geometry;
using System.Collections;
using BH.oM.Structure.MaterialFragments;

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

            Panel panel = null;

            List<int> panelIds = CheckAndGetIds(ids);

            RobotSelection robotPanelSelection = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_PANEL);
            RobotSelection robotCladdingPanelSelection = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_GEOMETRY);
            if (panelIds == null)
            {
                robotPanelSelection.FromText("all");
                robotCladdingPanelSelection.FromText("all");
            }
            else if (panelIds.Count == 0)
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
                panel = null;

                if (robotPanel.Main.Attribs.Meshed == 1)
                {
                    ICurve outline = Convert.FromRobot(robotPanel.Main.GetGeometry() as dynamic);
                    List<Opening> openings = new List<Opening>();

                    IRobotCollection robotOpenings = robotPanelServer.GetMany(robotPanel.GetHostedObjects());
                    for (int j = 1; j <= robotOpenings.Count; j++)
                    {
                        RobotObjObject robotOpening = (RobotObjObject)robotOpenings.Get(j);
                        openings.Add(BH.Engine.Structure.Create.Opening(Convert.FromRobot(robotOpening.Main.GetGeometry() as dynamic)));
                    }
                    try
                    {
                        panel = BH.Engine.Structure.Create.Panel(outline, openings);
                    }
                    catch
                    {
                        BH.Engine.Reflection.Compute.RecordWarning("Geometry for panel " + robotPanel.Number.ToString() + " not supported.");
                    }
                    panel.CustomData[AdapterIdName] = robotPanel.Number;
                    panel.CustomData["RobotFiniteElementIds"] = robotPanel.FiniteElems;
                    panel.CustomData["RobotNodeIds"] = robotPanel.Nodes;

                    //Get the coordinate system for the panel
                    BH.oM.Geometry.Point coordPoint = BH.Engine.Geometry.Query.StartPoint(outline as dynamic);

                    double x, y, z; robotPanel.Main.Attribs.GetDirX(out x, out y, out z);
                    BH.oM.Geometry.Vector coordXAxis = BH.Engine.Geometry.Create.Vector(x, y, z);
                    BH.oM.Geometry.Vector coordZAxis = BH.Engine.Geometry.Compute.FitPlane(outline as dynamic).Normal;
                    if (coordZAxis.Z == 0)
                    {
                        if ((coordZAxis.X > coordZAxis.Y && coordZAxis.X < 1) || (coordZAxis.Y > coordZAxis.X && coordZAxis.Y < 1))
                            coordZAxis = BH.Engine.Geometry.Modify.Reverse(coordZAxis);
                    }
                    if (robotPanel.Main.Attribs.DirZ == 0)
                        coordZAxis = BH.Engine.Geometry.Modify.Reverse(coordZAxis);

                    BH.oM.Geometry.CoordinateSystem.Cartesian tempCoordSys = BH.Engine.Geometry.Create.CartesianCoordinateSystem(coordPoint, coordXAxis, coordZAxis);
                    BH.oM.Geometry.CoordinateSystem.Cartesian coordinateSystem = BH.Engine.Geometry.Create.CartesianCoordinateSystem(coordPoint, coordXAxis, tempCoordSys.Z);

                    panel.CustomData["CoordinateSystem"] = coordinateSystem;
                    if (robotPanel.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) != 0)
                    {
                        string propName = robotPanel.GetLabelName(IRobotLabelType.I_LT_PANEL_THICKNESS);
                        if (!surfaceProperties.ContainsKey(propName))
                        {
                            ISurfaceProperty SurfaceProperty = ReadSurfacePropertyFromPanel(robotPanel, materials);
                            if (SurfaceProperty.Material == null)
                            {
                                SurfaceProperty.Material = ReadMaterialFromPanel(robotPanel);
                                if (SurfaceProperty.Material != null && SurfaceProperty.Material.Name != "" && !materials.ContainsKey(SurfaceProperty.Name))
                                {
                                    materials.Add(SurfaceProperty.Material.Name, SurfaceProperty.Material);
                                }
                            }
                            surfaceProperties.Add(propName, SurfaceProperty);
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
                        claddingPanel.CustomData[AdapterIdName] = robotCladdingPanel.Number;
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

        //Method is stripped dpwn to only return data needed for extracting mesh results
        private List<Panel> ReadPanelsLight(IList ids = null)
        {
            IRobotStructure robotStructureServer = m_RobotApplication.Project.Structure;
            IRobotObjObjectServer robotPanelServer = m_RobotApplication.Project.Structure.Objects;
            List<Panel> BHoMPanels = new List<Panel>();
            Panel BHoMPanel = null;

            List<int> panelIds = CheckAndGetIds(ids);
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
                RobotObjObject rpanel = (RobotObjObject)rPanels.Get(i);
                BHoMPanel = null;

                if (rpanel.Main.Attribs.Meshed == 1)
                {
                    ICurve outline = Convert.FromRobot(rpanel.Main.GetGeometry() as dynamic);

                    BHoMPanel = new Panel();
                    BHoMPanel.CustomData[AdapterIdName] = rpanel.Number;
                    BHoMPanel.CustomData["RobotFiniteElementIds"] = rpanel.FiniteElems;
                    BHoMPanel.CustomData["RobotNodeIds"] = rpanel.Nodes;

                    //Get the coordinate system for the panel
                    BH.oM.Geometry.Point coordPoint = BH.Engine.Geometry.Query.StartPoint(outline as dynamic);

                    double x, y, z; rpanel.Main.Attribs.GetDirX(out x, out y, out z);
                    BH.oM.Geometry.Vector coordXAxis = BH.Engine.Geometry.Create.Vector(x, y, z);
                    BH.oM.Geometry.Vector coordZAxis = BH.Engine.Geometry.Compute.FitPlane(outline as dynamic).Normal;
                    if (coordZAxis.Z == 0)
                    {
                        if ((coordZAxis.X > coordZAxis.Y && coordZAxis.X < 1) || (coordZAxis.Y > coordZAxis.X && coordZAxis.Y < 1))
                            coordZAxis = BH.Engine.Geometry.Modify.Reverse(coordZAxis);
                    }
                    if (rpanel.Main.Attribs.DirZ == 0)
                        coordZAxis = BH.Engine.Geometry.Modify.Reverse(coordZAxis);

                    BH.oM.Geometry.CoordinateSystem.Cartesian tempCoordSys = BH.Engine.Geometry.Create.CartesianCoordinateSystem(coordPoint, coordXAxis, coordZAxis);
                    BH.oM.Geometry.CoordinateSystem.Cartesian coordinateSystem = BH.Engine.Geometry.Create.CartesianCoordinateSystem(coordPoint, coordXAxis, tempCoordSys.Z);

                    BHoMPanel.CustomData["CoordinateSystem"] = coordinateSystem;

                }
                BHoMPanels.Add(BHoMPanel);
            }
            return BHoMPanels;
        }

        /***************************************************/
    }
}

