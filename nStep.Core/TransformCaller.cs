using nStep.Framework;

namespace nStep.Core
{
    public class TransformCaller : StepOrTransformCallerBase
    {
        public TransformCaller(TransformDefinition transformDefinition, TypeCaster typeCaster):base(typeCaster, transformDefinition)
        {}

        public object Call(string matchValue)
        {
            return new ActionCaller(DelegateToInvoke, GetParams(matchValue)).Call();
        }

    }
}