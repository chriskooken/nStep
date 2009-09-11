using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Cucumber
{
    class Program
    {
        static Dictionary<string, object> loadedsteps = new Dictionary<string, object>();
        
        static void Main(string[] args)
        {
            GetStepClassesFromAssembly();


            Feature feature = new Feature();
            feature.LoadAndParseFeatureFile("Sample.feature");
            WriteLevel1("Feature: ");

            foreach (var s in feature.SummaryLines)
                WriteLineLevel1(s);

            Console.WriteLine();
            
            if (feature.Background.Steps.Count > 0)
                WriteLineLevel2("Background:");
            foreach (var s in feature.Background.Steps)
            {
                if (!ProcessStep(s))
                    break;
            }
            Console.WriteLine();

            foreach (var scenario in feature.Scenarios)
            {
                WriteLineLevel2("Scenario: " + scenario.Title);
                foreach (var step in scenario.Steps)
                {
                    if (!ProcessStep(step))
                        break;
                }
            }
            Console.WriteLine();

            //Use reflection to scan input assembly for all step definitions & add them to a dictionary
          
            //parse the file from 1 keyword to the next, keywords (feature, scenario, scenario outline, background)

            //file must start with feature

            //output feature name and description to console. 

            //begin running steps, looking for matches in the dictionary.

            //if no match exists, generate a stub for the user to paste in the code
            System.Console.ForegroundColor = ConsoleColor.Red;
            Console.Title = "Cucumber";
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadLine();

        }


        static bool ProcessStep(string LineText)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var consoleOutput = LineText;
            LineText = Regex.Match(LineText, "(Given|When|Then)(.*)", RegexOptions.Singleline).Groups[2].Value.Trim();

            var results = from result in loadedsteps
                          where Regex.Match(LineText, result.Key, RegexOptions.Singleline).Success
                          select result;

            if (results.Count() == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                //throw new MissingStepException(LineText);
            }
            
            if (results.Count() > 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                return false;
                //throw new AmbiguousStepException(LineText);
            }

            if (results.Count() == 1)
            {
                try
                {
                    MatchRegex(LineText, results);
                }
                catch (Exception)
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    return false;
                }
            }

            WriteLineLevel3("    " + consoleOutput);
            Console.ForegroundColor = ConsoleColor.White;
            return true;
        }

        static void MatchRegex(string LineText, IEnumerable<KeyValuePair<string, object>> results)
        {
            var pattern = results.First().Key;
            var groups = Regex.Match(LineText, pattern, RegexOptions.Singleline).Groups;

            if (groups.Count == 2)
            {
                var methodToInvoke = results.First().Value as Action<string>;
                methodToInvoke.Invoke(groups[1].Value);
            }

            if (groups.Count == 3)
            {
                var methodToInvoke = results.First().Value as Action<string, string>;
                methodToInvoke.Invoke(groups[1].Value, groups[2].Value);
            }
        }

        private static void GetStepClassesFromAssembly()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.IsSubclassOf(typeof(StepMother)))
                {
                    var sm = (StepMother) Activator.CreateInstance(t);
                   loadedsteps = (sm.Steps);
                }
            }

        }

        static void WriteLineLevel1(string line)
        {
            Console.WriteLine(line);
        }

        static void WriteLevel1(string text)
        {
            Console.Write(text);
        }

        static void WriteLineLevel2(string line)
        {
            Console.WriteLine("  " + line);
        }

        static void WriteLineLevel3(string line)
        {
            Console.WriteLine("    " + line);
        }


    }

    public class AmbiguousStepException : Exception
    {
        public AmbiguousStepException(string text)
        {
                
        }
    }

    public class MissingStepException : Exception
    {
        public MissingStepException(string text)
        {
                
        }
    }
}
