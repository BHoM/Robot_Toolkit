using System.Collections.Generic;
using RobotOM;
using BH.oM.Structure.Results;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        public static Dictionary<string, object> MeshResultParameters(MeshResultType resultType)
        {
            Dictionary<string, object> meshResultParams = new Dictionary<string, object>();
            List<int> results = new List<int>();
            switch (resultType)
            {
                case MeshResultType.Stresses:
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SYY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SXY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_TXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_TYY);
                    results.Add((int)IRobotFeResultType.I_FRT_PRINCIPAL_S1);
                    results.Add((int)IRobotFeResultType.I_FRT_PRINCIPAL_S1_2);
                    results.Add((int)IRobotFeResultType.I_FRT_PRINCIPAL_S2);
                    break;

                case MeshResultType.Forces:
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NYY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NXY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MYY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MXY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_QXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_QYY);
                    break;

                case MeshResultType.VonMises:
                    results.Add((int)IRobotFeResultType.I_FRT_COMPLEX_S_MISES);
                    results.Add((int)IRobotFeResultType.I_FRT_COMPLEX_N_MISES);
                    results.Add((int)IRobotFeResultType.I_FRT_COMPLEX_M_MISES);
                    break;                    
            }
            meshResultParams.Add("ResultsToInclude", results);
            return meshResultParams;
        }
    }
}