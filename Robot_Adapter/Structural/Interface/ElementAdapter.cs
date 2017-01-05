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
    public partial class RobotAdapter : BHoM.Structural.Interface.IElementAdapter
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

        public ObjectSelection Selection
        {
            get; set;        
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
        public List<string> GetBars(out List<BHoME.Bar> bars, List<string> ids = null)
        {
            return Structural.Elements.BarIO.GetBars(Robot, out bars, Selection, ids);
        }

        /// <summary>
        /// Sets the bars in robot
        /// </summary>
        /// <param name="bars"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool CreateBars(List<BHoME.Bar> bars, out List<string> ids)
        {
            Structural.Elements.BarIO.CreateBars(Robot, bars, out ids);
            return true;
        }

        public List<string> GetLoadcases(out List<BHoML.ICase> cases)
        {
            return Loads.LoadIO.GetLoadcases(Robot, out cases);
        }

        public List<string> GetNodes(out List<BHoME.Node> nodes, List<string> ids = null)
        {
            return Structural.Elements.NodeIO.GetNodes(Robot, out nodes, Selection, ids);
        }

        public List<string> GetPanels(out List<BHoME.Panel> panels, List<string> ids = null)
        {
            return Structural.Elements.PanelIO.GetPanels(Robot, out panels, Selection, ids);
        }

        public List<string> GetLevels(out List<BHoME.Storey> levels, List<string> ids = null)
        {
            return Structural.Elements.LevelIO.GetLevels(Robot, out levels);
        }

        public List<string> GetOpenings(out List<BHoME.Opening> opening, List<string> ids = null)
        {
            throw new NotImplementedException();
        }

        public bool SetNodes(List<BHoME.Node> nodes, out List<string> ids)
        {
            Robot.Interactive = 0;
            Structural.Elements.NodeIO.CreateNodes(Robot, nodes, out ids);
            Robot.Interactive = 1;
            return true;
        }

        public bool SetBars(List<BHoME.Bar> bars, out List<string> ids)
        {
            return Structural.Elements.BarIO.CreateBars(Robot, bars, out ids);
        }

        public bool SetPanels(List<BHoME.Panel> panels, out List<string> ids)
        {
            return Structural.Elements.PanelIO.CreatePanels(Robot, panels, out ids);
        }

        public bool SetOpenings(List<BHoME.Opening> opening, out List<string> ids)
        {
            return Structural.Elements.PanelIO.CreateOpenings(Robot, opening, out ids);
        }

        public bool SetLevels(List<BHoME.Storey> stories, out List<string> ids)
        {
            return Structural.Elements.LevelIO.SetLevels(Robot, stories, out ids);
        }

        public bool SetLoads(List<BHoML.ILoad> loads)
        {
            return Loads.LoadIO.SetLoads(Robot, loads);
        }

        public bool SetLoadcases(List<BHoML.ICase> cases)
        {
            return Loads.LoadIO.SetLoadcases(Robot, cases);
        }

        public List<string> GetGrids(out List<BHoME.Grid> grids, List<string> ids = null)
        {
            return Elements.GridIO.GetGrids(Robot, out grids, ids);
        }

        public bool SetGrids(List<BHoME.Grid> grid, out List<string> ids)
        {
            return Elements.GridIO.SetGrids(Robot, grid, out ids);
        }

        public List<string> GetRigidLinks(out List<BHoME.RigidLink> links, List<string> ids = null)
        {
            return Elements.RigidLinkIO.GetRigidLinks(Robot, out links, Selection, ids);
        }

        public List<string> GetGroups(out List<IGroup> groups, List<string> ids = null)
        {
            return Elements.GroupIO.GetGroups(Robot, out groups);
        }

        public bool SetRigidLinks(List<BHoME.RigidLink> rigidLinks, out List<string> ids)
        {
            return Elements.RigidLinkIO.SetRigidLinks(Robot, rigidLinks, out ids);
        }

        public bool SetGroups(List<IGroup> groups, out List<string> ids)
        {
            return Elements.GroupIO.CreateGroups(Robot, groups, out ids);
        }

        public List<string> GetFEMeshes(out List<BHoME.FEMesh> meshes, List<string> ids = null)
        {
            return Structural.Elements.MeshIO.GetMeshes(Robot, out meshes, Selection, ids);
        }

        public bool SetFEMeshes(List<BHoME.FEMesh> meshes, out List<string> ids)
        {
            return Elements.MeshIO.CreateMesh(Robot, meshes, out ids);
        }
        public bool Run()
        {
            Robot.UserControl = true;
            Robot.Interactive = 1;
            Robot.Project.CalcEngine.AnalysisParams.IgnoreWarnings = true;
            Robot.Project.CalcEngine.AnalysisParams.AutoVerification = IRobotStructureAutoVerificationType.I_SAVT_NONE;
            Robot.Project.CalcEngine.Calculate();
            Robot.Project.CalcEngine.AutoFreezeResults = false;
            return true;
        }

        public void StopCalculations()
        {
            Robot.Project.CalcEngine.StopCalculation();
        }


        public bool GetLoads(out List<BHoML.ILoad> loads, List<BHoML.Loadcase> ids = null)
        {
            return Loads.LoadIO.GetLoads(Robot, ids, out loads);
        }
    }
}
