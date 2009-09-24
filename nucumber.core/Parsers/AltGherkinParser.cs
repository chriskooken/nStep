using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Nucumber.Core.Parsers.DataStructures;

namespace Nucumber.Core.Parsers
{
    public class AltGherkinParser
    {
        string previousName = "";
        public SimpleTreeNode<LineValue> GetParseTree(string filename)
        {
            // var parseTree = new SimpleSubtree<LineValue>();
            //SimpleTreeNode<LineValue> root = parseTree;
            var level1Pattern = "(^Feature:|^Background:|^Scenario Outline:|^Scenario:)(.*)";
            var level2Pattern = "(^Given|^When|^Then|^And|^But|^Examples:|^More Examples:)(.*)";
            var ignorePattern = "^#";


            var counter = 0;
            string line;

            // Read the file and display it line by line.
            var file = new StreamReader(filename);

            SimpleTreeNode<LineValue> CurrentLevel1Node = null;
            SimpleTreeNode<LineValue> CurrentLevel2Node = null;
            SimpleTreeNode<LineValue> CurrentLevel3Node = null;


            SimpleTreeNode<LineValue> rootNode = new SimpleTreeNode<LineValue>();
            rootNode.Value = new LineValue
            {
                Line = counter,
                NodeType = "root",
                Text = ""
            };
            //root = rootNode;
            CurrentLevel1Node = rootNode;



            while ((line = file.ReadLine()) != null)
            {
                counter++;
                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                    continue;

                if (Regex.Match(line, ignorePattern, RegexOptions.Singleline).Success)
                    continue;


                var match = Regex.Match(line, level1Pattern, RegexOptions.Singleline);
                var match2 = Regex.Match(line, level2Pattern, RegexOptions.Singleline);
                if (match.Success)
                {
                    CurrentLevel1Node = GetCurrentNode(rootNode, match, counter);
                }
                else if (match2.Success)
                {
                    CurrentLevel2Node = GetCurrentNode(CurrentLevel1Node, match2, counter);
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

            }

            file.Close();
            return rootNode;
        }

        private string ConvertAndToPrevious(string stepType)
        {
            if (stepType == "And")
                return previousName;
            else
            {
                previousName = stepType;
                return stepType;
            }
        }

        SimpleTreeNode<LineValue> GetCurrentNode(SimpleTreeNode<LineValue> rootNode, Match match, int counter)
        {
            SimpleTreeNode<LineValue> CurrentLevel1Node;
            SimpleTreeNode<LineValue> node = new SimpleTreeNode<LineValue>();
            node.Value = new LineValue
            {
                Line = counter,
                NodeType = ConvertAndToPrevious(match.Groups[1].Value.Trim()),
                Text = match.Groups[0].Value.Trim()
            };
            rootNode.Children.Add(node);
            CurrentLevel1Node = node;
            return CurrentLevel1Node;
        }
    }
}
