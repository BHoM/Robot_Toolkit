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
        private List<Mesh> ReadMeshes(List<string> ids = null)
        {
            //List<List<MeshFace>> bhomMeshes = new List<List<MeshFace>>();
            //Dictionary<string, List<MeshFace>> bhomMeshesDictionary = new Dictionary<string, List<MeshFace>>();

            Dictionary<string, Mesh> bhomMeshes = new Dictionary<string, Mesh>();

            Dictionary<string, Node> bhomNodes = ReadNodesQuery().ToDictionary(x => x.CustomData[AdapterId].ToString());

            RobotResultQueryParams queryParams = m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

            RobotSelection fe_sel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_FINITE_ELEMENT);
            
            fe_sel.FromText("all");
            queryParams.ResultIds.SetSize(5);
            queryParams.ResultIds.Set(1, 564);    //Corresponds to first node number of the mesh face topology          
            queryParams.ResultIds.Set(2, 565);    //Corresponds to second node number of the mesh face topology
            queryParams.ResultIds.Set(3, 566);    //Corresponds to third node number of the mesh face topology
            queryParams.ResultIds.Set(4, 567);    //Corresponds to fourth node number of the mesh face topology (if it exists/mesh face is 4 node)
            queryParams.ResultIds.Set(5, 1252);   //Corresponds to the panel number to which the mesh face belongs

            queryParams.Selection.Set(IRobotObjectType.I_OT_NODE, fe_sel);
            queryParams.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            queryParams.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            
            RobotResultRowSet rowSet = new RobotResultRowSet();
            IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;

            while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
            {
                ret = m_RobotApplication.Kernel.Structure.Results.Query(queryParams, rowSet);

                bool isOk = rowSet.MoveFirst();
                while (isOk)
                {
                    Node node1 = new Node();
                    Node node2 = new Node();
                    Node node3 = new Node();
                    Node node4 = new Node();
                    List<Node> meshFaceNodes = new List<Node>();

                    RobotResultRow row = rowSet.CurrentRow;

                    meshFaceNodes.Add(bhomNodes.TryGetValue(row.GetValue(564), out node1));
                    meshFaceNodes.Add(bhomNodes.TryGetValue(row.GetValue(565), out node2));
                    meshFaceNodes.Add(bhomNodes.TryGetValue(row.GetValue(566), out node3));

                    if (row.IsAvailable(567))
                    {                     
                        meshFaceNodes.Add(bhomNodes.TryGetValue(row.GetValue(567), out node4));
                    }

                    MeshFace meshFace = BH.Engine.Structure.Create.MeshFace(meshFaceNodes);

                    int panelNumber = row.GetValue(1252);

                    if (bhomMeshes.ContainsKey(panelNumber.ToString()))
                    {
                        bhomMeshes[panelNumber.ToString()].MeshFaces.Add(meshFace);
                    }
                    else
                    {
                        Mesh mesh = new Mesh();
                        mesh.MeshFaces.Add(meshFace);
                        bhomMeshes.Add(panelNumber.ToString(), mesh);
                    }

                    //if (bhomMeshesDictionary.ContainsKey(panelNumber.ToString()))
                    //{
                    //    bhomMeshesDictionary[panelNumber.ToString()].Add(meshFace);
                    //}
                    //else
                    //{
                    //    bhomMeshesDictionary.Add(panelNumber.ToString(), new List<MeshFace> { meshFace });
                    //}                    
                                      
                    isOk = rowSet.MoveNext();
                }
                //foreach (string key in bhomMeshesDictionary.Keys)
                //{
                //    bhomMeshes.Add(bhomMeshesDictionary[key]);
                //}
                rowSet.Clear();                
            }            
            queryParams.Reset();
            return bhomMeshes.Values.ToList();
        }
        
        /***************************************************/

    }

}

