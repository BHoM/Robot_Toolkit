using System.Collections.Generic;
using BH.oM.Structure.Loads;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<LoadCombination> lComabinations)
        {

            foreach (LoadCombination lComb in lComabinations)
            {
                RobotCaseCombination rCaseCombination = m_RobotApplication.Project.Structure.Cases.CreateCombination(lComb.Number, lComb.Name, IRobotCombinationType.I_CBT_ULS, IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_COMB);
                for (int i = 0; i < lComb.LoadCases.Count; i++)
                {
                    rCaseCombination.CaseFactors.New(lComb.LoadCases[i].Item2.Number, lComb.LoadCases[i].Item1);
                }
                updateview();
            }
        
            return true;
        }

        /***************************************************/

    }

}

