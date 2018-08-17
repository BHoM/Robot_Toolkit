using System.Collections.Generic;
using System.Linq;
using System;
using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using RobotOM;

using BH.Engine.Robot;
using BHEG = BH.Engine.Geometry;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Index Adapter Interface                   ****/
        /***************************************************/

        /***************************************************/
               
        private bool CreateCollection(IEnumerable<Loadcase> loadCase)
        {
            List<Loadcase> caseList = loadCase.ToList();
            IRobotCaseServer caseServer = m_RobotApplication.Project.Structure.Cases;

            m_RobotApplication.Project.Structure.Cases.BeginMultiOperation();
            for (int i = 0; i < caseList.Count; i++)
            {                
                int subNature;
                IRobotCaseNature rNature = BH.Engine.Robot.Convert.RobotLoadNature(caseList[i], out subNature);
                m_RobotApplication.Project.Structure.Cases.CreateSimple(caseList[i].Number, caseList[i].Name, rNature, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);                
                IRobotSimpleCase sCase = caseServer.Get(caseList[i].Number) as IRobotSimpleCase;
                if (subNature >= 0)
                    sCase.SetNatureExt(subNature);
            }
            m_RobotApplication.Project.Structure.Cases.EndMultiOperation();
            return true;
        }

        /***************************************************/

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

