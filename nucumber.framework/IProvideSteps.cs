using System;
using System.Collections.Generic;
using Nucumber.Framework.ScenarioHooks;

namespace Nucumber.Framework
{
    public interface IProvideSteps : IProvideScenarioHooks 
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