using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nucumber.Framework
{
    public interface IProvideSteps
    {
        IEnumerable<TransformDefinition> TransformDefinitions { get; }
        CombinedStepDefinitions StepDefinitions { get; }
        void SetWorldView(object worldView);
        void SetStepFromStringRunner(IRunStepsFromStrings runner);

        void BeforeStep();

        void AfterStep();
    }
}