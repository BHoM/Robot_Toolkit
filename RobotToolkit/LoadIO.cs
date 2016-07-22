using BHoM.Structural.Loads;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotToolkit
{
    public class LoadIO
    {
        public static bool SetLoads(RobotApplication robot, List<ILoad> loads)
        {
            loads.Sort(delegate (ILoad l1, ILoad l2)
            {
                return l1.Loadcase.Name.CompareTo(l2.Loadcase.Name);
            });

            string currentLoadcase = "";
            IRobotCase lCase = null;
            IRobotSimpleCase sCase = null;
            IRobotLoadRecord loadRecord = null;
            foreach (ILoad load in loads)
            {
                if (currentLoadcase != load.Loadcase.Name)
                {
                    currentLoadcase = load.Loadcase.Name;
                    lCase = robot.Project.Structure.Cases.Get(int.Parse(load.Loadcase.Name));
                    sCase = (lCase as IRobotSimpleCase);
                }

                if (sCase != null)
                {
                    switch (load.LoadType)
                    {
                        case LoadType.PointForce:
                            PointForce pL = load as PointForce;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_FORCE);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(pL.Objects));
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FX, pL.Force.X);
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FY, pL.Force.Y);
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FZ, pL.Force.Z);
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CX, pL.Moment.X);
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CY, pL.Moment.Y);
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CZ, pL.Moment.Z);
                            break;
                        case LoadType.BarUniformLoad:
                            BarUniformlyDistributedLoad lL = load as BarUniformlyDistributedLoad;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(lL.Objects));
                            loadRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PX, lL.ForceVector.X);
                            loadRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PY, lL.ForceVector.Y);
                            loadRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PZ, lL.ForceVector.Z);
                            break;
                        case LoadType.AreaUniformLoad:
                            AreaUniformalyDistributedLoad lA = load as AreaUniformalyDistributedLoad;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_UNIFORM);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(lA.Objects));
                            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PX, lA.Pressure.X);
                            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PY, lA.Pressure.Y);
                            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PZ, lA.Pressure.Z);
                            break;
                        case LoadType.AreaTemperature:
                            break;
                        case LoadType.BarTemperature:
                            BarTemperatureLoad tL = load as BarTemperatureLoad;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_THERMAL);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(tL.Objects));
                            loadRecord.SetValue((short)IRobotThermalRecordValues.I_TRV_T_1, tL.TemperatureChange.X);
                            loadRecord.SetValue((short)IRobotThermalRecordValues.I_TRV_T_2, tL.TemperatureChange.Y);
                            loadRecord.SetValue((short)IRobotThermalRecordValues.I_TRV_T_3, tL.TemperatureChange.Z);
                            break;
                        case LoadType.BarPointLoad:
                            BarPointLoad bPL = load as BarPointLoad;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_FORCE_CONCENTRATED);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(bPL.Objects));
                            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FX, bPL.ForceVector.X);
                            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FY, bPL.ForceVector.Y);
                            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FZ, bPL.ForceVector.Z);
                            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CX, bPL.MomentVector.X);
                            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CY, bPL.MomentVector.Y);
                            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CZ, bPL.MomentVector.Z);
                            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_X, bPL.DistanceFromA);
                            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_REL, 0);
                            break;
                        case LoadType.PointDisplacement:
                            PointDisplacement pD = load as PointDisplacement;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_VELOCITY);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(pD.Objects));
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UX, pD.Translation.X);
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UY, pD.Translation.Y);
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UZ, pD.Translation.Z);
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RX, pD.Rotation.X);
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RY, pD.Rotation.Y);
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RZ, pD.Rotation.Z);
                            break;
                        case LoadType.PointVelocity:
                            PointVelocity pV = load as PointVelocity;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_VELOCITY);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(pV.Objects));
                            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UX, pV.TranslationalVelocity.X);
                            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UY, pV.TranslationalVelocity.Y);
                            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UZ, pV.TranslationalVelocity.Z);
                            break;
                        case LoadType.PointAcceleration:
                            PointAcceleration pA = load as PointAcceleration;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_ACCELERATION);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(pA.Objects));
                            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UX, pA.TranslationalAcceleration.X);
                            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UY, pA.TranslationalAcceleration.Y);
                            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UZ, pA.TranslationalAcceleration.Z);
                            break;
                        case LoadType.Geometrical:
                            GeometricalLoad gL = load as GeometricalLoad;
                            break;
                        default:
                            break;
                    }
                }
            }
            return true;
        }

    }
}
