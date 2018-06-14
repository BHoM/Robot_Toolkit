using BH.oM.Geometry;
using BH.oM.Common.Materials;
using BH.Engine.Reflection;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.Engine.Reflection;
using BH.Engine.Structure;
using BH.oM.Reflection.Debuging;
using BH.oM.Adapters.Robot;


namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************/

        public static IProperty2D ToBHoMObject(this IRobotLabel rLabel, Dictionary<string, Material> Material)
        {
            IRobotThicknessData data = rLabel.Data;
            Material mat = null;
            if (Material.ContainsKey(data.MaterialName))
                mat = Material[data.MaterialName];

                IProperty2D BHoMProperty = null;

            switch (data.ThicknessType)
            {
                case IRobotThicknessType.I_TT_HOMOGENEOUS:
                    IRobotThicknessHomoData homoData = data.Data;
                    switch (homoData.Type)
                    {
                        case IRobotThicknessHomoType.I_THT_CONSTANT:
                            BHoMProperty = new ConstantThickness { Name = rLabel.Name, Thickness = homoData.ThickConst, Material = mat};
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
                            break;
                        default:
                            BHoMProperty = new ConstantThickness { Name = rLabel.Name, Thickness = orthoData.H, Material = mat };
                            BHoMProperty.ApplyModifiers(orthoData.HA, orthoData.H0, orthoData.HB, orthoData.HC, orthoData.A, orthoData.A1, orthoData.A2, orthoData.B, 1, orthoData.B1);
                            break;
                    }
                    break;
            }
            return BHoMProperty;


        }
    }
}