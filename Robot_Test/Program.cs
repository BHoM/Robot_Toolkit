using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structure;
using System.Reflection;
using System.IO;
using BH.oM.Structure.Results;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Structure.Loads;
using BH.oM.Base;
using BH.Engine.Structure;
using BH.oM.Geometry;
using BEG = BH.Engine.Geometry;
using BH.Adapter.Robot;

//using Robot_Adapter.Structural.Interface;
//using BH.oM.Base.Results;
//using RobotOM;

namespace Robot_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // PropertyIO.BarPropertiesExample(new RobotApplication(), 5);
            PushNodes();
        }

        private static void PushNodes()
        {
            RobotAdapter app = new RobotAdapter();
            Point pt = BEG.Create.Point(0, 5, 6);
            Node node = Create.Node(pt);
            List<IObject> nodes = new List<IObject>();
            nodes.Add(node);
            app.Push(nodes);
        }
        
        //private static void CreateLoadcase()
        //{
        //    Node n = new Node(60.086, 3.392, 38.5);
        //    n.CustomData.Add("Robot Number", 62828);
        //    n.Constraint = new BHoM.Structure.Properties.NodeConstraint("Test", new double[] {  200,200,556300,0,0,0});

        //    List<string> ids = null;
        //    List<Bar> bars = null;
        //    RobotAdapter app = new RobotAdapter();
        //    app.GetBars(out bars);

        //}

        //private static void SetAreaLoad()
        //{
        //    RobotAdapter app = new RobotAdapter();
        //    Polyline p = new Polyline(new List<Point>() { new Point(0, 0, 0), new Point(0, 5, 0), new Point(5, 5, 0), new Point(5, 0, 0), new Point(0, 0, 0) });

        //    BHoM.Structure.Loads.Loadcase lC = new BHoM.Structure.Loads.Loadcase("DL2", LoadNature.Dead, 1);
        //    //lC.CustomData[Utils.NUM_KEY] = "1";
        //    GeometricalAreaLoad aL = new GeometricalAreaLoad(p, new Vector(0, 0, -5000));
        //    aL.Loadcase = lC;
        //    GeometricalLineLoad lL = new GeometricalLineLoad(new Line(new Point(0, 0, 0), new Point(4, 0, 0)), new Vector(5000, 1000, 2000));
        //    lL.Loadcase = lC;

        //    app.SetLoads(new List<ILoad>() { aL, lL });
        //}

        //private static void GetBarResults()
        //{
        //    RobotAdapter app = new RobotAdapter();

        //    //app.GetBarForces(null, null, 3, BHoM.Base.Results.ResultOrder.Name, out 
        //    BHoM.Base.Results.ResultServer<BarForce> resultServer = new BHoM.Base.Results.ResultServer<BarForce>(app.Filename);

        //    resultServer.LoadData();

        //    BHoM.Base.Results.Envelope barEnvelope = resultServer.MaxEnvelope();

        //}

        //private static void GetBars()
        //{
        //    RobotAdapter app = new RobotAdapter();
        //    List<Node> bars = null;
        //    app.GetNodes(out bars);
        //    List<Bar> b2 = null;
        //    app.GetBars(out b2);
        //    Bar b = b2[1];


        //    string s = BHoM.Global.Project.ActiveProject.ToJSON();

        //    string json = s;
        //    using (System.IO.StreamWriter fs = new System.IO.StreamWriter(@"C:\Users\edalton\Desktop\Test.txt", false))
        //    {
        //        fs.WriteLine(json);
        //        fs.Close();
        //    }


        //}

        //private static void GetPanels()
        //{
        //    RobotAdapter app = new RobotAdapter();
        //    List<Panel> b2 = null;
        //    app.GetPanels(out b2);

        //    string s = BHoM.Global.Project.ActiveProject.ToJSON();

        //    string json = s;
        //    using (System.IO.StreamWriter fs = new System.IO.StreamWriter(@"C:\Users\edalton\Desktop\Test.txt", false))
        //    {
        //        fs.WriteLine(json);
        //        fs.Close();
        //    }
        //}

        //private static void SetPanels()
        //{
        //    RobotAdapter app = new RobotAdapter();

        //    string json = "";
        //    using (System.IO.StreamReader fs = new System.IO.StreamReader(@"C:\Users\edalton\Desktop\Test.txt"))
        //    {
        //        json = fs.ReadToEnd();
        //        fs.Close();
        //    }
        //    BHoM.Global.Project.FromJSON(json);
        //    List<string> ids = new List<string>();
        //    app.SetPanels(new BHoM.Base.ObjectFilter<Panel>().ToList(), out ids);
        //}

        //private static void SetBars()
        //{
        //    RobotAdapter app = new RobotAdapter();

        //    string json = "";
        //    using (System.IO.StreamReader fs = new System.IO.StreamReader(@"C:\Users\edalton\Desktop\Test.txt"))
        //    {
        //        json = fs.ReadToEnd();
        //        fs.Close();
        //    }
        //    BHoM.Global.Project.FromJSON(json);
        //    List<string> ids = new List<string>();

        //    app.CreateBars(new BHoM.Base.ObjectFilter<Bar>().ToList(), out ids);
        //}
    }
}
