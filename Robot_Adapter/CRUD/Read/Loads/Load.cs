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


namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<ILoad> ReadLoads(Type type, List<string> ids = null)
        {
            if (type == typeof(PointLoad))
                return ReadNodeLoads(ids);
            else if (type == typeof(oM.Structure.Loads.ContourLoad) || type == typeof(oM.Adapters.Robot.ContourLoad))
                return ReadContourLoads(ids);
            else if (type == typeof(oM.Structure.Loads.GeometricalLineLoad) || type == typeof(oM.Adapters.Robot.GeometricalLineLoad))
                return ReadGeometricalLineLoads(ids);

            return new List<ILoad>();
        }

        /***************************************************/

        private List<ILoad> ReadNodeLoads(List<string> ids = null)
        {
            List<ILoad> bhomLoads = new List<ILoad>();
            Dictionary<int, Node> bhomNodes = ReadNodes().ToDictionary(x => System.Convert.ToInt32(x.CustomData[AdapterIdName]));
            Dictionary<string, Loadcase> bhomLoadCases = new Dictionary<string, Loadcase>();
            List<Loadcase> lCases = ReadLoadCase();
            for (int i = 0; i < lCases.Count; i++)
            {
                if (bhomLoadCases.ContainsKey(lCases[i].Name) == false)
                    bhomLoadCases.Add(lCases[i].Name, lCases[i]);
            }

            //Dictionary<string, Loadcase> bhomLoadCases = ReadLoadCase().ToDictionary(x => x.Name);
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
                            List<int> elementIds = Convert.FromRobotSelectionString(loadRecord.Objects.ToText());
                            List<Node> objects = new List<Node>();
                            for (int k = 0; k < elementIds.Count; k++)
                            {
                                if (bhomNodes.ContainsKey(elementIds[k]))
                                    objects.Add(bhomNodes[elementIds[k]]);
                            }

                            switch (loadRecord.Type)
                            {
                                case IRobotLoadRecordType.I_LRT_NODE_FORCE:
                                    double fx = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_FX);
                                    double fy = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_FY);
                                    double fz = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_FZ);
                                    double mx = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_CX);
                                    double my = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_CY);
                                    double mz = loadRecord.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_CZ);
                                    PointLoad nodeForce = new PointLoad
                                    {
                                        Force = new Vector { X = fx, Y = fy, Z = fz },
                                        Moment = new Vector { X = mx, Y = my, Z = mz },
                                        Loadcase = bhomLoadCases[sCase.Name],
                                        Objects = BH.Engine.Base.Create.BHoMGroup(objects) as BHoMGroup<Node>
                                    };
                                    bhomLoads.Add(nodeForce);
                                    break;
                            }
                        }
                    }
                }
            }
            return bhomLoads;
        }

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


        private List<ILoad> ReadGeometricalLineLoads(List<string> ids = null)
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
                                case IRobotLoadRecordType.I_LRT_LINEAR_3D:
                                    double fxa = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PX1);
                                    double fya = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PY1);
                                    double fza = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PZ1);
                                    double fxb = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PX2);
                                    double fyb = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PY2);
                                    double fzb = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PZ2);

                                    double mxa = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MX1);
                                    double mya = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MY1);
                                    double mza = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MZ1);
                                    double mxb = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MX2);
                                    double myb = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MY2);
                                    double mzb = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MZ2);

                                    IRobotLoadRecordLinear3D linRecord = loadRecord as IRobotLoadRecordLinear3D;

                                    double xa, ya, za, xb, yb, zb;

                                    linRecord.GetPoint(1, out xa, out ya, out za);
                                    linRecord.GetPoint(2, out xb, out yb, out zb);

                                    oM.Structure.Loads.GeometricalLineLoad contourLoad = new oM.Structure.Loads.GeometricalLineLoad
                                    {
                                        ForceA = new Vector { X = fxa, Y = fya, Z = fza },
                                        ForceB = new Vector { X = fxb, Y = fyb, Z = fzb },
                                        MomentA = new Vector { X = mxa, Y = mya, Z = mza },
                                        MomentB = new Vector { X = mxb, Y = myb, Z = mzb },
                                        Location = new Line { Start = new Point { X = xa, Y = ya, Z = za }, End = new Point { X = xb, Y = yb, Z = zb } },
                                        Loadcase = bhomLoadCases[sCase.Name]
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
