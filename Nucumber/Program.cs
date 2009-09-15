﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Cucumber.CommandLineUtilities;
using nucumber.core.Parser;
using nucumber.core;
using nucumber.app.CommandLineUtilities;

namespace nucumber.app
{
    class Program
    {
        static StepMother stepMother;
        static void Main(string[] args)
        {
			var console = new CConsole();
            var commandLine = new Arguments(args);

            if (commandLine["param1"] != null)
                Console.WriteLine("Param1 value: " +
                    commandLine["param1"]);
            else
                Console.WriteLine("Param1 not defined !");


            Console.ForegroundColor = ConsoleColor.Gray;
            //GetStepClassesFromAssembly();
            stepMother = new StepMother(new CConsole());
            

            Feature feature = new Feature(new GherkinParser());
            feature.Parse("TestApp/Sample.feature");

            console.WriteLevel1("Feature: ");

            foreach (var s in feature.SummaryLines)
                console.WriteLineLevel1(s);

            Console.WriteLine();
            
            if (feature.Background.Steps.Count > 0)
                console.WriteLineLevel2("Background:");
            foreach (var s in feature.Background.Steps)
            {
                if (!stepMother.ProcessStep(s))
                    break;
            }
            Console.WriteLine();

            foreach (var scenario in feature.Scenarios)
            {
                console.WriteLineLevel2("Scenario: " + scenario.Title);
                foreach (var step in scenario.Steps)
                {
                    if (!stepMother.ProcessStep(step))
                        break;
                }
            }
            Console.WriteLine();

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
