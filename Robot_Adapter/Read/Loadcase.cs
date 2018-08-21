using System.Collections.Generic;
using RobotOM;
using BH.oM.Structural.Loads;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Loadcase> ReadLoadCase(List<string> ids = null)
        {
            RobotCaseCollection rLoadCases = m_RobotApplication.Project.Structure.Cases.GetAll();
            List<Loadcase> bhomLoadCases = new List<Loadcase>();

            for (int i = 1; i <= rLoadCases.Count; i++)
            {
                IRobotCase rLoadCase = rLoadCases.Get(i) as IRobotCase;
                Loadcase lCase = BH.Engine.Structure.Create.Loadcase(rLoadCase.Name, rLoadCase.Number, BH.Engine.Robot.Convert.BHoMLoadNature(rLoadCase.Nature));
                lCase.CustomData[AdapterId] = rLoadCase.Number;
                bhomLoadCases.Add(lCase);
            }
            return bhomLoadCases;
        }

        /***************************************************/

    }

}

