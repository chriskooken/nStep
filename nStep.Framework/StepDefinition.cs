using System;
using System.Text.RegularExpressions;

namespace nStep.Framework
{
    public class StepDefinition : DefinitionBase
    {
        public StepDefinition(Regex regex, Delegate action, StepKinds kind, IProvideSteps stepSet)
            : base(regex, action)
        {
            Kind = kind;
            StepSet = stepSet;
        }

        public StepKinds Kind { get; private set; }
        public IProvideSteps StepSet { get; private set; }        
    }
}