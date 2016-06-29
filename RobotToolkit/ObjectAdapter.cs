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

        /// <summary>
        /// Sets the bars in robot
        /// </summary>
        /// <param name="bars"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool CreateBars(List<Bar> bars, out List<string> ids)
        {
            BarIO.CreateBars(Robot, bars, out ids);
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

        public bool SetNodes(List<Node> nodes, out List<string> ids, string option = "")
        {
            return NodeIO.CreateNodes(Robot, nodes, out ids);
        }

        public bool SetBars(List<Bar> bars, out List<string> ids, string option = "")
        {
            return BarIO.CreateBars(Robot, bars, out ids);
        }

        public bool SetPanels(List<Panel> panels, out List<string> ids, string option = "")
        {
            return PanelIO.CreatePanels(Robot, panels, out ids);
        }

        public bool SetOpenings(List<Opening> opening, out List<string> ids, string option = "")
        {
            throw new NotImplementedException();
        }

        public bool SetLevels(List<Storey> stores, out List<string> ids, string option = "")
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
