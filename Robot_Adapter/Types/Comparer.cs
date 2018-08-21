using BH.oM.Common.Materials;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Properties;
using BH.Engine.Structure;
using System;
using System.Collections.Generic;
using BH.Engine.Base.Objects;

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
            {typeof(Node), new NodeDistanceComparer(3) },
            {typeof(ISectionProperty), new BHoMObjectNameOrToStringComparer() },
            {typeof(Material), new BHoMObjectNameComparer() },
            {typeof(Constraint6DOF), new BHoMObjectNameComparer() },
            {typeof(Loadcase), new BHoMObjectNameComparer() },
            {typeof(LinkConstraint), new BHoMObjectNameComparer() },
            {typeof(IProperty2D), new BHoMObjectNameComparer() },
            {typeof(BarRelease), new BHoMObjectNameComparer() }
        };

        /***************************************************/
    }
}
