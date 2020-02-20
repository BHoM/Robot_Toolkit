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
using BH.oM.Structure.Constraints;
using BH.Engine.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<Panel> panels)
        {
            m_RobotApplication.Interactive = 0;
            m_RobotApplication.Project.Structure.Objects.BeginMultiOperation();
            RobotObjObjectServer objServer = m_RobotApplication.Project.Structure.Objects;
            Dictionary<string, string> edgeConstraints = new Dictionary<string, string>();
            int freeObjectNumber = m_RobotApplication.Project.Structure.Objects.FreeNumber;

            foreach (Panel panel in panels)
            {
                panel.CustomData[AdapterIdName] = freeObjectNumber.ToString();
                RobotObjObject rPanel = objServer.Create(freeObjectNumber);
                freeObjectNumber++;
                List<Edge> subEdges = new List<Edge>();

                rPanel.Main.Geometry = CreateRobotContour(panel.ExternalEdges, out subEdges);
                rPanel.Main.Attribs.Meshed = 1;
                rPanel.Initialize();
                rPanel.Update();
                SetRobotPanelEdgeConstraints(rPanel, subEdges);
                if (panel.Property is LoadingPanelProperty)
                    rPanel.SetLabel(IRobotLabelType.I_LT_CLADDING, panel.Property.Name);
                else
                    rPanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, panel.Property.Name);

                RobotSelection rPanelOpenings = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_OBJECT);
                foreach (Opening opening in panel.Openings)
                {
                    RobotObjObject rPanelOpening = objServer.Create(freeObjectNumber);
                    freeObjectNumber++;
                    opening.CustomData[AdapterIdName] = rPanelOpening.Number.ToString();
                    rPanelOpening.Main.Geometry = CreateRobotContour(opening.Edges, out subEdges);
                    rPanelOpening.Initialize();
                    rPanelOpening.Update();
                    SetRobotPanelEdgeConstraints(rPanelOpening, subEdges);
                    rPanelOpenings.AddOne(rPanelOpening.Number);
                }
                rPanel.SetHostedObjects(rPanelOpenings);
            }
            m_RobotApplication.Project.Structure.Objects.EndMultiOperation();
            m_RobotApplication.Interactive = 1;
            return true;
        }

        /***************************************************/

        private RobotGeoObject CreateRobotContour(List<Edge> edges, out List<Edge> subEdges)
        {
            RobotGeoObject contourPanel = null;
            subEdges = new List<Edge>();
            foreach (Edge edge in edges) //Explode Edges into sub edges (as BHoM edges can contain polycurves, polylines etc.)
            {
                foreach (ICurve crv in BHEG.Query.ISubParts(edge.Curve).ToList())
                {
                    subEdges.Add(BH.Engine.Structure.Create.Edge(crv, edge.Support, edge.Release, edge.Name));
                }
            }
            if (subEdges.Count > 1)
            {
                contourPanel = m_RobotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CONTOUR);
                foreach (Edge edge in subEdges)
                {
                    RobotGeoSegment segment = m_RobotApplication.CmpntFactory.Create(Convert.IsConvertible(edge.Curve));
                    (contourPanel as RobotGeoContour).Add(Convert.ToRobot(edge.Curve, segment));
                }
            }
            else
            {
                contourPanel = m_RobotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CIRCLE);
                Convert.ToRobot(subEdges[0].Curve, contourPanel);
            }
            contourPanel.Initialize();
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
                    panelEdge.SetLabel(IRobotLabelType.I_LT_SUPPORT, support.Name);
                Constraint4DOF release = edges[i - 1].Release;
                if (release != null)
                    m_RobotApplication.Project.Structure.Objects.LinearReleases.Set(panel.Number, i, panel.Number, 1, release.Name);
            }
        }

        /***************************************************/
    }
}
