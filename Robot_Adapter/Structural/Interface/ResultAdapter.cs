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
using Robot_Adapter.Structural.Results;
using BHoM.Structural.Interface;

namespace Robot_Adapter.Structural.Interface
{
    public partial class RobotAdapter : IResultAdapter
    {
        public bool GetBarForces(List<string> bars, List<string> cases, int divisions, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.IResultSet> results)
        {
            BHoMBR.ResultServer<BHoMR.BarForce<int, int, int>> resultServer = new BHoMBR.ResultServer<BHoMR.BarForce<int, int, int>>();
            resultServer.OrderBy = orderBy;
            BarResults.GetBarForces(Robot, resultServer, bars, cases, divisions);
            results = resultServer.LoadData();

            return true;
        }

        public bool GetBarStresses()
        {
            throw new NotImplementedException();
        }

        public bool GetBarUtilisation(List<string> bars, List<string> cases, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetModalResults()
        {
            throw new NotImplementedException();
        }

        public bool GetNodeAccelerations(List<string> nodes, List<string> cases, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetNodeDisplacements(List<string> nodes, List<string> cases, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.IResultSet> results)
        {
            BHoMBR.ResultServer<BHoMR.NodeDisplacement> resultServer = new BHoMBR.ResultServer<BHoMR.NodeDisplacement>();
            resultServer.OrderBy = orderBy;
            NodeResults.GetNodeDisplacements(Robot, resultServer, nodes, cases);
            results = resultServer.LoadData();

            return true;
        }

        public bool GetNodeReactions(List<string> nodes, List<string> cases, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.IResultSet> results)
        {
            BHoMBR.ResultServer<BHoMR.NodeReaction> resultServer = new BHoMBR.ResultServer<BHoMR.NodeReaction>();
            resultServer.OrderBy = orderBy;
            NodeResults.GetNodeReacions(Robot, resultServer, nodes, cases);
            results = resultServer.LoadData();

            return true;
        }
       
        public bool GetNodeVelocities(List<string> nodes, List<string> cases, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetPanelForces(List<string> panels, List<string> cases, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.IResultSet> results)
        {
            BHoMBR.ResultServer<BHoMR.PanelForce> resultServer = new BHoMBR.ResultServer<BHoMR.PanelForce>();
            resultServer.OrderBy = orderBy;
            PanelResults.GetPanelForces(Robot, resultServer, panels, cases);
            results = resultServer.LoadData();

            return true;
        }

        public bool GetPanelStress(List<string> panels, List<string> cases, BHoMBR.ResultOrder orderBy, out Dictionary<string, BHoMBR.IResultSet> results)
        {
            BHoMBR.ResultServer<BHoMR.PanelStress> resultServer = new BHoMBR.ResultServer<BHoMR.PanelStress>();
            resultServer.OrderBy = orderBy;
            PanelResults.GetPanelStress(Robot, resultServer, panels, cases);
            results = resultServer.LoadData();

            return true;
        }

        public bool GetSlabReinforcement(List<string> panels, List<string> cases, BHoMBR.ResultOrder orderby, out Dictionary<string, BHoMBR.IResultSet> results)
        {
            BHoMBR.ResultServer<BHoMR.SlabReinforcement> resultServer = new BHoMBR.ResultServer<BHoMR.SlabReinforcement>();
            resultServer.OrderBy = orderby;
            SlabReinforcementResults.GetSlabReinforcement(Robot, resultServer, panels, cases);
            results = resultServer.LoadData();

            return true;
        }

        public bool StoreResults(string filename, List<BHoMBR.ResultType> resultTypes, List<string> loadcases, bool append = false)
        {
            foreach (BHoMBR.ResultType t in resultTypes)
            {
                switch (t)
                {
                    case BHoM.Base.Results.ResultType.BarForce:
                        BarResults.GetBarForces(Robot, new BHoMBR.ResultServer<BHoMR.BarForce<int, int, int>>(filename, append), null, loadcases, 3);
                        break;
                    case BHoM.Base.Results.ResultType.BarStress:
                       // BarResults.GetBarStress(Robot, new BHoMBR.ResultServer<BHoMR.BarStress>(filename), null, loadcases, 3);
                        break;
                    case BHoMBR.ResultType.NodeReaction:
                        NodeResults.GetNodeReacions(Robot, new BHoMBR.ResultServer<BHoMR.NodeReaction>(filename, append), null, loadcases);
                        break;
                    case BHoMBR.ResultType.NodeDisplacement:
                        NodeResults.GetNodeDisplacements(Robot, new BHoMBR.ResultServer<BHoMR.NodeDisplacement>(filename, append), null, loadcases);
                        break;
                    case BHoMBR.ResultType.PanelForce:
                        PanelResults.GetPanelForces(Robot, new BHoMBR.ResultServer<BHoMR.PanelForce>(filename, append), null, loadcases);
                        break;
                    case BHoMBR.ResultType.PanelStress:
                        PanelResults.GetPanelStress(Robot, new BHoMBR.ResultServer<BHoMR.PanelStress>(filename), null, loadcases);
                        break;

                }
            }
            return true;
        }
    }
}
