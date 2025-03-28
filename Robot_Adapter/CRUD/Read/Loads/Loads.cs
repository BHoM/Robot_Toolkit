/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using BH.oM.Base.Attributes;
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Geometry;
using BH.oM.Base;
using BH.oM.Structure.Loads;
using BH.oM.Adapters.Robot;
using BH.Engine.Adapters.Robot;


namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<ILoad> ReadLoads(Type type, List<string> ids = null)
        {
            //Ensure previous object caches have been cleared

            List<ILoad> loads = new List<ILoad>();

            //Element loads
            if (type.IsAssignableFrom(typeof(AreaUniformlyDistributedLoad)))
                loads.AddRange(ReadObjectLoads(record => record.FromRobotAreaUDL(), IRobotLoadRecordType.I_LRT_UNIFORM, ids));

            if (type.IsAssignableFrom(typeof(AreaUniformTemperatureLoad)))
                loads.AddRange(ReadObjectLoads(record => record.FromRobotAreaTempLoad(), IRobotLoadRecordType.I_LRT_THERMAL_IN_3_POINTS, ids));

            if (type.IsAssignableFrom(typeof(BarPointLoad)))
                loads.AddRange(ReadObjectLoads(record => record.FromRobotBarPtLoad(), IRobotLoadRecordType.I_LRT_BAR_FORCE_CONCENTRATED, ids));

            if (type.IsAssignableFrom(typeof(BarUniformTemperatureLoad)))
                loads.AddRange(ReadObjectLoads(record => record.FromRobotBarTempLoad(), IRobotLoadRecordType.I_LRT_BAR_THERMAL, ids));

            if (type.IsAssignableFrom(typeof(BarUniformlyDistributedLoad)))
            {
                loads.AddRange(ReadObjectLoads(record => record.FromRobotBarUDLForce(), IRobotLoadRecordType.I_LRT_BAR_UNIFORM, ids));
                loads.AddRange(ReadObjectLoads(record => record.FromRobotBarUDLMoment(), IRobotLoadRecordType.I_LRT_BAR_MOMENT_DISTRIBUTED, ids));
            }

            if (type.IsAssignableFrom(typeof(BarVaryingDistributedLoad)))
            {
                loads.AddRange(ReadObjectLoads(record => record.FromRobotBarVarDistLoad(), IRobotLoadRecordType.I_LRT_BAR_TRAPEZOIDALE, ids));
            }

            if (type.IsAssignableFrom(typeof(GravityLoad)))
                loads.AddRange(ReadObjectLoads(record => record.FromRobotGravityLoad(), IRobotLoadRecordType.I_LRT_DEAD, ids));

            if (type.IsAssignableFrom(typeof(PointAcceleration)))
                loads.AddRange(ReadObjectLoads(record => record.FromRobotPtAccel(), IRobotLoadRecordType.I_LRT_NODE_ACCELERATION, ids));

            if (type.IsAssignableFrom(typeof(PointDisplacement)))
                loads.AddRange(ReadObjectLoads(record => record.FromRobotPtDisp(), IRobotLoadRecordType.I_LRT_NODE_DISPLACEMENT, ids));

            if (type.IsAssignableFrom(typeof(PointLoad)))
                loads.AddRange(ReadObjectLoads(record => record.FromRobotPtLoad(), IRobotLoadRecordType.I_LRT_NODE_FORCE, ids));

            if (type.IsAssignableFrom(typeof(PointVelocity)))
                loads.AddRange(ReadObjectLoads(record => record.FromRobotPtVel(), IRobotLoadRecordType.I_LRT_NODE_VELOCITY, ids));

            //Geometrical loads
            if (type.IsAssignableFrom(typeof(oM.Structure.Loads.ContourLoad)))
                loads.AddRange(ReadContourLoads(ids));

            if (type.IsAssignableFrom(typeof(oM.Structure.Loads.GeometricalLineLoad)))
                loads.AddRange(ReadGeometricalLineLoads(ids));

            //Clean up the object cache

            return loads;
        }

        /***************************************************/

        [Description("Reads load data for a particular type, filters out type based on the provided function and loadtype enum")]
        [Input("convertMethod", "Method used for turning a load record into a BHoM load, setting all properties except for objects and case. For example `record => record.FromRobotAreaUDL()` for Areaload.")]
        [Input("loadType","Robot loadtype enum corresponding to the type of load being pulled.")]
        [Input("ids", "Not yet in use.")]
        private List<ILoad> ReadObjectLoads<T>(Func<IRobotLoadRecord, IElementLoad<T>> convertMethod, IRobotLoadRecordType loadType, List<string> ids = null) where T : class, IBHoMObject
        {
            //Main method looping through all loadcases and extracting the picked up load type
            List<ILoad> bhomLoads = new List<ILoad>();
            Dictionary<int, T> loadObjects = GetCachedOrReadLoadObjects<T>();
            Dictionary<int, Loadcase> bhomLoadCases = GetCachedOrReadAsDictionary<int, Loadcase>();

            IRobotCaseCollection loadCollection = m_RobotApplication.Project.Structure.Cases.GetAll();

            for (int i = 1; i <= loadCollection.Count; i++)
            {
                IRobotCase lCase = loadCollection.Get(i) as IRobotCase;

                if (lCase == null)
                {
                    Engine.Base.Compute.RecordError($"Failed to extract a loadcase trying to read a {typeof(T).Name}. Load for the failed case is not extracted.");
                    continue;
                }

                if (lCase.Type == IRobotCaseType.I_CT_SIMPLE)
                {
                    IRobotSimpleCase sCase = lCase as IRobotSimpleCase;

                    if (sCase == null)
                    {
                        Engine.Base.Compute.RecordError($"Failed to extract a loadcase trying to read a {typeof(T).Name}. Load for the failed case is not extracted.");
                        continue;
                    }

                    if (bhomLoadCases.ContainsKey(sCase.Number))
                    {
                        for (int j = 1; j <= sCase.Records.Count; j++)
                        {
                            IRobotLoadRecord loadRecord = sCase.Records.Get(j);

                            if (loadRecord == null)
                                continue;

                            if (loadRecord.Type == loadType)
                            {
                                List<T> objects = FilterLoadObjects(loadRecord, loadObjects);
                                IElementLoad<T> load = convertMethod.Invoke(loadRecord);
                                if (load != null)
                                {
                                    SetLoadGroup(load, objects.Cast<IBHoMObject>());
                                    load.Loadcase = bhomLoadCases[sCase.Number];
                                    bhomLoads.Add(load);
                                }
                                else
                                {
                                    Engine.Base.Compute.RecordError($"Failed to convert a {typeof(T).Name} for for the loadcase {sCase.Name}.");
                                }
                            }
                        }
                    }
                    else
                    {
                        Engine.Base.Compute.RecordError($"Failed to extract a loadcase named {sCase.Name} trying to read a {typeof(T).Name}. Load for the failed case is not extracted.");
                    }
                }
            }
            return bhomLoads;
        }

        /***************************************************/

        private static List<T> FilterLoadObjects<T>(IRobotLoadRecord loadRecord, Dictionary<int, T> objects)
        {
            try
            {
                //For case of entire structure, return all obejcts
                if (loadRecord.Type == IRobotLoadRecordType.I_LRT_DEAD && loadRecord.GetValue((short)IRobotDeadRecordValues.I_DRV_ENTIRE_STRUCTURE) == 1)
                {
                    return objects.Values.ToList();
                }

                List<int> elementIds = Convert.FromRobotSelectionString(loadRecord.Objects.ToText());
                List<T> loadObjects = new List<T>();
                for (int k = 0; k < elementIds.Count; k++)
                {
                    if (objects.ContainsKey(elementIds[k]))
                        loadObjects.Add(objects[elementIds[k]]);
                    else
                        Engine.Base.Compute.RecordWarning($"Failed to find a {typeof(T).Name} to apply to a Load being read.");
                }
                return loadObjects;
            }
            catch (Exception)
            {
                Engine.Base.Compute.RecordWarning($"Failed extract obejcts of type {typeof(T).Name} to apply to a Load being read.");
                return new List<T>();
            }
        }

        /***************************************************/

        private static void SetLoadGroup<T>(IElementLoad<T> load, IEnumerable<IBHoMObject> objects) where T: IBHoMObject
        {
            BHoMGroup<T> group = new BHoMGroup<T>();

            foreach (IBHoMObject obj in objects)
            {
                if (obj is T)
                    group.Elements.Add((T)obj);
            }
            load.Objects = group;
        }

        /***************************************************/

        private Dictionary<int, T> GetCachedOrReadLoadObjects<T>() where T : class, IBHoMObject
        {
            //Reads elements of a particular type.

            Type type = typeof(T);

            if (type == typeof(BHoMObject))
            {
                var bars = GetCachedOrReadAsDictionary<int, Bar>();
                var panels = GetCachedOrReadAsDictionary<int, Panel>();
                Dictionary<int, T> dict = new Dictionary<int, T>();

                foreach (var kvp in bars)
                {
                    dict[kvp.Key] = kvp.Value as T;
                }
                foreach (var kvp in panels)
                {
                    dict[kvp.Key] = kvp.Value as T;
                }

                return dict;
            }

            Type readType = type;
            if (type == typeof(IAreaElement))
                return GetCachedOrReadAsDictionary<int, Panel>().ToDictionary(x => x.Key, x => x.Value as T);

            return GetCachedOrReadAsDictionary<int, T>();
        }

        /***************************************************/

    }

}





