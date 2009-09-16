using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nucumber.Framework
{
    public interface IProvideSteps
    {
        IEnumerable<StepDefinition> StepDefinitions { get; }
        void SetWorldView(object worldView);
    }
}