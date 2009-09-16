using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Nucumber.Framework;

namespace Nucumber.Core
{
    public class StepCaller
    {
        public StepCaller(StepDefinition step)
        {
            this.CompiledRegex = step.Regex;
            this.Action = step.Action;
            this.Types = step.ParamsTypes.ToArray();
        }

        private Type[] Types { get; set; }

        private Regex CompiledRegex { get; set; }

        private Delegate Action { get; set; }

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
                objects.Add(MakeIntoType(groups[i].Value, Types[i-1]));
            }

            return objects.ToArray();
        }

        public static object MakeIntoType(string value, Type type)
        {
            if (type == typeof (string))
                return value;
            if (type == typeof(Int32))
                return Int32.Parse(value);

            throw new InvalidCastException("Don't know how to convert: " + value + " into a " + type.Name);
        }
    }
}
