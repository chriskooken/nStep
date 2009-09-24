using System;
using System.Collections.Generic;
using Nucumber.Core.Parsers;
using Nucumber.Core.Parsers.DataStructures;
using Nucumber.Framework;
using System.Linq;

namespace Nucumber.Core
{
    public class Feature
    {
		private readonly AltGherkinParser parser;

        IList<Scenario> scenarios;
        Scenario background;
        IList<LineValue> summaryLines;

        public Feature(AltGherkinParser parser)
        {
            this.parser = parser;
            scenarios = new List<Scenario>();
            summaryLines = new List<LineValue>();
            background = new Scenario();
        }

        public Feature()
        {
            
        } 

        public void Parse(string fileName)
        {
            var parseTree = parser.GetParseTree(fileName);
            RecursiveTreeLoad(parseTree, null);
        }

        public void RecursiveTreeLoad(SimpleTreeNode<LineValue> subtree, Scenario currentScenario)
        {

            if (subtree.Parent != null)
            {
                LoadHeading(subtree);

                LoadBackground(subtree);

                currentScenario = LoadScenario(subtree, currentScenario);
            }


            foreach (SimpleTreeNode<LineValue> node in subtree.Children)
            {
                RecursiveTreeLoad(node, currentScenario);
            }
        }

        void LoadBackground(SimpleTreeNode<LineValue> subtree)
        {
            var val = subtree.Value;
            if (subtree.Value.NodeType == "Background:")
            {
                background.Title = subtree.Value.Text;
                background.LineNumber = subtree.Value.Line;
            }

            if (subtree.Parent.Value.NodeType == "Background:")
                background.Steps.Add(new FeatureStep { FeatureLine = val.Text, Kind = val.NodeType.ToStepKind(), LineNumber = val.Line });
        }

        Scenario LoadScenario(SimpleTreeNode<LineValue> subtree, Scenario currentScenario)
        {
            if (currentScenario == null)
            {
                currentScenario = new Scenario();
                //scenarios.Add(currentScenario);
            }

            var val = subtree.Value;
            if (subtree.Value.NodeType == "Scenario:")
            {
                currentScenario = new Scenario();
                scenarios.Add(currentScenario);
                currentScenario.Title = subtree.Value.Text;
                currentScenario.LineNumber = subtree.Value.Line;
            }

            if (subtree.Parent.Value.NodeType == "Scenario:")
                currentScenario.Steps.Add(new FeatureStep { FeatureLine = val.Text, Kind = val.NodeType.ToStepKind(), LineNumber = val.Line });

            return currentScenario;
        }

        void LoadHeading(SimpleTreeNode<LineValue> subtree)
        {
            if ((subtree.Parent.Value.NodeType == "Feature:") || (subtree.Value.NodeType == "Feature:"))
                summaryLines.Add(subtree.Value);
        }

        public IList<LineValue> SummaryLines
        { get { return summaryLines; } }

        public Scenario Background
        { get { return background; } }

        public IList<Scenario> Scenarios
        { get { return scenarios; } }

        public string Description { get; set; }

        public FeatureParts WhatIsAtLine(int lineNmber)
        {
            if (background.LineNumber == lineNmber)
                return FeatureParts.Background;

            if (Scenarios.Where(x => x.LineNumber == lineNmber).Any())
                return FeatureParts.Scenario;

            if (summaryLines.Where(x => x.Line == lineNmber).Any())
                return FeatureParts.Feature;

            throw new InvalidScenarioLineNumberException("There is nothing to execute on line: " + lineNmber);
        }

        public Scenario GetScenarioAt(int lineNmber)
        {
            var foundScenario = Scenarios.Where(x => x.LineNumber == lineNmber);
            if (foundScenario.Any())
                return foundScenario.First();

            throw new InvalidScenarioLineNumberException("There are no scenario definitions on line: " + lineNmber);
        }
    }

    public class LineValue
    {
        public int Line { get; set; }
        public string Text { get; set; }
        public string NodeType { get; set; }
    }
}
