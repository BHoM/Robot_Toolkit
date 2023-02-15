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

namespace BH.Tests.Adapter.Robot
{
    public class PullTests
    {
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

        [Test]
        public void PullBarsWithTag()
        {
            //Checks correctly extracting all bars in the Robot group corresponding to the tag
            FilterRequest request = new FilterRequest { Type = typeof(Bar), Tag = "Bar group 1" };

            List<Bar> readBars = m_Adapter.Pull(request).Cast<Bar>().ToList();

            readBars.Count.ShouldBe(7, "Wrong number of Bars pulled compared to expected count.");
        }

        [Test]
        public void PullPanelsWithTag()
        {
            //Checks correctly extracting all Panels in the Robot group corresponding to the tag
            FilterRequest request = new FilterRequest { Type = typeof(Panel), Tag = "Panel Group 1" };

            List<Panel> readPanels = m_Adapter.Pull(request).Cast<Panel>().ToList();

            readPanels.Count.ShouldBe(2, "Wrong number of Panels pulled compared to expected count.");

        }

        [Test]
        public void PullNodesWithTag()
        {
            //Checks correctly extracting all Nodes in the Robot group corresponding to the tag
            FilterRequest request = new FilterRequest { Type = typeof(Node), Tag = "Node group 1" };

            List<Node> readNodes = m_Adapter.Pull(request).Cast<Node>().ToList();

            readNodes.Count.ShouldBe(50, "Wrong number of Nodes pulled compared to expected count.");
        }
    }
}