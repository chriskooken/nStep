using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nucumber.Framework
{
    public interface IProvideSteps
    {
        IEnumerable<StepDefinition> GivenStepDefinitions { get; }
        IEnumerable<StepDefinition> WhenStepDefinitions { get; }
        IEnumerable<StepDefinition> ThenStepDefinitions { get; }

        IEnumerable<TransformDefinition> TransformDefinitions { get; }

        void SetWorldView(object worldView);
    }
}