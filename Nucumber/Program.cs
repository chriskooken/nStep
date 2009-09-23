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

            var assemblyFile = new FileInfo(args.FirstOrDefault());

            var worldViews = new WorldViewDictionary();
            worldViews.Import(AssemblyLoader.GetWorldViewProviders(assemblyFile));

            EnvironmentBase env = AssemblyLoader.LoadEnvironment(new []{assemblyFile});

            env.GlobalBegin(worldViews);

            StepMother = new StepMother(worldViews);
            StepMother.AdoptSteps(AssemblyLoader.GetStepSets(assemblyFile));

            var feature = new Feature(new AltGherkinParser());
            feature.Parse(args[1]);

            new FeatureExecutor(formatter, StepMother).ExecuteFeature(feature);
           
            env.GlobalExit(worldViews);
            formatter.WriteResults(StepMother);
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //return Assembly.LoadFile(Path.GetFullPath(@"..\..\..\example\bin\debug\ThoughtWorks.Selenium.Core.dll"));
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.

        
                    //Build the path of the assembly from where it has to be loaded.				
                    var strTempAssmbPath = Path.GetFullPath(@"..\..\..\example\bin\debug\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll");
                  
                

            
            //Load the assembly from the specified path. 					
            var MyAssembly = Assembly.LoadFrom(strTempAssmbPath);

            //Return the loaded assembly.
            return MyAssembly;

        }

       
    }
}
