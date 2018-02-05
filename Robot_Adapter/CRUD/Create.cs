using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.Engine.Robot;

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

            if (typeof(T).IsAssignableFrom(typeof(Node)))
                success = Create(objects as IEnumerable<Node>);

            if (typeof(T).IsAssignableFrom(typeof(Bar)))
                success = Create(objects as IEnumerable<Bar>);
            //foreach (T obj in objects)
            //{
            //    success &= Create(obj as dynamic);
            //}

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



        private bool Create(IEnumerable<BH.oM.Structural.Elements.Node> nodes)
        {
            RobotStructureCache rcache = m_RobotApplication.Project.Structure.CreateCache();
            RobotSelection nodeSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);

            foreach (Node node in nodes)
            {
                int nodeNum = 0;
                int.TryParse(node.CustomData[AdapterId].ToString(), out nodeNum);
                rcache.AddNode(nodeNum, node.Position.X, node.Position.Y, node.Position.Z);
                nodeSel.AddText(nodeNum.ToString());
            }
            m_RobotApplication.Project.Structure.ApplyCache(rcache);
            IRobotCollection robotNodeCol = m_RobotApplication.Project.Structure.Nodes.GetMany(nodeSel);

            //for (int i = 1; i <= robotNodeCol.Count; i++)
            //{
            //    RobotNode node = robotNodeCol.Get(i);
            //    node.SetLabel()
            //}



            return true;

      
        }

        public bool Create(IEnumerable<Bar> bhomBars)
        {
            RobotStructureCache rcache = m_RobotApplication.Project.Structure.CreateCache();
            RobotSelection barSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            foreach (Bar bhomBar in bhomBars)
            {
                int barNum = System.Convert.ToInt32(bhomBar.CustomData[AdapterId]);
                rcache.AddBar(barNum,
                              System.Convert.ToInt32(bhomBar.StartNode.CustomData[AdapterId]),
                              System.Convert.ToInt32(bhomBar.EndNode.CustomData[AdapterId]),
                              "UC 305x305x97",
                              //bhomBar.SectionProperty.Name, 
                              "STEEL",
                              //bhomBar.SectionProperty.Material.Name, 
                              bhomBar.OrientationAngle);
                bhomBar.CustomData[AdapterId] = barNum;
                barSel.AddText(barNum.ToString());
            }
            m_RobotApplication.Project.Structure.ApplyCache(rcache);
            //IRobotCollection robotBarCol = m_RobotApplication.Project.Structure.Bars.GetMany(barSel);

            //if (robotBarCol.Count > 0)
            //    return true;
            //else
                return true;

        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

