using Nucumber.Framework;

namespace Nucumber.Core
{
    public class FeatureStep
    {
        public string FeatureLine { get; set; }
        public int LineNumber { get; set; }
        public string FeatureFile { get; set; }
        public StepKinds Kind { get; set; }
    }
}
