using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;

namespace Robot_Adapter.Results
{
    class Global
    {
        public void GetGlobalResults(
            BHoM.Structural.Loads.Loadcase loadcase,
            out BHoM.Structural.Results.GlobalResult globalResult,
            string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();

            globalResult = new BHoM.Structural.Results.GlobalResult(loadcase);
            RobotResultServer robotResult = robot.Project.Structure.Results;
            RobotMassSumServer robotMass = robotResult.Advanced.MassSum;
            RobotStructureValues robotValues = robotResult.Total;
            //RobotReactionData robotReactions = robotResult.Nodes.Reactions.Sum(loadcase.Number);

           // globalResult.SetReactions(robotReactions.FX, robotReactions.FY, robotReactions.FZ, robotReactions.MX, robotReactions.MY, robotReactions.MZ);
            //globalResult.SetSumOfMass(robotValues.GetMass(loadcase.Number));
           // globalResult.SetBaseShear(robotReactions.FX * 9.81 / globalResult.SumOfMass, robotReactions.FY * 9.81 / globalResult.SumOfMass);
        }
    }
}
