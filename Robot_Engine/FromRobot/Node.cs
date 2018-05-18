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
using BH.oM.Reflection.Debuging;
using BH.oM.Adapters.Robot.Properties;


namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************/

        public static Node ToBHoMObject(this RobotNode robotNode)
        {
            Node bhomNode = new Node { Position = new Point { X = robotNode.X, Y = robotNode.Y, Z = robotNode.Z } };
            if (robotNode.HasLabel(IRobotLabelType.I_LT_SUPPORT) == 1)
            {
                bhomNode.Constraint = BH.Engine.Robot.Convert.ToBHoMObject((RobotNodeSupport)robotNode.GetLabel(IRobotLabelType.I_LT_SUPPORT));
            }
            return bhomNode;
        }
            }
}
