using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Nucumber.App.CommandLineUtilities;
using Nucumber.Core.Parsers;
using Nucumber.Core;

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
                           Path.GetFullPath(@"..\..\..\example\bin\Debug\example.dll"),
                           Path.GetFullPath(@"..\..\..\example\example.feature")
                       };
            # endif
            new Program().Run(args);
        }

        private void Run(string[] args)
        {
            
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            formatter = new ConsoleOutputFormatter("Nucumber", new CSharpSyntaxSuggester());

            StepMother = new StepMother();
            StepMother.AdoptSteps(new AssemblyLoader().LoadStepAssembly(new FileInfo(args.FirstOrDefault())));

            var feature = new Feature(new AltGherkinParser());
            feature.Parse(args[1]);

            new FeatureExecutor(formatter, StepMother).ExecuteFeature(feature);
           
            formatter.WriteResults(StepMother);
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.LoadFile(Path.GetFullPath(@"..\..\..\example\bin\debug\ThoughtWorks.Selenium.Core.dll"));
        }

       
    }
}
