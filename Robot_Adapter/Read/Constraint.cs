using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.Engine.Serialiser;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Base;
using BH.oM.Common.Materials;
using BH.oM.Structural.Design;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {         
        
        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/
        
        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/
        
        /***************************************************/
     
        public List<Constraint6DOF> ReadConstraints6DOF(List<string> ids = null)
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

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

