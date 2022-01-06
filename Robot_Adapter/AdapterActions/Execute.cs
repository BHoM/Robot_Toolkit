/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
        /**** IAdapter Interface                        ****/
        /***************************************************/

        public override Output<List<object>, bool> Execute(IExecuteCommand command, ActionConfig actionConfig = null)
        {
            var output = new Output<List<object>, bool>() { Item1 = null, Item2 = false };

            output.Item2 = RunCommand(command as dynamic);

            return output;
        }

        /***************************************************/
        /**** Commands                                  ****/
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
            if (command.SaveBeforeClose)
                m_RobotApplication.Project.Save();

            m_RobotApplication.Project.Close();
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

        public bool RunCommand(Exit command)
        {
            if (command.SaveBeforeClose)
            {
                if (string.IsNullOrEmpty(m_RobotApplication.Project.FileName))
                {
                    Engine.Reflection.Compute.RecordError($"Application not exited. File does not have a name. Please manually save the file or use the {nameof(SaveAs)} command before trying to Exit the application. If you want to close the application anyway, please toggle {nameof(Exit.SaveBeforeClose)} to false.");
                    return false;
                }
                m_RobotApplication.Project.Save();
            }

            m_RobotApplication.Project.Close();

            m_RobotApplication.Quit(IRobotQuitOption.I_QO_DISCARD_CHANGES);
            m_RobotApplication = null;
            return true;
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

        private bool Analyse(IEnumerable<object> cases = null)
        {
            RobotSelection rSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            int index = m_RobotApplication.Project.Structure.Cases.FreeNumber;


            RobotCaseCollection rCases = m_RobotApplication.Project.Structure.Cases.GetAll();
            if (rCases.Count != 0)
                index = (rCases.Get(rCases.Count) as IRobotCase).Number;
            else
                index = 1;

            if (index > 2)
                rSelection.FromText("1to" + (index).ToString());
            else
                rSelection.FromText("1");
            SetAux(rSelection, false);

            rSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);

            if (cases != null && cases.Count() > 0)
            {
                List<int> caseNums = GetCaseNumbers(cases);
                string str = "";
                for (int i = 1; i <= index; i++)
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

        private List<int> GetCaseNumbers(IEnumerable<object> cases)
        {
            List<int> caseNums = new List<int>();

            foreach (object o in cases)
            {
                int id;
                if (o is Loadcase)
                {
                    caseNums.Add((o as Loadcase).Number);
                }
                else if (o is LoadCombination)
                {
                    LoadCombination lComb = (o as LoadCombination);
                    foreach (Tuple<double, ICase> lCase in (lComb as LoadCombination).LoadCases)
                    {
                        caseNums.Add(System.Convert.ToInt32(lCase.Item2.Number));
                    }
                    caseNums.Add(GetAdapterId<int>(lComb));
                }
                else if (o is int)
                {
                    caseNums.Add((int)o);
                }
                else if (int.TryParse(o.ToString(), out id))
                {
                    caseNums.Add(id);
                }
                else
                {
                    Engine.Reflection.Compute.RecordWarning("Could not extract case information from object " + o.ToString() + ". Case information need to be provided as Loadcases, Loadcombinations or as case numbers (string or int)");
                }
            }
            return caseNums;
            
        }

        /***************************************************/

        private void SetAux(RobotSelection cSelection, bool yn)
        {
            RobotCaseCollection Caux = m_RobotApplication.Project.Structure.Cases.GetMany(cSelection);
            RobotCaseServer CServer = m_RobotApplication.Project.Structure.Cases;

            for (int i = 1; i <= Caux.Count; i++)
            {
                CServer.SetAuxiliary(Caux.Get(i).Number, yn);
            }
        }

        /***************************************************/
    }
}



