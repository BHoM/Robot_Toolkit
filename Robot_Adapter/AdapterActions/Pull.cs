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
using System.IO;
using System.Collections.Generic;
using RobotOM;
using System.Diagnostics;
using BH.oM.Adapters.Robot;
using BH.Engine.Adapters.Robot;
using BH.oM.Data.Requests;
using BH.oM.Adapter;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Public  Methods                           ****/
        /***************************************************/

        /* Temporary override of the pull method until updates have been made to the MeshResults and IResultCOllection : IResult
         */
        public override IEnumerable<object> Pull(IRequest request, PullType pullType = PullType.AdapterDefault, ActionConfig actionConfig = null)
        {
            FilterRequest filter = request as FilterRequest;
            if (filter != null)
            {
                if (filter.Type == typeof(BH.oM.Structure.Results.MeshResult))
                {
                    return ReadMeshResults(filter);
                }
            }

            return base.Pull(request, pullType, actionConfig);
        }

        /***************************************************/
    }
}

