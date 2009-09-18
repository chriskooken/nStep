using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nucumber.Framework
{
    public interface IProvideSteps
    {
        IEnumerable<TransformDefinition> TransformDefinitions { get; }
        CombinedStepDefinitions StepDefinitions { get; }
        object WorldView { set; }
        IRunStepsFromStrings StepFromStringRunner { set; }

        void BeforeStep();
        void AfterStep();
    }
}