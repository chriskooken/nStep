using System;
using System.Collections.Generic;

namespace nucumber.core
{
    public class Feature
    {
        readonly IGherkinParser parser;
        IList<Scenario> scenarios;
        Scenario background;
        IList<string> summaryLines;

        public Feature(IGherkinParser parser)
        {
            this.parser = parser;
            scenarios = new List<Scenario>();
            summaryLines = new List<string>();
            background = new Scenario();
        }

        public void Parse(string fileName)
        {
            var parseTree = parser.GetParseTree(fileName);
            DisplayTree(parseTree, 0);

        }

        private void DisplayTree(SimpleTreeNode<LineValue> Subtree, int Level)
        {
            string indent = string.Empty.PadLeft(Level * 3);
            Console.WriteLine(indent + Subtree.Value.NodeType + " " + Subtree.Value.Text + ":" + Subtree.Value.Line);

            Level++;
            foreach (SimpleTreeNode<LineValue> node in Subtree.Children)
            {
                DisplayTree(node, Level);
            }
        }

        public IList<string> SummaryLines
        { get { return summaryLines; } }

        public Scenario Background
        { get { return background; } }

        public IList<Scenario> Scenarios
        { get { return scenarios; } }


    }

    public class LineValue
    {
        public int Line { get; set; }
        public string Text { get; set; }
        public string NodeType { get; set; }
    }
}
