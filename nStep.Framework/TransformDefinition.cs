using System;
using System.Text.RegularExpressions;

namespace nStep.Framework
{
    public class TransformDefinition
    {
        public Regex Regex { get; set; }

        public Delegate Func { get; set; }

        public Type ReturnType { get; set; }
    }
}