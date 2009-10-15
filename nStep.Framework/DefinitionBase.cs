using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace nStep.Framework
{
    public class DefinitionBase
    {
        protected DefinitionBase(Regex regex, Delegate action)
        {
            this.Regex = regex;
            this.Action = action;
            this.InputParamsTypes = action.GetType().GetGenericArguments();
        }

        public Regex Regex { get; private set; }
        public Delegate Action { get; private set; }
        public IEnumerable<Type> InputParamsTypes { get; private set; }
    }
}