using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Cucumber.Parser;

namespace Cucumber
{
    class Program
    {

        static StepMother stepMother;
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            //GetStepClassesFromAssembly();
            stepMother = new StepMother();
            

            Feature feature = new Feature(new GherkinParser());
            feature.Parse("TestApp/Sample.feature");

            CConsole.WriteLevel1("Feature: ");

            foreach (var s in feature.SummaryLines)
                CConsole.WriteLineLevel1(s);

            Console.WriteLine();
            
            if (feature.Background.Steps.Count > 0)
                CConsole.WriteLineLevel2("Background:");
            foreach (var s in feature.Background.Steps)
            {
                if (!stepMother.ProcessStep(s))
                    break;
            }
            Console.WriteLine();

            foreach (var scenario in feature.Scenarios)
            {
                CConsole.WriteLineLevel2("Scenario: " + scenario.Title);
                foreach (var step in scenario.Steps)
                {
                    if (!stepMother.ProcessStep(step))
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
            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Title = "Cucumber";
            
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" ");
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadLine();

        }


        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(IEnumerable<Dictionary<TKey, TValue>> dictionaries)
        {
            var result = new Dictionary<TKey, TValue>();
            foreach (var dict in dictionaries)
                foreach (var x in dict)
                    result[x.Key] = x.Value;
            return result;
        }





    }
}
