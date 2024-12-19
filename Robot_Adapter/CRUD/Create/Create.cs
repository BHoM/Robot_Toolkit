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

using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Loads;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.Offsets;
using BH.oM.Adapters.Robot;
using BH.oM.Base.Attributes;
using BH.Engine.Adapters.Robot;
using BH.oM.Adapter;
using BH.oM.Base;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Adapter Methods                 ****/
        /***************************************************/

        protected override bool ICreate<T>(IEnumerable<T> objects, ActionConfig actionConfig = null)
        {
            bool success = true;
            if (objects.Count() > 0)
            {
                T first = objects.First();
                if (first is Constraint6DOF)
                {
                    success = CreateCollection(objects as IEnumerable<Constraint6DOF>);
                }

                if (first is Constraint4DOF)
                {
                    success = CreateCollection(objects as IEnumerable<Constraint4DOF>);
                }

                if (first is RigidLink)
                {
                    success = CreateCollection(objects as IEnumerable<RigidLink>);
                }

                if (first is BarRelease)
                {
                    success = CreateCollection(objects as IEnumerable<BarRelease>);
                }

                if (first is Offset)
                {
                    success = CreateCollection(objects as IEnumerable<Offset>);
                }

                if (first is LinkConstraint)
                {
                    success = CreateCollection(objects as IEnumerable<LinkConstraint>);
                }

                if (typeof(ISectionProperty).IsAssignableFrom(first.GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<ISectionProperty>);
                }

                if (first is IMaterialFragment)
                {
                    success = CreateCollection(objects as IEnumerable<IMaterialFragment>);
                }

                if (first is Loadcase)
                {
                    success = CreateCollection(objects as IEnumerable<Loadcase>);
                }

                if (first is FEMesh)
                {
                    success = CreateCollection(objects as IEnumerable<FEMesh>);
                }

                if (first is Node)
                {
                    success = CreateCollection(objects as IEnumerable<Node>);
                }

                if (first is Bar)
                {
                    success = CreateCollection(objects as IEnumerable<Bar>);
                }

                if (typeof(ILoad).IsAssignableFrom(first.GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<ILoad>);
                }

                if (first is Panel)
                {
                    success = CreateCollection(objects as IEnumerable<Panel>);
                }

                if (first is Opening)
                {
                    success = CreateCollection(objects as IEnumerable<Opening>);
                }

                if (typeof(ISurfaceProperty).IsAssignableFrom(first.GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<ISurfaceProperty>);
                }

                if (first is LoadCombination)
                {
                    success = CreateCollection(objects as IEnumerable<LoadCombination>);
                }

                if (first is FramingElementDesignProperties)
                {
                    success = CreateCollection(objects as IEnumerable<FramingElementDesignProperties>);
                }

                if (first is DesignGroup)
                {
                    success = CreateCollection(objects as IEnumerable<BH.oM.Adapters.Robot.DesignGroup>);
                }

                if (first is IBHoMGroup)
                {
                    success = CreateCollection(objects as dynamic);
                }
            }

            UpdateView();
            return success;
        }

        /***************************************************/


    }

}







