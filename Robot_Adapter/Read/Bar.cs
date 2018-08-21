using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.Engine.Serialiser;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Base;
using BH.oM.Common.Materials;
using BH.oM.Structural.Design;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<Bar> ReadBars(List<string> ids = null)
        {
            IRobotCollection robotBars = m_RobotApplication.Project.Structure.Bars.GetAll();

            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.CustomData[AdapterId].ToString());
            Dictionary<string, BarRelease> bhombarReleases = ReadBarRelease().ToDictionary(x => x.Name.ToString());
            Dictionary<string, ISectionProperty> bhomSections = ReadSectionProperties().ToDictionary(x => x.Name.ToString());
            Dictionary<string, Material> bhomMaterial = ReadMaterial().ToDictionary(x => x.Name.ToString());
            Dictionary<string, FramingElementDesignProperties> bhomFramEleDesProps = ReadFramingElementDesignProperties().ToDictionary(x => x.Name.ToString());
            Dictionary<int, HashSet<string>> barTags = GetTypeTags(typeof(Bar));
            HashSet<string> tags = new HashSet<string>();

            m_RobotApplication.Project.Structure.Bars.BeginMultiOperation();
            if (ids == null)
            {
                for (int i = 1; i <= robotBars.Count; i++)
                {
                    RobotBar robotBar = robotBars.Get(i);
                    Bar bhomBar = BH.Engine.Robot.Convert.ToBHoMObject( robotBar, 
                                                                        bhomNodes, 
                                                                        bhomSections, 
                                                                        bhomMaterial, 
                                                                        bhombarReleases,
                                                                        bhomFramEleDesProps);
                    bhomBar.CustomData[AdapterId] = robotBar.Number;
                    if (barTags != null && !barTags.TryGetValue(robotBar.Number, out tags))
                        bhomBar.Tags = tags;
                    bhomBars.Add(bhomBar);
                }
            }
            else if (ids != null && ids.Count > 0)
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    RobotBar robotBar = m_RobotApplication.Project.Structure.Bars.Get(System.Convert.ToInt32(ids[i])) as RobotBar;
                    Bar bhomBar = BH.Engine.Robot.Convert.ToBHoMObject( robotBar, 
                                                                        bhomNodes, 
                                                                        bhomSections, 
                                                                        bhomMaterial,
                                                                        bhombarReleases,
                                                                        bhomFramEleDesProps);
                    bhomBar.CustomData[AdapterId] = robotBar.Number;
                    if (barTags != null && !barTags.TryGetValue(robotBar.Number, out tags))
                        bhomBar.Tags = tags;
                    bhomBars.Add(bhomBar);
                }
            }
            m_RobotApplication.Project.Structure.Bars.EndMultiOperation();

            return bhomBars;
        }

        /***************************************************/        

        //Fast query method - returns basic bar information, not full bar objects
        private List<Bar> ReadBarsQuery(List<string> ids = null)
        {
            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodesQuery();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.Name.ToString());

            RobotResultQueryParams result_params = default(RobotResultQueryParams);
            result_params = (RobotResultQueryParams)m_RobotApplication.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);
            RobotStructure rstructure = default(RobotStructure);
            rstructure = m_RobotApplication.Project.Structure;
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

            bar_sel = m_RobotApplication.Project.Structure.Selections.CreateFull(IRobotObjectType.I_OT_BAR);
            cas_sel = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            try
            {
                cas_sel.FromText(m_RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
            }
            catch
            {
                m_RobotApplication.Project.Structure.Cases.CreateSimple(1, "Dead Load", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                cas_sel.FromText(m_RobotApplication.Project.Structure.Cases.Get(1).Number.ToString());
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
                    Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode, Name = bar_num.ToString() };

                    bhomBar.SectionProperty = null;
                    //bhomBar.OrientationAngle = robotBar.Gamma * 180 / Math.PI;
                    bhomBar.Name = bar_num.ToString();

                    bhomBar.CustomData[AdapterId] = bar_num.ToString();
                    bhomBars.Add(bhomBar);

                    ok = row_set.MoveNext();
                }
                row_set.Clear();
            }
            result_params.Reset();
            return bhomBars;
        }

        /***************************************************/
 
    }

}

