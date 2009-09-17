using System.Collections.Generic;

namespace Nucumber.Framework
{
    public class CombinedStepDefinitions
    {
        public IEnumerable<StepDefinition> GivenStepDefinitions {get; set;}

        public IEnumerable<StepDefinition> WhenStepDefinitions { get; set; }

        public IEnumerable<StepDefinition> ThenStepDefinitions { get; set; }

        public IEnumerable<TransformDefinition> TransformDefinitions { get; set; }
    }
}