/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using System.Text;
using System.Threading.Tasks;
using BH.oM.Adapters.Robot;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Query
    {
        public static string GetStringFromEnum(this DesignCode_Steel designCode)
        {
            switch (designCode)
            {
                case DesignCode_Steel.BS5950:
                    return "BS5950";
                case DesignCode_Steel.BS5950_2000:
                    return "BS 5950:2000";
                case DesignCode_Steel.BS_EN_1993_1_2005_NA_2008_A1_2014:
                    return "BS-EN 1993-1:2005/NA:2008/A1:2014";
                case DesignCode_Steel.ANSI_AISC_360_10:
                    return "ANSI/AISC 360-10";
                default:
                    return "BS-EN 1993-1:2005/NA:2008/A1:2014";
            }
        }

        public static string GetStringFromEnum(this MaterialDB materialDB)
        {
            switch (materialDB)
            {
                case MaterialDB.American:
                    return "American";
                case MaterialDB.British:
                    return "British";
                case MaterialDB.Eurocode:
                    return "Eurocode";
                default:
                    return "British";
            }
        }

        public static string GetStringFromEnum(this SectionDB sectionDB)
        {
            switch (sectionDB)
            {
                case SectionDB.UKST:
                    return "UKST";
                case SectionDB.AISC:
                    return "AISC";
                default:
                    return "UKST";
            }
        }

    }
}





