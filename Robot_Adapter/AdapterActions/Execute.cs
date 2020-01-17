/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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

using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using BH.oM.Structure.Loads;
using RobotOM;
using BH.oM.Adapter;
using BH.oM.Reflection;
using BH.oM.Adapter.Commands;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Adapter Methods                 ****/
        /***************************************************/

        public override Output<List<object>, bool> Execute(IExecuteCommand command, ActionConfig actionConfig = null)
        {
            var output = new Output<List<object>, bool>() { Item1 = null, Item2 = false };

            output.Item2 = RunCommand(command as dynamic);

            return output;
        }

        /***************************************************/

        public bool RunCommand(NewModel command)
        {
            m_RobotApplication.Interactive = 1;
            m_RobotApplication.Project.New(IRobotProjectType.I_PT_SHELL);
            return true;
        }

        /***************************************************/

        public bool RunCommand(Close command)
        {
            m_RobotApplication.Quit(IRobotQuitOption.I_QO_PROMPT_TO_SAVE_CHANGES);
            return true;
        }

        /***************************************************/

        public bool RunCommand(Save command)
        {
            m_RobotApplication.Project.Save();
            return true;
        }

        /***************************************************/

        public bool RunCommand(SaveAs command)
        {
            m_RobotApplication.Project.SaveAs(command.FileName);
            return true;
        }

        /***************************************************/

        public bool RunCommand(AnalyseLoadCases command)
        {
            return Analyse(command.LoadCases);
        }

        /***************************************************/

        public bool RunCommand(Analyse command)
        {
            return Analyse();
        }

        /***************************************************/

        public bool RunCommand(IExecuteCommand command)
        {
            Engine.Reflection.Compute.RecordWarning($"The command {command.GetType().Name} is not supported by this Adapter.");
            return false;
        }

        /***************************************************/
        /****   Private Support Methods                 ****/
        /***************************************************/

        private bool Analyse(IList cases = null)
        {
            RobotSelection rSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            int index = m_RobotApplication.Project.Structure.Cases.FreeNumber;

            if (cases == null || cases.Count < 1)
            {

                if (index > 2)
                    rSelection.FromText("1to" + (index - 1).ToString());
                else
                    rSelection.FromText("1");
                SetAux(rSelection, false);

                rSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            }
            else
            {
                List<int> caseNums = GetCaseNumbers(cases);
                string str = "";
                for (int i = 1; i < index; i++)
                {
                    if (!caseNums.Contains(i))
                        str = str + i.ToString() + ",";
                }
                str.TrimEnd(',');
                rSelection.FromText(str);
                SetAux(rSelection, true);
            }

            m_RobotApplication.UserControl = true;
            m_RobotApplication.Interactive = 1;
            m_RobotApplication.Project.CalcEngine.AnalysisParams.IgnoreWarnings = true;
            m_RobotApplication.Project.CalcEngine.AnalysisParams.AutoVerification = IRobotStructureAutoVerificationType.I_SAVT_NONE;
            m_RobotApplication.Project.CalcEngine.Calculate();
            m_RobotApplication.Project.CalcEngine.AutoFreezeResults = false;

            return true;
        }

        /***************************************************/

        private List<int> GetCaseNumbers(IList cases)
        {
            List<int> caseNums = new List<int>();

            if (cases is List<string>)
                return (cases as List<string>).Select(x => int.Parse(x)).ToList();
            else if (cases is List<int>)
                return cases as List<int>;
            else if (cases is List<double>)
                return (cases as List<double>).Select(x => (int)Math.Round(x)).ToList();

            else if (cases is List<Loadcase>)
            {
                for (int i = 0; i < cases.Count; i++)
                {
                    caseNums.Add(System.Convert.ToInt32((cases[i] as Loadcase).Number));
                }
            }
            else if (cases is List<LoadCombination>)
            {
                foreach (object lComb in cases)
                {
                    foreach (Tuple<double, ICase> lCase in (lComb as LoadCombination).LoadCases)
                    {
                        caseNums.Add(System.Convert.ToInt32(lCase.Item2.Number));
                    }
                    caseNums.Add(System.Convert.ToInt32((lComb as LoadCombination).CustomData[AdapterIdName]));
                }
            }          

            else
            {
                List<int> idsOut = new List<int>();
                foreach (object o in cases)
                {
                    int id;
                    if (int.TryParse(o.ToString(), out id))
                    {
                        idsOut.Add(id);
                    }
                }
                return idsOut;
            }


            return caseNums;
        }

        /***************************************************/

        private void SetAux(RobotSelection CSelection, bool yn)
        {
            RobotCaseCollection Caux = m_RobotApplication.Project.Structure.Cases.GetMany(CSelection);
            RobotCaseServer CServer = m_RobotApplication.Project.Structure.Cases;

            for (int i = 1; i <= Caux.Count; i++)
            {
                CServer.SetAuxiliary(Caux.Get(i).Number, yn);
            }
        }

        /***************************************************/
    }
}
