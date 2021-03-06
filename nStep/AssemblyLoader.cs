﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using nStep.Framework;
using nStep.Framework.Exceptions;

namespace nStep.App
{
    public static class AssemblyLoader
    {
        public static IEnumerable<IProvideSteps> GetStepSets(IEnumerable<FileInfo> assemblyFiles)
        {
            IEnumerable<IProvideSteps> stepSets = new List<IProvideSteps>();
            foreach (var assemblyFile in assemblyFiles)
            {
                stepSets = stepSets.Concat(GetSealedTypesAssignableFrom<IProvideSteps>(assemblyFile));
            }

            return stepSets;
        }

        public static IEnumerable<IProvideWorldView> GetWorldViewProviders(IEnumerable<FileInfo> assemblyFiles)
        {
            return GetEnumerableOf<IProvideWorldView>(assemblyFiles);
        }

        private static IEnumerable<T> GetEnumerableOf<T>(IEnumerable<FileInfo> assemblyFiles)
        {
            IEnumerable<T> list = new List<T>();
            foreach (var assemblyFile in assemblyFiles)
            {
                list = list.Concat(GetTypesAssignableFrom<T>(assemblyFile));
            }
            return list;
        }

        public static EnvironmentBase GetEnvironment(IEnumerable<FileInfo> assemblyFiles)
        {
            IEnumerable<EnvironmentBase> environmentBases = new List<EnvironmentBase>();
            foreach (var assemblyFile in assemblyFiles)
            {
                environmentBases = environmentBases.Concat(GetTypesInheritingFrom<EnvironmentBase>(assemblyFile));
            }

            if (environmentBases.Count() > 1) throw new OnlyOneEnvironmentMayBeDefinedException();

            return environmentBases.FirstOrDefault();
        }
        
        private static List<TType> GetSealedTypesAssignableFrom<TType>(FileInfo assemblyFile)
        {
            return GetTypes<TType>(assemblyFile, t => typeof(TType).IsAssignableFrom(t) && t.IsSealed);
        }

        private static List<TType> GetTypesAssignableFrom<TType>(FileInfo assemblyFile)
        {
            return GetTypes<TType>(assemblyFile, t => typeof(TType).IsAssignableFrom(t));
        }

        private static List<TType> GetTypesInheritingFrom<TType>(FileInfo assemblyFile) where TType : class
        {
            return GetTypes<TType>(assemblyFile, t => t.IsSubclassOf(typeof(TType)) && ! t.IsAbstract );
        }

        static List<TType> GetTypes<TType>(FileInfo assemblyFile, Func<Type, bool> predicate)
        {
            var result = new List<TType>();
            foreach (Type t in Assembly.LoadFile(assemblyFile.FullName).GetTypes())
            {
                if ( predicate(t) )
                {
                    result.Add((TType)Activator.CreateInstance(t));
                }
            }
            return result;
        }
    }
}
