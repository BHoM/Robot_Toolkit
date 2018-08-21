using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<RigidLink> rigidLinks)
        {
            foreach (RigidLink rLink in rigidLinks)
            {
                string[] str = rLink.SlaveNodes.Select(x => x.CustomData[AdapterId].ToString() + ",").ToList().ToArray();
                string slaves = string.Join("", str).TrimEnd(',');
                m_RobotApplication.Project.Structure.Nodes.RigidLinks.Set(System.Convert.ToInt32(rLink.MasterNode.CustomData[AdapterId]), slaves, rLink.Constraint.Name);
            }
            return true;
        }

        /***************************************************/

    }

}

