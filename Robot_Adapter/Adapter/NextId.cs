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

using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Physical.Materials;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        protected override object NextId(Type type, bool refresh)
        {
            int index = 1;

            if (!refresh && m_indexDict.TryGetValue(type, out index))
            {
                index++;
                m_indexDict[type] = index;
            }
            else
            {
                if (type == typeof(BH.oM.Adapters.Robot.DesignGroup))
                {
                    List<int> groupNumbers = new List<int>();
                    foreach (BH.oM.Adapters.Robot.DesignGroup designGroup in ReadDesignGroups())
                    {
                        groupNumbers.Add(designGroup.Number);
                    }
                    groupNumbers.Sort();
                    index = groupNumbers.Count > 0 ? groupNumbers.Last() + 1 : 1;
                }
                if (type == typeof(Bar))
                {
                    index = m_RobotApplication.Project.Structure.Bars.FreeNumber;
                }

                if (type == typeof(Material))
                {
                    if (m_indexDict.ContainsKey(type))
                        index = m_indexDict[type] + 1;
                    else
                        index = 1;
                }
                if (type == typeof(Loadcase))
                {
                    index = m_RobotApplication.Project.Structure.Cases.FreeNumber;
                }
                if (type == typeof(LoadCombination))
                {
                    index = m_RobotApplication.Project.Structure.Cases.FreeNumber;
                }
                if (type == typeof(Node))
                {
                    index = m_RobotApplication.Project.Structure.Nodes.FreeNumber;
                }
                if (type == typeof(FEMesh))
                {
                    index = m_RobotApplication.Project.Structure.Objects.FreeNumber;
                }
                if (type == typeof(BH.oM.Structure.Elements.Panel)) //TODO: Check that this is the right rtype of panel
                {
                    index = m_RobotApplication.Project.Structure.Objects.FreeNumber;
                }
                m_indexDict[type] = index;
            }
            return index;
        }

        /***************************************************/

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private Dictionary<Type, int> m_indexDict = new Dictionary<Type, int>();

        /***************************************************/
    }

}
