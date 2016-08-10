using BHoM.Base.Results;
using BHoM.Structural.Results;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot_Adapter.Structural.Results
{
    public static class NodeResults
    {
        public static bool LoadNodeReacions(RobotApplication RobotApp, ResultServer<NodeReaction> resultServer, List<string> nodeNumbers, List<string> loadcaseNumber)
        {
            RobotReactionServer reactions = RobotApp.Project.Structure.Results.Nodes.Reactions;
            Dictionary<string, int> loadCaseNametoID = new Dictionary<string, int>();
            List<NodeReaction> nodeReactions = new List<NodeReaction>();

            for (int lC = 0; lC < loadcaseNumber.Count; lC++)
            {
                int loadCase = 0;

                if (!int.TryParse(loadcaseNumber[lC], out loadCase))
                {
                    if (loadCaseNametoID.Keys.Count == 0)
                    {
                        RobotCaseCollection collection = RobotApp.Project.Structure.Cases.GetAll();
                        for (int i = 1; i <= collection.Count; i++)
                        {
                            IRobotCase currentCase = collection.Get(i) as IRobotCase;
                            loadCaseNametoID.Add(currentCase.Name, currentCase.Number);
                        }
                    }
                    else
                    {
                        if (!loadCaseNametoID.TryGetValue(loadcaseNumber[lC], out loadCase))
                        {
                            throw new Exception("Loadcase: \"" + loadcaseNumber[lC] + "\" does not exist");
                        }
                    }
                }

                for (int i = 0; i < nodeNumbers.Count; i++)
                {
                    int node = int.Parse(nodeNumbers[i]);
                    RobotReactionData reactionData = reactions.Value(node, loadCase);
                    nodeReactions.Add(new NodeReaction(node, loadCase, 0, reactionData.FX, reactionData.FY, reactionData.FZ, reactionData.MX, reactionData.MY, reactionData.MZ));                  
                }
            }
            return true;
        }
    }
}
