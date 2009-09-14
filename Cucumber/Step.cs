using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cucumber
{
    public class Step
    {
        public string StepText { get; set; }
        public int LineNumber { get; set; }
        public string FeatureFile { get; set; }
    }
}
