using System;
using System.IO;
using System.Linq;
using System.Threading;
using Nucumber.App.CommandLineUtilities;
using Nucumber.Core.Parsers;
using Nucumber.Core;

namespace Nucumber.App
{
    class Program
    {
        private StepMother StepMother;
        private IFormatOutput Console;

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
            Console = new ConsoleOutputFormatter("Nucumber", new CSharpSyntaxSuggester());

            StepMother = new StepMother();
            StepMother.ImportSteps(new AssemblyLoader().LoadStepAssembly(new FileInfo(args.FirstOrDefault())));

            var feature = new Feature(new AltGherkinParser());
            feature.Parse(args[1]);

            new FeatureExecutor(Console, StepMother).ExecuteFeature(feature);
            Thread.Sleep(5000);
            
            
            
            Console.Complete();
        }

       
    }
}
