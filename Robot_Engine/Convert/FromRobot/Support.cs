using RobotOM;
using System.Collections.Generic;
using BH.oM.Structure.Properties;


namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static Constraint6DOF ToBHoMObject(this RobotNodeSupport robSupport)
        {
            List<bool> fixity = new List<bool>();
            RobotNodeSupportData suppData = robSupport.Data;
            fixity.Add(suppData.UX != 0);
            fixity.Add(suppData.UY != 0);
            fixity.Add(suppData.UZ != 0);
            fixity.Add(suppData.RX != 0);
            fixity.Add(suppData.RY != 0);
            fixity.Add(suppData.RZ != 0);

            List<double> stiffness = new List<double>();
            stiffness.Add(suppData.KX);
            stiffness.Add(suppData.KY);
            stiffness.Add(suppData.KZ);
            stiffness.Add(suppData.HX);
            stiffness.Add(suppData.HY);
            stiffness.Add(suppData.HZ);

            Constraint6DOF const6DOF = BH.Engine.Structure.Create.Constraint6DOF(robSupport.Name, fixity, stiffness);
            return const6DOF;
        }

        /***************************************************/

    }
}
