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
using RobotOM;
using BH.oM.Structure.Results;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        public static Dictionary<string, object> MeshResultParameters(MeshResultType resultType)
        {
            Dictionary<string, object> meshResultParams = new Dictionary<string, object>();
            List<int> results = new List<int>();
            switch (resultType)
            {
                case MeshResultType.Stresses:
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SYY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_SXY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_TXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_TYY);
                    results.Add((int)IRobotFeResultType.I_FRT_PRINCIPAL_S1);
                    results.Add((int)IRobotFeResultType.I_FRT_PRINCIPAL_S1_2);
                    results.Add((int)IRobotFeResultType.I_FRT_PRINCIPAL_S2);
                    break;

                case MeshResultType.Forces:
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NYY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_NXY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MYY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_MXY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_QXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_QYY);
                    break;

                case MeshResultType.VonMises:
                    results.Add((int)IRobotFeResultType.I_FRT_COMPLEX_S_MISES);
                    results.Add((int)IRobotFeResultType.I_FRT_COMPLEX_N_MISES);
                    results.Add((int)IRobotFeResultType.I_FRT_COMPLEX_M_MISES);
                    break;

                case MeshResultType.Displacements:
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_UXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_UYY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_WNORM);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_RXX);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_RYY);
                    results.Add((int)IRobotFeResultType.I_FRT_DETAILED_RNORM);

                    break;
            }
            meshResultParams.Add("ResultsToInclude", results);
            return meshResultParams;
        }
    }
}