using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Structural;
using System.Reflection;
using System.IO;
using BHoM.Structural.Results;
using BHoM.Structural.Elements;
using BHoM.Geometry;
using BHoM.Structural.Loads;

using Robot_Adapter.Structural.Interface;

namespace Robot_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            SetAreaLoad();
        }
        
        private static void CreateLoadcase()
        {
            List<ICase> l1 = new List<ICase>() {
                new BHoM.Structural.Loads.Loadcase(1, "Test", LoadNature.Dead),
                new BHoM.Structural.Loads.Loadcase(1, "DL1", LoadNature.Dead),
                new BHoM.Structural.Loads.Loadcase(1, "LL1", LoadNature.Live)
            };
            RobotAdapter app = new RobotAdapter();
            app.SetLoadcases(l1);
        }

        private static void SetAreaLoad()
        {
            RobotAdapter app = new RobotAdapter();
            Polyline p = new Polyline(new List<Point>() { new Point(0, 0, 0), new Point(0, 5, 0), new Point(5, 5, 0), new Point(5, 0, 0), new Point(0, 0, 0) });

            BHoM.Structural.Loads.Loadcase lC = new BHoM.Structural.Loads.Loadcase(1, "DL2", LoadNature.Dead, 1);
            //lC.CustomData[Utils.NUM_KEY] = "1";
            GeometricalAreaLoad aL = new GeometricalAreaLoad(p, new Vector(0, 0, -5000));
            aL.Loadcase = lC;
            GeometricalLineLoad lL = new GeometricalLineLoad(new Line(new Point(0, 0, 0), new Point(4, 0, 0)), new Vector(5000, 1000, 2000));
            lL.Loadcase = lC;

            app.SetLoads(new List<ILoad>() { aL, lL });
        }

        private static void GetBarResults()
        {
            RobotAdapter app = new RobotAdapter();

            app.GetBarForces(null, null, 3);
            BHoM.Base.Results.ResultServer<BarForce> resultServer = new BHoM.Base.Results.ResultServer<BarForce>(app.Filename);

            resultServer.LoadData();

            BHoM.Base.Results.Envelope barEnvelope = resultServer.MaxEnvelope();

        }

        private static void GetBars()
        {
            RobotAdapter app = new RobotAdapter();
            List<Node> bars = null;
            app.GetNodes(out bars);
            List<Bar> b2 = null;
            app.GetBars(out b2);
            Bar b = b2[1];


            string s = BHoM.Global.Project.ActiveProject.ToJSON();

            string json = s;
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(@"C:\Users\edalton\Desktop\Test.txt", false))
            {
                fs.WriteLine(json);
                fs.Close();
            }


        }

        private static void GetPanels()
        {
            RobotAdapter app = new RobotAdapter();
            List<Panel> b2 = null;
            app.GetPanels(out b2);

            string s = BHoM.Global.Project.ActiveProject.ToJSON();

            string json = s;
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(@"C:\Users\edalton\Desktop\Test.txt", false))
            {
                fs.WriteLine(json);
                fs.Close();
            }
        }

        private static void SetPanels()
        {
            RobotAdapter app = new RobotAdapter();

            string json = "";
            using (System.IO.StreamReader fs = new System.IO.StreamReader(@"C:\Users\edalton\Desktop\Test.txt"))
            {
                json = fs.ReadToEnd();
                fs.Close();
            }
            BHoM.Global.Project.FromJSON(json);
            List<string> ids = new List<string>();
            app.SetPanels(new BHoM.Base.ObjectFilter<Panel>().ToList(), out ids);
        }

        private static void SetBars()
        {
            RobotAdapter app = new RobotAdapter();

            string json = "";
            using (System.IO.StreamReader fs = new System.IO.StreamReader(@"C:\Users\edalton\Desktop\Test.txt"))
            {
                json = fs.ReadToEnd();
                fs.Close();
            }
            BHoM.Global.Project.FromJSON(json);
            List<string> ids = new List<string>();

            app.CreateBars(new BHoM.Base.ObjectFilter<Bar>().ToList(), out ids);
        }
    }
}
