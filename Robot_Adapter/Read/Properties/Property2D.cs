using System.Collections.Generic;
using System.Linq;
using RobotOM;
using BH.oM.Structure.Properties.Surface;
using BH.oM.Common.Materials;


namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<ISurfaceProperty> ReadProperty2D(List<string> ids = null)
        {
            List<ISurfaceProperty> BHoMProps = new List<ISurfaceProperty>();
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotCollection rThicknessProps = labelServer.GetMany(IRobotLabelType.I_LT_PANEL_THICKNESS);
            IRobotCollection rCladdingProps = labelServer.GetMany(IRobotLabelType.I_LT_CLADDING);
            Dictionary<string, Material> BHoMMat = new Dictionary<string, Material>();
            BHoMMat = (ReadMaterial().ToDictionary(x => x.Name));

            for (int i = 1; i <= rThicknessProps.Count; i++)
            {
                IRobotLabel rThicknessProp = rThicknessProps.Get(i);
                ISurfaceProperty tempProp = BH.Engine.Robot.Convert.ToBHoMObject(rThicknessProp, BHoMMat);
                tempProp.CustomData.Add(AdapterId, tempProp.Name);
                BHoMProps.Add(tempProp);
            }

            for (int i = 1; i <= rCladdingProps.Count; i++)
            {
                IRobotLabel rCladdingProp = rCladdingProps.Get(i);
                ISurfaceProperty tempProp = BH.Engine.Robot.Convert.ToBHoMObject(rCladdingProp, BHoMMat);
                tempProp.CustomData.Add(AdapterId, tempProp.Name);
                BHoMProps.Add(tempProp);
            }
            return BHoMProps;
        }

        /***************************************************/

    }
}