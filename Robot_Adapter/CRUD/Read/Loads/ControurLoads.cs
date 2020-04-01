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

using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using RobotOM;
using BH.oM.Geometry;
using BH.oM.Base;
using BH.oM.Structure.Loads;
using BH.oM.Adapters.Robot;
using BH.Engine.Robot;


namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<ILoad> ReadContourLoads(List<string> ids = null)
        {

            List<ILoad> bhomLoads = new List<ILoad>();
            Dictionary<string, Loadcase> bhomLoadCases = new Dictionary<string, Loadcase>();
            List<Loadcase> lCases = ReadLoadCase();
            for (int i = 0; i < lCases.Count; i++)
            {
                if (bhomLoadCases.ContainsKey(lCases[i].Name) == false)
                    bhomLoadCases.Add(lCases[i].Name, lCases[i]);
            }

            IRobotCaseCollection loadCollection = m_RobotApplication.Project.Structure.Cases.GetAll();

            for (int i = 1; i <= loadCollection.Count; i++)
            {
                IRobotCase lCase = loadCollection.Get(i) as IRobotCase;
                if (lCase.Type == IRobotCaseType.I_CT_SIMPLE)
                {
                    IRobotSimpleCase sCase = lCase as IRobotSimpleCase;
                    if (bhomLoadCases.ContainsKey(sCase.Name))
                    {
                        for (int j = 1; j <= sCase.Records.Count; j++)
                        {
                            IRobotLoadRecord loadRecord = sCase.Records.Get(j);

                            switch (loadRecord.Type)
                            {
                                case IRobotLoadRecordType.I_LRT_IN_CONTOUR:
                                    double fx = loadRecord.GetValue((short)IRobotInContourRecordValues.I_ICRV_PX1);
                                    double fy = loadRecord.GetValue((short)IRobotInContourRecordValues.I_ICRV_PY1);
                                    double fz = loadRecord.GetValue((short)IRobotInContourRecordValues.I_ICRV_PZ1);
                                    double nbPoints = loadRecord.GetValue((short)IRobotInContourRecordValues.I_ICRV_NPOINTS);
                                    double localAxis = loadRecord.GetValue((short)IRobotInContourRecordValues.I_ICRV_LOCAL);
                                    double projectedLoad = loadRecord.GetValue((short)IRobotInContourRecordValues.I_ICRV_PROJECTION);

                                    RobotLoadRecordInContour contourRecord = loadRecord as RobotLoadRecordInContour;

                                    List<Point> contourPoints = new List<Point>();

                                    for (int k = 0; k < nbPoints; k++)
                                    {
                                        double x, y, z;

                                        contourRecord.GetContourPoint(k + 1, out x, out y, out z);
                                        contourPoints.Add(new Point { X = x, Y = y, Z = z });
                                    }

                                    contourPoints.Add(contourPoints.First());

                                    oM.Structure.Loads.ContourLoad contourLoad = new oM.Structure.Loads.ContourLoad
                                    {
                                        Force = new Vector { X = fx, Y = fy, Z = fz },
                                        Contour = new Polyline { ControlPoints = contourPoints },
                                        Loadcase = bhomLoadCases[sCase.Name],
                                        Axis = localAxis == 1 ? LoadAxis.Local : LoadAxis.Global,
                                        Projected = projectedLoad == 1
                                    };
                                    bhomLoads.Add(contourLoad);
                                    break;
                            }
                        }
                    }
                }
            }
            return bhomLoads;
        }


        /***************************************************/

    }

}
