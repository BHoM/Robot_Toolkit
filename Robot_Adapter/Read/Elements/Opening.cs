﻿using System.Collections.Generic;
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Geometry;
using System.Collections;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Opening> ReadOpenings(IList ids = null)
        {
            List<Opening> openings = new List<Opening>();
            IRobotStructure rStructure = m_RobotApplication.Project.Structure;
            RobotSelection rSelect = rStructure.Selections.Create(IRobotObjectType.I_OT_GEOMETRY);

            List<int> openingIds = CheckAndGetIds(ids);

            if (openingIds == null)
            {
                rSelect.FromText("all");
            }
            else
            {
                rSelect.FromText(BH.Engine.Robot.Convert.ToRobotSelectionString(openingIds));
            }
            IRobotCollection rOpenings = rStructure.Objects.GetMany(rSelect);
            Opening tempOpening = null;
            
            for (int i = 1; i <= rOpenings.Count; i++)
            {
                RobotObjObject rOpening = (RobotObjObject)rOpenings.Get(i);
                System.Type type = rOpening.GetType(); 
               
                if (rOpening.Main.Attribs.Meshed != 1)
                {
                    ICurve outline = BH.Engine.Robot.Convert.ToBHoMGeometry(rOpening.Main.GetGeometry() as dynamic);
                    tempOpening = BH.Engine.Structure.Create.Opening(outline);
                }
                if (tempOpening != null)
                {
                    tempOpening.CustomData[AdapterId] = rOpening.Number;
                    openings.Add(tempOpening);
                }
            }

            return openings;
        }

        /***************************************************/

    }
}