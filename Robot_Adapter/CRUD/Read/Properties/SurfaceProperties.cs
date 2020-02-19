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

        private List<ISurfaceProperty> ReadSurfaceProperties(List<string> names = null)
        {
            List<ISurfaceProperty> BHoMProps = new List<ISurfaceProperty>();
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotCollection rThicknessProps = labelServer.GetMany(IRobotLabelType.I_LT_PANEL_THICKNESS);
            IRobotCollection rCladdingProps = labelServer.GetMany(IRobotLabelType.I_LT_CLADDING);
            Dictionary<string, IMaterialFragment> BHoMMat = new Dictionary<string, IMaterialFragment>();
            BHoMMat = (ReadMaterials().ToDictionary(x => x.Name));

            for (int i = 1; i <= rThicknessProps.Count; i++)
            {
                IRobotLabel rThicknessProp = rThicknessProps.Get(i);
                ISurfaceProperty tempProp = Convert.FromRobot(rThicknessProp, BHoMMat);
                tempProp.CustomData.Add(AdapterIdName, tempProp.Name);
                BHoMProps.Add(tempProp);
            }

            for (int i = 1; i <= rCladdingProps.Count; i++)
            {
                IRobotLabel rCladdingProp = rCladdingProps.Get(i);
                ISurfaceProperty tempProp = Convert.FromRobot(rCladdingProp, BHoMMat);
                tempProp.CustomData.Add(AdapterIdName, tempProp.Name);
                BHoMProps.Add(tempProp);
            }
            return BHoMProps;
        }

        /***************************************************/
        
        private ISurfaceProperty ReadSurfacePropertyFromPanel(IRobotObjObject robotPanel, Dictionary<string, IMaterialFragment> materials = null, bool isCladding = false)
        {
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            if (materials == null) materials = new Dictionary<string, IMaterialFragment>();
            ISurfaceProperty thicknessProperty = null;
            IRobotLabel robotThicknessLabel = null;
            if (!isCladding)
            {
                if (robotPanel.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) == -1)
                { 
                    robotThicknessLabel = robotPanel.GetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS);                  
                    IMaterialFragment material = ReadMaterialFromPanel(robotPanel);
                    if (material != null && material.Name != "")
                        materials.Add(material.Name, material);
                }
                thicknessProperty = (robotThicknessLabel == null)? null : Convert.FromRobot(robotThicknessLabel, materials);
            }
            else
            {
                IRobotLabel robotCladdingLabel = robotPanel.GetLabel(IRobotLabelType.I_LT_CLADDING);
                thicknessProperty = Convert.FromRobot(robotCladdingLabel, materials);
            }
            if (thicknessProperty == null)
            {
                BH.Engine.Reflection.Compute.RecordEvent("Failed to convert/create ConstantThickness property for panel " + robotPanel.Number.ToString(), oM.Reflection.Debugging.EventType.Warning);
                return null;
            }
            else
                return thicknessProperty;
        }
    }
}
