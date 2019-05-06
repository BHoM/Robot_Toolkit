/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System.Collections.Generic;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Loads;
using BH.oM.Physical.Materials;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected override bool UpdateObjects<T>(IEnumerable<T> objects)
        {
            bool success = true;
            success = Update(objects as dynamic);
            updateview();
            return success;
        }

        /***************************************************/

        protected bool Update(IEnumerable<IBHoMObject> bhomObjects)
        {
            return true;
        }

        /***************************************************/

        protected bool Update(IEnumerable<Node> nodes)
        {
            Dictionary<int, HashSet<string>> nodeTags = GetTypeTags(typeof(Node));
            foreach (Node node in nodes)
            {
                RobotNode robotNode = m_RobotApplication.Project.Structure.Nodes.Get(System.Convert.ToInt32(node.CustomData[AdapterId])) as RobotNode;
                if (robotNode == null)
                    return false;

                if (node.Constraint != null && !string.IsNullOrWhiteSpace(node.Constraint.Name))
                    robotNode.SetLabel(IRobotLabelType.I_LT_SUPPORT, node.Constraint.Name);
                oM.Geometry.Point position = Engine.Structure.Query.Position(node);
                robotNode.X = position.X;
                robotNode.Y = position.Y;
                robotNode.Z = position.Z;
                nodeTags[System.Convert.ToInt32(node.CustomData[AdapterId])] = node.Tags;
            }
            m_tags[typeof(Node)] = nodeTags;
            return true;
        }

        /***************************************************/

        protected bool Update(IEnumerable<Material> materials)
        {
            List<Material> matToCreate = new List<Material>();

            foreach (Material m in materials)
            {
                string match = BH.Engine.Robot.Convert.Match(m_dbMaterialNames, m);
                if (match == null)
                    matToCreate.Add(m);
            }

            bool success = true;
            success = Create(matToCreate);
            return success;
        }

        /***************************************************/

        protected bool Update(IEnumerable<ISectionProperty> sectionProperties)
        {
            List<ISectionProperty> secPropToCreate = new List<ISectionProperty>();

            foreach (ISectionProperty p in sectionProperties)
            {
                string match = BH.Engine.Robot.Convert.Match(m_dbSecPropNames, p);
                if (match == null)
                    secPropToCreate.Add(p);
            }

            bool success = true;
            success = Create(secPropToCreate);
            return success;
        }

        /***************************************************/

        protected bool Update(IEnumerable<ISurfaceProperty> property2Ds)
        {
            bool success = true;
            success = Create(property2Ds);
            return success;
        }

        /***************************************************/

        protected bool Update(IEnumerable<LinkConstraint> linkConstraints)
        {
            bool success = true;
            success = Create(linkConstraints);
            return success;
        }

        /***************************************************/

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

        /***************************************************/

        protected bool Update(IEnumerable<LoadCombination> loadCombinations)
        {
            bool success = true;
            success = Create(loadCombinations);
            return success;
        }

        /***************************************************/

        protected bool Update(IEnumerable<Constraint6DOF> nodeConst)
        {
            bool success = true;
            success = Create(nodeConst);
            return success;
        }

        /***************************************************/
    }
}
