using BH.oM.Geometry;
using BH.oM.Common.Materials;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Loads;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        public static IRobotObjectType RobotObjectType(Type type)
        {
            if (type == typeof(Bar))
                return IRobotObjectType.I_OT_BAR;
            
            else if (type == typeof(Node))
                return IRobotObjectType.I_OT_NODE;

            else if (typeof(ICase).IsAssignableFrom(type))
                return IRobotObjectType.I_OT_CASE;

            else
                return IRobotObjectType.I_OT_OBJECT;
        }

        public static string GeterateIdString(this IEnumerable<int> ids)
        {
            string str = "";

            int counter = 0;
            int prev = -10;

            foreach (int i in ids)
            {
                if (i - 1 == prev)
                {
                    counter++;
                }
                else
                {
                    if (counter > 1)
                        str += "to " + prev + " ";
                    else if (counter > 0)
                        str += prev + " ";

                    str += i.ToString() + " ";
                    counter = 0;
                }

                prev = i;
            }

            if (counter > 1)
                str += "to " + prev + " ";
            else if (counter > 0)
                str += prev + " ";

            return str;
        }
    }
}
