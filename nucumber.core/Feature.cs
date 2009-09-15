using System.Collections.Generic;
using Nucumber.Core.Parser;

namespace Nucumber.Core
{
    public class Feature
    {
        private readonly IGherkinParser parser;
        private readonly IConsoleWriter consoleWriter;
        IList<Scenario> scenarios;
        Scenario background;
        IList<string> summaryLines;

        public Feature(IGherkinParser parser, IConsoleWriter consoleWriter)
        {
            this.parser = parser;
            this.consoleWriter = consoleWriter;
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
            consoleWriter.WriteLineAtLevel(Level, Subtree.Value.NodeType + " " + Subtree.Value.Text + ":" + Subtree.Value.Line);

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
