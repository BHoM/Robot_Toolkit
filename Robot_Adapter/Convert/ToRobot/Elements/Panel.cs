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

using BH.oM.Geometry;
using BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SurfaceProperties;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static IRobotComponentType ToRobot(ICurve curve)
        {
            if (curve is Arc)
                return IRobotComponentType.I_CT_GEO_SEGMENT_ARC;

            if (curve is Line)
                return IRobotComponentType.I_CT_GEO_SEGMENT_LINE;

            else
                throw new Exception("Geometry is only valid for Line, Arc and Circle");              
        }

        /***************************************************/

        public static RobotGeoSegment ToRobot(ICurve curve, RobotGeoSegment segment)
        {
            if (curve is Arc)
            {
                Arc bhomArc = curve as Arc;
                RobotGeoSegmentArc arc = segment as RobotGeoSegmentArc;
                Point start = bhomArc.StartPoint();
                Point middle = bhomArc.PointAtParameter(0.5);

                arc.P1.Set(start.X, start.Y, start.Z);
                arc.P2.Set(middle.X, middle.Y, middle.Z);
                return segment;
            }

            if (curve is Line)
            {
                Line bhomLine = curve as Line;
                segment.P1.Set(bhomLine.Start.X, bhomLine.Start.Y, bhomLine.Start.Z);
                return segment;
            }

            else
                return null;
        }

        /***************************************************/

        public static void ToRobot(ICurve curve, RobotGeoObject contourGeo)
        {
                Circle bhomCircle = curve as Circle;
                RobotGeoCircle circle = contourGeo as RobotGeoCircle;
                Point bhomPoint1 = bhomCircle.IPointAtParameter(0);
                Point bhomPoint2 = bhomCircle.IPointAtParameter(0.33);
                Point bhomPoint3 = bhomCircle.IPointAtParameter(0.66);
                circle.P1.Set(bhomPoint1.X, bhomPoint1.Y, bhomPoint1.Z);
                circle.P2.Set(bhomPoint2.X, bhomPoint2.Y, bhomPoint2.Z);
                circle.P3.Set(bhomPoint3.X, bhomPoint3.Y, bhomPoint3.Z);
        }

        /***************************************************/

        public static string ToRobot(IRobotLabel rLabel, ISurfaceProperty property)
        {
            string name = "";

            if (property is LoadingPanelProperty)
            {
                LoadingPanelProperty panalProp = property as LoadingPanelProperty;
                if (panalProp.LoadApplication == LoadPanelSupportConditions.AllSides)
                {
                    name = "Two-way";
                }
                if (panalProp.LoadApplication == LoadPanelSupportConditions.TwoSides && panalProp.ReferenceEdge % 2 == 1)
                {
                    name = "One-way X";
                }
                if (panalProp.LoadApplication == LoadPanelSupportConditions.TwoSides && panalProp.ReferenceEdge % 2 == 0)
                {
                    name = "One-way Y";
                }
            }
            else
            {
                RobotThicknessOrthoData orthoData = null;
                RobotThicknessData thicknessData = rLabel.Data;
                thicknessData.MaterialName = property.Material.Name;
                name = property.Name;
                if (property is ConstantThickness)
                {
                    ConstantThickness constThickness = property as ConstantThickness;
                    if (BH.Engine.Structure.Query.HasModifiers(property))
                    {
                        thicknessData.ThicknessType = IRobotThicknessType.I_TT_ORTHOTROPIC;
                        orthoData = thicknessData.Data;
                        orthoData.Type = (IRobotThicknessOrthoType)14;
                        orthoData.H = constThickness.Thickness;
                        double[] modifiers = BH.Engine.Structure.Query.Modifiers(constThickness);
                        orthoData.HA = modifiers[(int)PanelModifier.f11];
                        orthoData.H0 = modifiers[(int)PanelModifier.f12];
                        orthoData.HB = modifiers[(int)PanelModifier.f22];
                        orthoData.HC = modifiers[(int)PanelModifier.m11];
                        orthoData.A = modifiers[(int)PanelModifier.m12];
                        orthoData.A1 = modifiers[(int)PanelModifier.m22];
                        orthoData.A2 = modifiers[(int)PanelModifier.v13];
                        orthoData.B = modifiers[(int)PanelModifier.v23];
                        orthoData.B1 = modifiers[(int)PanelModifier.Weight];
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
                    orthoData.Type = (IRobotThicknessOrthoType)14;
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
                    orthoData.Type = (IRobotThicknessOrthoType)14;
                    orthoData.H = ribbed.Thickness;
                    orthoData.HA = ribbed.TotalDepth;
                    orthoData.A = ribbed.Spacing;
                    orthoData.A1 = ribbed.StemWidth;
                }
            }
            return name;          
        }

        /***************************************************/

        public static List<Opening> FindOpening(ICurve panelOutline, List<Opening> openings)
        {
            List<Opening> openingsInPanel = new List<Opening>();

            foreach (Opening o in openings)
            {
                List<ICurve> crvsOpening = new List<ICurve>();
                foreach (Edge e in o.Edges)
                {
                    crvsOpening.Add(e.Curve);
                }
                PolyCurve crv = BH.Engine.Geometry.Modify.IJoin(crvsOpening)[0];
                List<Point> pts = new List<Point>();
                pts.Add(crv.Bounds().Centre());
                if (panelOutline.IIsContaining(pts))
                    openingsInPanel.Add(o);
            }
            return openingsInPanel;
        }

        /***************************************************/

    }
}

