﻿using System.Collections.Generic;
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
        /**** Adapter Methods                           ****/
        /***************************************************/


        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;

            //if (typeof(T).IsAssignableFrom(typeof(Node)))
            //    success = Create(objects as IEnumerable<Node>);
            //if (typeof(T).IsAssignableFrom(typeof(Bar)))
            //    success = Create(objects as IEnumerable<Bar>);
            //if (typeof(T).IsAssignableFrom(typeof(Constraint6DOF)))
            //    success = Create(objects as IEnumerable<Constraint6DOF>);
            //foreach (T obj in objects)
            //{
            //    success &= Create(obj as dynamic);
            //}

            success = Create(objects as dynamic);
            updateview();
            return success;
        }

        //private bool Create(BH.oM.Base.IObject obj)
        //{
        //    if (obj is Node)
        //        return Create(obj);
        //    else
        //        return false;
        //}


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

        private bool Create(IEnumerable<Loadcase> loadCase)
        {
            List<Loadcase> caseList = loadCase.ToList();
            IRobotCaseServer caseServer = m_RobotApplication.Project.Structure.Cases;

            for (int i = 0; i < caseList.Count; i++)
            {
                string name = IRobotCaseNature.I_CN_SEISMIC.ToString();
            }
            return true;
        }


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
                m_RobotApplication.Project.Structure.Nodes.Get(i).SetLabel(IRobotLabelType.I_LT_SUPPORT, nodeList[i - freeNum].Constraint.Name);
            }
            return true;
        }

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
                              //"UC 305x305x97",
                              //bhomBar.SectionProperty.Name, 
                              //"STEEL",
                              bhomBar.SectionProperty.Material.Name,
                              //bhomBar.SectionProperty.Material.Name, 
                              bhomBar.OrientationAngle);
                bhomBar.CustomData[AdapterId] = barNum;
                barSel.AddText(barNum.ToString());
            }
            m_RobotApplication.Project.Structure.ApplyCache(rcache);

            return true;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

