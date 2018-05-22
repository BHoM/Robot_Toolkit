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
using BH.oM.Adapters.Robot.Properties;
using BHE = BH.Engine.Adapters.Robot.Properties;

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
    
        public List<Loadcase> ReadLoadCase(List<string> ids = null)
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

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/
    }

}

