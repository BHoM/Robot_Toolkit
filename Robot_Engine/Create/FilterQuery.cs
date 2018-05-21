using BH.oM.DataManipulation.Queries;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BH.Engine.DataManipulation.Structural
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<FilterQuery> BarsNodes(string tag = "")
        {
            List<FilterQuery> typeList = new List<FilterQuery>();

            typeList.Add(new FilterQuery { Type = typeof(BH.oM.Structural.Elements.Bar), Tag = tag });
            typeList.Add(new FilterQuery { Type = typeof(BH.oM.Structural.Elements.Node), Tag = tag });

            return typeList;
        }

        /***************************************************/

        public static List<FilterQuery> BarsNodesPanels(string tag = "")
        {
            List<FilterQuery> typeList = new List<FilterQuery>();

            typeList.Add(new FilterQuery { Type = typeof(BH.oM.Structural.Elements.Bar), Tag = tag });
            typeList.Add(new FilterQuery { Type = typeof(BH.oM.Structural.Elements.Node), Tag = tag });
            typeList.Add(new FilterQuery { Type = typeof(BH.oM.Structural.Elements.PanelPlanar), Tag = tag });
            typeList.Add(new FilterQuery { Type = typeof(BH.oM.Structural.Elements.PanelFreeForm), Tag = tag });

            return typeList;
        }

        /***************************************************/


    }
}
