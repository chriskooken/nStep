using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
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
                new Program().Run(options);
            }
            catch (ConsoleOptionsException exception)
            {
                Console.WriteLine(exception.OutPut());
            }
            
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

            env.GlobalBegin(worldViews);

            StepMother = new StepMother(worldViews);
            StepMother.AdoptSteps(AssemblyLoader.GetStepSets(assemblyFiles));

            LoadAndExecuteFeatureFile(options.FeatureFiles);

            env.GlobalExit(worldViews);
            formatter.WriteResults(StepMother);
        }

        void LoadAndExecuteFeatureFile(string pathToFeature)
        {
            var filePath = new FileInfo(pathToFeature);

            if (filePath.Exists)
            {
                var feature = new Feature(new AltGherkinParser());
                feature.Parse(filePath.FullName);
                new FeatureExecutor(formatter, StepMother).ExecuteFeature(feature);
            }
            else
            {
                var files = new List<string>(Directory.GetFiles(filePath.FullName, "*.feature"));
                files.ForEach(x => {
                    var feature = new Feature(new AltGherkinParser());
                    feature.Parse(x);
                    new FeatureExecutor(formatter, StepMother).ExecuteFeature(feature);
                });
            }
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
