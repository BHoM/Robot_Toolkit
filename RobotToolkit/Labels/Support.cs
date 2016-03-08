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
    public class Support
    {
        /// <summary>
        /// Create a support label
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="restraint"></param>
        /// <returns></returns>
        public static bool CreateSupportLabel(RobotApplication robot, BHoM.Structural.Constraint restraint)
        {
            IRobotLabel robot_restraint = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_SUPPORT, restraint.Name);
            if (robot.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_SUPPORT, restraint.Name) == 1)
            {
                robot_restraint = robot.Project.Structure.Labels.Get(IRobotLabelType.I_LT_SUPPORT, restraint.Name);
            }
            IRobotNodeSupportData data = (IRobotNodeSupportData)robot_restraint.Data;

            data.UX = (restraint.DoFType(AxisDirection.X) == DOFType.Fixed) ? -1 : 0;
            data.UY = (restraint.DoFType(AxisDirection.Y) == DOFType.Fixed) ? -1 : 0;
            data.UZ = (restraint.DoFType(AxisDirection.Z) == DOFType.Fixed) ? -1 : 0;
            data.RX = (restraint.DoFType(AxisDirection.XX) == DOFType.Fixed) ? -1 : 0;
            data.RY = (restraint.DoFType(AxisDirection.YY) == DOFType.Fixed) ? -1 : 0;
            data.RZ = (restraint.DoFType(AxisDirection.ZZ) == DOFType.Fixed) ? -1 : 0;

            if (restraint.DoFType(AxisDirection.X) == DOFType.Spring) data.KX = restraint.Value(AxisDirection.X);
            if (restraint.DoFType(AxisDirection.Y) == DOFType.Spring) data.KY = restraint.Value(AxisDirection.Y);
            if (restraint.DoFType(AxisDirection.Z) == DOFType.Spring) data.KZ = restraint.Value(AxisDirection.Z);
            if (restraint.DoFType(AxisDirection.XX) == DOFType.Spring) data.HX = restraint.Value(AxisDirection.XX);
            if (restraint.DoFType(AxisDirection.YY) == DOFType.Spring) data.HY = restraint.Value(AxisDirection.YY);
            if (restraint.DoFType(AxisDirection.ZZ) == DOFType.Spring) data.HZ = restraint.Value(AxisDirection.ZZ);

            robot.Project.Structure.Labels.Store(robot_restraint);

            return true;
        }

        /// <summary>
        /// Generate a BHoM constraint object from a Robot support label
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="support"></param>
        /// <returns></returns>
        public static BHoM.Structural.Constraint CreateConstraintFromRobotSupport(RobotApplication robot, IRobotLabel support)
        {
            BHoM.Structural.Constraint constraint = new Constraint(support.Name);
            IRobotNodeSupportData supportData = support.Data;
            


            return constraint;
        }

    }    
}
