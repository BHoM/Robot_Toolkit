/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Offsets;
using BH.oM.Base;
using BH.oM.Physical.Materials;
using BH.oM.Adapters.Robot;
using BH.oM.Structure.Results;
using BH.oM.Analytical.Results;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Data.Requests;
using System.Linq;
using BH.oM.Adapter;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> IRead(Type type, IList indices, ActionConfig actionConfig = null)
        {
            if (type == null)
            {
                Engine.Base.Compute.RecordWarning("No Type was provided. No objects where read.");
                return new List<IBHoMObject>();
            }

            if (type == typeof(Node))
                return ReadNodes(indices);
            else if (type == typeof(Bar))
                return ReadBars(indices);
            else if (type == typeof(Opening))
                return ReadOpenings();
            else if (type == typeof(Constraint4DOF))
                return ReadConstraints4DOF(indices?.Cast<object>().Select(x => x.ToString()).ToList());
            else if (type == typeof(Constraint6DOF))
                return ReadConstraints6DOF(indices?.Cast<object>().Select(x => x.ToString()).ToList());
            else if (typeof(IMaterialFragment).IsAssignableFrom(type))
                return ReadMaterials(indices?.Cast<object>().Select(x => x.ToString()).ToList());
            else if (type == typeof(Panel))
                return ReadPanels(indices);
            else if (type == typeof(FEMesh))
                return ReadMeshes(indices);
            else if (typeof(ISurfaceProperty).IsAssignableFrom(type))
                return ReadSurfaceProperties(indices?.Cast<object>().Select(x => x.ToString()).ToList());
            else if (type == typeof(RigidLink))
                return ReadRigidLinks(indices);
            else if (type == typeof(LoadCombination))
                return ReadLoadCombinations();
            else if (type == typeof(LinkConstraint))
                return new List<LinkConstraint>();
            else if (type == typeof(BarRelease))
                return ReadBarRelease(indices?.Cast<object>().Select(x => x.ToString()).ToList());
            else if (type == typeof(Offset))
                return ReadOffsets(indices?.Cast<object>().Select(x => x.ToString()).ToList());
            else if (type == typeof(Loadcase))
                return ReadLoadCase();
            else if (typeof(ISectionProperty).IsAssignableFrom(type))
                return ReadSectionProperties(indices?.Cast<object>().Select(x => x.ToString()).ToList());
            else if (typeof(ILoad).IsAssignableFrom(type))
                return ReadLoads(type); //TODO: Implement load extraction
            else if (type.IsGenericType && type.Name == typeof(BHoMGroup<IBHoMObject>).Name)
                return ReadGroups();
            else if (type == typeof(FramingElementDesignProperties))
                return ReadFramingElementDesignProperties();
            else if (type == typeof(BH.oM.Adapters.Robot.DesignGroup))
                return ReadDesignGroups();
            else if (typeof(IResult).IsAssignableFrom(type))
            {
                Modules.Structure.ErrorMessages.ReadResultsError(type);
                return null;
            }
            else if (type == typeof(BHoMObject))
            {
                List<IBHoMObject> objects = new List<IBHoMObject>();
                objects.AddRange(ReadConstraints6DOF());
                objects.AddRange(ReadMaterials());
                objects.AddRange(ReadBarRelease());
                objects.AddRange(ReadLoadCase());
                objects.AddRange(ReadSectionProperties());
                objects.AddRange(ReadNodes());
                objects.AddRange(ReadBars());
                objects.AddRange(ReadPanels());
                objects.AddRange(ReadFramingElementDesignProperties());
                return objects;
            }
            else
            {
                Engine.Base.Compute.RecordWarning($"The provided Type {type.Name} is not supported to pull from RobotToolkit.");
            }

            return new List<IBHoMObject>();
        }

        /***************************************************/

        public IEnumerable<IBHoMObject> Read(SelectionRequest request, ActionConfig actionConfig = null)
        {
            List<IBHoMObject> objects = new List<IBHoMObject>();
            objects.AddRange(ReadSelected(typeof(Bar)));
            objects.AddRange(ReadSelected(typeof(Panel)));
            objects.AddRange(ReadSelected(typeof(Node)));
            return objects;
        }

        /***************************************************/

        protected override IEnumerable<IBHoMObject> Read(FilterRequest filterRequest, ActionConfig actionConfig = null)
        {
            // Extract the Ids from the FilterRequest
            IList objectIds = null;
            object idObject;
            if (filterRequest.Equalities.TryGetValue("ObjectIds", out idObject) && idObject is IList)
                objectIds = idObject as IList;

            if (objectIds != null)
                return IRead(filterRequest.Type, objectIds, actionConfig);

            //Check for tag
            if (!string.IsNullOrWhiteSpace(filterRequest.Tag))
            {
                RobotOM.IRobotObjectType robotType = Convert.RobotObjectType(filterRequest.Type);
                RobotOM.IRobotGroupServer groupServer = m_RobotApplication.Project.Structure.Groups;

                List<int> ids = null;

                for (int i = 1; i <= groupServer.GetCount(robotType); i++)
                {
                    RobotGroup robotGroup = groupServer.Get(robotType, i);

                    if (robotGroup.Name == filterRequest.Tag)
                    {
                        ids = Convert.ToSelectionList(robotGroup.SelList);
                    }
                }
                if (ids != null)
                    return IRead(filterRequest.Type, ids, actionConfig);
                else
                    return new List<IBHoMObject>();
            }

            return IRead(filterRequest.Type, objectIds, actionConfig);
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private IEnumerable<IBHoMObject> ReadSelected(Type type, ActionConfig actionConfig = null)
        {
            RobotOM.IRobotObjectType robotType = Convert.RobotObjectType(type);
            RobotOM.IRobotSelection selection = m_RobotApplication.Project.Structure.Selections.Get(robotType);

            if (selection.Count != 0)
                return IRead(type, Convert.ToSelectionList(selection.ToText()), actionConfig);

            return new List<IBHoMObject>();
        }

        /***************************************************/

        private List<int> CheckAndGetIds<T>(IEnumerable ids) where T : IBHoMObject
        {
            if (ids == null)
            {
                return null;
            }
            else
            {
                List<int> idsOut = new List<int>();
                foreach (object o in ids)
                {
                    if (o == null)
                        continue;

                    int id;
                    if (o is int)
                        idsOut.Add((int)o);
                    else if (int.TryParse(o.ToString(), out id))
                    {
                        idsOut.Add(id);
                    }
                    else if (o is T)
                    {
                        id = GetAdapterId<int>((T)o);
                        if (id != 0)
                            idsOut.Add(id);
                    }

                }
                return idsOut;
            }
        }

        /***************************************************/

    }

}
