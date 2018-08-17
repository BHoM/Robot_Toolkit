using System.Collections.Generic;
using System.Linq;
using System;
using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using RobotOM;

using BH.Engine.Robot;
using BHEG = BH.Engine.Geometry;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Index Adapter Interface                   ****/
        /***************************************************/
              
        /***************************************************/
              
        public bool CreateCollection(IEnumerable<PanelPlanar> panels)
        {
            m_RobotApplication.Interactive = 0;
            m_RobotApplication.Project.Structure.Objects.BeginMultiOperation();
            RobotObjObjectServer objServer = m_RobotApplication.Project.Structure.Objects;

            foreach (PanelPlanar panel in panels)
            {
                List<ICurve> segmentsPanel = new List<ICurve>();
                foreach (Edge edge in panel.ExternalEdges)
                {
                    segmentsPanel.AddRange(BHEG.Query.ISubParts(edge.Curve).ToList());
                }

                RobotGeoObject contourPanel;
                int panelNum = System.Convert.ToInt32(panel.CustomData[AdapterId]);
                RobotObjObject rPanel = objServer.Create(panelNum);
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
            m_RobotApplication.Interactive = 1;
            return true;
        }

        /***************************************************/

      
        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

