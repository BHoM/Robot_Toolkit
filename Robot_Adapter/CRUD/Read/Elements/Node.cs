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
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Structure.Constraints;
using System.Collections;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Node> ReadNodes(IList ids = null)
        {
            List<int> nodeIds = CheckAndGetIds(ids);
            List<Node> bhomNodes = new List<Node>();
            List<Constraint6DOF> constraints = ReadConstraints6DOF();
            Dictionary<int, HashSet<string>> nodeTags = GetTypeTags(typeof(Node));
            HashSet<string> tags = new HashSet<string>();

            if (nodeIds == null || nodeIds.Count == 0)
            {
                IRobotCollection robotNodes = m_RobotApplication.Project.Structure.Nodes.GetAll();
                for (int i = 1; i <= robotNodes.Count; i++)
                {
                    RobotNode robotNode = robotNodes.Get(i);
                    Node bhomNode = BH.Engine.Robot.Convert.ToBHoMObject(robotNode);
                    bhomNode.CustomData[AdapterIdName] = robotNode.Number;
                    if (nodeTags != null && nodeTags.TryGetValue(robotNode.Number, out tags))
                        bhomNode.Tags = tags;

                    bhomNodes.Add(bhomNode);
                }
            }
            else
            {
                for (int i = 0; i < nodeIds.Count; i++)
                {
                    RobotNode robotNode = m_RobotApplication.Project.Structure.Nodes.Get(System.Convert.ToInt32(nodeIds[i])) as RobotNode;
                    Node bhomNode = BH.Engine.Robot.Convert.ToBHoMObject(robotNode);
                    bhomNode.CustomData[AdapterIdName] = robotNode.Number;
                    if (nodeTags != null && nodeTags.TryGetValue(robotNode.Number, out tags))
                        bhomNode.Tags = tags;

                    bhomNodes.Add(bhomNode);
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

            result_params.Selection.Set(IRobotObjectType.I_OT_NODE, nod_sel);
            result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            query_return = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;
            RobotResultRowSet row_set = new RobotResultRowSet();
            bool ok = false;

            RobotResultRow result_row = default(RobotResultRow);
            int nod_num = 0;
            int kounta = 0;

            while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
            {
                query_return = m_RobotApplication.Project.Structure.Results.Query(result_params, row_set);
                ok = row_set.MoveFirst();
                while (ok)
                {
                    result_row = row_set.CurrentRow;
                    nod_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_NODE);
                    BH.oM.Geometry.Point point = new BH.oM.Geometry.Point
                    {
                        X = (double)row_set.CurrentRow.GetValue(0),
                        Y = (double)row_set.CurrentRow.GetValue(1),
                        Z = (double)row_set.CurrentRow.GetValue(2)
                    };
                    Node bhomNode = Engine.Structure.Create.Node(point, nod_num.ToString());
                    bhomNode.CustomData[AdapterIdName] = nod_num.ToString();
                    bhomNodes.Add(bhomNode);
                    point = null;
                    kounta++;
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

