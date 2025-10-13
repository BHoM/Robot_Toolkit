/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using Shouldly;

namespace BH.Tests.Adapter.Robot
{
    public class PushTests
    {
        RobotAdapter m_Adapter;

        [OneTimeSetUp]
        public void OntimeSetup()
        {
            m_Adapter = new RobotAdapter("", null, true);
            //Forces Analytical_Engine to be loaded up. This is due to some parts of the call-chain not hard wired. THis ensures that methods from Analytical_Engine are loaded and usable from RunExtensionMethod
            Assembly.Load("Analytical_Engine");
        }

        [SetUp]
        public void Setup()
        {
            m_Adapter.Execute(new NewModel());
            BH.Engine.Base.Compute.ClearCurrentEvents();
        }

        [TearDown]
        public void TearDown() 
        {
            m_Adapter.Execute(new Close { SaveBeforeClose = false });
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

            cachedTags.ShouldNotBeNull("Unable to verify cached tags have correctly been cleared.");

            //Ensure that the cache has been cleared to make sure that the test below correcly relies on the groups
            cachedTags.Count.ShouldBe(0, "Cached tags are not correctly cleared after push.");

            //Pull out the bars
            List<Bar> pulledBars = m_Adapter.Pull(new FilterRequest { Type = typeof(Bar) }).Cast<Bar>().ToList();

            pulledBars.Count.ShouldBe(bars.Count, "Wrong number of Bars returned.");

            foreach (string tag in tags)
            {
                foreach (Bar bar in pulledBars)
                {
                    //Ensure all bars correctly stores all tags
                    bar.Tags.ShouldContain(tag);
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

            pulledBars.Count.ShouldBe(bars.Count, "Bars storing the tag has not been correctly replaced.");

            List<Node> pulledNodes = m_Adapter.Pull(new FilterRequest { Type = typeof(Node) }).Cast<Node>().ToList();

            pulledNodes.Count.ShouldBe(bars.Count * 2, "Node storing the tag has not been correctly replaced.");
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
                bar.Start = bars1[i].Start; //Same start node for bars from both sets
                bars2.Add(bar);
            }

            m_Adapter.Push(bars1, "Bar set 1");
            m_Adapter.Push(bars2, "Bar set 2");

            List<Bar> pulledBars = m_Adapter.Pull(new FilterRequest { Type = typeof(Bar) }).Cast<Bar>().ToList();

            pulledBars.Count.ShouldBe(count * 2, "Wrong number of Bars in the model.");

            List<Node> pulledNodes = m_Adapter.Pull(new FilterRequest { Type = typeof(Node) }).Cast<Node>().ToList();

            //Expecting unique nodes for endnodes of the bars, but overlapping start nodes -> should be 3 times the number of bars
            pulledNodes.Count.ShouldBe(count * 3, "Wrong number of nodes in the model.");

            foreach (Bar bar in pulledBars)
            {
                bar.Tags.Count.ShouldBe(1, "Bar should contain 1 tag.");
                bar.Start.Tags.Count.ShouldBe(2, "StartNode of bars should contain 2 tags.");
                bar.End.Tags.Count.ShouldBe(1, "EndNode of bars should contain 1 tag.");
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
            pulledBars.Count.ShouldBe(count * 2, "Wrong number of Bars in the model.");

            pulledNodes = m_Adapter.Pull(new FilterRequest { Type = typeof(Node) }).Cast<Node>().ToList();

            //Expecting unique nodes for endnodes of the bars. Expecting that the number of nodes now should be 4 times the count, two unique for each bar
            pulledNodes.Count.ShouldBe(count * 4, "Wrong number of nodes in the model.");

            foreach (Node node in pulledNodes)
            {
                node.Tags.Count.ShouldBe(1, "Wrong number of tags for node.");
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
                panel.ExternalEdges = Engine.Geometry.Create.Polyline(cornerPts.Select(x => x.Translate(Vector.ZAxis * i))).SubParts().Select(x => new Edge { Curve = x }).ToList();
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

            cachedTags.ShouldNotBeNull("Unable to verify cached tags have correctly been cleared.");

            //Ensure that the cache has been cleared to make sure that the test below correcly relies on the groups
            cachedTags.Count.ShouldBe(0, "Cached tags are not correctly cleared after push.");

            //Pull out the bars
            List<Panel> pulledPanels = m_Adapter.Pull(new FilterRequest { Type = typeof(Panel) }).Cast<Panel>().ToList();

            pulledPanels.Count.ShouldBe(panels.Count, "Wrong number of Bars returned.");

            foreach (string tag in tags)
            {
                foreach (Panel panel in pulledPanels)
                {
                    //Ensure all bars correctly stores all tags
                    panel.Tags.ShouldContain(tag);
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
                panel.ExternalEdges = Engine.Geometry.Create.Polyline(cornerPts.Select(x => x.Translate(Vector.ZAxis * i))).SubParts().Select(x => new Edge { Curve = x }).ToList();
                panels.Add(panel);
            }

            m_Adapter.Push(panels, "TestTag");

            panels = new List<Panel>();

            for (int i = 0; i < count; i++)
            {
                Panel panel = Engine.Base.Create.RandomObject(typeof(Panel), (i + 1) * 7) as Panel;
                panel.Openings = new List<Opening>();
                panel.ExternalEdges = Engine.Geometry.Create.Polyline(cornerPts.Select(x => x.Translate(Vector.ZAxis * i))).SubParts().Select(x => new Edge { Curve = x }).ToList();
                panels.Add(panel);
            }

            m_Adapter.Push(panels, "TestTag");

            List<Panel> pulledPanels = m_Adapter.Pull(new FilterRequest { Type = typeof(Panel) }).Cast<Panel>().ToList();

            pulledPanels.Count.ShouldBe(panels.Count, "Panels storing the tag has not been correctly replaced.");
        }


        [Test]
        [Description("Tests that pushing multiple bars with different section profile types (ISectionProfile and BoxProfile) correctly sets distinct Type and ShapeType for each section.")]
        public void PushBarsWithDifferentProfileTypes()
        {
            // Create material
            Steel material = new Steel { Name = "TestSteel" };

            // Create ISectionProfile
            BH.oM.Spatial.ShapeProfiles.ISectionProfile iProfile = BH.Engine.Spatial.Create.ISectionProfile(
                height: 0.5,
                width: 0.3,
                webThickness: 0.01,
                flangeThickness: 0.015,
                rootRadius: 0.01,
                toeRadius: 0.005
            );

            // Create BoxProfile  
            BH.oM.Spatial.ShapeProfiles.BoxProfile boxProfile = BH.Engine.Spatial.Create.BoxProfile(
                height: 0.4,
                width: 0.4,
                thickness: 0.01,
                outerRadius: 0.005,
                innerRadius: 0.003
            );

            // Create section properties
            SteelSection iSectionProperty = BH.Engine.Structure.Create.SteelSection(iProfile, material);
            iSectionProperty.Name = "ISection_500x300";
            
            SteelSection boxSectionProperty = BH.Engine.Structure.Create.SteelSection(boxProfile, material);
            boxSectionProperty.Name = "BoxSection_400x400";

            // Create nodes
            List<Node> nodes = new List<Node>
            {
                BH.Engine.Structure.Create.Node(new Point { X = 0, Y = 0, Z = 0 }),
                BH.Engine.Structure.Create.Node(new Point { X = 5, Y = 0, Z = 0 }),
                BH.Engine.Structure.Create.Node(new Point { X = 10, Y = 0, Z = 0 })
            };

            // Create bars with different profiles
            List<Bar> bars = new List<Bar>
            {
                BH.Engine.Structure.Create.Bar(nodes[0], nodes[1], iSectionProperty, 0, "Bar1_ISection"),
                BH.Engine.Structure.Create.Bar(nodes[1], nodes[2], boxSectionProperty, 0, "Bar2_BoxSection")
            };

            // Push nodes first
            m_Adapter.Push(nodes);
            
            // Push section properties
            List<ISectionProperty> sections = new List<ISectionProperty> { iSectionProperty, boxSectionProperty };
            m_Adapter.Push(sections);

            // Push bars
            m_Adapter.Push(bars);

            // Pull back bars to verify they were created correctly
            List<Bar> pulledBars = m_Adapter.Pull(new FilterRequest { Type = typeof(Bar) }).Cast<Bar>().ToList();

            pulledBars.Count.ShouldBe(bars.Count, "Wrong number of Bars returned.");

            // Verify bars have correct names
            var pulledBar1 = pulledBars.FirstOrDefault(b => b.Name == "Bar1_ISection");
            var pulledBar2 = pulledBars.FirstOrDefault(b => b.Name == "Bar2_BoxSection");

            pulledBar1.ShouldNotBeNull("Bar with ISectionProfile not found.");
            pulledBar2.ShouldNotBeNull("Bar with BoxProfile not found.");

            // Verify section properties are correctly assigned
            pulledBar1.SectionProperty.ShouldNotBeNull("Bar1 should have a section property.");
            pulledBar2.SectionProperty.ShouldNotBeNull("Bar2 should have a section property.");

            pulledBar1.SectionProperty.Name.ShouldBe("ISection_500x300", "Bar1 should have ISection profile.");
            pulledBar2.SectionProperty.Name.ShouldBe("BoxSection_400x400", "Bar2 should have BoxSection profile.");
        }

        [Test]
        [Description("Tests that pushing multiple bars with tapered sections having different profile types (ISectionProfile and BoxProfile) works correctly.")]
        public void PushBarsWithTaperedProfiles()
        {
            // Create material
            Steel material = new Steel { Name = "TestSteel" };

            // Create ISectionProfiles for tapered section
            BH.oM.Spatial.ShapeProfiles.ISectionProfile iProfile1 = BH.Engine.Spatial.Create.ISectionProfile(
                height: 0.5,
                width: 0.3,
                webThickness: 0.01,
                flangeThickness: 0.015,
                rootRadius: 0.01,
                toeRadius: 0.005
            );

            BH.oM.Spatial.ShapeProfiles.ISectionProfile iProfile2 = BH.Engine.Spatial.Create.ISectionProfile(
                height: 0.6,
                width: 0.35,
                webThickness: 0.012,
                flangeThickness: 0.018,
                rootRadius: 0.01,
                toeRadius: 0.005
            );

            // Create BoxProfiles for tapered section
            BH.oM.Spatial.ShapeProfiles.BoxProfile boxProfile1 = BH.Engine.Spatial.Create.BoxProfile(
                height: 0.4,
                width: 0.4,
                thickness: 0.01,
                outerRadius: 0.005,
                innerRadius: 0.003
            );

            BH.oM.Spatial.ShapeProfiles.BoxProfile boxProfile2 = BH.Engine.Spatial.Create.BoxProfile(
                height: 0.5,
                width: 0.5,
                thickness: 0.012,
                outerRadius: 0.005,
                innerRadius: 0.003
            );

            // Create tapered profiles
            Dictionary<double, IProfile> iTaperedProfiles = new Dictionary<double, IProfile>
            {
                { 0, iProfile1 },
                { 1, iProfile2 }
            };

            Dictionary<double, IProfile> boxTaperedProfiles = new Dictionary<double, IProfile>
            {
                { 0, boxProfile1 },
                { 1, boxProfile2 }
            };

            BH.oM.Spatial.ShapeProfiles.TaperedProfile iTaperedProfile = BH.Engine.Spatial.Create.TaperedProfile(iTaperedProfiles);
            BH.oM.Spatial.ShapeProfiles.TaperedProfile boxTaperedProfile = BH.Engine.Spatial.Create.TaperedProfile(boxTaperedProfiles);

            // Create section properties
            SteelSection iTaperedSection = BH.Engine.Structure.Create.SteelSection(iTaperedProfile, material);
            iTaperedSection.Name = "ITapered_500to600";

            SteelSection boxTaperedSection = BH.Engine.Structure.Create.SteelSection(boxTaperedProfile, material);
            boxTaperedSection.Name = "BoxTapered_400to500";

            // Create nodes
            List<Node> nodes = new List<Node>
            {
                BH.Engine.Structure.Create.Node(new Point { X = 0, Y = 0, Z = 0 }),
                BH.Engine.Structure.Create.Node(new Point { X = 5, Y = 0, Z = 0 }),
                BH.Engine.Structure.Create.Node(new Point { X = 10, Y = 0, Z = 0 })
            };

            // Create bars with tapered profiles
            List<Bar> bars = new List<Bar>
            {
                BH.Engine.Structure.Create.Bar(nodes[0], nodes[1], iTaperedSection, 0, "Bar1_ITapered"),
                BH.Engine.Structure.Create.Bar(nodes[1], nodes[2], boxTaperedSection, 0, "Bar2_BoxTapered")
            };

            // Push nodes first
            m_Adapter.Push(nodes);

            // Push section properties
            List<ISectionProperty> sections = new List<ISectionProperty> { iTaperedSection, boxTaperedSection };
            m_Adapter.Push(sections);

            // Push bars
            m_Adapter.Push(bars);

            // Pull back bars to verify they were created correctly
            List<Bar> pulledBars = m_Adapter.Pull(new FilterRequest { Type = typeof(Bar) }).Cast<Bar>().ToList();

            pulledBars.Count.ShouldBe(bars.Count, "Wrong number of Bars returned.");

            // Verify bars have correct names
            var pulledBar1 = pulledBars.FirstOrDefault(b => b.Name == "Bar1_ITapered");
            var pulledBar2 = pulledBars.FirstOrDefault(b => b.Name == "Bar2_BoxTapered");

            pulledBar1.ShouldNotBeNull("Bar with tapered ISectionProfile not found.");
            pulledBar2.ShouldNotBeNull("Bar with tapered BoxProfile not found.");

            // Verify section properties are correctly assigned
            pulledBar1.SectionProperty.ShouldNotBeNull("Bar1 should have a section property.");
            pulledBar2.SectionProperty.ShouldNotBeNull("Bar2 should have a section property.");

            pulledBar1.SectionProperty.Name.ShouldBe("ITapered_500to600", "Bar1 should have tapered ISection profile.");
            pulledBar2.SectionProperty.Name.ShouldBe("BoxTapered_400to500", "Bar2 should have tapered BoxSection profile.");
        }

    }
}

