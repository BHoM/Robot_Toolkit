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

        public static void IRobotLoad(ILoad load, RobotCaseServer caseServer)
        {
            RobotLoad(load as dynamic, caseServer);
        }

        public static void RobotLoad(GravityLoad load, RobotCaseServer caseServer)
        {
            
            IRobotCase rCase = caseServer.Get(load.Loadcase.Number);
            RobotSimpleCase sCase = rCase as RobotSimpleCase;
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_DEAD);

            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_DEAD);
            //load.Objects.Elements[0].
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_X, load.GravityDirection.X);
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Y, load.GravityDirection.Y);
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Z, load.GravityDirection.Z);
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_ENTIRE_STRUCTURE, 1);
        }
    }
}
