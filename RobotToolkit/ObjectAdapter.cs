using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Global;
using BHoM.Structural;
using BHoM.Structural.Loads;
using RobotOM;

namespace RobotToolkit
{
    public partial class RobotAdapter : IStructuralAdapter
    {
        private RobotApplication Robot;
        private string Settings;

        public string Filename
        {
            get
            {
                return Robot.Project.FileName;
            }
        }

        public RobotAdapter()
        {
            Robot = new RobotApplication();
        }

        /// <summary>
        /// Gets the bars in the robot model base on the input seleciton
        /// </summary>
        /// <param name="bars">output bar list</param>
        /// <param name="option"></param>
        /// <returns>true is successful</returns>
        public bool GetBars(out List<Bar> bars, string option = "all")
        {
            BarIO.GetBars(Robot, out bars);
            return true;
        }

        public bool CreateBars(List<Bar> bars)
        {
            BarIO.CreateBars(Robot, bars);
            return true;
        }

        public bool GetLoadcases(out List<ICase> cases)
        {
            throw new NotImplementedException();
        }

        public bool GetLoads(out List<ILoad> loads, string option = "")
        {
            throw new NotImplementedException();
        }

        public bool GetNodes(out List<Node> nodes, string option = "all")
        {
            NodeIO.GetNodes(Robot, out nodes, option);
            return true;
        }

        public bool GetPanels(out List<BHoM.Structural.Panel> panels, string option = "all")
        {
            return PanelIO.GetPanels(Robot, out panels);
        }


        public bool GetLevels(out List<Storey> levels, string options = "")
        {
            throw new NotImplementedException();
        }

        public bool GetOpenings(out List<Opening> opening, string option = "")
        {
            throw new NotImplementedException();
        }

        public bool SetNodes(List<Node> nodes, string option = "")
        {
            return NodeIO.CreateNodes(Robot, nodes);
        }

        public bool SetBars(List<Bar> bars, string option = "")
        {
            return BarIO.CreateBars(Robot, bars);
        }

        public bool SetPanels(List<Panel> panels, string option = "")
        {
            return PanelIO.CreatePanels(Robot, panels);
        }

        public bool SetOpenings(List<Opening> opening, string option = "")
        {
            throw new NotImplementedException();
        }

        public bool SetLevels(List<Storey> stores, string option = "")
        {
            throw new NotImplementedException();
        }

        public bool SetLoads(List<ILoad> loads, string option = "")
        {
            throw new NotImplementedException();
        }

        public bool SetLoadcases(List<ICase> cases)
        {
            throw new NotImplementedException();
        }
    }
}
