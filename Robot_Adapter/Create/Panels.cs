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
                List<ICurve> segments = new List<ICurve>();
                foreach (Edge edge in panel.ExternalEdges)
                {
                    segments.AddRange(BHEG.Query.ISubParts(edge.Curve).ToList());
                }

                RobotGeoObject contour;
                RobotObjObject rpanel = objServer.Create(System.Convert.ToInt32(panel.CustomData[AdapterId]));
                if (segments.Count > 1)
                {
                    contour = m_RobotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CONTOUR);
                    foreach (ICurve crv in segments)
                    {
                        RobotGeoSegment segment = m_RobotApplication.CmpntFactory.Create(BH.Engine.Robot.Convert.SegmentType(crv));
                        (contour as RobotGeoContour).Add(BH.Engine.Robot.Convert.Segment(crv, segment));
                    }
                }
                else
                {
                    contour = m_RobotApplication.CmpntFactory.Create(IRobotComponentType.I_CT_GEO_CIRCLE);
                    BH.Engine.Robot.Convert.SingleContourGeometry(segments[0], contour);
                }

                rpanel.Main.Geometry = contour as RobotGeoObject;
                rpanel.Initialize();

                rpanel.Main.Attribs.Meshed = 1;
                rpanel.Update();
                if (panel.Property is LoadingPanelProperty)
                    rpanel.SetLabel(IRobotLabelType.I_LT_CLADDING, panel.Property.Name);

                else
                    rpanel.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, panel.Property.Name);
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

