﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotToolkit.Utilities
{
    public static class Utils
    {
        public static List<int> GetNumbersFromText(string selection)
        {
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
    }
}
