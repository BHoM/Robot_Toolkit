using BH.oM.Geometry;
using BH.oM.Common.Materials;
using GeometryEngine = BH.Engine.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Loads;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        //public static int ReadLoadNature(Loadcase lCase, IRobotCaseServer caseServer)
        //{
        //    IRobotCase rCase = caseServer.CreateSimple(10000000, "", (IRobotCaseNature)2, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
        //    RobotCodeCmbGenerationParams RCCGP = caseServer.CodeCmbEngine.Params;
        //    string[] names = new string[6];

        //    for (int i = 0; i < names.Length; i++)
        //    {
        //        rCase.Nature = (IRobotCaseNature)i;
        //        names[i] = rCase.Nature.ToString();
        //    }

        //    caseServer.Delete(10000000);
        //    int numOfNatures = RCCGP.Regulations.Actions.Count;

        //    string[] subNatures = new string[numOfNatures];
        //    string[] subNames = new string[numOfNatures];

        //    for (int i = 0; i < numOfNatures; i++)
        //    {
        //        subNames[i] = RCCGP.Regulations.Actions.GetName(i);

        //        if (subNames[i] == "")
        //            subNames[i] = names[(int)RCCGP.Regulations.Actions.GetNature(i)];

        //        subNatures[i] = RCCGP.Regulations.Actions.GetNature(i).ToString();
        //    }
        //}
    }
}
