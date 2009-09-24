using System.Collections.Generic;

namespace Nucumber.Core
{
    public class Scenario
    {
        private IList<FeatureStep> steps;
        public Scenario()
        {
            steps = new List<FeatureStep>();
        }

        public string Title { get; set; }
        public int LineNumber { get; set; }

        public IList<FeatureStep> Steps
        {
            get { return steps; }
            set { steps = value; }
        }
    }
}