using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Nucumber.Framework;

namespace Nucumber.Core
{
    public class StepCaller
    {
        public StepCaller(StepDefinition step, TypeCaster typeCaster)
        {
            TypeCaster = typeCaster;
            CompiledRegex = step.Regex;
            Action = step.Action;
            Types = step.ParamsTypes.ToArray();
        }

        private Type[] Types { get; set; }

        private Regex CompiledRegex { get; set; }

        private Delegate Action { get; set; }

        private TypeCaster TypeCaster { get; set; }

        public void Call(string featureLine)
        {
            new ActionCaller(Action, GetParams(featureLine)).Call();
        }

        private object[] GetParams(string featureLine)
        {
            var objects = new List<Object>();
            var groups = CompiledRegex.Match(featureLine).Groups;
            for (int i = 1; i < groups.Count; i++)
            {
                objects.Add(TypeCaster.MakeIntoType(groups[i].Value, Types[i-1]));
            }

            return objects.ToArray();
        }
    }
}
