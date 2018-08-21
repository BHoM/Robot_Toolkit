using BH.oM.Structural.Properties;
using RobotOM;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static IRobotBarSectionType FromBHoMEnum(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Angle:
                    return IRobotBarSectionType.I_BST_NS_L;
                case ShapeType.Box:
                    return IRobotBarSectionType.I_BST_NS_BOX;
                case ShapeType.Channel:
                    return IRobotBarSectionType.I_BST_NS_C;
                case ShapeType.Circle:
                    return IRobotBarSectionType.I_BST_NS_C;
                case ShapeType.DoubleAngle:
                    return IRobotBarSectionType.I_BST_NS_LP;
                case ShapeType.ISection:
                    return IRobotBarSectionType.I_BST_NS_I;
                case ShapeType.Rectangle:
                    return IRobotBarSectionType.I_BST_NS_RECT;
                case ShapeType.Tee:
                    return IRobotBarSectionType.I_BST_NS_T;
                case ShapeType.Tube:
                case ShapeType.Cable:
                    return IRobotBarSectionType.I_BST_NS_TUBE;
                case ShapeType.FreeForm:
                    return IRobotBarSectionType.I_BST_NS_POLYGONAL;
                default:
                    return IRobotBarSectionType.I_BST_NS_RECT;

            }
        }

        /***************************************************/
    }
}
