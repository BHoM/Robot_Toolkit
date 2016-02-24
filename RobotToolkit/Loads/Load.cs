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
        /// Get Load Objects for uniform area loads
        /// </summary>
        /// <param name="robapp"></param>
        /// <param name="loadcase"></param>
        /// <param name="loads"></param>
        public static void GetLoads(BHoM.Structural.Loads.Loadcase loadcase, out List<BHoM.Structural.Loads.AreaUniformalyDistributedLoad> loads)
        {
            RobotApplication robot = new RobotApplication();
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
                            BHoM.Structural.Loads.AreaUniformalyDistributedLoad load = new BHoM.Structural.Loads.AreaUniformalyDistributedLoad(Px, Py, Pz);
                            load.ObjectNumbers = RobotToolkit.Utilities.Utils.GetNumbersFromText(loadRecord.Objects.ToText());
                            loads.Add(load);
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Create a thermal load on a beam
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool CreateThermalBeamLoad(RobotApplication robot, int loadCaseId, string selString, double val)
        {
            IRobotLoadRecord loadRecord;
            IRobotSimpleCase loadCase = (IRobotSimpleCase)robot.Project.Structure.Cases.Get(loadCaseId);

            if (!Convert.ToBoolean(robot.Project.Structure.Cases.Exist(loadCaseId)))
                return false;

            loadRecord = loadCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_THERMAL);


            loadRecord.SetValue((int)IRobotThermalRecordValues.I_TRV_T_1, val);
            loadRecord.SetValue((int)IRobotThermalRecordValues.I_TRV_T_2, val);
            loadRecord.SetValue((int)IRobotThermalRecordValues.I_TRV_T_3, val);

            loadRecord.Objects.FromText(selString);

            return true;
        }

        /// <summary>
        /// Create a gravity load
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="valX"></param>
        /// <param name="valY"></param>
        /// <param name="valZ"></param>
        /// <returns></returns>
        public static bool CreateGravityLoad(RobotApplication robot, int loadCaseId, string selString, double valX, double valY, double valZ)
        {
            IRobotLoadRecord loadRecord;
            IRobotSimpleCase loadCase = (IRobotSimpleCase)robot.Project.Structure.Cases.Get(loadCaseId);

            if (!Convert.ToBoolean(robot.Project.Structure.Cases.Exist(loadCaseId)))
                return false;

            loadRecord = loadCase.Records.Create(IRobotLoadRecordType.I_LRT_DEAD);


            if (selString.Trim().ToUpper() == "ALL")
                loadRecord.SetValue((int)IRobotDeadRecordValues.I_DRV_ENTIRE_STRUCTURE, 1);

            loadRecord.SetValue((int)IRobotDeadRecordValues.I_DRV_X, valX);
            loadRecord.SetValue((int)IRobotDeadRecordValues.I_DRV_Y, valY);
            loadRecord.SetValue((int)IRobotDeadRecordValues.I_DRV_Z, valZ);

            loadRecord.Objects.FromText(selString);

            return true;
        }

        /// <summary>
        /// Create a distributed moment on a beam
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        /// <param name="mz"></param>
        /// <returns></returns>
        public static bool CreateBeamDistributedMomentLoad(RobotApplication robot, int loadCaseId, string selString, double mx, double my, double mz)
        {
            IRobotLoadRecord loadRecord;
            IRobotSimpleCase loadCase = (IRobotSimpleCase)robot.Project.Structure.Cases.Get(loadCaseId);

            if (!Convert.ToBoolean(robot.Project.Structure.Cases.Exist(loadCaseId)))
                return false;

            loadRecord = loadCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_MOMENT_DISTRIBUTED);

            loadRecord.SetValue((int)IRobotBarMomentDistributedRecordValues.I_BMDRV_MX, mx);
            loadRecord.SetValue((int)IRobotBarMomentDistributedRecordValues.I_BMDRV_MY, my);
            loadRecord.SetValue((int)IRobotBarMomentDistributedRecordValues.I_BMDRV_MZ, mz);

            loadRecord.Objects.FromText(selString);

            return true;
        }

        /// <summary>
        /// Create a beam distributed moment load
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="value"></param>
        /// <param name="AxisDirection"></param>
        /// <returns></returns>
        public static bool CreateBeamDistributedMomentLoad(RobotApplication robot, int loadCaseId, string selString, double value, AxisDirection AxisDirection)
        {
            switch (AxisDirection)
            {
                case AxisDirection.X:
                case AxisDirection.Y:
                case AxisDirection.Z:
                    return false;
                case AxisDirection.YY:
                    return CreateBeamDistributedMomentLoad(robot, loadCaseId, selString, value, 0, 0);
                case AxisDirection.XX:
                    return CreateBeamDistributedMomentLoad(robot, loadCaseId, selString, 0, value, 0);
                case AxisDirection.ZZ:
                    return CreateBeamDistributedMomentLoad(robot, loadCaseId, selString, 0, 0, value);
                default:
                    return false;
            }


        }

        /// <summary>
        /// Create a uniformly distributed load on a beam
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="pz"></param>
        /// <param name="projected"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public static bool CreateBeamUDL(RobotApplication robot, int loadCaseId, string selString, double px, double py, double pz, bool projected, bool local)
        {
            IRobotLoadRecord loadRecord;
            IRobotSimpleCase loadCase = (IRobotSimpleCase)robot.Project.Structure.Cases.Get(loadCaseId);

            if (!Convert.ToBoolean(robot.Project.Structure.Cases.Exist(loadCaseId)))
                return false;

            loadRecord = loadCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM);

            loadRecord.SetValue((int)IRobotBarUniformRecordValues.I_BURV_PX, px);
            loadRecord.SetValue((int)IRobotBarUniformRecordValues.I_BURV_PY, py);
            loadRecord.SetValue((int)IRobotBarUniformRecordValues.I_BURV_PZ, pz);

            loadRecord.SetValue((int)IRobotBarUniformRecordValues.I_BURV_PROJECTION, Convert.ToInt32(projected));

            loadRecord.SetValue((int)IRobotBarUniformRecordValues.I_BURV_LOCAL, Convert.ToInt32(local));

            loadRecord.Objects.FromText(selString);

            return true;
        }

        /// <summary>
        /// Create a uniformly distributed load on a beam
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="value"></param>
        /// <param name="AxisDirection"></param>
        /// <param name="projected"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public static bool CreateBeamUDL(RobotApplication robot, int loadCaseId, string selString, double value, AxisDirection AxisDirection, bool projected, bool local)
        {
            switch (AxisDirection)
            {
                case AxisDirection.X:
                    return CreateBeamUDL(robot, loadCaseId, selString, value, 0, 0, projected, local);
                case AxisDirection.Y:
                    return CreateBeamUDL(robot, loadCaseId, selString, 0, value, 0, projected, local);
                case AxisDirection.Z:
                    return CreateBeamUDL(robot, loadCaseId, selString, 0, 0, value, projected, local);
                case AxisDirection.YY:
                case AxisDirection.XX:
                case AxisDirection.ZZ:
                default:
                    return false;
            }

        }

        /// <summary>
        /// Create a patch load on a beam
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="px1"></param>
        /// <param name="py1"></param>
        /// <param name="pz1"></param>
        /// <param name="pos1"></param>
        /// <param name="px2"></param>
        /// <param name="py2"></param>
        /// <param name="pz2"></param>
        /// <param name="pos2"></param>
        /// <param name="normalised"></param>
        /// <param name="projected"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public static bool CreateBeamPatchLoad(RobotApplication robot, int loadCaseId, string selString, double px1, double py1, double pz1, double pos1,
            double px2, double py2, double pz2, double pos2, bool normalised, bool projected, bool local)
        {
            IRobotLoadRecord loadRecord;
            IRobotSimpleCase loadCase = (IRobotSimpleCase)robot.Project.Structure.Cases.Get(loadCaseId);

            if (!Convert.ToBoolean(robot.Project.Structure.Cases.Exist(loadCaseId)))
                return false;

            loadRecord = loadCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_TRAPEZOIDALE);

            loadRecord.SetValue((int)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX1, px1);
            loadRecord.SetValue((int)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY1, py1);
            loadRecord.SetValue((int)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ1, pz1);
            loadRecord.SetValue((int)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX2, px2);
            loadRecord.SetValue((int)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY2, py2);
            loadRecord.SetValue((int)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ2, pz2);

            loadRecord.SetValue((int)IRobotBarTrapezoidaleRecordValues.I_BTRV_X1, pos1);
            loadRecord.SetValue((int)IRobotBarTrapezoidaleRecordValues.I_BTRV_X2, pos2);



            loadRecord.SetValue((int)IRobotBarTrapezoidaleRecordValues.I_BTRV_PROJECTION, Convert.ToInt32(projected));
            loadRecord.SetValue((int)IRobotBarTrapezoidaleRecordValues.I_BTRV_LOCAL, Convert.ToInt32(local));

            loadRecord.SetValue((int)IRobotBarTrapezoidaleRecordValues.I_BTRV_RELATIVE, Convert.ToInt32(normalised));

            loadRecord.Objects.FromText(selString);

            return true;
        }

        /// <summary>
        /// Create a patch load on a beam
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="AxisDirection"></param>
        /// <param name="value1"></param>
        /// <param name="pos1"></param>
        /// <param name="value2"></param>
        /// <param name="pos2"></param>
        /// <param name="normalised"></param>
        /// <param name="projected"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public static bool CreateBeamPatchLoad(RobotApplication robot, int loadCaseId, string selString, AxisDirection AxisDirection, double value1, double pos1,
            double value2, double pos2, bool normalised, bool projected, bool local)
        {
            switch (AxisDirection)
            {
                case AxisDirection.X:
                    return CreateBeamPatchLoad(robot, loadCaseId, selString, value1, 0, 0, pos1, value2, 0, 0, pos2, normalised, projected, local);
                case AxisDirection.Y:
                    return CreateBeamPatchLoad(robot, loadCaseId, selString, 0, value1, 0, pos1, 0, value2, 0, pos2, normalised, projected, local);
                case AxisDirection.Z:
                    return CreateBeamPatchLoad(robot, loadCaseId, selString, 0, 0, value1, pos1, 0, 0, value2, pos2, normalised, projected, local);
                case AxisDirection.XX:
                case AxisDirection.YY:
                case AxisDirection.ZZ:
                default:
                    return false;
            }

        }

        /// <summary>
        /// Create a point load on a beam
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="pos"></param>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="fz"></param>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        /// <param name="mz"></param>
        /// <param name="normalised"></param>
        /// <param name="local"></param>
        /// <param name="generateCalcNode"></param>
        /// <returns></returns>
        public static bool CreateBeamPointLoad(RobotApplication robot, int loadCaseId, string selString, double pos, double fx, double fy, double fz,
            double mx, double my, double mz, bool normalised, bool local, bool generateCalcNode)
        {
            IRobotLoadRecord loadRecord;
            IRobotSimpleCase loadCase = (IRobotSimpleCase)robot.Project.Structure.Cases.Get(loadCaseId);

            if (!Convert.ToBoolean(robot.Project.Structure.Cases.Exist(loadCaseId)))
                return false;

            loadRecord = loadCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_FORCE_CONCENTRATED);

            loadRecord.SetValue((int)IRobotBarForceConcentrateRecordValues.I_BFCRV_FX, fx);
            loadRecord.SetValue((int)IRobotBarForceConcentrateRecordValues.I_BFCRV_FX, fy);
            loadRecord.SetValue((int)IRobotBarForceConcentrateRecordValues.I_BFCRV_FX, fz);

            loadRecord.SetValue((int)IRobotBarForceConcentrateRecordValues.I_BFCRV_CX, mx);
            loadRecord.SetValue((int)IRobotBarForceConcentrateRecordValues.I_BFCRV_CX, my);
            loadRecord.SetValue((int)IRobotBarForceConcentrateRecordValues.I_BFCRV_CX, mz);

            loadRecord.SetValue((int)IRobotBarForceConcentrateRecordValues.I_BFCRV_X, pos);
            loadRecord.SetValue((int)IRobotBarForceConcentrateRecordValues.I_BFCRV_GENERATE_CALC_NODE, Convert.ToInt32(generateCalcNode));


            loadRecord.SetValue((int)IRobotBarForceConcentrateRecordValues.I_BFCRV_REL, Convert.ToInt32(normalised));
            loadRecord.SetValue((int)IRobotBarForceConcentrateRecordValues.I_BFCRV_LOC, Convert.ToInt32(local));


            loadRecord.Objects.FromText(selString);

            return true;
        }

        /// <summary>
        /// Create a point load on a beam
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <param name="AxisDirection"></param>
        /// <param name="normalised"></param>
        /// <param name="local"></param>
        /// <param name="generateCalcNode"></param>
        /// <returns></returns>
        public static bool CreateBeamPointLoad(RobotApplication robot, int loadCaseId, string selString, double pos,
            double value, AxisDirection AxisDirection, bool normalised, bool local, bool generateCalcNode)
        {
            switch (AxisDirection)
            {
                case AxisDirection.X:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, value, 0, 0, 0, 0, 0,
                        normalised, local, generateCalcNode);
                case AxisDirection.Y:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, 0, value, 0, 0, 0, 0,
                        normalised, local, generateCalcNode);
                case AxisDirection.Z:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, 0, 0, value, 0, 0, 0,
                        normalised, local, generateCalcNode);
                case AxisDirection.XX:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, 0, 0, 0, value, 0, 0,
                        normalised, local, generateCalcNode);
                case AxisDirection.YY:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, 0, 0, 0, 0, value, 0,
                        normalised, local, generateCalcNode);
                case AxisDirection.ZZ:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, 0, 0, 0, 0, 0, value,
                        normalised, local, generateCalcNode);
                default:
                    return false;
            }

        }

        /// <summary>
        /// Create a nodal load
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="fz"></param>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        /// <param name="mz"></param>
        /// <returns></returns>
        public static bool CreateNodalLoad(RobotApplication robot, int loadCaseId, string selString, double fx, double fy, double fz,
            double mx, double my, double mz)
        {
            IRobotLoadRecord loadRecord;
            IRobotSimpleCase loadCase = (IRobotSimpleCase)robot.Project.Structure.Cases.Get(loadCaseId);

            if (!Convert.ToBoolean(robot.Project.Structure.Cases.Exist(loadCaseId)))
                return false;

            loadRecord = loadCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_FORCE);

            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_FX, fx);
            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_FY, fy);
            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_FZ, fz);


            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_CX, mx);
            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_CY, my);
            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_CZ, mz);


            loadRecord.Objects.FromText(selString);

            return true;
        }

        /// <summary>
        /// Create a nodal load
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="loadCaseId"></param>
        /// <param name="selString"></param>
        /// <param name="axis"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CreateNodalLoad(RobotApplication robot, int loadCaseId, string selString, AxisDirection axis, double value)
        {
            double fx = axis == AxisDirection.X ? value : 0;
            double fy = axis == AxisDirection.Y ? value : 0;
            double fz = axis == AxisDirection.Z ? value : 0;
            double mx = axis == AxisDirection.XX ? value : 0;
            double my = axis == AxisDirection.YY ? value : 0;
            double mz = axis == AxisDirection.ZZ ? value : 0;

            return CreateNodalLoad(robot, loadCaseId, selString, fx, fy, fz, mx, my, mz);
        }

    }
}