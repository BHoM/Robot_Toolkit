using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RobotOM;
using BHoMR = BHoM.Structural.Results;
using BHoMBR = BHoM.Base.Results;
using Robot_Adapter.Base;
using Robot_Adapter.Results;

namespace Robot_Adapter.Structural.Interface
{
    public partial class RobotAdapter : BHoM.Structural.Interface.IResultAdapter
    {
        public bool GetBarForces(List<string> bars, List<string> cases, int divisions, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.ResultSet<BHoMR.BarForce>> results)
        {
            RobotApplication RobotApp = Robot;

            BHoMBR.ResultServer<BHoMR.BarForce> resultServer = new BHoMBR.ResultServer<BHoMR.BarForce>();
            BarResults.LoadBarForces(RobotApp, resultServer, bars, cases, divisions);
            results = resultServer.LoadData();

            return true;
        }

        public bool GetBarStresses()
        {
            throw new NotImplementedException();
        }

        public bool GetModalResults()
        {
            throw new NotImplementedException();
        }

        public bool GetNodeAccelerations()
        {
            throw new NotImplementedException();
        }

        public bool GetNodeDisplacements()
        {
            throw new NotImplementedException();
        }

        public bool GetNodeReactions(List<string> nodes, List<string> cases, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.ResultSet<BHoMR.NodeReaction>> results)
        {
            throw new NotImplementedException();
        }

        public bool GetNodeVelocities()
        {
            throw new NotImplementedException();
        }

        public bool GetPanelForces()
        {
            throw new NotImplementedException();
        }

        public bool GetPanelStress()
        {
            throw new NotImplementedException();
        }
    }
}
