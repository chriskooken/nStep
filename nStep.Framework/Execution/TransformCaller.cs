using nStep.Core;

namespace nStep.Framework.Execution
{
	public class TransformCaller : StepOrTransformCallerBase
	{
		public TransformCaller(TransformDefinition transformDefinition, TypeCaster typeCaster):base(typeCaster, transformDefinition)
		{}

		public object Call(string matchValue)
		{
			return new ExecutableAction(DelegateToInvoke, GetParams(matchValue)).Call();
		}

	}
}