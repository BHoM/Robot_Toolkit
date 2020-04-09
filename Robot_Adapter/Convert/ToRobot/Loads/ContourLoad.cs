/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.Structure.Loads;
using RobotOM;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/
       
        public static void ToRobot(this oM.Structure.Loads.ContourLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            if (load.Force.Length() == 0)
            {
                Engine.Reflection.Compute.RecordWarning("Zero contour forces are not pushed to Robot");
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
    }
}

