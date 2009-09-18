using System.Collections.Generic;

namespace Nucumber.Framework
{
    public class CombinedStepDefinitions
    {
        public IEnumerable<StepDefinition> Givens {get; set;}

        public IEnumerable<StepDefinition> Whens { get; set; }

        public IEnumerable<StepDefinition> Thens { get; set; }
    }
}