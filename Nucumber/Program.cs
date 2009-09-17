using System;
using System.IO;
using System.Linq;
using Nucumber.App.CommandLineUtilities;
using Nucumber.Core.Parsers;
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

            Console.WriteFeatureHeading(feature);

            if (feature.Background.Steps.Count > 0)
                Console.WriteBackgroundHeading(feature.Background);

            foreach (var step in feature.Background.Steps)
            {
                ExecuteStep(step);
            }

            foreach (var scenario in feature.Scenarios)
            {
                Console.WriteScenarioTitle(scenario);
                foreach (var step in scenario.Steps)
                {
                    ExecuteStep(step);
                }
            }

            Console.Complete();
        }

        private void ExecuteStep(FeatureStep s)
        {
            switch (StepMother.ProcessStep(s))
            {
                case StepRunResults.Passed:
                    Console.WritePassedFeatureLine(s,StepMother.LastProcessStepDefinition);
                    break;
                case StepRunResults.Failed:
                    Console.WriteException(s, StepMother.LastProcessStepException);
                    break;
                case StepRunResults.Pending:
                    Console.WritePendingFeatureLine(s);
                    break;
                case StepRunResults.Missing:
                    Console.WritePendingFeatureLine(s);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
