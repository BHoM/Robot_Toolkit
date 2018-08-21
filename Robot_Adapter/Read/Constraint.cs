using System.Collections.Generic;
using RobotOM;
using BH.oM.Structural.Properties;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Constraint6DOF> ReadConstraints6DOF(List<string> ids = null)
        {
            IRobotCollection robSupport = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_SUPPORT);
            List<Constraint6DOF> constList = new List<Constraint6DOF>();

            for (int i = 1; i <= robSupport.Count; i++)
            {
                RobotNodeSupport rNodeSupp = robSupport.Get(i);
                Constraint6DOF bhomSupp = BH.Engine.Robot.Convert.ToBHoMObject(rNodeSupp);
                bhomSupp.CustomData.Add(AdapterId, rNodeSupp.Name);
                //if (m_SupportTaggs != null)
                //    bhomSupp.ApplyTaggedName(m_SupportTaggs[rNodeSupp.Name]);
                constList.Add(bhomSupp);
            }
            return constList;
        }

        /***************************************************/

    }

}

