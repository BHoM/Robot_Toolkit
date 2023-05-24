/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

using NUnit.Framework;
using BH.oM.Base;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using System.Diagnostics.Contracts;
using BH.Adapter.Robot;
using BH.oM.Data.Requests;
using BH.oM.Adapters.Robot;
using BH.oM.Analytical.Results;
using System;
using Shouldly;
using BH.oM.Adapter.Commands;
using System.Reflection;
using BH.oM.Structure.Requests;
using BH.oM.Structure.Results.Nodal_Results;
using System.Security.Cryptography.X509Certificates;
using BH.oM.Structure.Results;

namespace BH.Tests.Adapter.Robot
{
    public class PullTests
    {
        /***************************************************/
        /**** Public methods - setup                    ****/
        /***************************************************/

        RobotAdapter m_Adapter;
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            //Starts the RobotAdapter and opens the model file stored in the .ci folder
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            List<string> splitPath = currentDirectory.Split('\\').ToList();
            splitPath = splitPath.Take(splitPath.IndexOf(".ci") + 2).ToList();
            string modelPath = Path.Join(string.Join("\\", splitPath), "Models", "Simple 2-story structure with results.rtd");
            m_Adapter = new RobotAdapter(modelPath, null, true);
            //Forces Analytical_Engine to be loaded up. This is due to some parts of the call-chain not hard wired. This ensures that methods from Analytical_Engine are loaded and usable from RunExtensionMethod
            Assembly.Load("Analytical_Engine");
        }

        [SetUp]
        public void Setup()
        {
            BH.Engine.Base.Compute.ClearCurrentEvents();
        }

        [TearDown]
        public void TearDown()
        {
            var events = BH.Engine.Base.Query.CurrentEvents();
            if (events.Any())
            {
                Console.WriteLine("BHoM events raised during execution:");
                foreach (var ev in events)
                {
                    Console.WriteLine($"{ev.Type}: {ev.Message}");
                }
            }
        }

        /***************************************************/
        /**** Public methods - tests                    ****/
        /***************************************************/

        [Test]
        public void PullBarsWithTag()
        {
            //Checks correctly extracting all bars in the Robot group corresponding to the tag
            FilterRequest request = new FilterRequest { Type = typeof(Bar), Tag = "Bar group 1" };

            List<Bar> readBars = m_Adapter.Pull(request).Cast<Bar>().ToList();

            readBars.Count.ShouldBe(7, "Wrong number of Bars pulled compared to expected count.");
        }

        /***************************************************/

        [Test]
        public void PullPanelsWithTag()
        {
            //Checks correctly extracting all Panels in the Robot group corresponding to the tag
            FilterRequest request = new FilterRequest { Type = typeof(Panel), Tag = "Panel Group 1" };

            List<Panel> readPanels = m_Adapter.Pull(request).Cast<Panel>().ToList();

            readPanels.Count.ShouldBe(2, "Wrong number of Panels pulled compared to expected count.");

        }

        /***************************************************/

        [Test]
        public void PullNodesWithTag()
        {
            //Checks correctly extracting all Nodes in the Robot group corresponding to the tag
            FilterRequest request = new FilterRequest { Type = typeof(Node), Tag = "Node group 1" };

            List<Node> readNodes = m_Adapter.Pull(request).Cast<Node>().ToList();

            readNodes.Count.ShouldBe(50, "Wrong number of Nodes pulled compared to expected count.");
        }

        /***************************************************/

        [Test]
        public void PullGlobalReactions()
        {
            //Pull global results
            GlobalResultRequest globalRequest = new GlobalResultRequest { ResultType = GlobalResultType.Reactions };
            List<GlobalReactions> globRes = m_Adapter.Pull(globalRequest).Cast<GlobalReactions>().ToList();

            globRes.Count.ShouldBe(4);

            double tolerance = 1e-6;
            ValidateCase(globRes, 1, 6.04850356467068E-08, -1.61599018611013E-08, 6930724.87216325, 93157700.9778047, -72529973.9008961, -42.6532396448892, tolerance);
            ValidateCase(globRes, 2, -1.94105496120755E-08, 6.73935574013739E-10, 1440000.00000009, 23040000.239115, -12799997.8446157, -43.4173608493729, tolerance);
            ValidateCase(globRes, 3, 2.11014992146374E-07, -149999.999999873, 2.241768015665E-08, 314999.428178589, 0.00382923203142127, -1680013.62184307, tolerance);
            ValidateCase(globRes, 5, 2.52089478181005E-07, -149999.999999888, 8370724.87216337, 116512700.645098, -85329971.7416826, -1680099.69244356, tolerance);
        }

        /***************************************************/

        [Test]
        public void PullModalResults()
        {
            //Pull nodal modal results
            NodeResultRequest request = new NodeResultRequest { ResultType = NodeResultType.NodeModalResult };

            List<NodeModalResults> nodeModalRes = m_Adapter.Pull(request).Cast<NodeModalResults>().ToList();

            //Ensure all results are normalised the same way
            nodeModalRes.ShouldAllBe(x => x.NormalisationProcedure == nodeModalRes.First().NormalisationProcedure, "All results should have the same normalisation procedure.");

            double tol = 1e9;

            foreach (var caseGroup in nodeModalRes.GroupBy(x => new { x.ResultCase, x.ModeNumber }))
            {
                //Disp values for debugging purposes
                Console.WriteLine($"Case {caseGroup.Key.ResultCase}. Mode nb: {caseGroup.Key.ModeNumber}");
                Console.WriteLine($"Max mode disp tot: {OrderByAndDispNodeAndValue(caseGroup, x => TotalModalDisp(x))}");
                Console.WriteLine($"Max mode disp component: {OrderByAndDispNodeAndValue(caseGroup, x => MaxDispComponent(x))}");
                Console.WriteLine($"Max mode disp tot: {OrderByAndDispNodeAndValue(caseGroup, x => TotalModalRot(x))}");
                Console.WriteLine($"Max mode disp component: {OrderByAndDispNodeAndValue(caseGroup, x => MaxRotComponent(x))}");
                Console.WriteLine($"Max mode mass tot: {OrderByAndDispNodeAndValue(caseGroup, x => TotalModalMass(x))}");
                Console.WriteLine($"Max mode mass component: {OrderByAndDispNodeAndValue(caseGroup, x => MaxMassComponent(x))}");

                //Check results for each node are pulled
                caseGroup.Count().ShouldBe(980, "Wrong number of Nodes pulled compared to expected count.");

                ModalResultNormalisation normalisation = caseGroup.First().NormalisationProcedure;
                //Check that the results ahve been normalised as stated by the procedure
                switch (normalisation)
                {
                    case ModalResultNormalisation.EigenvectorComponent:
                        caseGroup.Max(x => MaxDispComponent(x)).ShouldBe(1, tol, $"Case {caseGroup.Key.ResultCase}. Mode nb: {caseGroup.Key.ModeNumber}");
                        break;
                    case ModalResultNormalisation.EigenvectorTotal:
                        caseGroup.Max(x => TotalModalDisp(x)).ShouldBe(1, tol, $"Case {caseGroup.Key.ResultCase}. Mode nb: {caseGroup.Key.ModeNumber}");
                        break;
                    case ModalResultNormalisation.MassComponent:
                        caseGroup.Max(x => MaxMassComponent(x)).ShouldBe(1, tol, $"Case {caseGroup.Key.ResultCase}. Mode nb: {caseGroup.Key.ModeNumber}");
                        break;
                    case ModalResultNormalisation.MassTotal:
                        caseGroup.Max(x => TotalModalMass(x)).ShouldBe(1, tol, $"Case {caseGroup.Key.ResultCase}. Mode nb: {caseGroup.Key.ModeNumber}");
                        break;
                    default:
                        break;
                }
            }

            //Pull global results
            GlobalResultRequest globalRequest = new GlobalResultRequest { ResultType = GlobalResultType.ModalMassAndFrequency };
            List<ModalMassAndFrequency> globRes = m_Adapter.Pull(globalRequest).Cast<ModalMassAndFrequency>().ToList();


            foreach (ModalMassAndFrequency res in globRes)
            {
                //Display value for debugging purposes
                Console.WriteLine($"Case: {res.ResultCase}");
                Console.WriteLine($"Mode: {res.ModeNumber}");
                Console.WriteLine($"Frequency: {res.Frequency}");
                Console.WriteLine($"Mass x: {res.MassX}");
                Console.WriteLine($"Mass y: {res.MassY}");
                Console.WriteLine($"Mass z: {res.MassZ}");
                Console.WriteLine("");
            }

            //Loop through and check that there is matching correspondence between nodal and global results
            foreach (var caseGroup in nodeModalRes.GroupBy(x => new { x.ResultCase, x.ModeNumber }))
            {
                //Find matching result by matching case and mode number
                ModalMassAndFrequency match = globRes.FirstOrDefault(x => x.ResultCase.Equals(caseGroup.Key.ResultCase) && x.ModeNumber == caseGroup.Key.ModeNumber);
                match.ShouldNotBeNull($"Cannot find a global result matching node result for case {caseGroup.Key.ResultCase} and mode {caseGroup.Key.ModeNumber}.");

                //Ensure masses add up to the same for nodal and global results
                match.MassX.ShouldBe(caseGroup.Sum(x => x.NodalMassX), tol);
                match.MassY.ShouldBe(caseGroup.Sum(x => x.NodalMassY), tol);
                match.MassZ.ShouldBe(caseGroup.Sum(x => x.NodalMassZ), tol);
            }
        }

        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/

        private static void ValidateCase(List<GlobalReactions> globRes, int caseNb, double fx, double fy, double fz, double mx, double my, double mz, double tolerance)
        {
            globRes.ShouldContain(x => x.ResultCase.Equals(caseNb));
            GlobalReactions res = globRes.FirstOrDefault(x => x.ResultCase.Equals(caseNb));
            res.FX.ShouldBe(fx, tolerance, $"Case: {res.ResultCase}");
            res.FY.ShouldBe(fy, tolerance, $"Case: {res.ResultCase}");
            res.FZ.ShouldBe(fz, tolerance, $"Case: {res.ResultCase}");

            res.MX.ShouldBe(mx, tolerance, $"Case: {res.ResultCase}");
            res.MY.ShouldBe(my, tolerance, $"Case: {res.ResultCase}");
            res.MZ.ShouldBe(mz, tolerance, $"Case: {res.ResultCase}");
        }

        /***************************************************/

        private static string OrderByAndDispNodeAndValue(IEnumerable<NodeModalResults> results, Func<NodeModalResults, double> func)
        {
            NodeModalResults maxRes = results.OrderByDescending(func).FirstOrDefault();
            if (maxRes == null)
                return "";
            return $"val: {func(maxRes)}, Id: {maxRes.ObjectId}";
        }

        /***************************************************/

        private static double TotalModalDisp(NodeModalResults res)
        {
            return Math.Sqrt(res.UX * res.UX + res.UY * res.UY + res.UZ * res.UZ);
        }

        /***************************************************/

        private static double MaxDispComponent(NodeModalResults res)
        {
            return Math.Max(Math.Max(Math.Abs(res.UX), Math.Abs(res.UY)), Math.Abs(res.UZ));
        }

        /***************************************************/

        private static double TotalModalRot(NodeModalResults res)
        {
            return Math.Sqrt(res.RX * res.RX + res.RY * res.RY + res.RZ * res.RZ);
        }

        /***************************************************/

        private static double MaxRotComponent(NodeModalResults res)
        {
            return Math.Max(Math.Max(Math.Abs(res.RX), Math.Abs(res.RY)), Math.Abs(res.RZ));
        }

        /***************************************************/

        private static double TotalModalMass(NodeModalResults res)
        {
            return Math.Sqrt(res.NodalMassX * res.NodalMassX + res.NodalMassY * res.NodalMassY + res.NodalMassZ * res.NodalMassZ);
        }

        /***************************************************/

        private static double MaxMassComponent(NodeModalResults res)
        {
            return Math.Max(Math.Max(res.NodalMassX, res.NodalMassY), res.NodalMassZ);
        }

        /***************************************************/
    }
}