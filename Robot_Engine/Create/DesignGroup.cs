using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Adapters.Robot;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Create
    {
        public static DesignGroup DesignGroup(int number = 0, string materialName = "", List<int> elementIds = null)
        {
            DesignGroup _designGroup = new DesignGroup();
            _designGroup.Number = number;
            _designGroup.MaterialName = materialName;
            if (elementIds != null)
                _designGroup.MemberIds = elementIds;

            return _designGroup;
        }     
    }
}
