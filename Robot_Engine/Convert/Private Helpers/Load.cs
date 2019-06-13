/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System.Collections.Generic;
using System.Linq;
using RobotOM;
using BH.oM.Structure.Loads;
using BH.oM.Adapters.Robot;
using BH.oM.Geometry;
using BH.Engine.Geometry;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

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

        /***************************************************/

        public static LoadNature BHoMLoadNature(IRobotCaseNature nature)
        {
            switch (nature)
            {
                case IRobotCaseNature.I_CN_PERMANENT:
                    return LoadNature.Dead;
                case IRobotCaseNature.I_CN_EXPLOATATION:
                    return LoadNature.Live;
                case IRobotCaseNature.I_CN_SEISMIC:
                    return LoadNature.Seismic;
                case IRobotCaseNature.I_CN_SNOW:
                    return LoadNature.Snow;
                case IRobotCaseNature.I_CN_TEMPERATURE:
                    return LoadNature.Temperature;
                case IRobotCaseNature.I_CN_WIND:
                    return LoadNature.Wind;
                default:
                    return LoadNature.Other;
            }
        }

        /***************************************************/

        public static void IRobotLoad(this ILoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            RobotLoad(load as dynamic, sCase, rGroupServer);
        }

        public static void RobotLoad(this GravityLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_DEAD);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_X, load.GravityDirection.X);
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Y, load.GravityDirection.Y);
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Z, load.GravityDirection.Z);
            loadRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_ENTIRE_STRUCTURE, 1);
        }

        /***************************************************/

        public static void RobotLoad(this PointLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.Force.Length() == 0 && load.Moment.Length() == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero forces and moments are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_FORCE);
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

        /***************************************************/

        public static void RobotLoad(this BarUniformlyDistributedLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.Force.Length() == 0 && load.Moment.Length() == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero forces and moments are not pushed to Robot");
                return;
            }

            if (load.Force.Length() != 0)
            {
                IRobotLoadRecord loadRecordForce = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM);
                loadRecordForce.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
                loadRecordForce.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PX, load.Force.X);
                loadRecordForce.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PY, load.Force.Y);
                loadRecordForce.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PZ, load.Force.Z);
            }
            else
            {
                Engine.Reflection.Compute.RecordError("Zero forces are not pushed to Robot");
            }
            if (load.Moment.Length() != 0)
            {
                IRobotLoadRecord loadRecordMoment = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_MOMENT_DISTRIBUTED);
                loadRecordMoment.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
                loadRecordMoment.SetValue((short)IRobotBarMomentDistributedRecordValues.I_BMDRV_MX, load.Moment.X);
                loadRecordMoment.SetValue((short)IRobotBarMomentDistributedRecordValues.I_BMDRV_MY, load.Moment.Y);
                loadRecordMoment.SetValue((short)IRobotBarMomentDistributedRecordValues.I_BMDRV_MZ, load.Moment.Z);
            }
            else
            {
                Engine.Reflection.Compute.RecordError("Zero moments are not pushed to Robot");
            }

        }

        /***************************************************/

        public static void RobotLoad(this AreaUniformlyDistributedLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.Pressure.Length() == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero pressures are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_UNIFORM);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PX, load.Pressure.X);
            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PY, load.Pressure.Y);
            loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PZ, load.Pressure.Z);

            if (load.Axis == LoadAxis.Local)
                loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, 1);

            if(load.Projected)
                loadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED, 1);
        }

        /***************************************************/

        public static void RobotLoad(this BarPointLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.Force.Length() == 0 && load.Moment.Length() == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero forces and moments are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_FORCE_CONCENTRATED);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FX, load.Force.X);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FY, load.Force.Y);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FZ, load.Force.Z);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CX, load.Moment.X);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CY, load.Moment.Y);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CZ, load.Moment.Z);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_X, load.DistanceFromA);
            loadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_REL, 0);
        }

        /***************************************************/

        public static void RobotLoad(this PointDisplacement load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.Translation.Length() == 0 && load.Rotation.Length() == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero point displacements are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_DISPLACEMENT);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UX, load.Translation.X);
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UY, load.Translation.Y);
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_UZ, load.Translation.Z);
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RX, load.Rotation.X);
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RY, load.Rotation.Y);
            loadRecord.SetValue((short)IRobotNodeDisplacementRecordValues.I_NDRV_RZ, load.Rotation.Z);
        }

        /***************************************************/

        public static void RobotLoad(this PointVelocity load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.TranslationalVelocity.Length() == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero point velocities are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_VELOCITY);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UX, load.TranslationalVelocity.X);
            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UY, load.TranslationalVelocity.Y);
            loadRecord.SetValue((short)IRobotNodeVelocityRecordValues.I_NVRV_UZ, load.TranslationalVelocity.Z);
        }

        /***************************************************/

        public static void RobotLoad(this PointAcceleration load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.TranslationalAcceleration.Length() == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero point accelerations are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_ACCELERATION);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UX, load.TranslationalAcceleration.X);
            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UY, load.TranslationalAcceleration.Y);
            loadRecord.SetValue((short)IRobotNodeAccelerationRecordValues.I_NACRV_UZ, load.TranslationalAcceleration.Z);

        }

        /***************************************************/

        public static void RobotLoad(this BarVaryingDistributedLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.ForceA.Length() == 0 && load.ForceB.Length() == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero distributed forces are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_TRAPEZOIDALE);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX1, load.ForceA.X);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY1, load.ForceA.Y);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ1, load.ForceA.Z);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_X1, load.DistanceFromA);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PX2, load.ForceB.X);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PY2, load.ForceB.Y);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_PZ2, load.ForceB.Z);
            loadRecord.SetValue((short)IRobotBarTrapezoidaleRecordValues.I_BTRV_X2, load.DistanceFromB);
        }

        /***************************************************/

        public static void RobotLoad(this BarTemperatureLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.TemperatureChange == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero thermal loads are not pushed to Robot");
                return;
            }
            IRobotLoadRecord loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_BAR_THERMAL);
            loadRecord.Objects.FromText(load.CreateIdListOrGroupName(rGroupServer));
            loadRecord.SetValue((short)IRobotBarThermalRecordValues.I_BTRV_TX, load.TemperatureChange);

        }

        /***************************************************/

        public static void RobotLoad(this ContourLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.Force.Length() == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero contour forces are not pushed to Robot");
                return;
            }

            RobotLoadRecordInContour loadRecord = sCase.Records.Create(IRobotLoadRecordType.I_LRT_IN_CONTOUR) as RobotLoadRecordInContour;

            List<Point> points = load.Contour.ControlPoints.ToList();

            //Remove last point in case of duplicant with start point
            if (points.First().SquareDistance(points.Last()) < Tolerance.Distance * Tolerance.Distance)
                points.RemoveAt(points.Count - 1);

            loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_PX1, load.Force.X);
            loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_PY1, load.Force.Y);
            loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_PZ1, load.Force.Z);
            loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_NPOINTS, points.Count);
            loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_AUTO_DETECT_OBJECTS, 1);
            loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_LOCAL, (load.Axis == LoadAxis.Global) ? 0 : 1);
            loadRecord.SetValue((short)IRobotInContourRecordValues.I_ICRV_PROJECTION, load.Projected ? 1 : 0);

            Vector normal = load.Contour.FitPlane().Normal;
            loadRecord.SetVector(normal.X, normal.Y, normal.Z);

            for (int cp = 0; cp < points.Count; cp++)
            {
                loadRecord.SetContourPoint(cp +1, points[cp].X, points[cp].Y, points[cp].Z);
            }

            
        }

        /***************************************************/

        public static void RobotLoad(this GeometricalLineLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.ForceA.Length() == 0 && load.ForceB.Length() == 0 && load.MomentA.Length() == 0 && load.MomentB.Length() == 0)
            {
                Engine.Reflection.Compute.RecordError("Zero geometrical forces and moments are not pushed to Robot");
                return;
            }
            IRobotLoadRecordLinear3D loadLin3D = sCase.Records.Create(IRobotLoadRecordType.I_LRT_LINEAR_3D) as IRobotLoadRecordLinear3D;

            loadLin3D.SetPoint(1, load.Location.Start.X, load.Location.Start.Y, load.Location.Start.Z);
            loadLin3D.SetPoint(2, load.Location.End.X, load.Location.End.Y, load.Location.End.Z);

            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PX1, load.ForceA.X);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PY1, load.ForceA.Y);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PZ1, load.ForceA.Z);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PX2, load.ForceB.X);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PY2, load.ForceB.Y);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PZ2, load.ForceB.Z);

            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MX1, load.MomentA.X);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MY1, load.MomentA.Y);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MZ1, load.MomentA.Z);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MX2, load.MomentB.X);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MY2, load.MomentB.Y);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MZ2, load.MomentB.Z);
        }

        /***************************************************/

    }
}
