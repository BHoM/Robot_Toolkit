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
using BH.oM.Adapter.Commands;
using System.Reflection;
using System.Collections;
using BH.oM.Geometry;
using BH.Engine.Geometry;

namespace BH.Tests.Adapter.Robot
{
    public class PushTests
    {
        RobotAdapter m_Adapter;

        [OneTimeSetUp]
        public void OntimeSetup()
        {
            m_Adapter = new RobotAdapter("", null, true);
        }

        [SetUp]
        public void Setup()
        {
            m_Adapter.Execute(new NewModel());
        }

        [TearDown]
        public void TearDown() 
        {
            m_Adapter.Execute(new Close { SaveBeforeClose = false });
        }

        [Test]
        public void PushBarsWithTag()
        {
            //Generate some random bars
            int count = 3;
            List<Bar> bars = new List<Bar>();
            for (int i = 0; i < count; i++)
            {
                bars.Add(Engine.Base.Create.RandomObject(typeof(Bar), i) as Bar);
            }

            List<string> tags = new List<string> { "Tag1", "SomeRandomTag", "Tag2" };
            //Add some random tags
            foreach (Bar bar in bars)
            {
                foreach (string tag in tags)
                {
                    bar.Tags.Add(tag);
                }
            }

            //Push the bars
            m_Adapter.Push(bars);
            
            //Get out the private field storing tags to verify that they have correctly been cleared
            FieldInfo tagField = typeof(RobotAdapter).GetField("m_tags", BindingFlags.Instance | BindingFlags.NonPublic);

            IDictionary cachedTags = tagField.GetValue(m_Adapter) as IDictionary;

            Assert.IsNotNull(cachedTags, "Unable to verify cached tags have correctly been cleared.");

            //Ensure that the cache has been cleared to make sure that the test below correcly relies on the groups
            Assert.IsTrue(cachedTags.Count == 0, "Cached tags are not correctly cleared after push.");

            //Pull out the bars
            List<Bar> pulledBars = m_Adapter.Pull(new FilterRequest { Type = typeof(Bar) }).Cast<Bar>().ToList();

            Assert.AreEqual(bars.Count, pulledBars.Count, "Wrong number of Bars returned.");

            foreach (string tag in tags)
            {
                foreach (Bar bar in pulledBars)
                {
                    //Ensure all bars correctly stores all tags
                    Assert.Contains(tag, bar.Tags.ToList());
                }
            }

        }

        [Test]
        [Description("Tests that pushing a new set of Bars with the same push tag correctly replaces previous pushed bars and nodes with the same tag.")]
        public void PushBarsWithTagTwice()
        {
            int count = 3;
            List<Bar> bars = new List<Bar>();
            for (int i = 0; i < count; i++)
            {
                bars.Add(Engine.Base.Create.RandomObject(typeof(Bar), i) as Bar);
            }

            m_Adapter.Push(bars, "TestTag");

            bars = new List<Bar>();
            for (int i = 0; i < count; i++)
            {
                bars.Add(Engine.Base.Create.RandomObject(typeof(Bar), i + count) as Bar);
            }

            m_Adapter.Push(bars, "TestTag");

            List<Bar> pulledBars = m_Adapter.Pull(new FilterRequest { Type = typeof(Bar) }).Cast<Bar>().ToList();

            Assert.AreEqual(bars.Count, pulledBars.Count, "Bars storing the tag has not been correctly replaced.");

            List<Node> pulledNodes = m_Adapter.Pull(new FilterRequest { Type = typeof(Node) }).Cast<Node>().ToList();

            Assert.AreEqual(bars.Count * 2, pulledNodes.Count, "Node storing the tag has not been correctly replaced.");
        }

        [Test]
        [Description("Tests that pushing two sets of Bars with different tags, but some overlapping nodes, then a new set of Bars for one of the groups gives correct replacements of nodes affected, but keeps the ones that needs to be kept.")]
        public void PushBarsWithDifferentTags()
        {
            int count = 3;
            List<Bar> bars1 = new List<Bar>();
            for (int i = 0; i < count; i++)
            {
                bars1.Add(Engine.Base.Create.RandomObject(typeof(Bar), i) as Bar);
            }


            List<Bar> bars2 = new List<Bar>();
            for (int i = 0; i < count; i++)
            {
                Bar bar = Engine.Base.Create.RandomObject(typeof(Bar), i + count) as Bar;
                bar.StartNode = bars1[i].StartNode; //Same start node for bars from both sets
                bars2.Add(bar);
            }

            m_Adapter.Push(bars1, "Bar set 1");
            m_Adapter.Push(bars2, "Bar set 2");

            List<Bar> pulledBars = m_Adapter.Pull(new FilterRequest { Type = typeof(Bar) }).Cast<Bar>().ToList();

            Assert.AreEqual(count * 2, pulledBars.Count, "Wrong number of Bars in the model.");

            List<Node> pulledNodes = m_Adapter.Pull(new FilterRequest { Type = typeof(Node) }).Cast<Node>().ToList();

            //Expecting unique nodes for endnodes of the bars, but overlapping start nodes -> should be 3 times the number of bars
            Assert.AreEqual(count * 3, pulledNodes.Count, "Wrong number of nodes in the model.");

            foreach (Bar bar in pulledBars)
            {
                Assert.IsTrue(bar.Tags.Count == 1, "Bar should contain 1 tag.");
                Assert.IsTrue(bar.StartNode.Tags.Count == 2, "StartNode of bars should contain 2 tags.");
                Assert.IsTrue(bar.EndNode.Tags.Count == 1, "EndNode of bars should contain 1 tag.");
            }

            //Create a new set to replace the first set of bars
            bars1 = new List<Bar>();
            for (int i = 0; i < count; i++)
            {
                bars1.Add(Engine.Base.Create.RandomObject(typeof(Bar), (i + 1) * 13) as Bar);
            }

            //Push the bars again with same tag
            m_Adapter.Push(bars1, "Bar set 1");

            //Pull back Bars
            pulledBars = m_Adapter.Pull(new FilterRequest { Type = typeof(Bar) }).Cast<Bar>().ToList();

            //Excepting the same number of Bars as before. 2*count, given the two sets pushed
            Assert.AreEqual(count * 2, pulledBars.Count, "Wrong number of Bars in the model.");

            pulledNodes = m_Adapter.Pull(new FilterRequest { Type = typeof(Node) }).Cast<Node>().ToList();

            //Expecting unique nodes for endnodes of the bars. Expecting that the number of nodes now should be 4 times the count, two unique for each bar
            Assert.AreEqual(count * 4, pulledNodes.Count, "Wrong number of nodes in the model.");

            foreach (Node node in pulledNodes)
            {
                Assert.IsTrue(node.Tags.Count == 1, "Wrong number of tags for node.");
            }
        }


        [Test]
        public void PushPanelsWithTag()
        {
            //Generate some random panels
            int count = 2;
            List<Panel> panels = new List<Panel>();
            List<Point> cornerPts = new List<Point>
            {
                new Point{ X = 0, Y = 0 },
                new Point{ X = 0, Y = 10},
                new Point{ X = 10,Y = 10},
                new Point{ X = 10,Y = 0},
            };

            for (int i = 0; i < count; i++)
            {
                Panel panel = Engine.Base.Create.RandomObject(typeof(Panel), i) as Panel;
                panel.Openings = new List<Opening>();
                panel.ExternalEdges = Create.Polyline(cornerPts.Select(x => x.Translate(Vector.ZAxis * i))).SubParts().Select(x => new Edge { Curve = x }).ToList();
                panels.Add(panel);
            }

            List<string> tags = new List<string> { "Tag1", "SomeRandomTag", "Tag2" };
            //Add some random tags
            foreach (Panel panel in panels)
            {
                foreach (string tag in tags)
                {
                    panel.Tags.Add(tag);
                }
            }

            //Push the bars
            m_Adapter.Push(panels);

            //Get out the private field storing tags to verify that they have correctly been cleared
            FieldInfo tagField = typeof(RobotAdapter).GetField("m_tags", BindingFlags.Instance | BindingFlags.NonPublic);

            IDictionary cachedTags = tagField.GetValue(m_Adapter) as IDictionary;

            Assert.IsNotNull(cachedTags, "Unable to verify cached tags have correctly been cleared.");

            //Ensure that the cache has been cleared to make sure that the test below correcly relies on the groups
            Assert.IsTrue(cachedTags.Count == 0, "Cached tags are not correctly cleared after push.");

            //Pull out the bars
            List<Panel> pulledPanels = m_Adapter.Pull(new FilterRequest { Type = typeof(Panel) }).Cast<Panel>().ToList();

            Assert.AreEqual(panels.Count, pulledPanels.Count, "Wrong number of Bars returned.");

            foreach (string tag in tags)
            {
                foreach (Panel panel in pulledPanels)
                {
                    //Ensure all bars correctly stores all tags
                    Assert.Contains(tag, panel.Tags.ToList());
                }
            }

        }

        [Test]
        [Description("Tests that pushing a new set of Bars with the same push tag correctly replaces previous pushed bars and nodes with the same tag.")]
        public void PushPanelsWithTagTwice()
        {
            //Generate some random panels
            int count = 2;
            List<Panel> panels = new List<Panel>();
            List<Point> cornerPts = new List<Point>
            {
                new Point{ X = 0, Y = 0 },
                new Point{ X = 0, Y = 10},
                new Point{ X = 10,Y = 10},
                new Point{ X = 10,Y = 0},
            };

            for (int i = 0; i < count; i++)
            {
                Panel panel = Engine.Base.Create.RandomObject(typeof(Panel), i) as Panel;
                panel.Openings = new List<Opening>();
                panel.ExternalEdges = Create.Polyline(cornerPts.Select(x => x.Translate(Vector.ZAxis * i))).SubParts().Select(x => new Edge { Curve = x }).ToList();
                panels.Add(panel);
            }

            m_Adapter.Push(panels, "TestTag");

            panels = new List<Panel>();

            for (int i = 0; i < count; i++)
            {
                Panel panel = Engine.Base.Create.RandomObject(typeof(Panel), (i + 1) * 7) as Panel;
                panel.Openings = new List<Opening>();
                panel.ExternalEdges = Create.Polyline(cornerPts.Select(x => x.Translate(Vector.ZAxis * i))).SubParts().Select(x => new Edge { Curve = x }).ToList();
                panels.Add(panel);
            }

            m_Adapter.Push(panels, "TestTag");

            List<Panel> pulledPanels = m_Adapter.Pull(new FilterRequest { Type = typeof(Panel) }).Cast<Panel>().ToList();

            Assert.AreEqual(panels.Count, pulledPanels.Count, "Panels storing the tag has not been correctly replaced.");
        }

    }
}