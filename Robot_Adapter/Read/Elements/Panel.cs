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
using RobotOM;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Physical.Materials;
using BH.oM.Geometry;
using System.Collections;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Panel> ReadPanels(IList ids = null)
        {
            Dictionary<string, ISurfaceProperty> BHoMProperties = ReadProperty2D().ToDictionary(x => x.Name);
            List<Panel> BHoMPanels = new List<Panel>();
            IRobotStructure robotStructureServer = m_RobotApplication.Project.Structure;
            IRobotObjObjectServer robotPanelServer = m_RobotApplication.Project.Structure.Objects;
            List<Material> bhomMaterials = new List<Material>();
            List<Opening> allOpenings = ReadOpenings();
            Panel BHoMPanel = null;

            List<int> panelIds = CheckAndGetIds(ids);

            RobotSelection rPanSelect = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_PANEL);
            RobotSelection rCladdingSelect = robotStructureServer.Selections.Create(IRobotObjectType.I_OT_GEOMETRY);
            if (panelIds == null)
            {
                rPanSelect.FromText("all");
                rCladdingSelect.FromText("all");
            }
            else if (panelIds.Count == 0)
            {
                rPanSelect.FromText("all");
                rCladdingSelect.FromText("all");
            }
            else
            {
                rPanSelect.FromText(BH.Engine.Robot.Convert.ToRobotSelectionString(panelIds));
                rCladdingSelect.FromText(BH.Engine.Robot.Convert.ToRobotSelectionString(panelIds));
            }

            IRobotCollection rPanels = robotPanelServer.GetMany(rPanSelect);
            IRobotCollection rCladdings = robotPanelServer.GetMany(rCladdingSelect);

            for (int i = 1; i <= rPanels.Count; i++)
            {
                RobotObjObject rpanel = (RobotObjObject)rPanels.Get(i);
                BHoMPanel = null;

                if (rpanel.Main.Attribs.Meshed == 1)
                {
                    ICurve outline = BH.Engine.Robot.Convert.ToBHoMGeometry(rpanel.Main.GetGeometry() as dynamic);
                    List<Opening> openings = BH.Engine.Robot.Convert.FindOpening(outline, allOpenings);
                    try
                    {
                        BHoMPanel = BH.Engine.Structure.Create.Panel(outline, openings);
                    }
                    catch
                    {
                        BH.Engine.Reflection.Compute.RecordWarning("Geometry for panel " + rpanel.Number.ToString() + " not supported.");
                    }
                    BHoMPanel.CustomData[AdapterId] = rpanel.Number;
                    BHoMPanel.CustomData["RobotFiniteElementIds"] = rpanel.FiniteElems;
                    BHoMPanel.CustomData["RobotNodeIds"] = rpanel.Nodes;

                    //Get the coordinate system for the panel
                    BH.oM.Geometry.Point coordPoint = BH.Engine.Geometry.Query.StartPoint(outline as dynamic);

                    double x, y, z; rpanel.Main.Attribs.GetDirX(out x, out y, out z);
                    BH.oM.Geometry.Vector coordXAxis = BH.Engine.Geometry.Create.Vector(x, y, z);
                    BH.oM.Geometry.Vector coordZAxis = BH.Engine.Geometry.Compute.FitPlane(outline as dynamic).Normal;
                    if (coordZAxis.Z == 0)
                    {
                        if ((coordZAxis.X > coordZAxis.Y && coordZAxis.X < 1) || (coordZAxis.Y > coordZAxis.X && coordZAxis.Y < 1))
                            coordZAxis = BH.Engine.Geometry.Modify.Reverse(coordZAxis);
                    }
                    if (rpanel.Main.Attribs.DirZ == 0)
                        coordZAxis = BH.Engine.Geometry.Modify.Reverse(coordZAxis);

                    BH.oM.Geometry.CoordinateSystem.Cartesian tempCoordSys = BH.Engine.Geometry.Create.CartesianCoordinateSystem(coordPoint, coordXAxis, coordZAxis);
                    BH.oM.Geometry.CoordinateSystem.Cartesian coordinateSystem = BH.Engine.Geometry.Create.CartesianCoordinateSystem(coordPoint, coordXAxis, tempCoordSys.Z);                    

                    BHoMPanel.CustomData["CoordinateSystem"] = coordinateSystem;

                    if (rpanel.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) != 0)
                    {
                        string propName = rpanel.GetLabelName(IRobotLabelType.I_LT_PANEL_THICKNESS);
                        if (BHoMProperties.ContainsKey(propName))
                            BHoMPanel.Property = BHoMProperties[propName];
                        else
                            BH.Engine.Reflection.Compute.RecordEvent("Failed to convert/create ConstantThickness property for panel " + rpanel.Number.ToString(), oM.Reflection.Debugging.EventType.Warning);
                    }
                    else
                    {
                        BH.Engine.Reflection.Compute.RecordEvent("Panel " + rpanel.Number.ToString() + " either has no property-label or the Property2D for this Robot label is not implemented", oM.Reflection.Debugging.EventType.Warning);
                    }
                }
                BHoMPanels.Add(BHoMPanel);
            }

            List<Opening> emptyOpenings = new List<Opening>();
            //robotPanelServer.BeginMultiOperation();
            for (int i = 1; i <= rCladdings.Count; i++)
            {
                RobotObjObject rCladding = (RobotObjObject)rCladdings.Get(i);

                if (rCladding.Main.Attribs.Meshed == 1)
                {
                    ICurve outline = BH.Engine.Robot.Convert.ToBHoMGeometry(rCladding.Main.GetGeometry() as dynamic);
                    BHoMPanel = BH.Engine.Structure.Create.Panel(outline, emptyOpenings);
                }
                if (BHoMPanel != null)
                {
                    BHoMPanel.CustomData[AdapterId] = rCladding.Number;

                    if (rCladding.HasLabel(IRobotLabelType.I_LT_CLADDING) != 0)
                    {
                        string propName = rCladding.GetLabelName(IRobotLabelType.I_LT_CLADDING);
                        if (BHoMProperties.ContainsKey(propName))
                            BHoMPanel.Property = BHoMProperties[propName];
                        else
                            BH.Engine.Reflection.Compute.RecordEvent("Failed to convert/create ConstantThickness property for Cladding " + rCladding.Number.ToString(), oM.Reflection.Debugging.EventType.Warning);
                    }

                    else
                    {
                        BH.Engine.Reflection.Compute.RecordEvent("Cladding " + rCladding.Number.ToString() + " either has no property-label or the Property2D for this Robot label is not implemented", oM.Reflection.Debugging.EventType.Warning);
                    }

                    BHoMPanels.Add(BHoMPanel);
                }
            }

            return BHoMPanels;
        }
    }
}
