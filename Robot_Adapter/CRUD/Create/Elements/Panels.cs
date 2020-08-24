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
using BH.oM.Geometry;
using BH.oM.Structure.SurfaceProperties;
using RobotOM;
using BHEG = BH.Engine.Geometry;
using BH.Engine.Structure;
using BH.oM.Structure.Constraints;
using BH.Engine.Spatial;
using BH.Engine.Geometry;
using System;

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
            Dictionary<string, string> edgeConstraints = new Dictionary<string, string>();

            foreach (Panel panel in panels)
            {
                RobotObjObject robotPanel = objServer.Create(System.Convert.ToInt32(panel.CustomData[AdapterIdName]));
                List<Edge> panelSubEdges = new List<Edge>();

                robotPanel.Main.Geometry = CreateRobotContour(panel.ExternalEdges, out panelSubEdges);
                robotPanel.Main.Attribs.Meshed = 1;
                robotPanel.Initialize();
                robotPanel.Update();
                SetRobotPanelEdgeConstraints(robotPanel, panelSubEdges);
                if (panel.Property is LoadingPanelProperty)
                    robotPanel.SetLabel(IRobotLabelType.I_LT_CLADDING, (panel.Property as LoadingPanelProperty).ToRobot());
                else
                    robotPanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, panel.Property.DescriptionOrName());

                RobotSelection rPanelOpenings = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_OBJECT);

                Vector bhNormal = panel.Normal();

                //Tolerance is lower than any geometry tolerance used in the BHoM, hence defined here
                double tolerance = 1e-16;
                if (Math.Abs(bhNormal.Z) > tolerance)
                {
                    if (bhNormal.Z < 0)
                        robotPanel.Main.Attribs.DirZ = 1;
                }
                else if (Math.Abs(bhNormal.X) > tolerance)
                {
                    if (bhNormal.X < 0)
                        robotPanel.Main.Attribs.DirZ = 1;
                }
                else
                {
                    if (bhNormal.Y < 0)
                        robotPanel.Main.Attribs.DirZ = 1;
                }

                Vector localX = panel.LocalOrientation().X;
                robotPanel.Main.Attribs.SetDirX(IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN, localX.X, localX.Y, localX.Z);

                foreach (Opening opening in panel.Openings)
                {
                    rPanelOpenings.AddOne(System.Convert.ToInt32(opening.CustomData[AdapterIdName]));
                }
                robotPanel.SetHostedObjects(rPanelOpenings);

                robotPanel.Initialize();
                robotPanel.Update();

            }
            m_RobotApplication.Project.Structure.Objects.EndMultiOperation();
            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<Opening> openings)
        {
            m_RobotApplication.Project.Structure.Objects.BeginMultiOperation();
            RobotObjObjectServer objServer = m_RobotApplication.Project.Structure.Objects;

            foreach (Opening opening in openings)
            {
                List<Edge> openingSubEdges = new List<Edge>();
                RobotObjObject rPanelOpening = objServer.Create(System.Convert.ToInt32(opening.CustomData[AdapterIdName]));
                opening.CustomData[AdapterIdName] = rPanelOpening.Number.ToString();
                rPanelOpening.Main.Geometry = CreateRobotContour(opening.Edges, out openingSubEdges);
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
            foreach (Edge edge in edges) //Explode Edges into sub edges (as BHoM edges can contain polycurves, polylines etc.)
            {
                foreach (ICurve curve in BHEG.Query.ISubParts(edge.Curve).ToList())
                {
                    subEdges.Add(BH.Engine.Structure.Create.Edge(curve, edge.Support, edge.Release, edge.Name));
                    subCurves.Add(curve);
                }
            }
            RobotGeoObject contourPanel = Convert.ToRobot(subCurves, m_RobotApplication) as RobotGeoObject;
            return contourPanel;
        }


        /***************************************************/

        private void SetRobotPanelEdgeConstraints(RobotObjObject panel, List<Edge> edges)
        {
            IRobotCollection panelEdges = panel.Main.Edges;
            for (int i = 1; i <= panelEdges.Count; i++)
            {
                IRobotObjEdge panelEdge = panelEdges.Get(i);
                Constraint6DOF support = edges[i - 1].Support;
                if (support != null)
                    panelEdge.SetLabel(IRobotLabelType.I_LT_SUPPORT, support.DescriptionOrName());
                Constraint4DOF release = edges[i - 1].Release;
                if (release != null)
                    m_RobotApplication.Project.Structure.Objects.LinearReleases.Set(panel.Number, i, panel.Number, 1, release.DescriptionOrName());
            }
        }

        /***************************************************/
    }
}

