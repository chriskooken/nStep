using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text.RegularExpressions;
using nStep.App.CommandLineUtilities;
using nStep.Core.Parsers;
using nStep.Core;
using nStep.Framework;

namespace nStep.App
{
    class Program
    {
        private StepMother StepMother;
        private IFormatOutput formatter;
        private nStepOptions Options;

        [STAThread]
        static void Main(string[] args)
        {
            ConsoleExceptionHandler consoleExceptionHandler = new ConsoleExceptionHandler(() => new Program().Run(args));
            consoleExceptionHandler.Execute();
            Environment.Exit(0);
        }

        private void Run(string[] args)
        {
            Options = new nStepOptions().Parse<nStepOptions>(args);
            if (Options.Debug)
            {
                Console.WriteLine("Please attach the .Net debugger and press any key to continue...");
                Console.ReadLine();
            }
            
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            formatter = new ConsoleOutputFormatter("nStep", new CSharpSyntaxSuggester());

            InitializeThenRun(Options.Assemblies.Select(x => new FileInfo(x)).ToList(), ()=>LoadAndExecuteFeatureFile(Options.FeatureFiles));

            formatter.WriteResults(StepMother);
        }

        private void InitializeThenRun(List<FileInfo> assemblyFiles, Action action)
        {
            var worldViews = GetWorldViews(assemblyFiles);
            var env = AssemblyLoader.GetEnvironment(assemblyFiles);
            var stepSets = AssemblyLoader.GetStepSets(assemblyFiles);
            var scenarioHooks = new ScenarioHooksRepository(stepSets);

            StepMother = new StepMother(worldViews, scenarioHooks);
            StepMother.AdoptSteps(stepSets);

            if (env != null)
                env.GlobalBegin(worldViews);

            action.Invoke();

            if (env != null)
                env.GlobalExit(worldViews);
        }

        private WorldViewDictionary GetWorldViews(List<FileInfo> assemblyFiles)
        {
            var worldViews = new WorldViewDictionary();
            worldViews.Import(AssemblyLoader.GetWorldViewProviders(assemblyFiles));
            return worldViews;
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
					var lineNumber = int.Parse(featureDescription.Groups[2].Value);
					var feature = GherkinParser.GetFeature(filePath);
					feature.Execute(StepMother, formatter, lineNumber);
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
                var feature = GherkinParser.GetFeature(filePath);
				feature.Execute(StepMother, formatter);
                return;
            }

            var files = new List<string>(Directory.GetFiles(filePath.FullName, "*.feature"));
            files.ForEach(x =>
                              {
								  var innerFilePath = new FileInfo(x);
								  var feature = GherkinParser.GetFeature(innerFilePath);

								  feature.Execute(StepMother, formatter);
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
