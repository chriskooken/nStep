using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cucumber
{
    public class Feature : IGherkinParser
    {
        IList<Scenario> scenarios;
        Scenario background;
        IList<string> summaryLines;
        
        public Feature()
        {
            scenarios = new List<Scenario>();
            summaryLines = new List<string>();
            background = new Scenario();
        }

        public void Parse(string fileName)
        {
            var parseTree = new SimpleSubtree<LineValue>();
            SimpleTreeNode<LineValue> root = parseTree;
            var level1Pattern = "(^Feature:|^Background:|^Scenario Outline:|^Scenario:)(.*)";
            var level2Pattern = "(^Given|^When|^Then|^And|^But)(.*)";
            var level3Pattern = "(^Examples:|^More Examples:)";
            var ignorePattern = "^#";
            

            var counter = 1;
            string line;

            // Read the file and display it line by line.
            var file = new StreamReader(fileName);
            SimpleTreeNode<LineValue> CurrentLevel1Node = root;
            SimpleTreeNode<LineValue> CurrentLevel2Node = null;
            SimpleTreeNode<LineValue> CurrentLevel3Node = null;
            while ((line = file.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                    continue;

                if (Regex.Match(line, ignorePattern, RegexOptions.Singleline).Success)
                    continue;

                var match = Regex.Match(line, level1Pattern, RegexOptions.Singleline);
                var match2 = Regex.Match(line, level2Pattern, RegexOptions.Singleline);
                if (match.Success)
                {
                    SimpleTreeNode<LineValue> node = new SimpleTreeNode<LineValue>();
                    node.Value = new LineValue
                                     {
                                         Line = counter,
                                         NodeType = match.Groups[1].Value,
                                         Text = match.Groups[2].Value
                                     };
                    if (match.Groups[1].Value == "Feature:")
                        root = node;
                    else
                    {
                        root.Children.Add(node);
                        CurrentLevel1Node = node;
                    }
                }
                else if (match2.Success)
                {
                    SimpleTreeNode<LineValue> node = new SimpleTreeNode<LineValue>();
                    node.Value = new LineValue
                    {
                        Line = counter,
                        NodeType = match2.Groups[1].Value,
                        Text = match2.Groups[2].Value
                    };
                    CurrentLevel1Node.Children.Add(node);
                    CurrentLevel2Node = node;
                }
                else
                {
                    SimpleTreeNode<LineValue> node = new SimpleTreeNode<LineValue>();
                    node.Value = new LineValue
                                     {
                                         Line = counter,
                                         NodeType = "",
                                         Text = line
                                     };
                    CurrentLevel1Node.Children.Add(node);
                }




                counter++;
                
            }

            file.Close();

            DisplayTree(root, 0);
           
        }

        private void DisplayTree(SimpleTreeNode<LineValue> Subtree, int Level)
        {
            string indent = string.Empty.PadLeft(Level * 3);
            Console.WriteLine(indent + Subtree.Value.NodeType + " "+ Subtree.Value.Text);

            Level++;
            foreach (SimpleTreeNode<LineValue> node in Subtree.Children)
            {
                DisplayTree(node, Level);
            }
        }

        public void ParseOld(string fileName)
        {
            string line;
            StreamReader SR = File.OpenText(fileName);
            
            var fullText = SR.ReadToEnd();
            SR.Close();

            var items = fullText.Split(new string[] {"Feature:","Background:","Scenario:","Scenario Outline:"}, StringSplitOptions.RemoveEmptyEntries);

            IList<string> types = new List<string>();
            SR = File.OpenText(fileName);
            line = SR.ReadLine();
            while (line != null)
            {
                if (line.Contains("Feature:"))
                    types.Add("Feature:");
                if (line.Contains("Background:"))
                    types.Add("Background:");
                if (line.Contains("Scenario:"))
                    types.Add("Scenario:");
                if (line.Contains("Scenario Outline:"))
                    types.Add("Scenario Outline:");

                line = SR.ReadLine();
            }
            SR.Close();

            IList<NameValue> nv = new List<NameValue>();
            for (int i=0;i<items.Length;i++)
            {
                nv.Add(new NameValue { Value = items[i], Name = types[i] });
            }

            foreach (var nameValue in nv)
            {
                if (nameValue.Name == "Feature:")
                {
                    var lines = nameValue.Value.Split(new string[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in lines)
                    {
                        summaryLines.Add(s.Trim());
                    }
                }

                if (nameValue.Name == "Background:")
                {
                    var lines = nameValue.Value.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in lines)
                    {
                        background.Steps.Add(new Step() {StepText = s.Trim()});
                    }
                }

                if (nameValue.Name == "Scenario:")
                {
                    var lines = nameValue.Value.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    var scenario = new Scenario();
                    scenario.Title = lines.First().Trim();
                    foreach (var s in lines)
                    {
                        if (s != lines.First())
                            scenario.Steps.Add(new Step() { StepText = s.Trim() });
                    }
                    scenarios.Add(scenario);
                }

                if (nameValue.Name == "Scenario Outline:")
                {
                    var lines = nameValue.Value.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    var scenario = new Scenario();
                    scenario.Title = lines.First().Trim();
                    foreach (var s in lines)
                    {
                        if (s != lines.First())
                            scenario.Steps.Add(new Step() { StepText = s.Trim() });
                    }
                    scenarios.Add(scenario);
                }
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
