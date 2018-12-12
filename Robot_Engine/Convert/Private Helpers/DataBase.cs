using BH.oM.Base;
using BH.oM.Common.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Properties.Section;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static string IDataBaseStringFormat(IBHoMObject obj)
        {
            return DataBaseStringFormat(obj as dynamic);
        }

        /***************************************************/

        private static string DataBaseStringFormat(ISectionProperty sectionProperty)
        {
            string objName = sectionProperty.Name;
            try
            {
                char[] charArray = objName.ToCharArray();
                string result = "";

                if (string.IsNullOrEmpty(objName) || charArray.Length < 3 || ContainsNumber(objName) == -1)
                    return objName;

                else
                {
                    List<string> name = new List<string>();
                    objName = objName.Replace(" ", "");
                    objName = objName.ToUpper();
                    objName = objName.Insert(ContainsNumber(objName), " ");
                    string[] secSizeArray = objName.Split(' ');
                    if (secSizeArray.Length < 2)
                        return objName;
                    else
                    {
                        name.Add(secSizeArray[0] + " ");
                        string[] dimArray = secSizeArray[1].Split('X');
                        if (dimArray.Length > 1)
                        {
                            foreach (string s in dimArray)
                            {
                                string endChar = s.TrimEnd('0');
                                endChar = endChar.TrimEnd('.');
                                string[] array2 = endChar.Split('.');
                                if (array2.Length == 2)
                                {

                                    if (System.Convert.ToInt32(array2[1]) > 0)
                                        name.Add(endChar + "x");
                                    else
                                        name.Add(array2[0] + "x");
                                }
                                else
                                {
                                    name.Add(array2[0] + "x");
                                }
                            }
                        }
                        else
                        {
                            name.Add(dimArray[0]);
                        }

                        for (int i = 0; i < name.Count; i++)
                        {
                            result = result + name[i];
                        }
                        result = result.TrimEnd('x');

                        return result;
                    }
                }
            }
            catch (Exception)
            {
                return objName;
            }
        }

        /***************************************************/

        private static string DataBaseStringFormat(Material material)
        {
            string objectName = material.Name;
            objectName = objectName.TrimEnd(' ').TrimStart(' ');
            return objectName;
        }

        public static string Match(List<string> dbNames, IBHoMObject obj)
        {
            if (dbNames.Contains(obj.Name))
                return obj.Name;

            else
            {
                string objectName = IDataBaseStringFormat(obj);
                objectName = objectName.Replace(" ", "");
                objectName = objectName.ToUpper();

                Func<string, bool> conditionalMatch = delegate (string x)
                {
                    string name = x;
                    name = name.Replace(" ", "");
                    name = name.ToUpper();
                    return name == objectName;
                };

                return dbNames.Where(conditionalMatch).FirstOrDefault();
            }
        }

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private static int ContainsNumber(string objName)
        {
            int counter = 0;
            int check = -1;
            int num;
            if (!string.IsNullOrEmpty(objName))
            {
                char[] charArray = objName.ToCharArray();
                foreach (char c in charArray)
                {
                    if (Int32.TryParse(c.ToString(), out num))
                    {
                        break;
                    }
                    counter++;
                }
                if (counter != charArray.Length)
                    check = counter;               
            }
            return check;
        }

        /***************************************************/

    }
}