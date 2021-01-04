/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

using BH.oM.Structure.MaterialFragments;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structure.SurfaceProperties;
using BH.Engine.Structure;
using BH.oM.Structure.Fragments;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static ISurfaceProperty FromRobot(this IRobotLabel robotLabel, Dictionary<string, IMaterialFragment> material, string robotLabelName = "")
        {
            if (robotLabel?.Data == null)
            {
                Engine.Reflection.Compute.RecordWarning("Failed to extract at least one ISurfaceProperty from Robot.");
                return null;
            }

            ISurfaceProperty bhomProperty = null;
            IMaterialFragment mat = null;

            if (robotLabelName == "")
                robotLabelName = robotLabel.Name;

            try
            {
                if (robotLabel.Type == IRobotLabelType.I_LT_PANEL_THICKNESS)
                {
                    IRobotThicknessData data = robotLabel.Data;

                    if (data == null)
                    {
                        Engine.Reflection.Compute.RecordWarning("Failed to extract at least one ISurfaceProperty from Robot.");
                        return null;
                    }

                    mat = null;
                    if (material.ContainsKey(data.MaterialName))
                        mat = material[data.MaterialName];
                    else
                        BH.Engine.Reflection.Compute.RecordEvent("SurfaceProperty " + robotLabelName + " has no material assigned", oM.Reflection.Debugging.EventType.Warning);

                    bhomProperty = null;

                    switch (data.ThicknessType)
                    {
                        case IRobotThicknessType.I_TT_HOMOGENEOUS:
                            IRobotThicknessHomoData homoData = data.Data;
                            switch (homoData.Type)
                            {
                                case IRobotThicknessHomoType.I_THT_CONSTANT:
                                    bhomProperty = new ConstantThickness { Name = robotLabelName, Thickness = homoData.ThickConst, Material = mat };
                                    break;
                                case IRobotThicknessHomoType.I_THT_VARIABLE_ON_PLANE:
                                    break;
                            }
                            break;
                        case IRobotThicknessType.I_TT_ORTHOTROPIC:

                            IRobotThicknessOrthoData orthoData = data.Data;
                            ConstantThickness BHoMConstThick = new ConstantThickness();

                            switch (orthoData.Type)
                            {
                                case IRobotThicknessOrthoType.I_TOT_ONE_SIDED_UNIDIR_RIBS:
                                    bhomProperty = new Ribbed { Name = robotLabelName, Thickness = orthoData.H, TotalDepth = orthoData.HA, StemWidth = orthoData.A1, Spacing = orthoData.A, Direction = orthoData.DirType == IRobotThicknessOrthoDirType.I_TODT_DIR_X ? PanelDirection.X : PanelDirection.Y, Material = mat };
                                    break;
                                case IRobotThicknessOrthoType.I_TOT_ONE_SIDED_BIDIR_RIBS:
                                    bhomProperty = new Waffle { Name = robotLabelName, Thickness = orthoData.H, TotalDepthX = orthoData.HA, TotalDepthY = orthoData.HB, StemWidthX = orthoData.A1, StemWidthY = orthoData.B1, SpacingX = orthoData.A, SpacingY = orthoData.B, Material = mat };
                                    break;
                                case IRobotThicknessOrthoType.I_TOT_MATERIAL:
                                    bhomProperty = new ConstantThickness { Name = robotLabelName, Thickness = orthoData.H, Material = mat };
                                    double n1 = orthoData.N1;
                                    double n2 = orthoData.N2;
                                    bhomProperty = bhomProperty.ApplyModifiers(n1, Math.Sqrt(n1 + n2), n2, n1, Math.Sqrt(n1 + n2), n2, n1, n2);
                                    break;
                                case IRobotThicknessOrthoType.I_TOT_UNIDIR_BOX_FLOOR:
                                case IRobotThicknessOrthoType.I_TOT_SLAB_ON_TRAPEZOID_PLATE:
                                case IRobotThicknessOrthoType.I_TOT_GRILLAGE:
                                case IRobotThicknessOrthoType.I_TOT_BIDIR_BOX_FLOOR:
                                case IRobotThicknessOrthoType.I_TOT_HOLLOW_CORE_SLAB:
                                case IRobotThicknessOrthoType.I_TOT_USER:
                                    bhomProperty = new ConstantThickness { Name = robotLabelName, Thickness = double.NaN, Material = mat };
                                    BH.Engine.Reflection.Compute.RecordWarning("Thickness property '" + robotLabelName + "' of type " + orthoData.Type + " Not yet supported. A constant thickness property with NaN thickness has been created in its place");
                                    break;
                                default:
                                    bhomProperty = new ConstantThickness { Name = robotLabelName, Thickness = orthoData.H, Material = mat };
                                    SurfacePropertyModifier modifier = new SurfacePropertyModifier
                                    {
                                        FXX = orthoData.HA,
                                        FXY = orthoData.HB,
                                        FYY = orthoData.H0,
                                        MXX = orthoData.HC,
                                        MXY = orthoData.A,
                                        MYY = orthoData.A1,
                                        VXZ = orthoData.A2,
                                        VYZ = orthoData.B,
                                        Mass = 1,   //Cant extract mass via the API
                                        Weight = orthoData.B1
                                    };
                                    bhomProperty.Fragments.Add(modifier);
                                    break;
                            }
                            break;
                    }
                }
                else if (robotLabel.Type == IRobotLabelType.I_LT_CLADDING)
                {
                    IRobotCladdingData claddData = robotLabel.Data;

                    if (claddData == null)
                    {
                        Engine.Reflection.Compute.RecordWarning("Failed to extract at least one ISurfaceProperty from Robot.");
                        return null;
                    }

                    switch (claddData.Type)
                    {
                        case IRobotCladdingType.I_CT_X:
                            bhomProperty = new LoadingPanelProperty
                            {
                                LoadApplication = LoadPanelSupportConditions.TwoSides,
                                Name = robotLabelName,
                                ReferenceEdge = 1
                            };
                            break;

                        case IRobotCladdingType.I_CT_Y:
                            bhomProperty = new LoadingPanelProperty
                            {
                                LoadApplication = LoadPanelSupportConditions.TwoSides,
                                Name = robotLabelName,
                                ReferenceEdge = 2
                            };
                            break;

                        case IRobotCladdingType.I_CT_XY:
                            bhomProperty = new LoadingPanelProperty
                            {
                                LoadApplication = LoadPanelSupportConditions.AllSides,
                                Name = robotLabelName,
                                ReferenceEdge = 1
                            };
                            break;

                        default:
                            break;
                    }
                }
                return bhomProperty;
            }
            catch (Exception)
            {
                Engine.Reflection.Compute.RecordWarning("Failed to extract at least one ISurfaceProperty from Robot.");
                return null;
            }
        }

        /***************************************************/

    }
}

