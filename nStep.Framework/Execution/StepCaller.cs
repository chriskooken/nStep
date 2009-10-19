using nStep.Core;
using nStep.Framework.Features;
using nStep.Framework.StepDefinitions;

namespace nStep.Framework.Execution
{
	public class StepCaller : StepOrTransformCallerBase
	{
		public StepCaller(StepDefinition step, TypeCaster typeCaster) : base(typeCaster, step)
		{}

		public void Call(Step step)
		{
			new ActionCaller(DelegateToInvoke, GetParams(step.Body)).Call();
		}
	}
}