using System.IO;
using System.Linq;
using Nucumber.App.CommandLineUtilities;
using Nucumber.Core.Parsing;
using Nucumber.Core;

namespace Nucumber.App
{
    class Program
    {
        private StepMother StepMother;
        private IConsoleWriter Console;

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
            Console = new CConsole("Nucumber");

            StepMother = new StepMother(new AssemblyLoader().LoadStepAssembly(new FileInfo(args.FirstOrDefault())));

            var feature = new Feature(new AltGherkinParser());
            feature.Parse(args[1]);

            new FeatureExecutor(Console, StepMother).ExecuteFeature(feature);

            Console.Complete();
        }

       
    }
}
