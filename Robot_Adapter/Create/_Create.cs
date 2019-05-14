/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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

                if (objects.First() is IMaterialFragment)
                {
                    success = CreateCollection(objects as IEnumerable<IMaterialFragment>);
                }

                if (objects.First() is Loadcase)
                {
                    success = CreateCollection(objects as IEnumerable<Loadcase>);
                }

                if (objects.First() is FEMesh)
                {
                    success = CreateCollection(objects as IEnumerable<FEMesh>);
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

                if (objects.First() is Panel)
                {
                    success = CreateCollection(objects as IEnumerable<Panel>);
                }

                if (typeof(ISurfaceProperty).IsAssignableFrom(objects.First().GetType()))
                {
                    success = CreateCollection(objects as IEnumerable<ISurfaceProperty>);
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

