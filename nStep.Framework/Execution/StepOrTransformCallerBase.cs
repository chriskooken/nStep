using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace nStep.Framework.Execution
{
	public class StepOrTransformCallerBase
	{
		private readonly TypeCaster typeCaster;
		private Type[] InputParameterTypes { get; set; }
		private Regex CompiledRegex { get; set; }
		protected Delegate DelegateToInvoke { get; set; }

		protected StepOrTransformCallerBase(TypeCaster typeCaster, DefinitionBase definition)
		{
			CompiledRegex = definition.Regex;
			DelegateToInvoke = definition.Action;
			InputParameterTypes = definition.InputParamsTypes.ToArray();
			this.typeCaster = typeCaster;
		}

		protected object[] GetParams(string stepBody)
		{
			var objects = new List<Object>();
			var groups = CompiledRegex.Match(stepBody).Groups;

			for (int i = 1; i < groups.Count; i++)
			{
				objects.Add(typeCaster.MakeIntoType(groups[i].Value, InputParameterTypes[i-1]));
			}

			return objects.ToArray();
		}
	}
}