using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using RobotOM;

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

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;

            foreach (T obj in objects)
            {
                success &= Create(obj as dynamic);
            }

            updateview();
            return success;
        }

        private bool Create(BH.oM.Base.IObject obj)
        {
            if (obj is Node)
                return Create(obj);
            else
                return false;
        }



        private bool Create(BH.oM.Structural.Elements.Node nodes)
        {
            RobotStructureCache rcache = m_RobotApplication.Project.Structure.CreateCache();
            RobotSelection nodeSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);

            int nodeNum = 0;
            int.TryParse(nodes.CustomData[AdapterId].ToString(), out nodeNum);
            rcache.AddNode(nodeNum, nodes.Position.X, nodes.Position.Y, nodes.Position.Z);
            nodes.CustomData[AdapterId] = nodeNum;
            nodeSel.AddText(nodeNum.ToString());

            m_RobotApplication.Project.Structure.ApplyCache(rcache);
            IRobotCollection robotNodeCol = m_RobotApplication.Project.Structure.Nodes.GetMany(nodeSel);

            if (robotNodeCol.Count > 0)
                return true;
            else
                return false;           
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

