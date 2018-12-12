using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Surface;
using BH.oM.Structure.Properties.Constraint;
using BH.oM.Structure.Loads;
using BH.oM.Base;
using BH.oM.Common.Materials;
using BH.oM.Adapters.Robot;
using BH.oM.Structure.Results;
using BH.oM.Common;
using BH.oM.DataManipulation.Queries;
using System.Linq;

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
                return ReadPanels(indices);

            if (type == typeof(FEMesh))
                return ReadMeshes();

            if (typeof(ISurfaceProperty).IsAssignableFrom(type))
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
                objects.AddRange(ReadFramingElementDesignProperties());
                return objects;
            }

            return null;         
        }

        /***************************************************/

        protected override IEnumerable<IResult> ReadResults(Type type, IList ids = null, IList cases = null, int divisions = 5)
        {
            if (type == typeof(BarForce))
                return ReadBarForce(ids, cases, divisions);

            if (type == typeof(NodeDisplacement))
                return ReadNodeDisplacement(ids, cases, divisions);

            if (type == typeof(NodeReaction))
                return ReadNodeReactions(ids, cases, divisions);

            return base.ReadResults(type, ids, cases, divisions);

        }

        protected override IEnumerable<IResultCollection> ReadResults(FilterQuery query)
        {
            if (query.Type == typeof(MeshResults))
                return ReadMeshResults(query);

            return null;
        }

        /***************************************************/

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<int> CheckAndGetIds(IList ids)
        {
            if (ids == null)
            {
                return null;
            }
            else
            {
                if (ids is List<string>)
                    return (ids as List<string>).Select(x => int.Parse(x)).ToList();
                else if (ids is List<int>)
                    return ids as List<int>;
                else if (ids is List<double>)
                    return (ids as List<double>).Select(x => (int)Math.Round(x)).ToList();
                else
                {
                    List<int> idsOut = new List<int>();
                    foreach (object o in ids)
                    {
                        int id;
                        object idObj;
                        if (int.TryParse(o.ToString(), out id))
                        {
                            idsOut.Add(id);
                        }
                        else if (o is IBHoMObject && (o as IBHoMObject).CustomData.TryGetValue(AdapterId, out idObj) && int.TryParse(idObj.ToString(), out id))
                            idsOut.Add(id);
                    }
                    return idsOut;
                }
            }
        }

        /***************************************************/

    }

}

