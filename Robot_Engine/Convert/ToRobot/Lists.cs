using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Design;
using RobotOM;
using System.Collections.Generic;
using BH.oM.Base;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************/
        
        public static string FromSelectionList(List<int> numbers)
        {
            string selection = "";
            for (int i = 0; i < numbers.Count; i++)
            {
                selection += numbers[i] + " ";
            }
            return selection.Trim();
            
        }

        public static string FromSelectionList(List<string> ids)
        {
            string selection = "";
            for (int i = 0; i < ids.Count; i++)
            {
                selection += ids[i] + " ";
            }
            return selection.Trim();
        }

        public static string FromSelectionList<T>(IEnumerable<T> objects) where T : IBHoMObject
        {
            string selection = "";
            foreach (T obj in objects)
            {
                object objNumber = null;
                obj.CustomData.TryGetValue("Robot Number", out objNumber);                
                selection += objNumber.ToString() + " ";
            }
            return selection.Trim();
        }
    }
}
