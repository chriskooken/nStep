using System;
using System.Linq;
using nStep.Core;
using nStep.Framework.StepDefinitions;
using nStep.Framework.Steps;

namespace nStep.Framework.Execution
{
	public class StepCaller : StepOrTransformCallerBase
	{
		public StepCaller(StepDefinition step, TypeCaster typeCaster) : base(typeCaster, step)
		{}

		public void Call(Step step)
		{
			new ActionCaller(DelegateToInvoke, GetParams(step)).Call();
		}

		private object[] GetParams(Step step)
		{
			var regularParameters = GetParams(step.Body);
			if (step.Table == null)
				return regularParameters;
			else                
				return regularParameters.Concat(new[] { step.Table }).ToArray();
		}
	}
}