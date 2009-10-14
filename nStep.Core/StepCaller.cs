using nStep.Framework;

namespace nStep.Core
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
