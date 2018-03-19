using BH.oM.Geometry;
using BH.oM.Common.Materials;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        public static void RobotConstraint(IRobotNodeSupportData suppData, Constraint6DOF constraint)
        {
            suppData.UX = (int)(constraint.TranslationX); suppData.UY = (int)(constraint.TranslationY); suppData.UZ = (int)(constraint.TranslationZ);
            suppData.RX = (int)(constraint.RotationX); suppData.RY = (int)(constraint.RotationY); suppData.RZ = (int)(constraint.RotationZ);
            suppData.KX = constraint.TranslationalStiffnessX;
            suppData.KY = constraint.TranslationalStiffnessY;
            suppData.KZ = constraint.TranslationalStiffnessZ;
            suppData.HX = constraint.RotationalStiffnessX;
            suppData.HY = constraint.RotationalStiffnessY;
            suppData.HZ = constraint.RotationalStiffnessZ;
        }
    }
}
