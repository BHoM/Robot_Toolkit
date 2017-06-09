using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Global;
using BHoM.Structural;
using RobotOM;
using BHoME = BHoM.Structural.Elements;
using BHoMP = BHoM.Structural.Properties;
using BHoML = BHoM.Structural.Loads;
using BHoM.Structural.Interface;
using BHoM.Base;
namespace Robot_Adapter.Structural.Interface
{
    public partial class RobotAdapter : BHoM.Structural.Interface.ICommandAdapter
    {
        public bool Analyse(List<string> cases = null)
        {
            Robot.UserControl = true;
            Robot.Interactive = 1;
            Robot.Project.CalcEngine.AnalysisParams.IgnoreWarnings = true;
            Robot.Project.CalcEngine.AnalysisParams.AutoVerification = IRobotStructureAutoVerificationType.I_SAVT_NONE;
            Robot.Project.CalcEngine.Calculate();
            Robot.Project.CalcEngine.AutoFreezeResults = false;
            return true;
        }

        public bool ClearResults()
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool Save(string fileName = null)
        {
            throw new NotImplementedException();
        }
    }
}
