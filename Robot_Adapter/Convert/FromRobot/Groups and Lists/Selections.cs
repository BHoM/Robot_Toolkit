/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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


namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static List<int> ToSelectionList(string selection)
        {
            if (selection.Contains("EDGE")) return null;
            List<int> output = new List<int>();
            string[] numbers = selection.Split(' ');
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i].Length > 0)
                {
                    if (numbers[i].Contains("to"))
                    {
                        int increment = 1;
                        if (numbers[i].Contains("By"))
                        {
                            string[] inc = numbers[i].Replace("By", ",").Split(',');
                            numbers[i] = numbers[i].Replace("By" + inc[1], "");

                            increment = int.Parse(inc[1]);
                        }

                        string[] range = numbers[i].Replace("to", ",").Split(',');
                        int startNum = int.Parse(range[0]);
                        int endNum = int.Parse(range[1]);

                        for (int j = startNum; j <= endNum; j += increment)
                        {
                            output.Add(j);
                        }
                    }
                    else
                    {
                        output.Add(int.Parse(numbers[i]));
                    }
                }
            }
            return output;
        }

        /***************************************************/

    }
}


