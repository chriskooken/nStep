using System.Collections.Generic;

namespace Cucumber
{
    public class Scenario
    {
        private IList<string> steps;
        public Scenario()
        {
            steps = new List<string>();
        }

        public string Title { get; set; }


        public IList<string> Steps
        {
            get { return steps; }
            set { steps = value; }
        }
    }
}