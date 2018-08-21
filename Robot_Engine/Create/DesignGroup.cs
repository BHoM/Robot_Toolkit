using System.Collections.Generic;
using BH.oM.Adapters.Robot;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Create
    {
        /***************************************************/
        /****           Public Constructors             ****/
        /***************************************************/

        public static DesignGroup DesignGroup(int number = 0, string materialName = "", List<int> elementIds = null)
        {
            DesignGroup designGroup = new DesignGroup();
            designGroup.Number = number;
            designGroup.MaterialName = materialName;
            if (elementIds != null)
                designGroup.MemberIds = elementIds;

            return designGroup;
        }

        /***************************************************/
    }
}
