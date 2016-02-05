using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit
{
    public class Load
    {
        public static bool CreatePanelLoad(RobotApplication robot, int loadCaseId, IRobotLoadRecordType type, string selString, LoadAxis axis, bool localAxis, bool projected, double value)
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
                case LoadAxis.X:
                    loadRecord.SetValue((int)IRobotUniformRecordValues.I_URV_PX, value);
                    break;
                case LoadAxis.Y:
                    loadRecord.SetValue((int)IRobotUniformRecordValues.I_URV_PY, value);
                    break;
                case LoadAxis.Z:
                    loadRecord.SetValue((int)IRobotUniformRecordValues.I_URV_PZ, value);
                    break;
                default:
                    return false;
            }

            loadRecord.Objects.FromText(selString);

            return true;
        }

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

        public static bool CreateBeamDistributedMomentLoad(RobotApplication robot, int loadCaseId, string selString, double value, LoadAxis loadAxis)
        {
            switch (loadAxis)
            {
                case LoadAxis.X:
                case LoadAxis.Y:
                case LoadAxis.Z:
                    return false;
                case LoadAxis.YY:
                    return CreateBeamDistributedMomentLoad(robot, loadCaseId, selString, value, 0, 0);
                case LoadAxis.XX:
                    return CreateBeamDistributedMomentLoad(robot, loadCaseId, selString, 0, value, 0);
                case LoadAxis.ZZ:
                    return CreateBeamDistributedMomentLoad(robot, loadCaseId, selString, 0, 0, value);
                default:
                    return false;
            }


        }

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

        public static bool CreateBeamUDL(RobotApplication robot, int loadCaseId, string selString, double value, LoadAxis loadAxis, bool projected, bool local)
        {
            switch (loadAxis)
            {
                case LoadAxis.X:
                    return CreateBeamUDL(robot, loadCaseId, selString, value, 0, 0, projected, local);
                case LoadAxis.Y:
                    return CreateBeamUDL(robot, loadCaseId, selString, 0, value, 0, projected, local);
                case LoadAxis.Z:
                    return CreateBeamUDL(robot, loadCaseId, selString, 0, 0, value, projected, local);
                case LoadAxis.YY:
                case LoadAxis.XX:
                case LoadAxis.ZZ:
                default:
                    return false;
            }

        }

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

        public static bool CreateBeamPatchLoad(RobotApplication robot, int loadCaseId, string selString, LoadAxis loadAxis, double value1, double pos1,
            double value2, double pos2, bool normalised, bool projected, bool local)
        {
            switch (loadAxis)
            {
                case LoadAxis.X:
                    return CreateBeamPatchLoad(robot, loadCaseId, selString, value1, 0, 0, pos1, value2, 0, 0, pos2, normalised, projected, local);
                case LoadAxis.Y:
                    return CreateBeamPatchLoad(robot, loadCaseId, selString, 0, value1, 0, pos1, 0, value2, 0, pos2, normalised, projected, local);
                case LoadAxis.Z:
                    return CreateBeamPatchLoad(robot, loadCaseId, selString, 0, 0, value1, pos1, 0, 0, value2, pos2, normalised, projected, local);
                case LoadAxis.XX:
                case LoadAxis.YY:
                case LoadAxis.ZZ:
                default:
                    return false;
            }

        }

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
        
        public static bool CreateBeamPointLoad(RobotApplication robot, int loadCaseId, string selString, double pos,
            double value, LoadAxis loadAxis, bool normalised, bool local, bool generateCalcNode)
        {
            switch (loadAxis)
            {
                case LoadAxis.X:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, value, 0, 0, 0, 0, 0,
                        normalised, local, generateCalcNode);
                case LoadAxis.Y:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, 0, value, 0, 0, 0, 0,
                        normalised, local, generateCalcNode);
                case LoadAxis.Z:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, 0, 0, value, 0, 0, 0,
                        normalised, local, generateCalcNode);
                case LoadAxis.XX:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, 0, 0, 0, value, 0, 0,
                        normalised, local, generateCalcNode);
                case LoadAxis.YY:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, 0, 0, 0, 0, value, 0,
                        normalised, local, generateCalcNode);
                case LoadAxis.ZZ:
                    return CreateBeamPointLoad(robot, loadCaseId, selString, pos, 0, 0, 0, 0, 0, value,
                        normalised, local, generateCalcNode);
                default:
                    return false;
            }

        }

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

        public static bool CreateNodalLoad(RobotApplication robot, int loadCaseId, string selString, LoadAxis axis, double value)
        {
            double fx = axis == LoadAxis.X ? value : 0;
            double fy = axis == LoadAxis.Y ? value : 0;
            double fz = axis == LoadAxis.Z ? value : 0;
            double mx = axis == LoadAxis.XX ? value : 0;
            double my = axis == LoadAxis.YY ? value : 0;
            double mz = axis == LoadAxis.ZZ ? value : 0;

            return CreateNodalLoad(robot, loadCaseId, selString, fx, fy, fz, mx, my, mz);
        }

    }
}
