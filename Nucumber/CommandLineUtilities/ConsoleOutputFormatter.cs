using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Nucumber.Core;
using System.Linq;
using Nucumber.Core.Features;
using Nucumber.Framework;

namespace Nucumber.App.CommandLineUtilities
{
    public class ConsoleOutputFormatter : IFormatOutput 
    {
        readonly ISuggestSyntax syntaxSuggester;
        DateTime startTime;

        public ConsoleOutputFormatter(string consoleTitle, ISuggestSyntax syntaxSuggester)
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
            WriteLineLevel3(step.FeatureLine + ":" + step.LineNumber);
			WriteLineLevel3(ex.Message);
			WriteLineLevel4(ex.StackTrace);
			Console.ForegroundColor = ConsoleColor.Gray;	
		}

        public void WritePassedFeatureLine(FeatureStep featureStep, StepDefinition stepDefinition)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            WriteLineLevel3(featureStep.FeatureLine);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void WritePendingFeatureLine(FeatureStep featureStep)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            WriteLineLevel3(featureStep.FeatureLine + ":" + featureStep.LineNumber);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void WriteSkippedFeatureLine(FeatureStep featureStep)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteLineLevel3(featureStep.FeatureLine + ":" + featureStep.LineNumber);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void WritePendingFeatureSnippets(IEnumerable<FeatureStep> pendingFeatureSteps)
        {
            if (pendingFeatureSteps.Count() == 0) return;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("You can implement step definitions for undefined steps with these snippets:");

            var snippets = pendingFeatureSteps.Select(x => syntaxSuggester.TurnFeatureIntoSnippet(x)).Distinct().ToList();

            foreach (var snippet in snippets)
            {
                Console.WriteLine();
                Console.WriteLine(snippet);
                
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void WriteFeatureHeading(Feature feature)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            WriteLineLevel1(feature.Description);
            foreach (var s in feature.SummaryLines)
                WriteLineLevel1(s.Text);
            WriteLineLevel1(string.Empty);
        }

        public void WriteResults(StepMother stepMother)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            WriteFailedFeatureLines(stepMother.FailedSteps);
            Console.ForegroundColor = ConsoleColor.Gray;
            WriteDuration();
            WriteLineBreak();
            WritePendingFeatureSnippets(stepMother.PendingSteps);
            
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void WriteFailedFeatureLines(IList<FeatureStep> failedSteps)
        {
            if (failedSteps.Count == 0) return;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(failedSteps.Count + " Failed Steps: ");
            foreach (var failedStep in failedSteps)
            {
                Console.WriteLine(failedStep.FeatureLine + ":" + failedStep.LineNumber);
            }
        }

        void WriteDuration()
        {
            var timespan = DateTime.Now.Subtract(startTime);
            var formatString = "Finished in: ";
            if (timespan.Hours > 0)
                formatString += timespan.Hours + "h ";
            if (timespan.Minutes > 0)
                formatString += timespan.Minutes + "m ";
            if (timespan.Seconds > 0)
                formatString += timespan.Seconds + "s ";
            if (timespan.Milliseconds > 0)
                formatString += timespan.Milliseconds + "ms ";
            
            WriteLineLevel1(formatString);
        }

        public void WriteScenarioTitle(Scenario scenario)
        {
            WriteLineLevel2(scenario.Title);
        }

        public void WriteBackgroundHeading(Background background)
        {
            WriteLineLevel2(background.Title);
        }

        public void WriteLineBreak()
        {
            Console.WriteLine();
        }
    }
}