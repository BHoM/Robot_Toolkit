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

        /***************************************************/
        /**** Index Adapter Interface                   ****/
        /***************************************************/

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

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

