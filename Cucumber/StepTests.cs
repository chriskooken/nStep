using System;
using System.Linq;
using System.Text;

namespace Cucumber
{
    public class StepTests : StepMother
    {
        public StepTests()
        {
            Given("^My Name is \"([^\"]*)\"$", (name) =>
            {
                //throw  new Exception("you suck");
                Console.WriteLine("Name is: "+ name);
            });

            Given("^I live at \"([^\"]*)\"$", (location) =>
            {
                //Console.WriteLine("Location is: " + location);
            });

            Given("^My city is \"([^\"]*)\" and my state is \"([^\"]*)\"$", (city, state) =>
            {
                //Console.WriteLine("City is: " + city);
                //Console.WriteLine("State is: " + state);
            });
        }




    }
}
