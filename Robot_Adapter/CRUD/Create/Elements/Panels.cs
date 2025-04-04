/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using BH.oM.Geometry;
using BH.oM.Structure.SurfaceProperties;
using RobotOM;
using BHEG = BH.Engine.Geometry;
using BH.Engine.Structure;
using BH.oM.Structure.Constraints;
using BH.Engine.Spatial;
using BH.Engine.Geometry;
using System;
using System.Runtime.InteropServices;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<Panel> panels)
        {
            m_RobotApplication.Project.Structure.Objects.BeginMultiOperation();
            RobotObjObjectServer objServer = m_RobotApplication.Project.Structure.Objects;
            Dictionary<int, HashSet<string>> panelTags = GetTypeTags(typeof(Panel));

            foreach (Panel panel in panels)
            {
                int panelId;

                if (!CheckInputObjectAndExtractAdapterIdInt(panel, out panelId))
                    continue;

                RobotObjObject robotPanel = objServer.Create(panelId);
                List<Edge> panelSubEdges = new List<Edge>();

                RobotGeoObject contour = CreateRobotContour(panel.ExternalEdges, out panelSubEdges);

                if (contour == null)
                    continue;

                robotPanel.Main.Geometry = contour;

                robotPanel.Main.Attribs.Meshed = 1;
                robotPanel.Initialize();
                robotPanel.Update();
                SetRobotPanelEdgeConstraints(robotPanel, panelSubEdges);

                if (CheckNotNull(panel.Property, oM.Base.Debugging.EventType.Warning, typeof(Panel)))
                {
                    if (panel.Property is LoadingPanelProperty)
                        robotPanel.SetLabel(IRobotLabelType.I_LT_CLADDING, (panel.Property as LoadingPanelProperty).ToRobot());
                    else
                        robotPanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, panel.Property.DescriptionOrName());
                }

                try
                {
                    //Using try catch in the event that something fails in the geometry engine or similar.
                    Basis localOrientation = panel.LocalOrientation();

                    if (localOrientation != null)
                    {
                        robotPanel.Main.Attribs.DirZ = Convert.ToRobotFlipPanelZ(localOrientation.Z);

                        Vector localX = localOrientation.X;
                        robotPanel.Main.Attribs.SetDirX(IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN, localX.X, localX.Y, localX.Z);
                    }
                    else
                    {
                        Engine.Base.Compute.RecordWarning("Could not extract local orientation of a Panel. Could not set local orientations to Robot.");
                    }
                }
                catch (Exception e)
                {
                    string message = "Failed to set local orientations for a Panel. Exception message: " + e.Message;

                    if (!string.IsNullOrEmpty(e.InnerException?.Message))
                    {
                        message += "\nInnerException: " + e.InnerException.Message;
                    }

                    Engine.Base.Compute.RecordWarning(message);
                }


                RobotSelection rPanelOpenings = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_OBJECT);

                foreach (Opening opening in panel.Openings)
                {
                    int openingId;
                    if (CheckInputObjectAndExtractAdapterIdInt(opening, out openingId, oM.Base.Debugging.EventType.Error, typeof(Panel)))
                        rPanelOpenings.AddOne(openingId);
                }
                robotPanel.SetHostedObjects(rPanelOpenings);

                robotPanel.Initialize();
                robotPanel.Update();

                panelTags[panelId] = panel.Tags;
            }

            m_RobotApplication.Project.Structure.Objects.EndMultiOperation();
            m_tags[typeof(Panel)] = panelTags;
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Opening> openings)
        {
            m_RobotApplication.Project.Structure.Objects.BeginMultiOperation();
            RobotObjObjectServer objServer = m_RobotApplication.Project.Structure.Objects;

            foreach (Opening opening in openings)
            {
                int openingId;
                if (!CheckInputObjectAndExtractAdapterIdInt(opening, out openingId, oM.Base.Debugging.EventType.Error))
                    continue;

                List<Edge> openingSubEdges = new List<Edge>();
                RobotObjObject rPanelOpening = objServer.Create(openingId);
                SetAdapterId(opening, rPanelOpening.Number);

                RobotGeoObject contour = CreateRobotContour(opening.Edges, out openingSubEdges);

                if (contour == null)
                    continue;

                rPanelOpening.Main.Geometry = contour;

                rPanelOpening.Initialize();
                rPanelOpening.Update();
                SetRobotPanelEdgeConstraints(rPanelOpening, openingSubEdges);
            }
            m_RobotApplication.Project.Structure.Objects.EndMultiOperation();
            return true;
        }
        /***************************************************/

        private RobotGeoObject CreateRobotContour(List<Edge> edges, out List<Edge> subEdges)
        {
            subEdges = new List<Edge>();
            List<ICurve> subCurves = new List<ICurve>();
            if (CheckNotNull(edges, oM.Base.Debugging.EventType.Error, typeof(Panel)))
            {
                foreach (Edge edge in edges) //Explode Edges into sub edges (as BHoM edges can contain polycurves, polylines etc.)
                {
                    //Check edge not null
                    if (CheckNotNull(edge, oM.Base.Debugging.EventType.Error, typeof(Panel)) &&
                        CheckNotNull(edge.Curve, oM.Base.Debugging.EventType.Error, typeof(Edge)))
                    {
                        foreach (ICurve curve in BHEG.Query.ISubParts(edge.Curve).ToList())
                        {
                            //Check curves not null
                            if (CheckNotNull(curve, oM.Base.Debugging.EventType.Error, typeof(Edge)))
                            {
                                subEdges.Add(new Edge { Curve = curve, Support = edge.Support, Release = edge.Release, Name = edge.Name });
                                subCurves.Add(curve);
                            }
                            else
                                return null;
                        }
                    }
                    else
                        return null;
                }
                RobotGeoObject contourPanel = Convert.ToRobot(subCurves, m_RobotApplication) as RobotGeoObject;
                return contourPanel;
            }
            else
                return null;
        }


        /***************************************************/

        private void SetRobotPanelEdgeConstraints(RobotObjObject panel, List<Edge> edges)
        {
            if (edges.All(x => x.Support == null && x.Release == null)) //All supports and releases are null can just return without making any com calls
                return;

            IRobotCollection panelEdges = panel.Main.Edges;

            if (panelEdges.Count != edges.Count)
            {
                Engine.Base.Compute.RecordWarning("Could not set supports and releases to panel edges.");
                return;
            }

            for (int i = 1; i <= panelEdges.Count; i++)
            {
                Constraint6DOF support = edges[i - 1].Support;
                Constraint4DOF release = edges[i - 1].Release;

                if (support != null)
                {
                    IRobotObjEdge panelEdge = panelEdges.Get(i);
                    if (panelEdge == null)
                        continue;

                    panelEdge.SetLabel(IRobotLabelType.I_LT_SUPPORT, support.DescriptionOrName());
                }

                if (release != null)
                    m_RobotApplication.Project.Structure.Objects.LinearReleases.Set(panel.Number, i, panel.Number, 1, release.DescriptionOrName());
            }
        }

        /***************************************************/
    }
}






