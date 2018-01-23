using BH.oM.Common.Materials;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.Engine.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /**** BHoM Adapter Interface                    ****/
        /***************************************************/

        protected override IEqualityComparer<T> Comparer<T>()
        {
            Type type = typeof(T);

            if (m_Comparers.ContainsKey(type))
            {
                return m_Comparers[type] as IEqualityComparer<T>;
            }
            else
            {
                return EqualityComparer<T>.Default;
            }
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private static Dictionary<Type, object> m_Comparers = new Dictionary<Type, object>
        {
            {typeof(Node), new BH.Engine.Structure.NodeDistanceComparer(3) },
            //{typeof(Bar), new BH.Engine.Structure.BarEndNodesDistanceComparer(3) },
            {typeof(ISectionProperty), new BH.Engine.Base.Objects.BHoMObjectNameOrToStringComparer() },
            //{typeof(Material), new BH.Engine.Base.BHoMObjectNameComparer() },
        };

    }
}
