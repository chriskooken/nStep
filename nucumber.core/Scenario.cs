using System.Collections.Generic;

namespace nucumber.core
{
    public class Scenario
    {
        private IList<Step> steps;
        public Scenario()
        {
            steps = new List<Step>();
        }

        public string Title { get; set; }


        public IList<Step> Steps
        {
            get { return steps; }
            set { steps = value; }
        }
    }
}