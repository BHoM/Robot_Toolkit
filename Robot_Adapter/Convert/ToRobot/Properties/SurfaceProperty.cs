/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using BH.oM.Geometry;
using BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Fragments;
using BH.Engine.Base;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static bool ToRobot(IRobotLabel rLabel, ISurfaceProperty property)
        {

            if (property is LoadingPanelProperty)
            {
                (property as LoadingPanelProperty).ToRobot();
                return true;
            }
            else
            {
                Type propType = property.GetType();

                RobotThicknessOrthoData orthoData = null;
                RobotThicknessData thicknessData = rLabel.Data;

                if (RobotAdapter.CheckNotNull(property.Material, oM.Reflection.Debugging.EventType.Warning, propType))
                    thicknessData.MaterialName = property.Material.Name;

                if (property is ConstantThickness)
                {
                    ConstantThickness constThickness = property as ConstantThickness;
                    SurfacePropertyModifier modifier = property.FindFragment<SurfacePropertyModifier>();
                    if (modifier != null)
                    {
                        thicknessData.ThicknessType = IRobotThicknessType.I_TT_ORTHOTROPIC;
                        orthoData = thicknessData.Data;
                        orthoData.Type = IRobotThicknessOrthoType.I_TOT_CONST_THICK_WITH_REDUCED_STIFFNESS;
                        orthoData.H = constThickness.Thickness;
                        orthoData.HA = modifier.FXX;
                        orthoData.H0 = modifier.FYY;
                        orthoData.HB = modifier.FXY;
                        orthoData.HC = modifier.MXX;
                        orthoData.A = modifier.MXY;
                        orthoData.A1 = modifier.MYY;
                        orthoData.A2 = modifier.VXZ;
                        orthoData.B = modifier.VYZ;
                        orthoData.B1 = modifier.Weight;

                        if (modifier.Mass != 1)
                            Engine.Reflection.Compute.RecordNote("Can't apply modifier for mass for SurfaceProperties via the Robot API.");
                    }
                    else
                    {
                        thicknessData.ThicknessType = IRobotThicknessType.I_TT_HOMOGENEOUS;
                        (thicknessData.Data as RobotThicknessHomoData).ThickConst = constThickness.Thickness;
                    }
                }

                else if (property is Waffle)
                {
                    Waffle waffle = property as Waffle;
                    thicknessData.ThicknessType = IRobotThicknessType.I_TT_ORTHOTROPIC;
                    orthoData = thicknessData.Data;
                    orthoData.Type = IRobotThicknessOrthoType.I_TOT_ONE_SIDED_BIDIR_RIBS;
                    orthoData.H = waffle.Thickness;
                    orthoData.HA = waffle.TotalDepthX;
                    orthoData.HB = waffle.TotalDepthY;
                    orthoData.A = waffle.SpacingX;
                    orthoData.A1 = waffle.StemWidthX;
                    orthoData.B = waffle.SpacingY;
                    orthoData.B1 = waffle.StemWidthY;
                }

                else
                {
                    Ribbed ribbed = property as Ribbed;
                    thicknessData.ThicknessType = IRobotThicknessType.I_TT_ORTHOTROPIC;
                    orthoData = thicknessData.Data;
                    orthoData.Type = IRobotThicknessOrthoType.I_TOT_ONE_SIDED_UNIDIR_RIBS;
                    orthoData.H = ribbed.Thickness;
                    orthoData.HA = ribbed.TotalDepth;
                    orthoData.A = ribbed.Spacing;
                    orthoData.A1 = ribbed.StemWidth;
                    orthoData.DirType = ribbed.Direction == PanelDirection.X ? IRobotThicknessOrthoDirType.I_TODT_DIR_X : IRobotThicknessOrthoDirType.I_TODT_DIR_Y;
                }
            }
            return true;
        }

        /***************************************************/

        public static string ToRobot(this LoadingPanelProperty property)
        {
            string name = "";
            LoadingPanelProperty panalProp = property as LoadingPanelProperty;
            if (panalProp.LoadApplication == LoadPanelSupportConditions.AllSides)
            {
                name = "Two-way";
            }
            else if (panalProp.LoadApplication == LoadPanelSupportConditions.TwoSides && panalProp.ReferenceEdge % 2 == 1)
            {
                name = "One-way X";
            }
            else if (panalProp.LoadApplication == LoadPanelSupportConditions.TwoSides && panalProp.ReferenceEdge % 2 == 0)
            {
                name = "One-way Y";
            }
            else
            {
                name = "Two-way";
                Engine.Reflection.Compute.RecordWarning("Panel support condintion not supported in Robot for property named '" + property.Name + "'. The cladding will be assumed Two-way.");
            }
            return name;
        }

        /***************************************************/
    }
}



