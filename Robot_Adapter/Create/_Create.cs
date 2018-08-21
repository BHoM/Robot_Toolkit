using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.oM.Common.Materials;
using BH.oM.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Adapter Methods                 ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;
            if (objects.Count() > 0)
            {
                var watch = new System.Diagnostics.Stopwatch();
                if (objects.First() is Constraint6DOF)
                {
                    success = CreateCollection(objects as IEnumerable<Constraint6DOF>);
                }

                if (objects.First() is RigidLink)
                {
                    success = CreateCollection(objects as IEnumerable<RigidLink>);
                }

                if (objects.First() is BarRelease)
                {
                    success = CreateCollection(objects as IEnumerable<BarRelease>);
                }

                if (objects.First() is LinkConstraint)
                {
                    success = CreateCollection(objects as IEnumerable<LinkConstraint>);
                }

                if (typeof(ISectionProperty).IsAssignableFrom(objects.First().GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<ISectionProperty>);
                }

                if (objects.First() is Material)
                {
                    success = CreateCollection(objects as IEnumerable<Material>);
                }

                if (objects.First() is Loadcase)
                {
                    success = CreateCollection(objects as IEnumerable<Loadcase>);
                }

                if (objects.First() is MeshFace)
                {
                    success = CreateCollection(objects as IEnumerable<MeshFace>);
                }

                if (objects.First() is Node)
                {
                    success = CreateCollection(objects as IEnumerable<Node>);
                }

                if (objects.First() is Bar)
                {
                    success = CreateCollection(objects as IEnumerable<Bar>);
                }

                if (typeof(ILoad).IsAssignableFrom(objects.First().GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<ILoad>);
                }

                if (objects.First() is PanelPlanar)
                {
                    success = CreateCollection(objects as IEnumerable<PanelPlanar>);
                }

                if (typeof(IProperty2D).IsAssignableFrom(objects.First().GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<IProperty2D>);
                }

                if (objects.First() is LoadCombination)
                {
                    success = CreateCollection(objects as IEnumerable<LoadCombination>);
                }

                if (objects.First() is FramingElementDesignProperties)
                {
                    success = CreateCollection(objects as IEnumerable<FramingElementDesignProperties>);
                }

                if (objects.First() is DesignGroup)
                {
                    success = CreateCollection(objects as IEnumerable<BH.oM.Adapters.Robot.DesignGroup>);
                }
            }
            
            updateview();
            return success;
        }          
      
        /***************************************************/

    }

}

