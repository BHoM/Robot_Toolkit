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

namespace BH.oM.Adapters.Robot
{
    /***************************************************/
    /****               Public Enums                ****/
    /***************************************************/

    public enum MaterialDB
    {
        American,
        British,
        Eurocode        
    }

    /***************************************************/

    public enum SectionDB
    {
        UKST,
        AISC
    }

    /***************************************************/

    public enum ObjectProperties
    {
        Name, 
        Number
    }

    /***************************************************/

    public enum BarProperties
    {
        FramingElementDesignProperties,
        StructureObjectType                
    }

    /***************************************************/

    public enum NodeProperties
    {
        CoordinateSystem
    }

    /***************************************************/

    public enum Commands
    {

    }

    /***************************************************/

    public enum DesignCode_Steel
    {
        Default = 0,
        BS5950,
        BS5950_2000,
        BS_EN_1993_1_2005_NA_2008_A1_2014,
        ANSI_AISC_360_10   
    }

    /***************************************************/

}

