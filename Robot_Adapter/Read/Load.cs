using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.oM.Geometry;
using BH.oM.Base;
using BH.oM.Structural.Loads;


namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<ILoad> ReadLoads(List<string> ids = null)
        {
            List<ILoad> bhomLoads = new List<ILoad>();
            Dictionary<int, Node> bhomNodes = ReadNodes().ToDictionary(x => System.Convert.ToInt32(x.CustomData[AdapterId]));
            Dictionary<string, Loadcase> bhomLoadCases = new Dictionary<string, Loadcase>();
            List<Loadcase> lCases = ReadLoadCase();
            for (int i = 0; i < lCases.Count; i++)
            {
                if(bhomLoadCases.ContainsKey(lCases[i].Name) == false)
                    bhomLoadCases.Add(lCases[i].Name, lCases[i]);
            }

            //Dictionary<string, Loadcase> bhomLoadCases = ReadLoadCase().ToDictionary(x => x.Name);
            IRobotCaseCollection loadCollection = m_RobotApplication.Project.Structure.Cases.GetAll();

            for (int i = 1; i <= loadCollection.Count; i++)
            {
                IRobotCase lCase = loadCollection.Get(i) as IRobotCase;
                if (lCase.Type == IRobotCaseType.I_CT_SIMPLE)
                {
                    IRobotSimpleCase sCase = lCase as IRobotSimpleCase;
                    if (bhomLoadCases.ContainsKey(sCase.Name))
                    {
                        for (int j = 1; j <= sCase.Records.Count; j++)
                        {
                            IRobotLoadRecord loadRecord = sCase.Records.Get(j);
                            List<int> elementIds = BH.Engine.Robot.Convert.GetNumbersFromText(loadRecord.Objects.ToText());
                            List<Node> objects = new List<Node>();
                            for (int k = 0; k < elementIds.Count; k++)
                            {
                                if (bhomNodes.ContainsKey(elementIds[k]))
                                    objects.Add(bhomNodes[elementIds[k]]);
                            }
                            
                            switch (loadRecord.Type)
                            {
                                case IRobotLoadRecordType.I_LRT_NODE_FORCE:
                                    double fx = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_FX);
                                    double fy = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_FY);
                                    double fz = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_FZ);
                                    double mx = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_CX);
                                    double my = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_CY);
                                    double mz = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_CZ);
                                    PointForce nodeForce = new PointForce
                                    {
                                        Force = new Vector {X = fx, Y = fy,  Z = fz},
                                        Moment = new Vector { X = mx, Y = my, Z = mz },
                                        Loadcase = bhomLoadCases[sCase.Name],
                                        Objects = BH.Engine.Base.Create.BHoMGroup(objects) as BHoMGroup<Node>
                                    };
                                    bhomLoads.Add(nodeForce);
                                    break;
                            }
                        }
                    }
                }
            }
            return bhomLoads;
        }

        /***************************************************/

    }

}