using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit
{
    /// <summary>
    /// Load objects and methods
    /// </summary>
    public class Load
    {
        /// <summary>
        /// Create a panel load
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="type"></param>
        /// <param name="selString"></param>
        /// <param name="axis"></param>
        /// <param name="localAxis"></param>
        /// <param name="projected"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CreatePanelLoad(RobotApplication robot, int loadCaseId, IRobotLoadRecordType type, string selString, AxisDirection axis, bool localAxis, bool projected, double value)
        {
            IRobotLoadRecord loadRecord;
            IRobotSimpleCase loadCase = (IRobotSimpleCase)robot.Project.Structure.Cases.Get(loadCaseId);

            if (!Convert.ToBoolean(robot.Project.Structure.Cases.Exist(loadCaseId)))
                return false;

            loadRecord = loadCase.Records.Create(type);
           
            loadRecord.SetValue((int)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, Convert.ToInt32(localAxis));
            loadRecord.SetValue((int)IRobotUniformRecordValues.I_URV_PROJECTED, Convert.ToInt32(projected));

            switch (axis)
            {
                case AxisDirection.X:
                    loadRecord.SetValue((int)IRobotUniformRecordValues.I_URV_PX, value);
                    break;
                case AxisDirection.Y:
                    loadRecord.SetValue((int)IRobotUniformRecordValues.I_URV_PY, value);
                    break;
                case AxisDirection.Z:
                    loadRecord.SetValue((int)IRobotUniformRecordValues.I_URV_PZ, value);
                    break;
                default:
                    return false;
            }

            loadRecord.Objects.FromText(selString);

            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="robapp"></param>
        /// <param name="loadcase"></param>
        /// <param name="loads"></param>
        public void GetLoads(RobotToolkit.App robapp, BHoM.Structural.Loads.Loadcase loadcase, out List<BHoM.Structural.Loads.AreaUniformalyDistributedLoad> loads)
        {
            RobotApplication robot = robapp.RobApp;
            IRobotCase lCase = robot.Project.Structure.Cases.Get(loadcase.Number);
            loads = new List<BHoM.Structural.Loads.AreaUniformalyDistributedLoad>();
            if (lCase.Type == IRobotCaseType.I_CT_SIMPLE || loadcase.Number < 0)
            {
                IRobotSimpleCase sCase = (lCase as IRobotSimpleCase);

                for (int j = 1; j <= sCase.Records.Count; j++)
                {
                    IRobotLoadRecord loadRecord = sCase.Records.Get(j);
                    switch (loadRecord.Type)
                    {
                        case IRobotLoadRecordType.I_LRT_UNIFORM:
                            double Px = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_PX);
                            double Py = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_PY);
                            double Pz = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_PZ);
                            loads.Add(new BHoM.Structural.Loads.AreaUniformalyDistributedLoad());
                            break;
                    }
                }
            }
        }
    }
}
