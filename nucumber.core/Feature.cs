﻿using System;
using System.Collections.Generic;
using Nucumber.Core.Parser;

namespace Nucumber.Core
{
    public class Feature
    {
        private readonly IGherkinParser parser;

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
        }

        public IList<string> SummaryLines
        { get { return summaryLines; } }

        public Scenario Background
        { get { return background; } }

        public IList<Scenario> Scenarios
        { get { return scenarios; } }

        public string Description { get; set; }
    }

    public class LineValue
    {
        public int Line { get; set; }
        public string Text { get; set; }
        public string NodeType { get; set; }
    }
}
