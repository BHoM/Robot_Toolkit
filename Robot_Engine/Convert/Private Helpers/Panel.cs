﻿using BH.oM.Geometry;
using BH.oM.Common.Materials;
using BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {

        public static IRobotComponentType SegmentType(ICurve curve)
        {
            if (curve is Arc)
                return IRobotComponentType.I_CT_GEO_SEGMENT_ARC;

            if (curve is Line)
                return IRobotComponentType.I_CT_GEO_SEGMENT_LINE;

            else
                throw new Exception("Geometry is only valid for Line, Arc and Circle");              
        }

        public static RobotGeoSegment Segment(ICurve curve, RobotGeoSegment segment)
        {
            if (curve is Arc)
            {
                Arc bhomArc = curve as Arc;
                RobotGeoSegmentArc arc = segment as RobotGeoSegmentArc;
                arc.P1.Set(bhomArc.Start.X, bhomArc.Start.Y, bhomArc.Start.Z);
                arc.P2.Set(bhomArc.Middle.X, bhomArc.Middle.Y, bhomArc.Middle.Z);
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

        public static void SingleContourGeometry(ICurve curve, RobotGeoObject contourGeo)
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

        public static void ThicknessProperty(RobotThicknessData thicknessData, Property2D property)
        {
            RobotThicknessOrthoData orthoData = null;
            if (property is ConstantThickness)
            {
                if(BH.Engine.Structure.Query.HasModifiers(property))
                {
                    thicknessData.ThicknessType = IRobotThicknessType.I_TT_ORTHOTROPIC;
                    orthoData = thicknessData.Data;
                    orthoData.Type = (IRobotThicknessOrthoType)14;
                    orthoData.H = property.Thickness;
                    double[] modifiers = property.Modifiers;
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
                    (thicknessData.Data as RobotThicknessHomoData).ThickConst = property.Thickness;
                }
            }

            else if (property is Waffle)
            {
                Waffle waffle = property as Waffle;
                thicknessData.ThicknessType = IRobotThicknessType.I_TT_ORTHOTROPIC;
                orthoData = thicknessData.Data;
                orthoData.Type = (IRobotThicknessOrthoType)14;
                orthoData.H = property.Thickness;
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
                orthoData.H = property.Thickness;
                orthoData.HA = ribbed.TotalDepth;
                orthoData.A = ribbed.Spacing;
                orthoData.A1 = ribbed.StemWidth;
            }
        }
    }
}
