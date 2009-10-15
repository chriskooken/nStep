using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace nStep.Framework
{
    public class TransformDefinition : DefinitionBase
    {
        public TransformDefinition(Regex regex, Delegate action, Type returnType)
            : base(regex, action)
        {
            ReturnType = returnType;
        }

        public Type ReturnType { get; private set; }
    }
}