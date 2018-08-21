using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Common.Materials;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        protected override object NextId(Type type, bool refresh)
        {
            int index = 1;

            if (!refresh && m_indexDict.TryGetValue(type, out index))
            {
                index++;
                m_indexDict[type] = index;
            }
            else
            {
                if (type == typeof(BH.oM.Adapters.Robot.DesignGroup))
                {
                    List<int> groupNumbers = new List<int>();
                    foreach (BH.oM.Adapters.Robot.DesignGroup designGroup in ReadDesignGroups())
                    {
                        groupNumbers.Add(designGroup.Number);
                    }
                    groupNumbers.Sort();
                    index = groupNumbers.Count > 0 ? groupNumbers.Last() + 1 : 1;
                }
                if (type == typeof(Bar))
                {
                    index = m_RobotApplication.Project.Structure.Bars.FreeNumber;
                }
                //if (typeof(ISectionProperty).IsAssignableFrom(type))
                //{
                //    if (m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_SECTION) != null)
                //        index = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_SECTION).Count + 1;
                //    else
                //        index = 1;
                //}
                //if (type == typeof(Material))
                //{
                //    if (m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_MATERIAL) != null)
                //        index = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_MATERIAL).Count + 1;
                //    else
                //        index = 1;
                //}
                if (type == typeof(Material))
                {
                    if (m_indexDict.ContainsKey(type))
                        index = m_indexDict[type] + 1;
                    else
                        index = 1;
                }
                if (type == typeof(Loadcase))
                {
                    index = m_RobotApplication.Project.Structure.Cases.FreeNumber;
                }
                if (type == typeof(PanelPlanar))
                {
                    index = m_RobotApplication.Project.Structure.Objects.FreeNumber;
                }
                if (type == typeof(LoadCombination))
                {
                    index = m_RobotApplication.Project.Structure.Cases.FreeNumber;
                }
                if (type == typeof(Node))
                {
                    index = m_RobotApplication.Project.Structure.Nodes.FreeNumber;
                }
                if (type == typeof(MeshFace))
                {
                    index = m_RobotApplication.Project.Structure.FiniteElems.FreeNumber;
                }
                if (type == typeof(BH.oM.Structure.Elements.PanelPlanar)) //TODO: Check that this is the right rtype of panel
                {
                    index = m_RobotApplication.Project.Structure.Objects.FreeNumber;
                }
                m_indexDict[type] = index;
            }
            return index;
        }

        /***************************************************/

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private Dictionary<Type, int> m_indexDict = new Dictionary<Type, int>();

        /***************************************************/
    }

}
