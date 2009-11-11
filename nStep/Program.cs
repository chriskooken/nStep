using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading;
using nStep.App.CommandLineUtilities;
using nStep.Core.Parsers;
using nStep.Core;
using nStep.Framework;
using nStep.Framework.WorldViews;

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
                Console.WriteLine("Please attach the .Net debugger to continue...");
                int seconds = 0;
                while (!System.Diagnostics.Debugger.IsAttached)
                {
                    Thread.Sleep(1000);
                    seconds += 1;
                    if (seconds > 60)
                        return;
                }
                Console.Clear();
            }

            if (Options.Rerun)
                Options = ReadRerunFile();

            WriteRerunFile(Options);

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            formatter = new ConsoleOutputFormatter("nStep", new CSharpSyntaxSuggester());

            InitializeThenRun(Options.Assemblies.Select(x => new FileInfo(x)).ToList(), ()=>LoadAndExecuteFeatureFile(Options.FeatureFiles));

            formatter.WriteResults(StepMother);
        }

        void WriteRerunFile(nStepOptions options)
        {
            FileInfo fi = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "rerun.dat");
            Stream stream = File.Open(fi.ToString(), FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, options);
            stream.Close();
        }

        nStepOptions ReadRerunFile()
        {
            FileInfo fi = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "rerun.dat");
            nStepOptions objectToSerialize;
            Stream stream = File.Open(fi.ToString(), FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objectToSerialize = (nStepOptions)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }

        private void InitializeThenRun(List<FileInfo> assemblyFiles, Action action)
        {
            var env = InitializeEnvironment(assemblyFiles);
            var worldViews = InitializeWorldViews(assemblyFiles);
            
            var stepSets = AssemblyLoader.GetStepSets(assemblyFiles);
            var scenarioHooks = new ScenarioHooksRepository(stepSets);

            StepMother = new StepMother(worldViews, scenarioHooks);
            StepMother.AdoptSteps(stepSets);
            
            action.Invoke();

            if (env != null)
                env.GlobalExit(worldViews, StepMother.RunResult);
        }

        private EnvironmentBase InitializeEnvironment(List<FileInfo> assemblyFiles)
        {
            var env = AssemblyLoader.GetEnvironment(assemblyFiles);
            if (env != null)
                env.GlobalBegin();
            return env;
        }

        private WorldViewDictionary InitializeWorldViews(List<FileInfo> assemblyFiles)
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
                    do
                    {
                        feature.Execute(StepMother, StepMother, formatter, lineNumber);
                    } while (Options.Loop);
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
                do
                {
                    feature.Execute(StepMother, StepMother, formatter);
                } while (Options.Loop); 
                return;
            }
            do
            {
                var files = new List<string>(Directory.GetFiles(filePath.FullName, "*.feature"));
                files.ForEach(x =>
                                  {
                                      var innerFilePath = new FileInfo(x);
                                      var feature = GherkinParser.GetFeature(innerFilePath);

                                      feature.Execute(StepMother, StepMother, formatter);
                                  });

            } while (Options.Loop);

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
