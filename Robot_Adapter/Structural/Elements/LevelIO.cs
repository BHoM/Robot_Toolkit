//using BH.oM.Base;
//using BH.oM.Structural.Elements;
//using RobotOM;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Robot_Adapter.Structural.Elements
//{
//    public class LevelIO
//    {
//        public static bool SetLevels(RobotApplication Robot, List<Storey> stories, out List<string> ids)
//        {
//            ids = new List<string>();
//            if (stories.Count > 0)
//            {
//                stories.Sort(delegate (Storey s1, Storey s2)
//                {
//                    return s1.Elevation.CompareTo(s2.Elevation);
//                });

//                double height = stories[1].Elevation - stories[0].Elevation;

//                for (int i = 0; i < stories.Count; i++)
//                {
//                    Robot.Project.Structure.Storeys.Create2(stories[i].Name, stories[i].Elevation);
//                    ids.Add(stories[i].Name);
//                }
//            }
//            return true;
//        }

//        public static List<string> GetLevels(RobotApplication Robot, out List<Storey> stories)
//        {
//            List<string> outIds = new List<string>();
//            stories = new List<Storey>();

//            RobotStoreyMngr sm = Robot.Project.Structure.Storeys;
//            for (int i = 1; i <= sm.Count; i++)
//            {
//                RobotStorey s = sm.Get(i);
//                stories.Add(new Storey(s.Name, s.Level, s.Height));
//                outIds.Add(s.Name);
//            }
//            return outIds;
//        }
//    }
//}
