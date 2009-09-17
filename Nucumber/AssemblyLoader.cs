using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Nucumber.Framework;

namespace Nucumber.App
{
    public class AssemblyLoader
    {
        public CombinedStepDefinitions LoadStepAssembly(FileInfo assemblyFile)
        {
            CombinedStepDefinitions combinedStepDefinitions = null;
            foreach (Type t in Assembly.LoadFile(assemblyFile.FullName).GetTypes())
            {
                if ((typeof(IProvideSteps).IsAssignableFrom(t) && (t != typeof(StepSetBase<>))))
                {
                    var sm = (IProvideSteps)Activator.CreateInstance(t);
                    //Todo: join from all assemblies

                    combinedStepDefinitions = sm.CombinedStepDefinitions;
                }
            }

            return combinedStepDefinitions;
        }
    }
}
