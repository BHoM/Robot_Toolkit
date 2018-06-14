using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.Engine.Serialiser;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Base;
using BH.oM.Common.Materials;
using BH.oM.Structural.Design;
using BH.oM.Adapters.Robot;
using BH.oM.Geometry;
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/

        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        /***************************************************/

        public List<PanelPlanar> ReadPanels(List<string> ids = null)
        {
            Dictionary<string, IProperty2D> BHoMProperties = ReadProperty2D().ToDictionary(x => x.Name);
            List<PanelPlanar> BHoMPanels = new List<PanelPlanar>();
            IRobotStructure rStructure = m_RobotApplication.Project.Structure;
            List<Material> bhomMaterials = new List<Material>();

            if (ids == null)
            {
                RobotSelection rSelect = rStructure.Selections.Create(IRobotObjectType.I_OT_PANEL);
                rSelect.FromText("all");
                IRobotCollection rPanels = rStructure.Objects.GetMany(rSelect);
                for (int i = 1; i <= rPanels.Count; i++)
                {
                    RobotObjObject rpanel = (RobotObjObject)rPanels.Get(i);
                    PanelPlanar BHoMPanel = null;

                    if (rpanel.Main.Attribs.Meshed == 1)
                    {
                        ICurve outline = BH.Engine.Robot.Convert.ToBHoMGeometry(rpanel.Main.GetGeometry() as dynamic);
                        List<Opening> openings = new List<Opening>();
                        BHoMPanel = BH.Engine.Structure.Create.PanelPlanar(outline, openings);

                        if (rpanel.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) != 0)
                        {
                            string propName = rpanel.GetLabelName(IRobotLabelType.I_LT_PANEL_THICKNESS);
                            if (BHoMProperties.ContainsKey(propName))
                                BHoMPanel.Property = BHoMProperties[propName];
                        }
                        BHoMPanel.CustomData[AdapterId] = rpanel.Number;
                    }
                    BHoMPanels.Add(BHoMPanel);
                }
            }
            return BHoMPanels;
        }
    }
}
