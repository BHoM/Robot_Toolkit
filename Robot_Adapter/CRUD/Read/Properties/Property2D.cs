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
using RobotOM;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.MaterialFragments;


namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<ISurfaceProperty> ReadProperty2D(List<string> ids = null)
        {
            List<ISurfaceProperty> BHoMProps = new List<ISurfaceProperty>();
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotCollection rThicknessProps = labelServer.GetMany(IRobotLabelType.I_LT_PANEL_THICKNESS);
            IRobotCollection rCladdingProps = labelServer.GetMany(IRobotLabelType.I_LT_CLADDING);
            Dictionary<string, IMaterialFragment> BHoMMat = new Dictionary<string, IMaterialFragment>();
            BHoMMat = (ReadMaterial().ToDictionary(x => x.Name));

            for (int i = 1; i <= rThicknessProps.Count; i++)
            {
                IRobotLabel rThicknessProp = rThicknessProps.Get(i);
                ISurfaceProperty tempProp = BH.Engine.Robot.Convert.ToBHoMObject(rThicknessProp, BHoMMat);
                tempProp.CustomData.Add(AdapterIdName, tempProp.Name);
                BHoMProps.Add(tempProp);
            }

            for (int i = 1; i <= rCladdingProps.Count; i++)
            {
                IRobotLabel rCladdingProp = rCladdingProps.Get(i);
                ISurfaceProperty tempProp = BH.Engine.Robot.Convert.ToBHoMObject(rCladdingProp, BHoMMat);
                tempProp.CustomData.Add(AdapterIdName, tempProp.Name);
                BHoMProps.Add(tempProp);
            }
            return BHoMProps;
        }

        /***************************************************/

    }
}