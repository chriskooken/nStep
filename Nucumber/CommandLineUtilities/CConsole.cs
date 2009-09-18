using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Nucumber.Core;
using Nucumber.Framework;

namespace Nucumber.App.CommandLineUtilities
{
    public class CConsole : IConsoleWriter 
    {
        readonly ISuggestSyntax syntaxSuggester;
        DateTime startTime;

        public CConsole(string consoleTitle, ISuggestSyntax syntaxSuggester)
        {
            startTime = DateTime.Now;
            this.syntaxSuggester = syntaxSuggester;
            Console.Title = consoleTitle;
        }

        private string LevelPad(int indent, string text)
        {
            switch (indent)
            {
                case 1:
                    return text;
                default:
                    return string.Empty.PadLeft(indent * 3) + text;
            }
        }

        private void WriteLineLevel1(string line)
        {
            Console.WriteLine(LevelPad(1, line));
        }

        private void WriteLineLevel2(string line)
        {
            Console.WriteLine(LevelPad(2, line));
        }

        private void WriteLineLevel3(string line)
        {
            Console.WriteLine(LevelPad(3, line));
        }

        private void WriteLineLevel4(string line)
        {
            Console.WriteLine(LevelPad(4, line));
        }

        public void WriteException(FeatureStep step, Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
            WriteLineLevel3(step.FeatureLine);
			WriteLineLevel3(ex.Message);
			WriteLineLevel4(ex.StackTrace);
			Console.ForegroundColor = ConsoleColor.Gray;	
		}

        public void WritePassedFeatureLine(FeatureStep featureStep, StepDefinition stepDefinition)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            WriteLineLevel3(featureStep.FeatureLine);
        }
        public void WritePendingFeatureLine(FeatureStep featureStep)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteLineLevel3(featureStep.FeatureLine + " : " + featureStep.LineNumber);
        }

        public void WritePendingFeatureSnippets(IEnumerable<FeatureStep> pendingFeatureSteps)
        {
            //Todo: use an actual feature line to genrate snippet
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You can implement step definitions for undefined steps with these snippets:");

            foreach (var step in pendingFeatureSteps)
            {
                Console.WriteLine(syntaxSuggester.TurnFeatureIntoSnippet(step));
            }

        }

        public void WriteFeatureHeading(Feature feature)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            WriteLineLevel1("Feature: " + feature.Description);
            foreach (var s in feature.SummaryLines)
                WriteLineLevel1(s);
            WriteLineLevel1(string.Empty);
        }

        public void Complete()
        { 
            var timespan = DateTime.Now.Subtract(startTime);
            Console.ForegroundColor = ConsoleColor.Gray;
            WriteDuration(timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
            
            WriteLineLevel1(string.Empty);
            WriteLineLevel1("Press any key to continue . . .");
            Console.ReadLine();
        }

        void WriteDuration(int hours, int minutes, int seconds, int milliseconds)
        {
            var formatString = "Finished in: ";
            if (hours > 0)
                formatString += hours + "h ";
            if (minutes > 0)
                formatString += minutes + "m ";
            if (seconds > 0)
                formatString += seconds + "s ";
            if (milliseconds > 0)
                formatString += milliseconds + "ms ";
            
            WriteLineLevel1(formatString);
        }

        public void WriteScenarioTitle(Scenario scenario)
        {
            WriteLineLevel1("Scenario: " + scenario.Title);
        }

        public void WriteBackgroundHeading(Scenario background)
        {
            WriteLineLevel2("Background:" + background.Title);
        }

        public void WriteSkippedFeatureLine(FeatureStep featureStep)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            WriteLineLevel3(featureStep.FeatureLine + " : " + featureStep.LineNumber);
        }
    }
}