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

            data.UX = (restraint.X.Type == DOFType.Fixed) ? -1 : 0;
            data.UY = (restraint.Y.Type == DOFType.Fixed) ? -1 : 0;
            data.UZ = (restraint.Z.Type == DOFType.Fixed) ? -1 : 0;
            data.RX = (restraint.XX.Type == DOFType.Fixed) ? -1 : 0;
            data.RY = (restraint.YY.Type == DOFType.Fixed) ? -1 : 0;
            data.RZ = (restraint.ZZ.Type == DOFType.Fixed) ? -1 : 0;

            if (restraint.X.Type == DOFType.Spring) data.KX = restraint.X.Value;
            if (restraint.Y.Type == DOFType.Spring) data.KY = restraint.Y.Value;
            if (restraint.Z.Type == DOFType.Spring) data.KZ = restraint.Z.Value;
            if (restraint.XX.Type == DOFType.Spring) data.HX = restraint.XX.Value;
            if (restraint.YY.Type == DOFType.Spring) data.HY = restraint.YY.Value;
            if (restraint.ZZ.Type == DOFType.Spring) data.HZ = restraint.ZZ.Value;

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
