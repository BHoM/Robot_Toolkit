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

using BH.oM.Structure.MaterialFragments;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structure.SurfaceProperties;
using BH.Engine.Structure;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static ISurfaceProperty FromRobot(this IRobotLabel rLabel, Dictionary<string, IMaterialFragment> Material)
        {
            ISurfaceProperty BHoMProperty = null;
            IMaterialFragment mat = null;
            if (rLabel.Type == IRobotLabelType.I_LT_PANEL_THICKNESS)
            {
                IRobotThicknessData data = rLabel.Data;
                mat = null;
                if (Material.ContainsKey(data.MaterialName))
                    mat = Material[data.MaterialName];
                else
                    BH.Engine.Reflection.Compute.RecordEvent("Property2D " + rLabel.Name + "has no material assigned", oM.Reflection.Debugging.EventType.Warning);

                BHoMProperty = null;

                switch (data.ThicknessType)
                {
                    case IRobotThicknessType.I_TT_HOMOGENEOUS:
                        IRobotThicknessHomoData homoData = data.Data;
                        switch (homoData.Type)
                        {
                            case IRobotThicknessHomoType.I_THT_CONSTANT:
                                BHoMProperty = new ConstantThickness { Name = rLabel.Name, Thickness = homoData.ThickConst, Material = mat };
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
                                BHoMProperty = new Ribbed { Name = rLabel.Name, Thickness = orthoData.H, TotalDepth = orthoData.HA, StemWidth = orthoData.A1, Spacing = orthoData.A, Direction = orthoData.DirType == IRobotThicknessOrthoDirType.I_TODT_DIR_X ? PanelDirection.X : PanelDirection.Y, Material = mat };
                                break;
                            case IRobotThicknessOrthoType.I_TOT_ONE_SIDED_BIDIR_RIBS:
                                BHoMProperty = new Waffle { Name = rLabel.Name, Thickness = orthoData.H, TotalDepthX = orthoData.HA, TotalDepthY = orthoData.HB, StemWidthX = orthoData.A1, StemWidthY = orthoData.B1, SpacingX = orthoData.A, SpacingY = orthoData.B, Material = mat };
                                break;
                            case IRobotThicknessOrthoType.I_TOT_MATERIAL:
                                BHoMProperty = new ConstantThickness { Name = rLabel.Name, Thickness = orthoData.H, Material = mat };
                                double n1 = orthoData.N1;
                                double n2 = orthoData.N2;
                                BHoMProperty.ApplyModifiers(n1, Math.Sqrt(n1 + n2), n2, n1, Math.Sqrt(n1 + n2), n2, n1, n2);
                                break;
                            case IRobotThicknessOrthoType.I_TOT_UNIDIR_BOX_FLOOR:
                            case IRobotThicknessOrthoType.I_TOT_SLAB_ON_TRAPEZOID_PLATE:
                            case IRobotThicknessOrthoType.I_TOT_GRILLAGE:
                            case IRobotThicknessOrthoType.I_TOT_BIDIR_BOX_FLOOR:
                            case IRobotThicknessOrthoType.I_TOT_HOLLOW_CORE_SLAB:
                            case IRobotThicknessOrthoType.I_TOT_USER:
                                BHoMProperty = new ConstantThickness { Name = rLabel.Name, Thickness = double.NaN, Material = mat };
                                Reflection.Compute.RecordWarning("Could not pull property of type " + orthoData.Type + " with name " + rLabel.Name + ". A constant thickness property with NaN thickness as been created in its place");
                                break;
                            default:
                                BHoMProperty = new ConstantThickness { Name = rLabel.Name, Thickness = orthoData.H, Material = mat };
                                BHoMProperty.ApplyModifiers(orthoData.HA, orthoData.H0, orthoData.HB, orthoData.HC, orthoData.A, orthoData.A1, orthoData.A2, orthoData.B, 1, orthoData.B1);
                                break;
                        }
                        break;
                }
            }

            if (rLabel.Type == IRobotLabelType.I_LT_CLADDING)
            {
                IRobotCladdingData claddData = rLabel.Data;
                switch (claddData.Type)
                {
                    case IRobotCladdingType.I_CT_X:
                        BHoMProperty = new LoadingPanelProperty
                        {
                            LoadApplication = LoadPanelSupportConditions.TwoSides,
                            Name = rLabel.Name,
                            ReferenceEdge = 1
                        };
                        break;

                    case IRobotCladdingType.I_CT_Y:
                        BHoMProperty = new LoadingPanelProperty
                        {
                            LoadApplication = LoadPanelSupportConditions.TwoSides,
                            Name = rLabel.Name,
                            ReferenceEdge = 2
                        };
                        break;

                    case IRobotCladdingType.I_CT_XY:
                        BHoMProperty = new LoadingPanelProperty
                        {
                            LoadApplication = LoadPanelSupportConditions.TwoSides,
                            Name = rLabel.Name,
                            ReferenceEdge = 1
                        };
                        break;

                    default:
                        break;
                }
            }
            return BHoMProperty;
        }

        /***************************************************/

    }
}
