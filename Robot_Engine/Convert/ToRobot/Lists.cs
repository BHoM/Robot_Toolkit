using System.Collections.Generic;
using BH.oM.Base;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static string FromSelectionList(List<int> numbers)
        {
            string selection = "";
            for (int i = 0; i < numbers.Count; i++)
            {
                selection += numbers[i] + " ";
            }
            return selection.Trim();            
        }

        /***************************************************/

        public static string FromSelectionList(List<string> ids)
        {
            string selection = "";
            for (int i = 0; i < ids.Count; i++)
            {
                selection += ids[i] + " ";
            }
            return selection.Trim();
        }

        /***************************************************/

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

        /***************************************************/
    }
}
