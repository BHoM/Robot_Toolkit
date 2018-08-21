using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using BH.oM.Structure.Loads;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Adapter Methods                 ****/
        /***************************************************/

        public override bool Execute(string command, Dictionary<string, object> parameters = null, Dictionary<string, object> config = null)
        {
            string commandUpper = command.ToUpper();

            if (commandUpper == "CLOSE")
                return Close();

            else if (commandUpper == "SAVE")
            {
                string fileName = default(string);
                string[] fileNameStringAlt = {
                    "Filename",
                    "File name",
                    "File_name",
                    "filename",
                    "file name",
                    "file_name",
                    "FileName",
                    "File Name",
                    "File_Name",
                    "FILENAME",
                    "FILE NAME",
                    "FILE_NAME"
                };
                foreach (string str in fileNameStringAlt)
                {
                    if (parameters.ContainsKey(str))
                    {
                        fileName = (string)parameters[str];
                        break;
                    }
                }
                return Save(fileName);
            }

            //else if (commandUpper == "CLEARRESULTS" || commandUpper == "DELETERESULTS")
            //{
            //    return ClearResults();
            //}

            else if (commandUpper == "ANALYSE" || commandUpper == "RUN")
            {
                IList cases = null;
                string[] caseStringAlt =
                {
                    "Cases",
                    "CASES",
                    "cases",
                    "LoadCases",
                    "LOADCASES",
                    "loadcases",
                    "Loadcases",
                    "Load Cases",
                    "LOAD CASES",
                    "load cases",
                    "Load cases",
                    "Load_Cases",
                    "LOAD_CASES",
                    "load_cases",
                    "Load_cases"
                };
                foreach (string str in caseStringAlt)
                {
                    object obj;
                    if (parameters.TryGetValue(str, out obj))
                    {
                        cases = obj as IList;
                        break;
                    }
                }
                return Analyse(cases);
            }

            else
                return false;
        }

        /***************************************************/

        public bool Close()
        {
            m_RobotApplication.Quit(IRobotQuitOption.I_QO_PROMPT_TO_SAVE_CHANGES);
            return true;
        }

        /***************************************************/

        public bool Save(string fileName = null)
        {
            if (fileName == null)
            {
                m_RobotApplication.Project.Save();
                return true;
            }
            else
            {
                m_RobotApplication.Project.SaveAs(fileName);
                return true;
            }
        }

        /***************************************************/

        public bool Analyse(IList cases = null)
        {
            RobotSelection rSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            int index = m_RobotApplication.Project.Structure.Cases.FreeNumber;
            if(index > 2)
                rSelection.FromText("1to" + (index - 1).ToString());
            else
                rSelection.FromText("1");
            SetAux(rSelection, false);

            rSelection = m_RobotApplication.Project.Structure.Selections.Create(IRobotObjectType.I_OT_CASE);
            
            if (cases != null && cases.Count > 0)
            {
                List<int> caseNums = GetCases(cases);
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

        private List<int> GetCases(IList cases)
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
                    caseNums.Add(System.Convert.ToInt32((lComb as LoadCombination).CustomData[AdapterId]));
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

        public void SetAux(RobotSelection CSelection, bool yn)
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
