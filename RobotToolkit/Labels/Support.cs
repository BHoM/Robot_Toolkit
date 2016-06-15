using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit.Labels
{
    /// <summary>
    /// Import/export/operate on Robot support labels
    /// </summary>
    public class SupportIO
    {
        /// <summary>
        /// Create a support label
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="restraint"></param>
        /// <returns></returns>
        public static bool CreateSupportLabel(RobotApplication robot, BHoM.Structural.NodeConstraint restraint)
        {
            IRobotLabel robot_restraint = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_SUPPORT, restraint.Name);
            if (robot.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_SUPPORT, restraint.Name) == 1)
            {
                robot_restraint = robot.Project.Structure.Labels.Get(IRobotLabelType.I_LT_SUPPORT, restraint.Name);
            }
            IRobotNodeSupportData data = (IRobotNodeSupportData)robot_restraint.Data;

            data.UX = (restraint.UX.Type == DOFType.Fixed) ? -1 : 0;
            data.UY = (restraint.UY.Type == DOFType.Fixed) ? -1 : 0;
            data.UZ = (restraint.UZ.Type == DOFType.Fixed) ? -1 : 0;
            data.RX = (restraint.RX.Type == DOFType.Fixed) ? -1 : 0;
            data.RY = (restraint.RY.Type == DOFType.Fixed) ? -1 : 0;
            data.RZ = (restraint.RZ.Type == DOFType.Fixed) ? -1 : 0;

            if (restraint.UX.Type == DOFType.Spring) data.KX = restraint.UX.Value;
            if (restraint.UY.Type == DOFType.Spring) data.KY = restraint.UY.Value;
            if (restraint.UZ.Type == DOFType.Spring) data.KZ = restraint.UZ.Value;
            if (restraint.RX.Type == DOFType.Spring) data.HX = restraint.RX.Value;
            if (restraint.RY.Type == DOFType.Spring) data.HY = restraint.RY.Value;
            if (restraint.RZ.Type == DOFType.Spring) data.HZ = restraint.RZ.Value;

            robot.Project.Structure.Labels.Store(robot_restraint);

            return true;
        }

        /// <summary>
        /// Generate a BHoM constraint object from a Robot support label
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="support"></param>
        /// <returns></returns>
        public static BHoM.Structural.NodeConstraint CreateConstraintFromRobotSupport(RobotApplication robot, IRobotLabel support)
        {
            BHoM.Structural.NodeConstraint constraint = new NodeConstraint(support.Name);
            IRobotNodeSupportData supportData = support.Data;
            
            return constraint;
        }
    }    
}
