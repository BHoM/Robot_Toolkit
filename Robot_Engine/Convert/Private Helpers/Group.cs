using BH.oM.Geometry;
using BH.oM.Base;
using BH.oM.Common.Materials;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Loads;
using System.Linq;

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

        public static string CreateIdListOrGroupName<T>(this Load<T> load, RobotGroupServer rServer) where T : IBHoMObject
        {
            //For a named group, appy loads to the group name
            if (!string.IsNullOrWhiteSpace(load.Objects.Name))
            {
                IRobotObjectType rType = RobotObjectType(load.Objects.GetType());
                int gIndex = rServer.Find(rType, load.Objects.Name);
                RobotGroup rGroup = rServer.Get(rType, gIndex);
                return rGroup.SelList;
            }

            //Otherwise apply to the corresponding indecies
            return load.Objects.Elements.Select(x => int.Parse(x.CustomData[AdapterID].ToString())).GeterateIdString();

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

        //public static IRobotObjectType ElementType(this BHoMGroup<Node> group)
        //{
        //    return IRobotObjectType.I_OT_BAR;
        //}

        ///***************************************************/

        //public static IRobotObjectType ElementType(this BHoMGroup<Bar> group)
        //{
        //    return IRobotObjectType.I_OT_BAR;
        //}

        ///***************************************************/

        //public static IRobotObjectType ElementType(this BHoMGroup<MeshFace> group)
        //{
        //    return "ELEMENT";
        //}

        ///***************************************************/

        //public static IRobotObjectType ElementType(this BHoMGroup<RigidLink> group)
        //{
        //    return "ELEMENT";
        //}

        ///***************************************************/

        //public static IRobotObjectType ElementType(this BHoMGroup<IBHoMObject> group)
        //{
        //    if (group.Elements.Where(x => x.GetType() == typeof(Node)).Count() == group.Elements.Count)
        //        return "NODE";

        //    return "ELEMENT";
        //}

        ///***************************************************/

        //public static IRobotObjectType ElementType(this BHoMGroup<BHoMObject> group)
        //{
        //    if (group.Elements.Where(x => x.GetType() == typeof(Node)).Count() == group.Elements.Count)
        //        return "NODE";

        //    return "ELEMENT";
        //}

        ///***************************************************/

        //public static IRobotObjectType ElementType(this BHoMGroup<IAreaElement> group)
        //{
        //    return "ELEMENT";
        //}

        ///***************************************************/
        ///**** Public Methods - Interfaces               ****/
        ///***************************************************/
        //public static IRobotObjectType IElementType<T>(this BHoMGroup<T> group) where T : IBHoMObject
        //{
        //    return ElementType(group as dynamic);
        //}

        ///***************************************************/
    }
}
