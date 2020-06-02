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
                                    double local = loadRecord.GetValue((short)IRobotLinear3DRecordValues.I_L3DRV_LOCAL);

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
                                        Axis = local.FromRobotLoadAxis(),
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
