using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.oM.Structural.Properties;
using BH.oM.Common.Materials;
using BH.oM.Geometry;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<PanelPlanar> ReadPanels(List<string> ids = null)
        {
            Dictionary<string, IProperty2D> BHoMProperties = ReadProperty2D().ToDictionary(x => x.Name);
            List<PanelPlanar> BHoMPanels = new List<PanelPlanar>();
            IRobotStructure rStructure = m_RobotApplication.Project.Structure;
            List<Material> bhomMaterials = new List<Material>();
            List<Opening> allOpenings = ReadOpenings();
            PanelPlanar BHoMPanel = null;

            if (ids == null)
            {
                RobotSelection rPanSelect = rStructure.Selections.Create(IRobotObjectType.I_OT_PANEL);
                RobotSelection rCladdingSelect = rStructure.Selections.Create(IRobotObjectType.I_OT_GEOMETRY);
                rPanSelect.FromText("all");
                rCladdingSelect.FromText("all");
                IRobotCollection rPanels = rStructure.Objects.GetMany(rPanSelect);                
                IRobotCollection rCladdings = rStructure.Objects.GetMany(rCladdingSelect);
                for (int i = 1; i <= rPanels.Count; i++)
                {
                    RobotObjObject rpanel = (RobotObjObject)rPanels.Get(i);
                    BHoMPanel = null;

                    if (rpanel.Main.Attribs.Meshed == 1)
                    {
                        ICurve outline = BH.Engine.Robot.Convert.ToBHoMGeometry(rpanel.Main.GetGeometry() as dynamic);
                        List<Opening> openings = BH.Engine.Robot.Convert.FindOpening(outline, allOpenings);
                        BHoMPanel = BH.Engine.Structure.Create.PanelPlanar(outline, openings);
                        BHoMPanel.CustomData[AdapterId] = rpanel.Number;

                        if (rpanel.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) != 0)
                        {
                            string propName = rpanel.GetLabelName(IRobotLabelType.I_LT_PANEL_THICKNESS);
                            if (BHoMProperties.ContainsKey(propName))
                                BHoMPanel.Property = BHoMProperties[propName];
                            else
                                BH.Engine.Reflection.Compute.RecordEvent("Failed to convert/create ConstantThickness property for panel " + rpanel.Number.ToString(), oM.Reflection.Debuging.EventType.Warning);
                        }
                        else
                        {
                            BH.Engine.Reflection.Compute.RecordEvent("Panel " + rpanel.Number.ToString() + " either has no property-label or the Property2D for this Robot label is not implemented", oM.Reflection.Debuging.EventType.Warning);
                        }
                    }
                    BHoMPanels.Add(BHoMPanel);
                }

                List<Opening> emptyOpenings = new List<Opening>();
                for (int i = 1; i <= rCladdings.Count; i++)
                {
                    RobotObjObject rCladding = (RobotObjObject)rCladdings.Get(i);

                    if (rCladding.Main.Attribs.Meshed == 1)
                    {
                        ICurve outline = BH.Engine.Robot.Convert.ToBHoMGeometry(rCladding.Main.GetGeometry() as dynamic);
                        BHoMPanel = BH.Engine.Structure.Create.PanelPlanar(outline, emptyOpenings);
                    }
                    if (BHoMPanel != null)
                    {
                        BHoMPanel.CustomData[AdapterId] = rCladding.Number;

                        if (rCladding.HasLabel(IRobotLabelType.I_LT_CLADDING) != 0)
                        {
                            string propName = rCladding.GetLabelName(IRobotLabelType.I_LT_CLADDING);
                            if (BHoMProperties.ContainsKey(propName))
                                BHoMPanel.Property = BHoMProperties[propName];
                            else
                                BH.Engine.Reflection.Compute.RecordEvent("Failed to convert/create ConstantThickness property for Cladding " + rCladding.Number.ToString(), oM.Reflection.Debuging.EventType.Warning);
                        }

                        else
                        {
                            BH.Engine.Reflection.Compute.RecordEvent("Cladding " + rCladding.Number.ToString() + " either has no property-label or the Property2D for this Robot label is not implemented", oM.Reflection.Debuging.EventType.Warning);
                        }

                        BHoMPanels.Add(BHoMPanel);
                    }
                }
            }
            return BHoMPanels;
        }
    }
}
