using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Base;
using BH.oM.Common.Materials;
using BH.oM.Adapters.Robot;

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
            if (type == typeof(Opening))
                return ReadOpenings();
            if (type == typeof(Constraint6DOF))
                return ReadConstraints6DOF();
            if (type == typeof(Material))
                return ReadMaterial();
            if (type == typeof(PanelPlanar))
                return ReadPanels();
            if (type == typeof(MeshFace))
                return new List<MeshFace>();
            if (typeof(IProperty2D).IsAssignableFrom(type))
                return ReadProperty2D();
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
            if (typeof(ILoad).IsAssignableFrom(type))
                return ReadLoads(); //TODO: Implement load extraction
            if (type.IsGenericType && type.Name == typeof(BHoMGroup<IBHoMObject>).Name)
                return new List<BHoMGroup<IBHoMObject>>();
            if (type == typeof(FramingElementDesignProperties))
                return ReadFramingElementDesignProperties();
            if (type == typeof(BH.oM.Adapters.Robot.DesignGroup))
                return ReadDesignGroups();

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
                objects.AddRange(ReadPanels());
                //objects.AddRange(ReadFramingElementDesignProperties());
                return objects;
            }

            return null;         
        }

        /***************************************************/

    }

}

