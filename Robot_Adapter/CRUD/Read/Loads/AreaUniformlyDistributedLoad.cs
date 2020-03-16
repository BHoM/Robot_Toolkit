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

        private List<ILoad> ReadAreaUniformlyDistributedLoad(List<string> ids = null)
        {
            List<ILoad> bhomLoads = new List<ILoad>();
            Dictionary<int, Panel> bhomPanel = ReadPanels().ToDictionary(x => System.Convert.ToInt32(x.CustomData[AdapterIdName]));
            Dictionary<string, Loadcase> bhomLoadCases = new Dictionary<string, Loadcase>();
            List<Loadcase> lCases = ReadLoadCase();
            for (int i = 0; i < lCases.Count; i++)
            {
                if (!bhomLoadCases.ContainsKey(lCases[i].Name))
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
                            List<Panel> objects = new List<Panel>();
                            for (int k = 0; k < elementIds.Count; k++)
                            {
                                if (bhomPanel.ContainsKey(elementIds[k]))
                                    objects.Add(bhomPanel[elementIds[k]]);
                            }

                            switch (loadRecord.Type)
                            {
                                case IRobotLoadRecordType.I_LRT_UNIFORM:
                                    double fx = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_PX);
                                    double fy = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_PY);
                                    double fz = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_PZ);
                                    double ls = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM);
                                    double pj = loadRecord.GetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED);


                                    AreaUniformlyDistributedLoad PanelPressure = new AreaUniformlyDistributedLoad
                                    {
                                        Pressure = new Vector { X = fx, Y = fy, Z = fz },
                                        Loadcase = bhomLoadCases[sCase.Name],
                                        Objects = new BHoMGroup<IAreaElement>() { Elements = objects.ToList<IAreaElement>() },
                                        Axis = ls == 0 ? LoadAxis.Global : LoadAxis.Local,
                                        Projected = pj == 1
                                    };
                                    bhomLoads.Add(PanelPressure);
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
