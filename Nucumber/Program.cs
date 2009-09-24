using System;
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

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;


            formatter = new ConsoleOutputFormatter("Nucumber", new CSharpSyntaxSuggester());

            var assemblyFiles = options.Assemblies.Select(x => new FileInfo(x)).ToList();

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

        void LoadAndExecuteFeatureFile(string fileName)
        {
            var feature = new Feature(new AltGherkinParser());
            feature.Parse(fileName);

            new FeatureExecutor(formatter, StepMother).ExecuteFeature(feature);
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var strTempAssmbPath =
                Path.GetFullPath(@"..\..\..\example\bin\debug\" + args.Name.Substring(0, args.Name.IndexOf(",")) +
                                 ".dll");

            return Assembly.LoadFrom(strTempAssmbPath);
        }
    }
}
