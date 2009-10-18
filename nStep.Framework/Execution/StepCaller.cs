using nStep.Core;
using nStep.Framework.StepDefinitions;

namespace nStep.Framework.Execution
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