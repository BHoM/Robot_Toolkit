using BHoM.Geometry;
using BHoM.Global;
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
                    lCase = SetCase(robot, load.Loadcase, true);
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
                            if (load is GeometricalAreaLoad)
                            {
                                GeometricalAreaLoad aL = load as GeometricalAreaLoad;
                                List<Point> points = aL.Contour.ControlPoints.ToList();

                                loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_IN_CONTOUR);
                                loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_PX1, aL.Force.X);
                                loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_PY1, aL.Force.Y);
                                loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_PZ1, aL.Force.Z);
                                loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_NPOINTS, points.Count);
                                loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_AUTO_DETECT_OBJECTS, -1);

                                RobotLoadRecordInContour iContour = loadRecord as RobotLoadRecordInContour;
                                for (int cp = 0; cp < points.Count; cp++)
                                {
                                    iContour.SetContourPoint(cp + 1, points[cp].X, points[cp].Y, points[cp].Z);
                                }
                            }
                            else if (load is GeometricalLineLoad)
                            {
                                GeometricalLineLoad lineL = load as GeometricalLineLoad;
                                loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_LINEAR_3D);
                                RobotNodeServer objServer = robot.Project.Structure.Nodes;
                                BHoM.Geometry.Point p1 = lineL.Location.StartPoint;
                                BHoM.Geometry.Point p2 = lineL.Location.EndPoint;

                                IRobotLoadRecordLinear3D l3D = loadRecord as IRobotLoadRecordLinear3D;
                                l3D.SetPoint(1, p1.X, p1.Y, p1.Z);
                                l3D.SetPoint(2, p2.X, p2.Y, p2.Z);
                                
                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PX1, lineL.ForceA.X);
                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PY1, lineL.ForceA.Y);
                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PZ1, lineL.ForceA.Z);
                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PX2, lineL.ForceB.X);
                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PY2, lineL.ForceB.Y);
                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PZ2, lineL.ForceB.Z);

                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MX1, lineL.MomentA.X);
                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MY1, lineL.MomentA.Y);
                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MZ1, lineL.MomentA.Z);
                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MX2, lineL.MomentB.X);
                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MY2, lineL.MomentB.Y);
                                loadRecord.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MZ2, lineL.MomentB.Z);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return true;
        }

        internal static IRobotCaseNature GetLoadNature(LoadNature nature)
        {
            switch (nature)
            {
                case LoadNature.Dead:
                    return IRobotCaseNature.I_CN_PERMANENT;
                case LoadNature.Live:
                    return IRobotCaseNature.I_CN_EXPLOATATION;
                case LoadNature.Seismic:
                    return IRobotCaseNature.I_CN_SEISMIC;
                case LoadNature.Snow:
                    return IRobotCaseNature.I_CN_SNOW;
                case LoadNature.Temperature:
                    return IRobotCaseNature.I_CN_TEMPERATURE;
                case LoadNature.Wind:
                    return IRobotCaseNature.I_CN_WIND;
                default:
                    return IRobotCaseNature.I_CN_PERMANENT;
            }
        }

        public static int RobotCaseNumber(RobotApplication robot, string name)
        {
            RobotCaseCollection caseCollection = robot.Project.Structure.Cases.GetAll();
            for (int i = 1; i <= caseCollection.Count; i++)
            {
                IRobotCase rCase = caseCollection.Get(i);
                if (rCase.Name == name)
                {
                    return rCase.Number;
                }
            }
            return -1;
        }

        public static IRobotCase SetCase(RobotApplication robot, ICase bhCase, bool checkExists = false)
        {
            RobotCaseServer caseServer = robot.Project.Structure.Cases;
            BHoMObject loadcase = bhCase as BHoMObject;
            int caseNum = 0;

            if (checkExists && loadcase[Utils.NUM_KEY] == null)
            {
                caseNum = RobotCaseNumber(robot, loadcase.Name);               
            }
            else if (loadcase[Utils.NUM_KEY] != null)
            {
                IRobotCase rCase = caseServer.Get(int.Parse(loadcase[Utils.NUM_KEY].ToString()));
                if (rCase != null) return rCase;
            }

            if (caseNum <= 0)
            {
                switch (bhCase.CaseType)
                {
                    case CaseType.Simple:
                        BHoM.Structural.Loads.Loadcase simpleCase = bhCase as BHoM.Structural.Loads.Loadcase;
                        if (simpleCase[Utils.NUM_KEY] == null)
                        {
                            caseNum = caseServer.FreeNumber;
                            simpleCase.CustomData.Add(Utils.NUM_KEY, caseNum);
                        }
                        else
                        {
                            caseNum = int.Parse(simpleCase[Utils.NUM_KEY].ToString());
                        }

                        caseServer.CreateSimple(caseNum, loadcase.Name, GetLoadNature(simpleCase.Nature), IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                        break;
                    case CaseType.Combination:
                        //LoadCombination combo = loadcase as LoadCombination;
                        //RobotCaseCombination cCase = RobotApp.Project.Structure.Cases.CreateCombination(loadcase.Id, loadcase.Name, IRobotCombinationType.I_CBT_ULS, IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_COMB);
                        //RobotCaseCollection collection = RobotApp.Project.Structure.Cases.GetAll();

                        //for (int i = 0; i < combo.LoadFactors.Count; i++)
                        //{
                        //    for (int j = 1; j <= collection.Count; j++)
                        //    {
                        //        IRobotCase c = (collection.Get(j) as IRobotCase);
                        //        if (combo.LoadcaseNames[i] == c.Name)
                        //        {
                        //            cCase.CaseFactors.New(c.Number, combo.LoadFactors[i]);
                        //        }
                        //    }
                        //}

                        break;
                }
            }
            return caseServer.Get(caseNum);
        }

        public static bool SetLoadcases(RobotApplication robot, List<ICase> cases)
        {
            IRobotCase lCase = null;
            RobotCaseCollection caseCollection = robot.Project.Structure.Cases.GetAll();
            Dictionary<string, int> addedCases = new Dictionary<string, int>();

            for (int i = 1; i <= caseCollection.Count; i++)
            {
                IRobotCase rCase = caseCollection.Get(i);
                addedCases.Add(rCase.Name, rCase.Number);
            }

            foreach (ICase bhCase in cases)
            {
                BHoMObject loadcase = bhCase as BHoMObject;
                int robotNumber = 0;
                if (!addedCases.TryGetValue(loadcase.Name, out robotNumber))
                {
                    lCase = SetCase(robot, bhCase);
                    robotNumber = lCase.Number;
                }
                if (loadcase[Utils.NUM_KEY] == null)
                {
                    loadcase.CustomData.Add(Utils.NUM_KEY, robotNumber);
                }
                else
                {
                    loadcase.CustomData[Utils.NUM_KEY] = robotNumber;
                }
            }

            return true;
        }
    }
}
