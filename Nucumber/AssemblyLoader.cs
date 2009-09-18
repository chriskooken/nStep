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
        public IEnumerable<IProvideSteps> LoadStepAssembly(FileInfo assemblyFile)
        {
            var result = new List<IProvideSteps>();
            foreach (Type t in Assembly.LoadFile(assemblyFile.FullName).GetTypes())
            {
                if ((typeof(IProvideSteps).IsAssignableFrom(t) && (t != typeof(StepSetBase<>))))
                {
                    result.Add((IProvideSteps)Activator.CreateInstance(t));
                    
                }
            }

            return result;
        }
    }
}
