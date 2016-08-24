﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoMG = BHoM.Geometry;
using BHoML = BHoM.Structural.Loads;
using BHoMB = BHoM.Base;
using Robot_Adapter.Base;

namespace Robot_Adapter.Structural.Loads
{
    public class LoadIO
    {
        public static bool SetLoads(RobotApplication robot, List<BHoML.ILoad> loads)
        {
            loads.Sort(delegate (BHoML.ILoad l1, BHoML.ILoad l2)
            {
                return l1.Loadcase.Name.CompareTo(l2.Loadcase.Name);
            });

            string currentLoadcase = "";
            IRobotCase lCase = null;
            IRobotSimpleCase sCase = null;
            IRobotLoadRecord loadRecord = null;
            foreach (BHoML.ILoad load in loads)
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
                        case BHoML.LoadType.PointForce:
                            BHoML.PointForce pL = load as BHoML.PointForce;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_FORCE);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(pL.Objects));
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FX, pL.Force.X);
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FY, pL.Force.Y);
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FZ, pL.Force.Z);
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CX, pL.Moment.X);
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CY, pL.Moment.Y);
                            loadRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CZ, pL.Moment.Z);
                            break;
                        case BHoML.LoadType.BarUniformLoad:
                            BHoML.BarUniformlyDistributedLoad lL = load as BHoML.BarUniformlyDistributedLoad;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(lL.Objects));
                            loadRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PX, lL.ForceVector.X);
                            loadRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PY, lL.ForceVector.Y);
                            loadRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PZ, lL.ForceVector.Z);
                            break;
                        case BHoML.LoadType.AreaUniformLoad:
                            BHoML.AreaUniformalyDistributedLoad lA = load as BHoML.AreaUniformalyDistributedLoad;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_UNIFORM);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(lA.Objects));
                            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PX, lA.Pressure.X);
                            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PY, lA.Pressure.Y);
                            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PZ, lA.Pressure.Z);
                            break;
                        case BHoML.LoadType.AreaTemperature:
                            break;
                        case BHoML.LoadType.BarTemperature:
                            BHoML.BarTemperatureLoad tL = load as BHoML.BarTemperatureLoad;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_THERMAL);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(tL.Objects));
                            loadRecord.SetValue((short)IRobotThermalRecordValues.I_TRV_T_1, tL.TemperatureChange.X);
                            loadRecord.SetValue((short)IRobotThermalRecordValues.I_TRV_T_2, tL.TemperatureChange.Y);
                            loadRecord.SetValue((short)IRobotThermalRecordValues.I_TRV_T_3, tL.TemperatureChange.Z);
                            break;
                        case BHoML.LoadType.BarPointLoad:
                            BHoML.BarPointLoad bPL = load as BHoML.BarPointLoad;
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
                        case BHoML.LoadType.PointDisplacement:
                            BHoML.PointDisplacement pD = load as BHoML.PointDisplacement;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_VELOCITY);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(pD.Objects));
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UX, pD.Translation.X);
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UY, pD.Translation.Y);
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UZ, pD.Translation.Z);
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RX, pD.Rotation.X);
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RY, pD.Rotation.Y);
                            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RZ, pD.Rotation.Z);
                            break;
                        case BHoML.LoadType.PointVelocity:
                            BHoML.PointVelocity pV = load as BHoML.PointVelocity;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_VELOCITY);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(pV.Objects));
                            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UX, pV.TranslationalVelocity.X);
                            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UY, pV.TranslationalVelocity.Y);
                            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UZ, pV.TranslationalVelocity.Z);
                            break;
                        case BHoML.LoadType.PointAcceleration:
                            BHoML.PointAcceleration pA = load as BHoML.PointAcceleration;
                            loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_ACCELERATION);
                            loadRecord.Objects.FromText(Utils.GetSelectionString(pA.Objects));
                            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UX, pA.TranslationalAcceleration.X);
                            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UY, pA.TranslationalAcceleration.Y);
                            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UZ, pA.TranslationalAcceleration.Z);
                            break;
                        case BHoML.LoadType.Geometrical:
                            if (load is BHoML.GeometricalAreaLoad)
                            {
                                BHoML.GeometricalAreaLoad aL = load as BHoML.GeometricalAreaLoad;
                                List<BHoMG.Point> points = aL.Contour.ControlPoints.ToList();

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
                            else if (load is BHoML.GeometricalLineLoad)
                            {
                                BHoML.GeometricalLineLoad lineL = load as BHoML.GeometricalLineLoad;
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

        internal static bool GetLoads(RobotApplication robot, out List<BHoML.ILoad> loads)
        {
            throw new NotImplementedException();
        }

        internal static IRobotCaseNature GetLoadNature(BHoML.LoadNature nature)
        {
            switch (nature)
            {
                case BHoML.LoadNature.Dead:
                    return IRobotCaseNature.I_CN_PERMANENT;
                case BHoML.LoadNature.Live:
                    return IRobotCaseNature.I_CN_EXPLOATATION;
                case BHoML.LoadNature.Seismic:
                    return IRobotCaseNature.I_CN_SEISMIC;
                case BHoML.LoadNature.Snow:
                    return IRobotCaseNature.I_CN_SNOW;
                case BHoML.LoadNature.Temperature:
                    return IRobotCaseNature.I_CN_TEMPERATURE;
                case BHoML.LoadNature.Wind:
                    return IRobotCaseNature.I_CN_WIND;
                default:
                    return IRobotCaseNature.I_CN_PERMANENT;
            }
        }
        internal static BHoML.LoadNature GetLoadNature(IRobotCaseNature nature)
        {
            switch (nature)
            {
                case IRobotCaseNature.I_CN_PERMANENT:
                    return BHoML.LoadNature.Dead;
                case IRobotCaseNature.I_CN_EXPLOATATION:
                    return BHoML.LoadNature.Live;
                case IRobotCaseNature.I_CN_SEISMIC:
                    return BHoML.LoadNature.Seismic;
                case IRobotCaseNature.I_CN_SNOW:
                    return BHoML.LoadNature.Snow;
                case IRobotCaseNature.I_CN_TEMPERATURE:
                    return BHoML.LoadNature.Temperature;
                case IRobotCaseNature.I_CN_WIND:
                    return BHoML.LoadNature.Wind;
                default:
                    return BHoML.LoadNature.Other;
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

        public static IRobotCase SetCase(RobotApplication robot, BHoML.ICase bhCase, bool checkExists = false)
        {
            RobotCaseServer caseServer = robot.Project.Structure.Cases;
            BHoMB.BHoMObject loadcase = bhCase as BHoMB.BHoMObject;
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
                    case BHoML.CaseType.Simple:
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
                    case BHoML.CaseType.Combination:
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

        public static bool SetLoadcases(RobotApplication robot, List<BHoML.ICase> cases)
        {
            IRobotCase lCase = null;
            RobotCaseCollection caseCollection = robot.Project.Structure.Cases.GetAll();
            Dictionary<string, int> addedCases = new Dictionary<string, int>();

            for (int i = 1; i <= caseCollection.Count; i++)
            {
                IRobotCase rCase = caseCollection.Get(i);
                addedCases.Add(rCase.Name, rCase.Number);
            }

            foreach (BHoML.ICase bhCase in cases)
            {
                BHoMB.BHoMObject loadcase = bhCase as BHoMB.BHoMObject;
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


        public static BHoML.LoadCombination GetLoadCombination(RobotApplication robot, IRobotCase robotCase, BHoMB.ObjectManager<string, BHoML.ICase> caseManager)
        {
            if (robotCase.Type == IRobotCaseType.I_CT_COMBINATION)
            {
                IRobotCaseCombination combo = (robotCase as IRobotCaseCombination);
                List<double> factors = new List<double>();
                List<BHoML.ICase> caseNames = new List<BHoML.ICase>();
                for (int i = 1; i <= combo.CaseFactors.Count; i++)
                {
                    RobotCaseFactor cF = combo.CaseFactors.Get(i);
                    IRobotCase rCase = robot.Project.Structure.Cases.Get(cF.CaseNumber);
                    if (caseManager[rCase.Number.ToString()] == null)
                    {
                        caseManager.Add(rCase.Number.ToString(), GetLoadcase(robot, rCase.Number));
                    }
                    caseNames.Add(caseManager[rCase.Number.ToString()]);
                    factors.Add(cF.Factor);
                }

                return new BHoML.LoadCombination(robotCase.Name, caseNames, factors);
            }
            return null;
        }

        public static BHoML.ICase GetSimpleCase(IRobotCase robotCase)
        {
            return new BHoML.Loadcase(robotCase.Name, GetLoadNature(robotCase.Nature));
        }

        public static BHoML.ICase GetLoadcase(RobotApplication RobotApp, int robotCase)
        {
            IRobotCase currentCase = RobotApp.Project.Structure.Cases.Get(robotCase);
            return GetSimpleCase(currentCase);
        }

        public static List<string> GetLoadcases(RobotApplication RobotApp, out List<BHoML.ICase> cases)
        {
            BHoMB.ObjectManager<string, BHoML.ICase> caseManager = new BHoM.Base.ObjectManager<string, BHoML.ICase>(Utils.NUM_KEY, BHoMB.FilterOption.UserData);
            RobotApp.Interactive = 0;
            RobotApp.Project.Structure.Cases.BeginMultiOperation();
            RobotCaseCollection collection = RobotApp.Project.Structure.Cases.GetAll();
            IRobotCase currentCase;
            cases = new List<BHoM.Structural.Loads.ICase>();
            List<string> outIds = new List<string>();
            for (int i = 1; i <= collection.Count; i++)
            {
                BHoML.ICase newCase = null;
                currentCase = collection.Get(i) as IRobotCase;
                switch (currentCase.Type)
                {
                    case IRobotCaseType.I_CT_COMBINATION:
                    case IRobotCaseType.I_CT_CODE_COMBINATION:
                        caseManager.Add(currentCase.Number.ToString(), GetLoadCombination(RobotApp, currentCase, caseManager));
                        break;
                    case IRobotCaseType.I_CT_MOBILE:
                        continue;
                    case IRobotCaseType.I_CT_SIMPLE:
                        caseManager.Add(currentCase.Number.ToString(), GetSimpleCase(currentCase));
                        break;
                }
                outIds.Add(currentCase.Number.ToString());
            }
            RobotApp.Project.Structure.Cases.EndMultiOperation();
            RobotApp.Interactive = 1;

            cases = caseManager.GetRange(outIds);
            return outIds;
        }
    }
}