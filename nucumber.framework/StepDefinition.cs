using System;
using System.Text.RegularExpressions;

namespace Nucumber.Framework
{
    public class StepDefinition
    {
        public StepKinds Kind { get; set; }

        public Regex Regex { get; set; }

        public object Action { get; set; }

        public Type ParamsType { get; set; }
    }
}