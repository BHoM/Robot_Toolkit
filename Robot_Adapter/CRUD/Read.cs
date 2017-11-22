using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.oM.Structural.Properties;
using BH.Adapter;
using BH.oM.Base;
using BH.oM.Materials;
using BH.Adapter.Queries;
using BH.oM.Structural.Design;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter : BHoMAdapter
    {         
        
        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/

        protected override IEnumerable<BHoMObject> Read(Type type, IList indices = null)
        {
            if (type == typeof(Node))
               return  (this.UseNodeQueryMethod)? ReadNodesQuery() : ReadNodes();
            if (type == typeof(Bar))
                return (this.UseBarQueryMethod) ? ReadBarsQuery() : ReadBars();
            if (type == typeof(DesignGroup))
                return ReadDesignGroups();
            else
                return null;         
        }

        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        public List<Bar> ReadBars(List<string> ids = null)
        {
            IRobotCollection robotBars = this.RobotApplication.Project.Structure.Bars.GetAll();
            
            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodesQuery();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.Name.ToString());

            this.RobotApplication.Project.Structure.Bars.BeginMultiOperation();
            for (int i = 1; i <= robotBars.Count; i++)
            {
                RobotBar robotBar = robotBars.Get(i);
                Bar bhomBar = Robot.Convert.ToBHoMObject(robotBar as dynamic, bhomNodes as dynamic);
                bhomBar.CustomData[AdapterName] = robotBar.Name;
                bhomBars.Add(bhomBar);
            }           
            this.RobotApplication.Project.Structure.Bars.EndMultiOperation();

            return bhomBars;
        }

        //Fast query method - returns basic bar information, not full bar objects
        public List<Bar> ReadBarsQuery(List<string> ids = null)
        {
            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodesQuery();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.Name.ToString());

            RobotResultQueryParams result_params = default(RobotResultQueryParams);
            result_params = (RobotResultQueryParams)this.RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
            RobotStructure rstructure = default(RobotStructure);
            rstructure = this.RobotApplication.Project.Structure;
            RobotSelection cas_sel = default(RobotSelection);
            RobotSelection bar_sel = default(RobotSelection);
            IRobotResultQueryReturnType query_return = default(IRobotResultQueryReturnType);

            bool ok = false;
            RobotResultRow result_row = default(RobotResultRow);
            int bar_num = 0;

            int nod1 = 0;
            int nod2 = 0;

            int nod1_id = 15;
            int nod2_id = 16;

            bar_sel = this.RobotApplication.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_BAR);
            cas_sel = this.RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            try
            {
                cas_sel.FromText(this.RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
            }
            catch
            {
                this.RobotApplication.Project.Structure.Cases.CreateSimple(1, "Dead Load", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                cas_sel.FromText(this.RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
            }

            result_params.ResultIds.SetSize(5);
            result_params.ResultIds.Set(1, nod1_id);
            result_params.ResultIds.Set(2, nod2_id);
            result_params.ResultIds.Set(3, (int)IRobotExtremeValueType.I_EVT_FORCE_BAR_FX);
            result_params.ResultIds.Set(4, 269); 
            result_params.ResultIds.Set(5, 270);
            
            result_params.SetParam(IRobotResultParamType.I_RPT_BAR_RELATIVE_POINT, 0);
            result_params.Selection.Set(IRobotObjectType.I_OT_BAR, bar_sel);
            result_params.Selection.Set(IRobotObjectType.I_OT_CASE, cas_sel);
            result_params.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
            result_params.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
            RobotResultRowSet row_set = new RobotResultRowSet();

            while (!(query_return == IRobotResultQueryReturnType.I_RQRT_DONE))
            {
                query_return = rstructure.Results.Query(result_params, row_set);
                ok = row_set.MoveFirst();
                while (ok)
                {
                    result_row = row_set.CurrentRow;
                    bar_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_BAR);

                    nod1 = (int)result_row.GetValue(nod1_id);
                    nod2 = (int)result_row.GetValue(nod2_id);

                    Node startNode = null; bhomNodes.TryGetValue(nod1.ToString(), out startNode);
                    Node endNode = null; bhomNodes.TryGetValue(nod2.ToString(), out endNode);
                    Bar bhomBar = new Bar(startNode, endNode, bar_num.ToString());

                    bhomBar.SectionProperty = null;
                    //bhomBar.OrientationAngle = robotBar.Gamma * 180 / Math.PI;
                    bhomBar.Name = bar_num.ToString();

                    bhomBar.CustomData[RobotAdapter.ID] = bar_num.ToString();
                    bhomBars.Add(bhomBar);

                    ok = row_set.MoveNext();
                }
                row_set.Clear();
            }
            result_params.Reset();   
            return bhomBars;
        }

        /***************************************************/

        public List<Material> ReadMaterials(List<string> ids = null)
        {
            return null;
        }

        /***************************************************/
            
        public List<Node> ReadNodes(List<string> ids = null)
        {
            IRobotCollection robotNodes = this.RobotApplication.Project.Structure.Nodes.GetAll();
            List<Node> bhomNodes = new List<Node>();

            for (int i = 1;i<= robotNodes.Count; i++)
            {
                RobotNode robotNode = robotNodes.Get(i);
                Node bhomNode = Robot.Convert.ToBHoMObject(robotNode);
                bhomNodes.Add(bhomNode);
            }
            return bhomNodes;
        }

        //Fast query method - only returns basic node information, not full node objects
        public List<Node> ReadNodesQuery(List<string> ids = null)
        {
            List<Node> bhomNodes = new List<Node>();
           
            RobotResultQueryParams result_params = this.RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
                        
            RobotSelection nod_sel = this.RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
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
                query_return = this.RobotApplication.Project.Structure.Results.Query(result_params, row_set);
                ok = row_set.MoveFirst();
                while (ok)
                {
                    result_row = row_set.CurrentRow;
                    nod_num = (int)result_row.GetParam(IRobotResultParamType.I_RPT_NODE);
                    BH.oM.Geometry.Point point = new BH.oM.Geometry.Point((double)row_set.CurrentRow.GetValue(0),
                                                                          (double)row_set.CurrentRow.GetValue(1),
                                                                          (double)row_set.CurrentRow.GetValue(2));
                    Node bhomNode = new Node(point, nod_num.ToString());
                    bhomNode.CustomData[RobotAdapter.ID] = nod_num.ToString();
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

        /***************************************/

        public List<SectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            return null;
        }

        /***************************************************/

        public List<DesignGroup> ReadDesignGroups()
        {
            RobotApplication robot = this.RobotApplication;
            RDimServer RDServer = this.RobotApplication.Kernel.GetExtension("RDimServer");
            RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
            RDimStream RDStream = RDServer.Connection.GetStream();
            RDimGroups RDGroups = RDServer.GroupsService;
            RDimGrpProfs RDGroupProfs = RDServer.Connection.GetGrpProfs();
            List<DesignGroup> designGroupList = new List<DesignGroup>();

            for (int i = 0; i <= RDGroups.Count - 1; i++)
            {
                int designGroupNumber = RDGroups.GetUserNo(i);
                RDimGroup designGroup = RDGroups.Get(designGroupNumber);                
                DesignGroup bhomDesignGroup = new DesignGroup();
                bhomDesignGroup.Name = designGroup.Name;
                bhomDesignGroup.Number = designGroup.UsrNo;
                bhomDesignGroup.CustomData[RobotAdapter.ID] = designGroup.UsrNo;
                bhomDesignGroup.CustomData[AdapterName] = designGroup.Name;
                bhomDesignGroup.MaterialName = designGroup.Material;
                designGroup.GetMembList(RDStream);
                string test = RDStream.ReadText();
                if(RDStream.Size(IRDimStreamType.I_DST_TEXT)>0)
                    bhomDesignGroup.MemberIds = Convert.ToSelectionList(RDStream.ReadText());
                designGroupList.Add(bhomDesignGroup);
            }
            return designGroupList;
    }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

