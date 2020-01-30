/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Loads;
using BH.oM.Base;
using BH.oM.Physical.Materials;
using BH.oM.Adapters.Robot;
using BH.oM.Structure.Results;
using BH.oM.Common;
using BH.oM.Data.Requests;
using System.Linq;
using BH.oM.Adapter;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {                 
        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/
        
        protected override IEnumerable<IBHoMObject> IRead(Type type, IList indices, ActionConfig actionConfig = null)
        {
            if (type == typeof(Node))
                return ReadNodes(indices);

            if (type == typeof(Bar))
                return ReadBars(indices);

            if (type == typeof(Opening))
                return ReadOpenings();

            if (type == typeof(Constraint6DOF))
                return ReadConstraints6DOF();

            if (type == typeof(Material))
                return ReadMaterial();

            if (type == typeof(Panel))
                return ReadPanels(indices);

            if (type == typeof(FEMesh))
                return ReadMeshes();

            if (typeof(ISurfaceProperty).IsAssignableFrom(type))
                return ReadProperty2D();

            if (type == typeof(RigidLink))
                return ReadRigidLinks();
                //return new List<RigidLink>();

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
                return ReadLoads(type); //TODO: Implement load extraction

            if (type.IsGenericType && type.Name == typeof(BHoMGroup<IBHoMObject>).Name)
                return ReadGroups();

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

            return new List<IBHoMObject>();         
        }

        /***************************************************/

        protected override IEnumerable<IResult> ReadResults(Type type, IList ids = null, IList cases = null, int divisions = 5, ActionConfig actionConfig = null)
        {
            IResultRequest request = Engine.Structure.Create.IResultRequest(type, ids?.Cast<object>(), cases?.Cast<object>(), divisions);

            if (request != null)
                return this.ReadResults(request as dynamic);
            else
                return base.ReadResults(type, ids, cases, divisions);
        }

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
                        else if (o is IBHoMObject && (o as IBHoMObject).CustomData.TryGetValue(AdapterIdName, out idObj) && int.TryParse(idObj.ToString(), out id))
                            idsOut.Add(id);
                    }
                    return idsOut;
                }
            }
        }

        /***************************************************/

    }

}


