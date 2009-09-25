#define ALTPARSER

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Nucumber.App.CommandLineUtilities;
using Nucumber.Core.Parsers;
using Nucumber.Core;
using Nucumber.Framework;

namespace Nucumber.App
{
    class Program
    {
        private StepMother StepMother;
        private IFormatOutput formatter;
        private NucumberOptions Options;

        static void Main(string[] args)
        {
            try
            {
                var options = new NucumberOptions().Parse<NucumberOptions>(args);
                if (options.Debug)
                {
                    Console.WriteLine("Please attach the .Net debugger and press any key to continue...");
                    Console.ReadLine();
                }
                new Program().Run(options);
            }
            catch (ConsoleOptionsException exception)
            {
                exception.PrintMessageToConsole();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.ReadKey();
            }
            catch(InvalidScenarioLineNumberException ex)
            {
                WriteException(ex.Message);
            }
            catch(ArgumentException ex)
            {
                WriteException(ex.Message);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine();
                }
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private static void WriteException(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void Run(NucumberOptions options)
        {
            Options = options;

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            formatter = new ConsoleOutputFormatter("Nucumber", new CSharpSyntaxSuggester());

            var assemblyFiles = Options.Assemblies.Select(x => new FileInfo(x)).ToList();

            var worldViews = new WorldViewDictionary();

            worldViews.Import(AssemblyLoader.GetWorldViewProviders(assemblyFiles));
            EnvironmentBase env = AssemblyLoader.GetEnvironment(assemblyFiles);

            if (env != null)
                env.GlobalBegin(worldViews);

            StepMother = new StepMother(worldViews);
            StepMother.AdoptSteps(AssemblyLoader.GetStepSets(assemblyFiles));

            LoadAndExecuteFeatureFile(options.FeatureFiles);

            if (env != null)
                env.GlobalExit(worldViews);
            formatter.WriteResults(StepMother);
        }

        void LoadAndExecuteFeatureFile(string pathToFeature)
        {
            FileInfo filePath;

            var featureDescription = Regex.Match(pathToFeature, @"(.*):(\d+)$");

            if (featureDescription.Success)
            {
                try
                {
					filePath = new FileInfo(featureDescription.Groups[1].Value);
#if ALTPARSER
					var parser = new AltGherkinParser();
					var feature = new Feature(parser);
					feature.Parse(filePath.FullName);
#else
					var feature = GherkinParser.GetFeature(filePath);
#endif
					new FeatureExecutor(formatter, StepMother).ExecuteFeature(feature, int.Parse(featureDescription.Groups[2].Value));
                }
                catch (FormatException e)
                {
                    throw new ArgumentException("Invalid feature file description");
                }
                
                return;
            }


            filePath = new FileInfo(pathToFeature);

            if (filePath.Exists)
            {
                var feature = new Feature(new AltGherkinParser());
                feature.Parse(filePath.FullName);
                new FeatureExecutor(formatter, StepMother).ExecuteFeature(feature);
                return;
            }

            var files = new List<string>(Directory.GetFiles(filePath.FullName, "*.feature"));
            files.ForEach(x =>
                              {
#if ALTPARSER
								  var parser = new AltGherkinParser();
								  var feature = new Feature(parser);
								  feature.Parse(x);
#else
								  var innerFilePath = new FileInfo(x);
								  var feature = GherkinParser.GetFeature(innerFilePath);
#endif

								  new FeatureExecutor(formatter, StepMother).ExecuteFeature(feature);
                              });

        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var path = new FileInfo(Options.Assemblies.FirstOrDefault()).Directory.FullName;

            var strTempAssmbPath =
                Path.GetFullPath(path + @"\" +args.Name.Substring(0, args.Name.IndexOf(",")) +
                                 ".dll");

            return Assembly.LoadFrom(strTempAssmbPath);
        }
    }
}
