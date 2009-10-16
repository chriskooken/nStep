using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using nStep.Core;
using nStep.Framework.StepDefinitions;
using nStep.Framework;

namespace nStep.Framework
{
	public class StepCaller : StepOrTransformCallerBase
	{
		public StepCaller(StepDefinition step, TypeCaster typeCaster) : base(typeCaster, step)
		{}

		public void Call(string featureLine)
		{
			new ActionCaller(DelegateToInvoke, GetParams(featureLine)).Call();
		}
	}
}