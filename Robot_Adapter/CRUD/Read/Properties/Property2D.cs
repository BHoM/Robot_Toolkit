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

        private List<ISurfaceProperty> ReadProperty2D(List<string> ids = null)
        {
            List<ISurfaceProperty> properties2D = new List<ISurfaceProperty>();
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotNamesArray robotCladdingLabelNames = robotLabelServer.GetAvailableNames(IRobotLabelType.I_LT_CLADDING);
            IRobotNamesArray robotThicknessLabelNames = robotLabelServer.GetAvailableNames(IRobotLabelType.I_LT_PANEL_THICKNESS);
            Dictionary<string, IMaterialFragment> materials = new Dictionary<string, IMaterialFragment>();
            IRobotLabel robotLabel = null;
            materials = (ReadMaterial().ToDictionary(x => x.Name));

            for (int i = 1; i <= robotThicknessLabelNames.Count; i++)
            {
                string robotLabelName = robotThicknessLabelNames.Get(i);
                if (robotLabelServer.IsUsed(IRobotLabelType.I_LT_PANEL_THICKNESS, robotLabelName))
                    robotLabel = robotLabelServer.Get(IRobotLabelType.I_LT_PANEL_THICKNESS, robotLabelName);
                else
                    robotLabel = robotLabelServer.CreateLike(IRobotLabelType.I_LT_PANEL_THICKNESS, "", robotLabelName);
                if (robotLabel == null)
                    BH.Engine.Reflection.Compute.RecordWarning("Failed to read label '" + robotLabelName);
                else
                {
                    ISurfaceProperty property2D = Convert.FromRobot(robotLabel, materials, robotLabelName);
                    property2D.CustomData.Add(AdapterIdName, property2D.Name);
                    properties2D.Add(property2D);
                }
            }

            for (int i = 1; i <= robotCladdingLabelNames.Count; i++)
            {
                string robotLabelName = robotCladdingLabelNames.Get(i);
                if (robotLabelServer.IsUsed(IRobotLabelType.I_LT_CLADDING, robotLabelName))
                    robotLabel = robotLabelServer.Get(IRobotLabelType.I_LT_CLADDING, robotLabelName);
                else
                    robotLabel = robotLabelServer.CreateLike(IRobotLabelType.I_LT_CLADDING, "", robotLabelName);
                if (robotLabel == null)
                    BH.Engine.Reflection.Compute.RecordWarning("Failed to read label '" + robotLabelName);
                else
                {
                    ISurfaceProperty property2D = Convert.FromRobot(robotLabel, materials, robotLabelName);
                    property2D.CustomData.Add(AdapterIdName, property2D.Name);
                    properties2D.Add(property2D);
                }
            }
            return properties2D;
        }

        /***************************************************/
        
        private ISurfaceProperty ReadProperty2DFromPanel(IRobotObjObject robotPanel, Dictionary<string, IMaterialFragment> materials = null, bool isCladding = false)
        {
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            if (materials == null) materials = new Dictionary<string, IMaterialFragment>();
            IRobotLabel robotLabel = null;
            ISurfaceProperty thicknessProperty = null;
            if (!isCladding)
            {
                robotLabel = robotPanel.GetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS);
                IRobotThicknessData thicknessData = robotLabel.Data;
                if (thicknessData.MaterialName != "" && !materials.ContainsKey(thicknessData.MaterialName))
                    materials.Add(thicknessData.MaterialName, MaterialFromLabel(robotLabelServer.Get(IRobotLabelType.I_LT_MATERIAL, thicknessData.MaterialName)));
                thicknessProperty = Convert.FromRobot(robotLabel, materials);
            }
            else
            {
                robotLabel = robotPanel.GetLabel(IRobotLabelType.I_LT_CLADDING);
                thicknessProperty = Convert.FromRobot(robotLabel, materials);
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
