using BH.oM.Common.Materials;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using BH.oM.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /**** BHoM Adapter Interface                    ****/
        /***************************************************/

        protected override List<Type> DependencyTypes<T>()
        {
            Type type = typeof(T);

            if (m_DependencyTypes.ContainsKey(type))
                return m_DependencyTypes[type];

            else if (m_DependencyTypes.ContainsKey(type.BaseType))
                return m_DependencyTypes[type.BaseType];

            else
            {
                foreach (Type interType in type.GetInterfaces())
                {
                    if (m_DependencyTypes.ContainsKey(interType))
                        return m_DependencyTypes[interType];
                }
            }

            return new List<Type>();
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private static Dictionary<Type, List<Type>> m_DependencyTypes = new Dictionary<Type, List<Type>>
        {
            {typeof(Bar), new List<Type> { typeof(ISectionProperty), typeof(Node), typeof(BarRelease), typeof(FramingElementDesignProperties)}},
            {typeof(ISectionProperty), new List<Type> { typeof(Material) } },
            {typeof(Node), new List<Type> { typeof(Constraint6DOF) } },
            {typeof(ILoad), new List<Type> { typeof(Loadcase) } },
            {typeof(LoadCombination), new List<Type> { typeof(Loadcase) } },
            {typeof(PanelPlanar), new List<Type> { typeof(IProperty2D) } },
            {typeof(IProperty2D), new List<Type> { typeof(Material) } },
            {typeof(RigidLink), new List<Type> { typeof(LinkConstraint), typeof(Node) } },
            {typeof(MeshFace), new List<Type> { typeof(IProperty2D), typeof(Node) } }
        };

        /***************************************************/
    }
}
