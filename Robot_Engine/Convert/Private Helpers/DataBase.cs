using BH.oM.Geometry;
using BH.oM.Base;
using BH.oM.Common.Materials;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        public static string IDataBaseStringFormat(IBHoMObject obj)
        {
            return DataBaseStringFormat(obj as dynamic);
        }

        public static string DataBaseStringFormat(ISectionProperty sectionProperty)
        {
            string objectName = sectionProperty.Name;
            string result = "";
            List<string> name = new List<string>();
            objectName.TrimEnd(' ').TrimStart(' ');
            objectName.ToUpper();
            string[] array;
            char[] characters;
            char character;
            int firstNum;

            if (objectName.Contains(" "))
            {
                array = objectName.Split(' ');
                character = (array[1].ToCharArray())[0];
            }

            else
            {
                characters = objectName.ToCharArray();
                int counter = 0;
                foreach (char c in characters)
                {
                    if (Int32.TryParse(c.ToString(), out firstNum))
                    {
                        break;
                    }
                    counter++;
                }
                objectName = objectName.Insert(counter, " ");
                array = objectName.Split(' ');
                character = (array[1].ToCharArray())[0];
            }

            if (array.Length == 2 && Int32.TryParse(character.ToString(), out firstNum))
            {
                name.Add(array[0] + " ");
                string[] array1 = array[1].Split('x');
                if (array1.Length > 1)
                {
                    foreach (string s in array1)
                    {
                        string[] array2 = s.Split('.');
                        if (array2.Length == 2)
                        {
                            if (System.Convert.ToInt32(array2[1]) > 0)
                                name.Add(s + "x");
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
                    name.Add(array1[0]);
                }

                for (int i = 0; i < name.Count; i++)
                {
                    result = result + name[i];
                }

                result = result.TrimEnd('x');
            }
            else
                result = objectName;

            return result;
        }

        public static string DataBaseStringFormat(Material material)
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

    }
}