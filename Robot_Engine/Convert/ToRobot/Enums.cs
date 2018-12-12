using BH.oM.Structure.Properties.Section;
using RobotOM;
using BH.oM.Structure.Results;

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

        public static IRobotFeLayerType FromBHoMEnum(MeshResultLayer bhomMeshLayer)
        {
            IRobotFeLayerType robotLayerType = new IRobotFeLayerType();

            switch (bhomMeshLayer)
            {
                case (MeshResultLayer.AbsoluteMaximum):
                    robotLayerType = IRobotFeLayerType.I_FLT_ABSOLUTE_MAXIMUM;
                    break;
                case (MeshResultLayer.Lower):
                    robotLayerType = IRobotFeLayerType.I_FLT_LOWER;
                    break;
                case (MeshResultLayer.Maximum):
                    robotLayerType = IRobotFeLayerType.I_FLT_MAXIMUM;
                    break;
                case (MeshResultLayer.Middle):
                    robotLayerType = IRobotFeLayerType.I_FLT_MIDDLE;
                    break;
                case (MeshResultLayer.Minimum):
                    robotLayerType = IRobotFeLayerType.I_FLT_MINIMUM;
                    break;
                case (MeshResultLayer.Upper):
                    robotLayerType = IRobotFeLayerType.I_FLT_UPPER;
                    break;
                case (MeshResultLayer.Arbitrary):
                    robotLayerType = IRobotFeLayerType.I_FLT_ARBITRARY;
                    break;
            }
            return robotLayerType;
        }

        /***************************************************/

        public static IRobotFeResultSmoothing FromBHoMEnum(MeshResultSmoothingType bhomPanelSmoothingType)
        {
            IRobotFeResultSmoothing robotSmoothingType = new IRobotFeResultSmoothing();

            switch (bhomPanelSmoothingType)
            {
                case (MeshResultSmoothingType.None):
                    robotSmoothingType = IRobotFeResultSmoothing.I_FRS_NO_SMOOTHING;
                    break;
                case (MeshResultSmoothingType.ByPanel):
                    robotSmoothingType = IRobotFeResultSmoothing.I_FRS_SMOOTHING_WITHIN_A_PANEL;
                    break;
                case (MeshResultSmoothingType.BySelection):
                    robotSmoothingType = IRobotFeResultSmoothing.I_FRS_SMOOTHING_ACCORDING_TO_SELECTION;
                    break;
                case (MeshResultSmoothingType.Global):
                    robotSmoothingType = IRobotFeResultSmoothing.I_FRS_GLOBAL_SMOOTHING;
                    break;
                case (MeshResultSmoothingType.ByFiniteElementCentres):
                    robotSmoothingType = IRobotFeResultSmoothing.I_FRS_IN_ELEMENT_CENTER;
                    break;           
            }
            return robotSmoothingType;
        }

        /***************************************************/

    }
}
