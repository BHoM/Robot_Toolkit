using BH.oM.Geometry;
using BH.oM.Common.Materials;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Loads;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        public static IRobotCaseNature RobotLoadNature(Loadcase lCase, out int subNature)
        {
            subNature = -1;
            switch (lCase.Nature)
            {
                case LoadNature.Dead:
                case LoadNature.SuperDead:
                    subNature = 1;
                    return IRobotCaseNature.I_CN_PERMANENT;
                case LoadNature.Live:
                    subNature = 4;
                    return IRobotCaseNature.I_CN_EXPLOATATION;
                case LoadNature.Accidental:
                    subNature = 15;
                    return IRobotCaseNature.I_CN_ACCIDENTAL;
                case LoadNature.Snow:
                    subNature = 10;
                    return IRobotCaseNature.I_CN_SNOW;
                case LoadNature.Wind:
                    subNature = 13;
                    return IRobotCaseNature.I_CN_WIND;
                case LoadNature.Temperature:
                    subNature = 14;
                    return IRobotCaseNature.I_CN_TEMPERATURE;
                case LoadNature.Seismic:
                    return IRobotCaseNature.I_CN_SEISMIC;
                default:
                    return IRobotCaseNature.I_CN_PERMANENT;
            }
        }

        public static void IRobotLoad(this ILoad load, IRobotLoadRecord loadRecord, RobotGroupServer rGroupServer)
        {
            RobotLoad(load as dynamic, loadRecord, rGroupServer);
        }

        public static void RobotLoad(this GravityLoad load, IRobotLoadRecord loadRecord, RobotGroupServer rGroupServer)
        {
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_X, load.GravityDirection.X);
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Y, load.GravityDirection.Y);
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Z, load.GravityDirection.Z);
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_ENTIRE_STRUCTURE, 1);
        }

        public static void RobotLoad(this PointForce load, IRobotLoadRecord loadRecord, RobotGroupServer rGroupServer)
        {
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FX, load.Force.X);
            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FY, load.Force.Y);
            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FZ, load.Force.Z);
            if (load.Moment != null)
            {
                loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CX, load.Moment.X);
                loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CY, load.Moment.Y);
                loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CZ, load.Moment.Z);
            }
        }

        public static void RobotLoad(this BarUniformlyDistributedLoad load, IRobotLoadRecord loadRecord, RobotGroupServer rGroupServer)
        {
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PX, load.Force.X);
            loadRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PY, load.Force.Y);
            loadRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PZ, load.Force.Z);
        }

    }
}
