using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using RobotOM;
using BH.Engine.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Index Adapter Interface                   ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;
            success = Create(objects as dynamic);
            updateview();
            return success;
        }

        /***************************************************/

        private bool Create(IEnumerable<Constraint6DOF> constraints)
        {
            List<Constraint6DOF> constList = constraints.ToList();
            for (int i = 0; i < constList.Count; i++)
            {
                IRobotLabel lable = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_SUPPORT, constList[i].Name);
                RobotNodeSupportData suppData = lable.Data;
                BH.Engine.Robot.Convert.RobotConstraint(suppData, constList[i]);
                m_RobotApplication.Project.Structure.Labels.Store(lable);
            }
            return true;
        }

        /***************************************************/

        private bool Create(IEnumerable<ISectionProperty> secProp)
        {
            List<ISectionProperty> secPropList = secProp.ToList();

            for (int i = 0; i < secPropList.Count; i++)
            {
                IRobotLabel lable = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, secPropList[i].Name);
                RobotBarSectionData secData = lable.Data;
                BH.Engine.Robot.Convert.ISectionType(secPropList[i], secData);
                m_RobotApplication.Project.Structure.Labels.Store(lable);
            }
            return true;
        }

        /***************************************************/

        private bool Create(IEnumerable<Material> mat)
        {
            List<Material> matList = mat.ToList();
    
            for (int i = 0; i < matList.Count; i++)
            {
                IRobotLabel lable = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_MATERIAL, matList[i].Name);
                IRobotMaterialData matData = lable.Data;
                BH.Engine.Robot.Convert.RobotMaterial(matData, matList[i]);
                m_RobotApplication.Project.Structure.Labels.Store(lable);
            }
            return true;
        }

        /***************************************************/

        private bool Create(IEnumerable<Loadcase> loadCase)
        {
            List<Loadcase> caseList = loadCase.ToList();
            IRobotCaseServer caseServer = m_RobotApplication.Project.Structure.Cases;

            for (int i = 0; i < caseList.Count; i++)
            {
                int subNature;
                IRobotCaseNature rNature = BH.Engine.Robot.Convert.RobotLoadNature(caseList[i], out subNature);
                m_RobotApplication.Project.Structure.Cases.CreateSimple(caseList[i].Number, caseList[i].Name, rNature, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                IRobotSimpleCase sCase = caseServer.Get(caseList[i].Number) as IRobotSimpleCase;
                if (subNature >= 0)
                    sCase.SetNatureExt(subNature);
            }
            return true;
        }

        /***************************************************/


        private bool Create(IEnumerable<BH.oM.Structural.Elements.Node> nodes)
        {
            List<Node> nodeList = nodes.ToList();
            RobotStructureCache rcache = m_RobotApplication.Project.Structure.CreateCache();
            RobotSelection nodeSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            int freeNum = m_RobotApplication.Project.Structure.Nodes.FreeNumber;

            foreach (Node node in nodes)
            {
                int nodeNum = 0;
                int.TryParse(node.CustomData[AdapterId].ToString(), out nodeNum);
                rcache.AddNode(nodeNum, node.Position.X, node.Position.Y, node.Position.Z);
                nodeSel.AddText(nodeNum.ToString());
            }
            m_RobotApplication.Project.Structure.ApplyCache(rcache);
            IRobotCollection robotNodeCol = m_RobotApplication.Project.Structure.Nodes.GetAll();

            for (int i = freeNum; i <= (freeNum - 1 + nodeList.Count); i++)
            {
                if(nodeList[i - freeNum].Constraint != null)
                    m_RobotApplication.Project.Structure.Nodes.Get(i).SetLabel(IRobotLabelType.I_LT_SUPPORT, nodeList[i - freeNum].Constraint.Name);
            }
            return true;
        }

        /***************************************************/

        public bool Create(IEnumerable<Bar> bhomBars)
        {
            RobotStructureCache rcache = m_RobotApplication.Project.Structure.CreateCache();
            RobotSelection barSel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            foreach (Bar bhomBar in bhomBars)
            {
                int barNum = System.Convert.ToInt32(bhomBar.CustomData[AdapterId]);
                rcache.AddBar(barNum,
                              System.Convert.ToInt32(bhomBar.StartNode.CustomData[AdapterId]),
                              System.Convert.ToInt32(bhomBar.EndNode.CustomData[AdapterId]),
                              bhomBar.SectionProperty.Name,
                              bhomBar.SectionProperty.Material.Name,
                              bhomBar.OrientationAngle);
                bhomBar.CustomData[AdapterId] = barNum;
                barSel.AddText(barNum.ToString());
            }
            m_RobotApplication.Project.Structure.ApplyCache(rcache);

            return true;
        }

        /***************************************************/

        public bool Create(IEnumerable<ILoad> loads)
        {
            RobotCaseServer caseServer = m_RobotApplication.Project.Structure.Cases;
            RobotGroupServer rGroupServer = m_RobotApplication.Project.Structure.Groups;

            foreach (ILoad load in loads)
            {
                IRobotCase rCase = caseServer.Get(load.Loadcase.Number);
                RobotSimpleCase sCase = rCase as RobotSimpleCase;
                IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_DEAD);
                Convert.IRobotLoad(load, loadRecord, rGroupServer);
            }

            return true;
        }

        /***************************************************/

        public bool Create<T>(IEnumerable<BH.oM.Base.BHoMGroup<T>> groups) where T : BH.oM.Base.IObject
        {

            RobotGroupServer rGroupServer = m_RobotApplication.Project.Structure.Groups;
            foreach (BHoMGroup<T> group in groups)
            {
                IRobotObjectType rType = BH.Engine.Robot.Convert.RobotObjectType(typeof(T));
                string members = group.Elements.Select(x => int.Parse(x.CustomData[BH.Engine.Robot.Convert.AdapterID].ToString())).GeterateIdString();
                rGroupServer.Create(rType, group.Name, members);
            }

            return true;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

