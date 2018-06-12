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
using BH.oM.Adapters.Robot;
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

        public List<IProperty2D> ReadProperty2D(List<string> ids = null)
        {
            List<IProperty2D> BHoMProps = new List<IProperty2D>();
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotCollection rThicknessProps = labelServer.GetMany(IRobotLabelType.I_LT_PANEL_THICKNESS);
            Dictionary<string, Material> BHoMMat = new Dictionary<string, Material>();
            BHoMMat = (ReadMaterial().ToDictionary(x => x.Name));

            for (int i = 1; i <= rThicknessProps.Count; i++)
            {

                IRobotLabel rThicknessProp = rThicknessProps.Get(i);
                BHoMProps.Add(BH.Engine.Robot.Convert.ToBHoMObject(rThicknessProp, BHoMMat));
            }

            return BHoMProps;
        }

        /***************************************************/


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}