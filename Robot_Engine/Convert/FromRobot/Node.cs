using BH.oM.Geometry;
using RobotOM;
using BH.oM.Structural.Elements;


namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static Node ToBHoMObject(this RobotNode robotNode)
        {
            Node bhomNode = new Node { Position = new Point { X = robotNode.X, Y = robotNode.Y, Z = robotNode.Z } };
            if (robotNode.HasLabel(IRobotLabelType.I_LT_SUPPORT) == 1)
            {
                bhomNode.Constraint = BH.Engine.Robot.Convert.ToBHoMObject((RobotNodeSupport)robotNode.GetLabel(IRobotLabelType.I_LT_SUPPORT));
            }
            return bhomNode;
        }

        /***************************************************/
    }
}
