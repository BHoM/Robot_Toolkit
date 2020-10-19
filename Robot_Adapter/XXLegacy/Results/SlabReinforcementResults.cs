/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

//using BH.oM.Base.Results;
//using BH.oM.Structure.Interface;
//using BH.oM.Structure.Results;
//using Robot_Adapter.Base;
//using RobotOM;
using BH.Engine.Adapter;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Robot_Adapter.Structural.Results
//{
//    public class SlabReinforcementResults
//    {
//        public static bool GetSlabReinforcement(RobotApplication Robot, ResultServer<SlabReinforcement> resultServer, List<string> panelNumbers, List<string> loadcaseNumbers)
//        {
//            IRobotObjObjectServer server = Robot.Project.Structure.Objects;
            
//            //RConcrSlabRequiredReinfCalcParams FEParams = new RConcrSlabRequiredReinfCalcParams();
//            RobotFeResultServer feserver = Robot.Project.Structure.Results.FiniteElems;
//            //RobotFeResultParams FEParams = new RobotFeResultParams();
            
//            //RobotSelection panelSelection = selection == ObjectSelection.Selected ?
//            //Robot.Project.Structure.Selections.Get(IRobotObjectType.I_OT_PANEL) :
//            //Robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);

//            RobotSelection panelSelection = Robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);
//            //RobotSelection panelSelection = Robot.Project.Structure.Selections.Get(IRobotObjectType.I_OT_PANEL);
//            RobotSelection nodeSelection = Robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_NODE);
            
//            //RobotResultQueryParams queryParams = (RobotResultQueryParams)Robot.Kernel.CmpntFactory.Create(IRobotComponentType.I_CT_RESULT_QUERY_PARAMS);

//            if (panelNumbers == null || panelNumbers.Count == 0)
//            {
//                panelSelection.FromText("all");
//            }
//            else
//            {
//                panelSelection.FromText(Utils.GetSelectionString(panelNumbers));
//            }

//            IRobotCollection panelCollection = Robot.Project.Structure.Objects.GetMany(panelSelection);

            
//            //panelSelection = null;
//            Robot.Project.Structure.Objects.BeginMultiOperation();

//            List<SlabReinforcement> reinforcementLevel = new List<SlabReinforcement>();

//            for (int i = 1; i <= panelCollection.Count; i++)
//            {
//                RobotObjObject rpanel = (RobotObjObject)panelCollection.Get(i);
//                nodeSelection.FromText(rpanel.Nodes);
//                RobotNodeCollection nodeCollection = (RobotNodeCollection)Robot.Project.Structure.Nodes.GetMany(nodeSelection);

//                List<int> coll = Utils.GetNumbersFromText(rpanel.Nodes);

//                double x = 0;
//                double y = 0;
//                double z = 0;

//                int loadcase = -1;
//                int timeStep = 1;
                
//                rpanel.Main.Attribs.GetDirX(out x, out y, out z);
//                BH.oM.Geometry.Vector dirX = new BH.oM.Geometry.Vector(x, y, z);

//                for (int j = 0; j < coll.Count; j++)
//                {

//                    //Parallel.For(1, nodeSelection.Count, j =>
//                    //{


//                    //for (int j = 1; j <= nodeSelection.Count; j++)
//                    //{
//                    //RobotNode rnode = (RobotNode)nodeCollection.Get(j);

//                    RobotFeResultReinforcement reinfor = feserver.Reinforcement(rpanel.Number, coll[j]);

//                    double axP = reinfor.AX_TOP * 10000;
//                    double axM = reinfor.AX_BOTTOM * 10000;
//                    double ayP = reinfor.AY_TOP * 10000;
//                    double ayM = reinfor.AY_BOTTOM * 10000;

//                    //double axP = Robot.Project.Structure.Results.FiniteElems.Reinforcement(rpanel.Number, j).AX_TOP;
//                    //double axM = Robot.Project.Structure.Results.FiniteElems.Reinforcement(rpanel.Number, j).AX_BOTTOM;
//                    //double ayP = Robot.Project.Structure.Results.FiniteElems.Reinforcement(rpanel.Number, j).AY_TOP;
//                    //double ayM = Robot.Project.Structure.Results.FiniteElems.Reinforcement(rpanel.Number, j).AY_BOTTOM;

//                    reinforcementLevel.Add(new SlabReinforcement(rpanel.Number, coll[j], loadcase, timeStep, axP, axM, ayP, ayM));
//                    //}
//                }
//                // });
//            }
//            Robot.Project.Structure.Objects.EndMultiOperation();
//            resultServer.StoreData(reinforcementLevel);
//            return true;
          
//            //List<int> results = new List<int>();
//            //results.Add((int)IRobotFeResultType.I_FRT_REINF_E_AX_TOP);
//            //results.Add((int)IRobotFeResultType.I_FRT_REINF_E_AX_BOTTOM);
//            //results.Add((int)IRobotFeResultType.I_FRT_REINF_E_AY_TOP);
//            //results.Add((int)IRobotFeResultType.I_FRT_REINF_E_AY_BOTTOM);

//            //queryParams.ResultIds.SetSize(results.Count);

//            //for (int i = 0; i < results.Count; i++)
//            //{
//                //int id = (int)results[i];
//                //queryParams.ResultIds.Set(i + 1, id);
//            //}

//            // Creating RobotSelection for loadcases and panels
//            //RobotSelection caseSelection = Robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
//            //RobotSelection panelSelection = Robot.Project.Structure.Selections.Create(IRobotObjectType.I_OT_PANEL);

//            //if (panelNumbers == null || panelNumbers.Count == 0) panelSelection.FromText("all"); else panelSelection.FromText(Utils.GetSelectionString(panelNumbers));            

//            //queryParams.Selection.Set(IRobotObjectType.I_OT_PANEL, panelSelection);
//            //queryParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSelection);
//            //queryParams.SetParam(IRobotResultParamType.I_RPT_MULTI_THREADS, true);
//            //queryParams.SetParam(IRobotResultParamType.I_RPT_REINFORCE_CALC_METHOD, IRConcrReinforceCalcType.I_CRCT_BENDING_COMPRESSION_TENSION);
//            //queryParams.SetParam(IRobotResultParamType.I_RPT_THREAD_COUNT, 4);
//            //queryParams.SetParam(IRobotResultParamType.I_RPT_SMOOTHING, IRobotFeResultSmoothing.I_FRS_SMOOTHING_WITHIN_A_PANEL); // TODO: check furthur with eddie
//            //queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X_DEFTYPE, IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN);
//            //queryParams.SetParam(IRobotResultParamType.I_RPT_DIR_X, new double[] { 1, 0, 0 }); // TODO: check the outcome later and discuss with eddie

//            //RobotResultRowSet rowSet = new RobotResultRowSet();
//            //IRobotResultQueryReturnType ret = IRobotResultQueryReturnType.I_RQRT_MORE_AVAILABLE;

//            //int counter = 0;


//            //while (ret != IRobotResultQueryReturnType.I_RQRT_DONE)
//            //{
//            //    ret = Robot.Kernel.Structure.Results.Query(queryParams, rowSet);

//            //    bool isOk = rowSet.MoveFirst();
//            //    while (isOk)
//            //    {
//            //        List<double> line = new List<double>();

//            //        RobotResultRow row = rowSet.CurrentRow;
//            //        int idCase = -1; //(int)row.GetParam(IRobotResultParamType.I_RPT_LOAD_CASE);
//            //        int idPanel = (int)row.GetParam(IRobotResultParamType.I_RPT_PANEL);
//            //        int idNode = (int)row.GetParam(IRobotResultParamType.I_RPT_NODE);
                    
//            //        line.Add(idPanel);
//            //        line.Add(idNode);
//            //        line.Add(idCase);

//            //        double Axp = (double)row.GetValue((int)IRobotFeResultType.I_FRT_REINF_E_AX_TOP);
//            //        double Axm = (double)row.GetValue((int)IRobotFeResultType.I_FRT_REINF_E_AX_BOTTOM);
//            //        double Ayp = (double)row.GetValue((int)IRobotFeResultType.I_FRT_REINF_E_AY_TOP);
//            //        double Aym = (double)row.GetValue((int)IRobotFeResultType.I_FRT_REINF_E_AY_BOTTOM);

//            //        BH.oM.Geometry.Vector dirX = null;                   

//            //        reinforcementLevel.Add(new SlabReinforcement(idPanel, idNode, idCase,1, Axm, Axp, Aym, Ayp, dirX)); // WhatAbout timestep?? Ask eddie
//            //        isOk = rowSet.MoveNext();
//            //        counter++;

//            //        if (counter % 1000000 == 0 && resultServer.CanStore)
//            //        {
//            //            resultServer.StoreData(reinforcementLevel);
//            //            reinforcementLevel.Clear();
//            //        }
//            //    }
//            //}
//            //resultServer.StoreData(reinforcementLevel);
//            //return true;
//        }
//    }
//}
