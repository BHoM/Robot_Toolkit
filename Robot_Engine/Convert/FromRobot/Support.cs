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
    }
}
