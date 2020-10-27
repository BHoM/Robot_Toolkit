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

using System.Collections.Generic;
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Geometry;
using System.Collections;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Opening> ReadOpenings(IList ids = null)
        {
            List<Opening> openings = new List<Opening>();
            IRobotStructure rStructure = m_RobotApplication.Project.Structure;
            RobotSelection rSelect = rStructure.Selections.Create(IRobotObjectType.I_OT_GEOMETRY);

            List<int> openingIds = CheckAndGetIds<Opening>(ids);

            if (openingIds == null)
            {
                rSelect.FromText("all");
            }
            else
            {
                rSelect.FromText(Convert.ToRobotSelectionString(openingIds));
            }
            IRobotCollection rOpenings = rStructure.Objects.GetMany(rSelect);
            Opening tempOpening = null;
            
            for (int i = 1; i <= rOpenings.Count; i++)
            {
                RobotObjObject rOpening = (RobotObjObject)rOpenings.Get(i);
                System.Type type = rOpening.GetType(); 
               
                if (rOpening.Main.Attribs.Meshed != 1)
                {
                    ICurve outline = Convert.FromRobot(rOpening.Main.GetGeometry() as dynamic);
                    tempOpening = BH.Engine.Structure.Create.Opening(outline);
                }
                if (tempOpening != null)
                {
                    SetAdapterId(tempOpening, rOpening.Number);
                    openings.Add(tempOpening);
                }
            }

            return openings;
        }

        /***************************************************/

    }
}

