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

        public List<Opening> ReadOpenings(List<string> ids = null)
        {
            List<Opening> openings = new List<Opening>();
            IRobotStructure rStructure = m_RobotApplication.Project.Structure;

            if (ids == null)
            {
                RobotSelection rSelect = rStructure.Selections.Create(IRobotObjectType.I_OT_GEOMETRY);
                rSelect.FromText("all");
                IRobotCollection rOpenings = rStructure.Objects.GetMany(rSelect);
                Opening tempOpening = null;
                for (int i = 1; i <= rOpenings.Count; i++)
                {
                    RobotObjObject rOpening = (RobotObjObject)rOpenings.Get(i);

                    if (rOpening.Main.Attribs.Meshed != 1)
                    {
                        ICurve outline = BH.Engine.Robot.Convert.ToBHoMGeometry(rOpening.Main.GetGeometry() as dynamic);
                        tempOpening = BH.Engine.Structure.Create.Opening(outline);
                    }
                    if(tempOpening != null)
                    {
                        tempOpening.CustomData[AdapterId] = rOpening.Number;
                        openings.Add(tempOpening);
                    }
                }
            }
            return openings;
        }
    }
}
