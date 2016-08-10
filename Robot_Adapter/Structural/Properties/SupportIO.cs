using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoMP = BHoM.Structural.Properties;

namespace Robot_Adapter.Labels
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
        public static bool CreateSupportLabel(RobotApplication robot, BHoMP.NodeConstraint restraint)
        {
            IRobotLabel robot_restraint = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_SUPPORT, restraint.Name);
            if (robot.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_SUPPORT, restraint.Name) == 1)
            {
                robot_restraint = robot.Project.Structure.Labels.Get(IRobotLabelType.I_LT_SUPPORT, restraint.Name);
            }
            IRobotNodeSupportData data = (IRobotNodeSupportData)robot_restraint.Data;

            data.UX = (restraint.UX.Type == BHoMP.DOFType.Fixed) ? -1 : 0;
            data.UY = (restraint.UY.Type == BHoMP.DOFType.Fixed) ? -1 : 0;
            data.UZ = (restraint.UZ.Type == BHoMP.DOFType.Fixed) ? -1 : 0;
            data.RX = (restraint.RX.Type == BHoMP.DOFType.Fixed) ? -1 : 0;
            data.RY = (restraint.RY.Type == BHoMP.DOFType.Fixed) ? -1 : 0;
            data.RZ = (restraint.RZ.Type == BHoMP.DOFType.Fixed) ? -1 : 0;

            if (restraint.UX.Type == BHoMP.DOFType.Spring) data.KX = restraint.UX.Value;
            if (restraint.UY.Type == BHoMP.DOFType.Spring) data.KY = restraint.UY.Value;
            if (restraint.UZ.Type == BHoMP.DOFType.Spring) data.KZ = restraint.UZ.Value;
            if (restraint.RX.Type == BHoMP.DOFType.Spring) data.HX = restraint.RX.Value;
            if (restraint.RY.Type == BHoMP.DOFType.Spring) data.HY = restraint.RY.Value;
            if (restraint.RZ.Type == BHoMP.DOFType.Spring) data.HZ = restraint.RZ.Value;

            robot.Project.Structure.Labels.Store(robot_restraint);

            return true;
        }

        /// <summary>
        /// Generate a BHoM constraint object from a Robot support label
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="support"></param>
        /// <returns></returns>
        public static BHoMP.NodeConstraint CreateConstraintFromRobotSupport(RobotApplication robot, IRobotLabel support)
        {
            BHoMP.NodeConstraint constraint = new BHoMP.NodeConstraint(support.Name);
            IRobotNodeSupportData supportData = support.Data;
            
            return constraint;
        }
    }    
}
