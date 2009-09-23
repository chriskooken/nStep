using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Nucumber.Core;
using Nucumber.Framework;

namespace Nucumber.App
{
    public class AssemblyLoader
    {
        public IEnumerable<IProvideSteps> GetStepSets(FileInfo assemblyFile)
        {
            return GetTypesAssignableFrom<IProvideSteps>(assemblyFile);
        }

        private List<TType> GetTypesAssignableFrom<TType>(FileInfo assemblyFile)
        {
            var result = new List<TType>();
            foreach (Type t in Assembly.LoadFile(assemblyFile.FullName).GetTypes())
            {
                if ((typeof(TType).IsAssignableFrom(t) && (t != typeof(StepSetBase<>))))
                {
                    result.Add((TType)Activator.CreateInstance(t));
                    
                }
            }
            return result;
        }

        public IWorldViewDictionary GetWorldViewProviders(FileInfo assemblyFile)
        {
            var providers = GetTypesAssignableFrom<IProvideWorldView>(assemblyFile);

            var result = new WorldViewDictionary();
            foreach (var provider in providers)
            {
                result.Import(provider);
            }

            return result;
        }
    }
}
