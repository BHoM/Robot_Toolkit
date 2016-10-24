using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Base;
using RobotOM;
using BHoM.Structural.Elements;
using BHoM.Structural.Loads;
namespace Robot_Adapter.Structural.Elements
{
    public class GroupIO
    {
        public static IRobotObjectType GetObjectType(Type type)
        {
            if (type == typeof(Bar))
            {
                return IRobotObjectType.I_OT_BAR;
            }
            else if (type == typeof(Node))
            {
                return IRobotObjectType.I_OT_NODE;
            }
            else if (type == typeof(Panel))
            {
                return IRobotObjectType.I_OT_PANEL;
            }
            else if (typeof(ICase).IsAssignableFrom(type))
            {
                return IRobotObjectType.I_OT_CASE;
            }
            else
            {
                return IRobotObjectType.I_OT_OBJECT;
            }
        }

        public static bool CreateGroups(RobotApplication robot, List<IGroup> groups, out List<string> ids)
        {
            ids = new List<string>();
            RobotGroupServer server = robot.Project.Structure.Groups;
            foreach (IGroup group in groups)
            {
                ids.Add(group.Name);
                server.Create(GetObjectType(group.ObjectType), group.Name, Robot_Adapter.Base.Utils.GetSelectionString<BHoMObject>(group.Objects));
            }
            return true;
        }

        public static List<string> GetGroups(RobotApplication robot, out List<IGroup> groups)
        {
            List<string> ids = new List<string>();
            RobotGroupServer server = robot.Project.Structure.Groups;
            ObjectManager<string, Node> nodes = new ObjectManager<string, Node>(Robot_Adapter.Base.Utils.NUM_KEY, FilterOption.UserData);
            ObjectManager<string, Bar> bars = new ObjectManager<string, Bar>(Robot_Adapter.Base.Utils.NUM_KEY, FilterOption.UserData);
            ObjectManager<string, Panel> panels = new ObjectManager<string, Panel>(Robot_Adapter.Base.Utils.NUM_KEY, FilterOption.UserData);
            groups = new List<IGroup>();
            for (int i = (int)IRobotObjectType.I_OT_NODE; i <= (int)IRobotObjectType.I_OT_PANEL; i++)
            {
                for (int j = 1; j <= server.GetCount((IRobotObjectType)i); j++)
                {
                    RobotGroup robotGroup = server.Get((IRobotObjectType)i, j);
                    
                    string list = robotGroup.SelList;
                    List<string> groupObjectIds = Robot_Adapter.Base.Utils.GetIdsAsTextFromText(list);

                    switch ((IRobotObjectType)i)
                    {
                        case IRobotObjectType.I_OT_NODE:
                            groups.Add(new Group<Node>(robotGroup.Name, nodes.GetRange(groupObjectIds)));
                            ids.Add(robotGroup.Name);
                            break;
                        case IRobotObjectType.I_OT_BAR:
                            groups.Add(new Group<Bar>(robotGroup.Name, bars.GetRange(groupObjectIds)));
                            ids.Add(robotGroup.Name);
                            break;
                        case IRobotObjectType.I_OT_PANEL:
                            groups.Add(new Group<Panel>(robotGroup.Name, panels.GetRange(groupObjectIds)));
                            ids.Add(robotGroup.Name);
                            break;
                    }
                }
            }

            return ids;
        }
    }
}
