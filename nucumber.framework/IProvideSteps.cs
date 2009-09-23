using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nucumber.Framework
{
    public interface IProvideSteps
    {
        IEnumerable<TransformDefinition> TransformDefinitions { get; }
        CombinedStepDefinitions StepDefinitions { get; }
        object WorldView { set; }
        Type WorldViewType { get; }
        IRunStepsFromStrings StepFromStringRunner { set; }

        void BeforeStep();
        void AfterStep();
    }
}