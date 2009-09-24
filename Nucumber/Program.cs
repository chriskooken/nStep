using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
# if DEBUG
            args = new[]
                       {
                           Path.GetFullPath(@"..\..\..\example\example.feature"),
                           "-r",
                           Path.GetFullPath(@"..\..\..\example\bin\Debug\example.dll")
                       };
# endif
            var options = new NucumberOptions().Parse<NucumberOptions>(args);
            new Program().Run(options);
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
            FileInfo filePath;

            var featureDescription = Regex.Match(pathToFeature, @"(.*):(\d+)$");

            if (featureDescription.Success)
            {
                try
                {
                    filePath = new FileInfo(featureDescription.Groups[1].Value);
                    var feature = new Feature(new AltGherkinParser());
                    feature.Parse(filePath.FullName);
                    new FeatureExecutor(formatter, StepMother).ExecuteFeature(feature, int.Parse(featureDescription.Groups[2].Value));
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Invalid feature file description", e);
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
                                  var feature = new Feature(new AltGherkinParser());
                                  feature.Parse(x);
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
