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

        public override int UpdateProperty(Type type, IEnumerable<object> ids, string property, object newValue)
        {

            if (property == "Tags")
            {
                List<int> indecies = ids.Select(x => (int)x).ToList();
                if (indecies.Count < 1)
                    return 0;

                List<HashSet<string>> tags = (newValue as IEnumerable<HashSet<string>>).ToList();
                return UpdateTags(type, indecies, tags);
            }

            return 0;
        }

        private int UpdateTags(Type t, List<int> indecies, List<HashSet<string>> tags)
        {
            Dictionary<int, HashSet<string>> typeTags = this.GetTypeTags(t);// = m_tags[t];

            for (int i = 0; i < indecies.Count; i++)
            {
                typeTags[indecies[i]] = tags[i];
            }
            
            return indecies.Count;
        }

    }
}
