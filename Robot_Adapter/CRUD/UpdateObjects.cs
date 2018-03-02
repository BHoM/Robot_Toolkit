using System.Collections.Generic;
using System.Linq;
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
        protected override bool UpdateObjects<T>(IEnumerable<T> objects)
        {
            bool success = true;
            success = Update(objects as dynamic);
            updateview();
            return success;
        }

        protected bool Update(IEnumerable<IBHoMObject> bhomObjects)
        {
            return true;
        }


        protected bool Update(IEnumerable<Node> nodes)
        {

            foreach (Node node in nodes)
            { 
                RobotNode robotNode = m_RobotApplication.Project.Structure.Nodes.Get((int)node.CustomData[AdapterId]) as RobotNode;
                if (robotNode == null)
                    return false;

                if (node.Constraint != null && !string.IsNullOrWhiteSpace(node.Constraint.Name))
                    robotNode.SetLabel(IRobotLabelType.I_LT_SUPPORT, node.Constraint.Name);

                robotNode.X = node.Position.X; robotNode.Y = node.Position.Y; robotNode.Z = node.Position.Z;
            }
            return true;
        }

        protected bool Update(IEnumerable<Material> materials)
        {
            bool success = true;
            success = Create(materials);
            return success;
        }
    }
}
