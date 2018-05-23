using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.Engine.Serialiser;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Base;
using BH.oM.Common.Materials;
using BH.oM.Structural.Design;
using BH.oM.Adapters.Robot;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {         
        
        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> Read(Type type, IList indices = null)
        {
            if (type == typeof(Node))
                return ReadNodes();
            if (type == typeof(Bar))
                return ReadBars();
            if (type == typeof(Constraint6DOF))
                return ReadConstraints6DOF();
            if (type == typeof(Material))
                return ReadMaterial();
            if (type == typeof(PanelPlanar))
                return new List<PanelPlanar>();
            if (type == typeof(MeshFace))
                return new List<MeshFace>();
            if (type == typeof(IProperty2D))
                return new List<IProperty2D>();
            if (type == typeof(RigidLink))
                return new List<RigidLink>();
            if (type == typeof(LoadCombination))
                return new List<LoadCombination>();
            if (type == typeof(LinkConstraint))
                return new List<LinkConstraint>();
            if (type == typeof(BarRelease))
                return ReadBarRelease();
            if (type == typeof(Loadcase))
                return ReadLoadCase();
            if (typeof(ISectionProperty).IsAssignableFrom(type))
                return ReadSectionProperties();
            else if (type == typeof(ILoad) || type.GetInterfaces().Contains(typeof(ILoad)))
                return new List<ILoad>(); //TODO: Implement load extraction
            if (type.IsGenericType && type.Name == typeof(BHoMGroup<IBHoMObject>).Name)
                return new List<BHoMGroup<IBHoMObject>>();

            if (type == typeof(BHoMObject))
            {
                List<IBHoMObject> objects = new List<IBHoMObject>();
                objects.AddRange(ReadConstraints6DOF());
                objects.AddRange(ReadMaterial());
                objects.AddRange(ReadBarRelease());
                objects.AddRange(ReadLoadCase());
                objects.AddRange(ReadSectionProperties());
                objects.AddRange(ReadNodes());
                objects.AddRange(ReadBars());
                objects.AddRange(ReadDesignGroups());
                objects.AddRange(ReadFramingElementDesignProperties());
                return objects;
            }

            return null;         
        }

        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/
             
        /***************************************************/


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

