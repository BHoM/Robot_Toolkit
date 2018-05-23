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
using BH.oM.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Index Adapter Interface                   ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;
            if (objects.Count() > 0)
            {
                var watch = new System.Diagnostics.Stopwatch();
                if (objects.First() is Constraint6DOF)
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    success = CreateCollection(objects as IEnumerable<Constraint6DOF>);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
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
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    success = CreateCollection(objects as IEnumerable<ISectionProperty>);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                }

                if (objects.First() is Material)
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    success = CreateCollection(objects as IEnumerable<Material>);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
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
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    success = CreateCollection(objects as IEnumerable<Node>);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                }

                if (objects.First() is Bar)
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    success = CreateCollection(objects as IEnumerable<Bar>);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
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

            }
            //success = CreateObjects(objects as dynamic);
            updateview();
            return success;
        }          
      
        /***************************************************/
        

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

