/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using System.Linq;
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Structure.Constraints;
using System.Collections;
using BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Node> ReadNodes(IList ids = null)
        {
            List<int> nodeIds = CheckAndGetIds<Node>(ids);
            List<Node> bhomNodes = new List<Node>();

            Dictionary<int, HashSet<string>> nodeTags = GetTypeTags(typeof(Node));
            HashSet<string> tags = new HashSet<string>();

            IRobotCollection robotNodes;
            if (nodeIds == null || nodeIds.Count == 0)
                robotNodes = m_RobotApplication.Project.Structure.Nodes.GetAll();
            else
            {
                RobotSelection nodeSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
                nodeSelection.FromText(Convert.ToRobotSelectionString(nodeIds));
                robotNodes = m_RobotApplication.Project.Structure.Nodes.GetMany(nodeSelection);
            }

            for (int i = 1; i <= robotNodes.Count; i++)
            {
                RobotNode robotNode = robotNodes.Get(i);

                if (robotNode == null)
                {
                    Engine.Base.Compute.RecordError("At least one Node failed to get extracted from the Robot API.");
                    continue;
                }

                Node bhomNode = Convert.FromRobotConstraintName(robotNode);

                if (bhomNode == null)
                {
                    Engine.Base.Compute.RecordError($"Failed convert Node with number {robotNode.Number}. This Node in not extracted from the model.");
                    continue;
                }

                SetAdapterId(bhomNode, robotNode.Number);
                if (nodeTags != null && nodeTags.TryGetValue(robotNode.Number, out tags))
                    bhomNode.Tags = tags;

                bhomNodes.Add(bhomNode);
            }
            List<string> contraintIds = bhomNodes.Select(x => x.Support?.Name).Where(x => x != null).Distinct().ToList();
            Dictionary<string, Constraint6DOF> supports = contraintIds.Count == 0 ? new Dictionary<string, Constraint6DOF>() : ReadConstraints6DOF(contraintIds).ToDictionaryDistinctCheck(x => x.Name.ToString());

            foreach (Node node in bhomNodes)
            {
                if (node.Support != null)
                {
                    Constraint6DOF constraint;
                    if (supports.TryGetValue(node.Support.Name, out constraint))
                        node.Support = constraint;
                    else
                        Engine.Base.Compute.RecordWarning($"Failed to extract the {nameof(Node.Support)} for Node {this.GetAdapterId(node)}");

                }
            }

            return bhomNodes;
        }

        /***************************************************/              

        //Fast query method - only returns basic node information, not full node objects
        private List<Node> ReadNodesQuery(List<string> ids = null)
        {
            List<Node> bhomNodes = new List<Node>();

            RobotResultQueryParams result_params = m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            RobotSelection nod_sel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            IRobotResultQueryReturnType query_return = default(IRobotResultQueryReturnType);

            nod_sel.FromText("all");
            result_params.ResultIds.SetSize(3);
            result_params.ResultIds.Set(1, 0);
            result_params.ResultIds.Set(2, 1);
            result_params.ResultIds.Set(3, 2);

            //Setting case selection to only pull the mesh faces once
            RobotSelection caseSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            try
            {
                caseSelection.FromText(m_RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
            }
            catch
            {
                m_RobotApplication.Project.Structure.Cases.CreateSimple(1, "Dead Load", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                caseSelection.FromText(m_RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
            }

            result_params.Selection.Set(IRobotObjectType.I_OT_NODE, nod_sel);
            result_params.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
            result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            query_return = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
            RobotResultRowSet row_set = new RobotResultRowSet();
            bool ok = false;

            RobotResultRow result_row = default(RobotResultRow);
            int nod_num = 0;

            while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
            {
                query_return = m_RobotApplication.Project.Structure.Results.Query(result_params, row_set);
                ok = row_set.MoveFirst();
                while (ok)
                {
                    result_row = row_set.CurrentRow;
                    nod_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_NODE);
                    if (result_row.IsAvailable(0) && result_row.IsAvailable(1) && result_row.IsAvailable(2))
                    {
                        BH.oM.Geometry.Point point = new BH.oM.Geometry.Point
                        {
                            X = (double)row_set.CurrentRow.GetValue(0),
                            Y = (double)row_set.CurrentRow.GetValue(1),
                            Z = (double)row_set.CurrentRow.GetValue(2)
                        };
                        Node bhomNode = new Node { Position = point };
                        SetAdapterId(bhomNode, nod_num);
                        bhomNodes.Add(bhomNode);
                    }
                    else
                    {
                        Engine.Base.Compute.RecordError($"Failed to extract Node with id {nod_num}.");
                    }
                    ok = row_set.MoveNext();
                }
                row_set.Clear();
            }
            result_params.Reset();
            return bhomNodes;
        }
        
        /***************************************************/

    }

}





