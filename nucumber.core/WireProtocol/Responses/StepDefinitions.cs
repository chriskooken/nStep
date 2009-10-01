using System.Collections.Generic;
using Nucumber.Framework;
using System.Linq;

namespace Nucumber.Core.WireProtocol.Responses
{
    public class StepDefinitions : VersionOneMessage
    {
        public static StepDefinitions FromStepDefinitionsEnumerable(IEnumerable<StepDefinition> stepDefinitions)
        {
            return new StepDefinitions
                       {
                           step_definitions =
                               stepDefinitions.Select(s => new StepDefinitionMessage {regexp = s.Regex.ToString(), id = s.Guid.ToString()}).ToArray()
                       };
        }

        public StepDefinitionMessage[] step_definitions { get; set; }

        public class StepDefinitionMessage
        {
            public string id { get; set; }
            public string regexp { get; set; }
        }
    }
}
