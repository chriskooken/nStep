using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nucumber.Framework
{
    public class StepDefinition
    {
        public StepKinds Kind { get; set; }

        public Regex Regex { get; set; }

        public Delegate Action { get; set; }

        public IEnumerable<Type> ParamsTypes { get; set; }
    }
}