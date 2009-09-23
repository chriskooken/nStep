using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Nucumber.Core;
using Nucumber.Framework;

namespace Nucumber.App
{
    public static class AssemblyLoader
    {
        public static IEnumerable<IProvideSteps> GetStepSets(FileInfo assemblyFile)
        {
            return GetTypesAssignableFrom<IProvideSteps>(assemblyFile);
        }

        private static List<TType> GetTypesAssignableFrom<TType>(FileInfo assemblyFile)
        {
            return GetTypes<TType>(assemblyFile, t => typeof(TType).IsAssignableFrom(t));
        }
        
        private static List<TType> GetTypesInheritingFrom<TType>(FileInfo assemblyFile) where TType : class 
        {
            return GetTypes<TType>(assemblyFile, t => t.IsSubclassOf(typeof(TType)));
        }

        static List<TType> GetTypes<TType>(FileInfo assemblyFile, Func<Type, bool> predicate)
        {
            var result = new List<TType>();
            foreach (Type t in Assembly.LoadFile(assemblyFile.FullName).GetTypes())
            {
                if ((predicate(t) && (t != typeof(StepSetBase<>))))
                {
                    result.Add((TType)Activator.CreateInstance(t));
                    
                }
            }
            return result;
        }

        public static IEnumerable<IProvideWorldView> GetWorldViewProviders(FileInfo assemblyFile)
        {
            return GetTypesAssignableFrom<IProvideWorldView>(assemblyFile);
        }

        public static EnvironmentBase LoadEnvironment(IEnumerable<FileInfo> assemblyFiles)
        {
            IEnumerable<EnvironmentBase> environmentBases = new List<EnvironmentBase>();
            foreach (var assemblyFile in assemblyFiles)
            {
                environmentBases = environmentBases.Concat(GetTypesInheritingFrom<EnvironmentBase>(assemblyFile));
            }

            if (environmentBases.Count() > 1) throw new OnlyOneEnvironmentMayBeDefinedException();

            return environmentBases.FirstOrDefault();
        }
    }
}
