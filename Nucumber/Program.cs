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
            formatter = new ConsoleOutputFormatter("Nucumber", new CSharpSyntaxSuggester());

            StepMother = new StepMother();
            StepMother.ImportSteps(new AssemblyLoader().LoadStepAssembly(new FileInfo(args.FirstOrDefault())));

            var feature = new Feature(new AltGherkinParser());
            feature.Parse(args[1]);

            new FeatureExecutor(formatter, StepMother).ExecuteFeature(feature);
            Thread.Sleep(5000);
            
            
            
            formatter.Complete();
        }

       
    }
}
