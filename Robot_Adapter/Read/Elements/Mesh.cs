using System.Collections.Generic;
using BH.oM.Structure.Elements;
using RobotOM;
using System.Linq;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/
                     

        //Fast query method - only returns basic node information, not full node objects
        private List<FEMesh> ReadMeshes(List<string> ids = null)
        {
            SortedDictionary<int, FEMesh> bhomMeshes = new SortedDictionary<int, FEMesh>();

            List<FEMeshFace> meshFaces = new List<FEMeshFace>();

            Dictionary<int, Node> bhomNodes = ReadNodesQuery().ToDictionary(x => System.Convert.ToInt32(x.CustomData[AdapterId]));

            Dictionary<int, List<Node>> meshNodes_allMeshes = new Dictionary<int, List<Node>>();

            Dictionary<int, List<int>> meshNodeIds_allMeshes = new Dictionary<int, List<int>>();

            RobotResultQueryParams queryParams = m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            RobotSelection fe_sel = m_RobotApplication.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_FINITE_ELEMENT);
            
            queryParams.ResultIds.SetSize(5);
            queryParams.ResultIds.Set(1, 564);    //Corresponds to first node number of the mesh face topology          
            queryParams.ResultIds.Set(2, 565);    //Corresponds to second node number of the mesh face topology
            queryParams.ResultIds.Set(3, 566);    //Corresponds to third node number of the mesh face topology
            queryParams.ResultIds.Set(4, 567);    //Corresponds to fourth node number of the mesh face topology (if it exists/mesh face is 4 node)
            queryParams.ResultIds.Set(5, 1252);   //Corresponds to the panel number to which the mesh face belongs

            queryParams.Selection.Set(IRobotObjectType.I_OT_FINITE_ELEMENT, fe_sel);
            queryParams.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            queryParams.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            queryParams.SetParam(IRobotResultParamType.I_RPT_SMOOTHING, IRobotFeResultSmoothing.I_FRS_IN_ELEMENT_CENTER);
            
            RobotResultRowSet rowSet = new RobotResultRowSet();
            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;

            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = m_RobotApplication.Kernel.Structure.Results.Query(queryParams, rowSet);

                bool isOk = rowSet.MoveFirst();
                while (isOk)
                {
                    RobotResultRow row = rowSet.CurrentRow;

                    int panelNumber = System.Convert.ToInt32(row.GetValue(1252));

                    List<int> meshNodeIds = (meshNodeIds_allMeshes.ContainsKey(panelNumber)? meshNodeIds_allMeshes[panelNumber] : new List<int>());
                    List<int> meshFaceNodeIds = new List<int>();

                    if (!meshNodeIds.Contains(System.Convert.ToInt32(row.GetValue(564))))
                        meshNodeIds.Add(System.Convert.ToInt32(row.GetValue(564)));
                    meshFaceNodeIds.Add(meshNodeIds.IndexOf(System.Convert.ToInt32(row.GetValue(564))));

                    if (!meshNodeIds.Contains(System.Convert.ToInt32(row.GetValue(565))))
                        meshNodeIds.Add(System.Convert.ToInt32(row.GetValue(565)));
                    meshFaceNodeIds.Add(meshNodeIds.IndexOf(System.Convert.ToInt32(row.GetValue(565))));

                    if (!meshNodeIds.Contains(System.Convert.ToInt32(row.GetValue(566))))
                        meshNodeIds.Add(System.Convert.ToInt32(row.GetValue(566)));
                    meshFaceNodeIds.Add(meshNodeIds.IndexOf(System.Convert.ToInt32(row.GetValue(566))));


                    if (row.IsAvailable(567))
                    {
                        if (!meshNodeIds.Contains(System.Convert.ToInt32(row.GetValue(567))))
                            meshNodeIds.Add(System.Convert.ToInt32(row.GetValue(567)));
                        meshFaceNodeIds.Add(meshNodeIds.IndexOf(System.Convert.ToInt32(row.GetValue(567))));
                    }

                    if (!meshNodeIds_allMeshes.ContainsKey(panelNumber))
                        meshNodeIds_allMeshes.Add(panelNumber, meshNodeIds);

                    FEMeshFace meshFace = new FEMeshFace();
                    meshFace.NodeListIndices = meshFaceNodeIds;

                    meshFaces.Add(meshFace);                    

                    if (bhomMeshes.ContainsKey(panelNumber))
                    {
                        bhomMeshes[panelNumber].MeshFaces.Add(meshFace);
                        bhomMeshes[panelNumber].CustomData[AdapterId] = panelNumber;
                    }
                    else
                    {
                        FEMesh mesh = new FEMesh();
                        mesh.MeshFaces.Add(meshFace);
                        mesh.CustomData[AdapterId] = panelNumber;
                        bhomMeshes.Add(panelNumber, mesh);
                    }                   
                    isOk = rowSet.MoveNext();
                }
                 rowSet.Clear();       
            }            
            queryParams.Reset();
            foreach (int panelId in meshNodeIds_allMeshes.Keys)
                {
                    foreach (int nodeId in meshNodeIds_allMeshes[panelId])
                    {
                        bhomMeshes[panelId].Nodes.Add(bhomNodes[nodeId]);
                    }
                }
            return bhomMeshes.Values.ToList();
        }
        
        /***************************************************/

    }

}

