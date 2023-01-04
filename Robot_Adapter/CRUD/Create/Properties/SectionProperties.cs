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

using System.Collections.Generic;
using BH.oM.Structure.SectionProperties;
using BH.Engine.Structure;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<ISectionProperty> secProp)
        {
            foreach (ISectionProperty p in secProp)
            {
                //Check property is not null
                if (!CheckNotNull(p, oM.Base.Debugging.EventType.Warning))
                    continue;

                IRobotLabel label;
                IRobotBarSectionData secData;
                string match = Convert.Match(m_dbSecPropNames, p);
                if (match != null)
                {
                    label = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, match);
                    secData = label.Data;
                    string matName = Convert.Match(m_dbMaterialNames, p.Material);
                    if (matName == null)
                        matName = p.Material.Name;
                    
                    secData.LoadFromDBase(match);
                    secData.MaterialName = matName;
                    m_RobotApplication.Project.Structure.Labels.Store(label);
                }

                else
                {
                    label = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, p.DescriptionOrName());
                    secData = label.Data;
                    Convert.IToRobot(p, secData);
                    m_RobotApplication.Project.Structure.Labels.Store(label);
                }
                SetAdapterId(p, p.DescriptionOrName());
            }
            return true;
        }

        /***************************************************/
         
    }

}





