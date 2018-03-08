using System.Collections.Generic;
using System;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using RobotOM;
using BH.Engine.Robot;
using BHEG = BH.Engine.Geometry;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        protected override bool UpdateObjects<T>(IEnumerable<T> objects)
        {
            bool success = true;
            success = Update(objects as dynamic);
            updateview();
            return success;
        }

        protected bool Update(IEnumerable<IBHoMObject> bhomObjects)
        {
            return true;
        }


        protected bool Update(IEnumerable<Node> nodes)
        {

            foreach (Node node in nodes)
            { 
                RobotNode robotNode = m_RobotApplication.Project.Structure.Nodes.Get(System.Convert.ToInt32(node.CustomData[AdapterId])) as RobotNode;
                if (robotNode == null)
                    return false;

                if (node.Constraint != null && !string.IsNullOrWhiteSpace(node.Constraint.Name))
                    robotNode.SetLabel(IRobotLabelType.I_LT_SUPPORT, node.Constraint.Name);

                robotNode.X = node.Position.X;
                robotNode.Y = node.Position.Y;
                robotNode.Z = node.Position.Z;
            }
            return true;
        }

        protected bool Update(IEnumerable<Bar> bars)
        {

            foreach (Bar bar in bars)
            {
                RobotBar robotBar = m_RobotApplication.Project.Structure.Bars.Get((int)bar.CustomData[AdapterId]) as RobotBar;
                if (robotBar == null)
                    return false;

                robotBar.StartNode = System.Convert.ToInt32(bar.StartNode.CustomData[AdapterId]);
                robotBar.EndNode = System.Convert.ToInt32(bar.EndNode.CustomData[AdapterId]);

                if (bar.SectionProperty != null && !string.IsNullOrWhiteSpace(bar.SectionProperty.Name))
                    robotBar.SetSection(bar.SectionProperty.Name, false);

                robotBar.Gamma = bar.OrientationAngle * Math.PI / 180;
                BH.Engine.Robot.Convert.SetFEAType(robotBar, bar);

            }
            return true;
        }

        protected bool Update(IEnumerable<Material> materials)
        {

            bool success = true;
            success = Create(materials);
            return success;
        }

        protected bool Update(IEnumerable<ISectionProperty> sectionProperties)
        {
            bool success = true;
            success = Create(sectionProperties);
            return success;
        }

        protected bool Update(IEnumerable<Property2D> property2Ds)
        {
            bool success = true;
            success = Create(property2Ds);
            return success;
        }

        protected bool Update(IEnumerable<LinkConstraint> linkConstraints)
        {
            bool success = true;
            success = Create(linkConstraints);
            return success;
        }

        protected bool Update(IEnumerable<Loadcase> loadCases)
        {
            bool success = true;
            foreach (Loadcase lCase in loadCases)
            {
                RobotSimpleCase robotSimpCase = m_RobotApplication.Project.Structure.Cases.Get(System.Convert.ToInt32(lCase.CustomData[AdapterId])) as RobotSimpleCase;
                int subNature;
                IRobotCaseNature rNature = BH.Engine.Robot.Convert.RobotLoadNature(lCase, out subNature);
                robotSimpCase.AnalizeType = IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR;
                robotSimpCase.Nature = rNature;
                robotSimpCase.Number = System.Convert.ToInt32(lCase.CustomData[AdapterId]);
            }
            return success;
        }

        protected bool Update(IEnumerable<LoadCombination> loadCombinations)
        {
            bool success = true;
            success = Create(loadCombinations);
            return success;
        }

        protected bool Update(IEnumerable<Constraint6DOF> nodeConst)
        {
            bool success = true;
            success = Create(nodeConst);
            return success;
        }
    }
}
