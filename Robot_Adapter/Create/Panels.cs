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
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.SurfaceProperties;
using RobotOM;
using BHEG = BH.Engine.Geometry;

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

            foreach (Panel panel in panels)
            {
                RobotGeoObject contourPanel;
                int panelNum = System.Convert.ToInt32(panel.CustomData[AdapterId]);
                RobotObjObject rPanel = objServer.Create(panelNum);
                List<ICurve> segmentsPanel = new List<ICurve>();
                int edgeCount = 0;

                foreach (Edge edge in panel.ExternalEdges)
                {
                    edgeCount++;
                    segmentsPanel.AddRange(BHEG.Query.ISubParts(edge.Curve).ToList());
                    if (edge.Support != null)
                    {
                        if (!edgeConstraints.ContainsKey(edge.Support.Name))
                        {
                            edgeConstraints.Add(edge.Support.Name, panelNum.ToString() + "_" + "Edge(" + edgeCount.ToString() + ")");
                        }
                        else
                        {
                            edgeConstraints[edge.Support.Name] = edgeConstraints[edge.Support.Name] + " " + panelNum.ToString() + "_" + "Edge(" + edgeCount.ToString() + ")";
                        }
                    }
                }

                if (segmentsPanel.Count > 1)
                {
                    contourPanel = m_RobotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CONTOUR);
                    foreach (ICurve crv in segmentsPanel)
                    {
                        RobotGeoSegment segment = m_RobotApplication.CmpntFactory.Create(BH.Engine.Robot.Convert.SegmentType(crv));
                        (contourPanel as RobotGeoContour).Add(BH.Engine.Robot.Convert.Segment(crv, segment));
                    }
                }
                else
                {
                    contourPanel = m_RobotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CIRCLE);
                    BH.Engine.Robot.Convert.SingleContourGeometry(segmentsPanel[0], contourPanel);
                }

                contourPanel.Initialize();
                rPanel.Main.Geometry = contourPanel;

                rPanel.Main.Attribs.Meshed = 1;
                rPanel.Initialize();
                rPanel.Update();

                if (panel.Property is LoadingPanelProperty)
                    rPanel.SetLabel(IRobotLabelType.I_LT_CLADDING, panel.Property.Name);

                else
                    rPanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, panel.Property.Name);

                RobotGeoObject contourOpening;
                foreach (Opening opening in panel.Openings)
                {
                    List<ICurve> segmentsOpening = new List<ICurve>();
                    foreach (Edge edge in opening.Edges)
                    {
                        segmentsOpening.AddRange(BHEG.Query.ISubParts(edge.Curve).ToList());
                    }

                    if (segmentsOpening.Count > 1)
                    {
                        contourOpening = m_RobotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CONTOUR);
                        foreach (ICurve crv in segmentsOpening)
                        {
                            RobotGeoSegment segment = m_RobotApplication.CmpntFactory.Create(BH.Engine.Robot.Convert.SegmentType(crv));
                            (contourOpening as RobotGeoContour).Add(BH.Engine.Robot.Convert.Segment(crv, segment));
                        }
                    }
                    else
                    {
                        contourOpening = m_RobotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CIRCLE);
                        BH.Engine.Robot.Convert.SingleContourGeometry(segmentsOpening[0], contourOpening);
                    }
                    contourOpening.Initialize();
                    rPanel = objServer.Create(panelNum);
                    rPanel.Main.Geometry = contourOpening;
                    rPanel.Initialize();
                    rPanel.Update();
                }
            }
            m_RobotApplication.Project.Structure.Objects.EndMultiOperation();

            foreach (string supportName in edgeConstraints.Keys)
            {
                RobotObjEdgeSelection panelEdgeSel = m_RobotApplication.Project.Structure.Selections.CreateEdgeSelection();
                panelEdgeSel.FromText(edgeConstraints[supportName]);
                m_RobotApplication.Project.Structure.Objects.SetLabel((RobotSelection)panelEdgeSel, IRobotLabelType.I_LT_SUPPORT, supportName);
                //m_RobotApplication.Project.Structure.Objects.SetLabel(panelEdgeSel, IRobotLabelType.I_LT_SUPPORT, supportName);
            }
            m_RobotApplication.Interactive = 1;
            return true;
        }

        /***************************************************/

    }

}

