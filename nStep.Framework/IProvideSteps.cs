using System;
using System.Collections.Generic;
using nStep.Framework.Execution;
using nStep.Framework.ScenarioHooks;

namespace nStep.Framework
{
    public interface IProvideSteps : IProvideScenarioHooks 
    {
        IEnumerable<TransformDefinition> TransformDefinitions { get; }
        CombinedStepDefinitions StepDefinitions { get; }
        object WorldView { set; }
        Type WorldViewType { get; }
        IRunSteps StepRunner { set; }

        void BeforeStep();
        void AfterStep();
    }
}