using System;
using Selenium;
using Nucumber.Framework;
using NUnit.Framework;

namespace Cucumber
{

    public class User
    {
        public string Name;
    }

    public class StepTests : StepSetBase<TestWorldView>
    {
        public override void BeforeStep()
        {
            //Do something cool, like open a Transaction
        }

        public override void AfterStep()
        {
            //Maybe close the transaction here?
        }

        public StepTests()
        {
            Transform("([Uu]ser .*)", userName =>
                {
                    return new User {Name = userName};
                });

            Given("I am on the google homepage", () =>
            {
                World.Browser.Open("http://www.google.com");
                World.Browser.WaitForPageToLoad("10");
                World.Browser.Type("q", "dogs are cool things");
            });

            When("I type \"([^\"]*)\" in the \"([^\"]*)\" field", (string arg1, string arg2) =>
            {
                
            });
            
            Given("Given ProviderLocation \"([^\"]*)\" exists", locationName =>
            {
                    Console.WriteLine("Got location name " + locationName);
            });

            Given("^I live at \"([^\"]*)\"$", location =>
            {
                Console.WriteLine("Location is: " + location);
                location = location.Remove(5);
            });

            Given("^I live at home$", () =>
            {
                Console.WriteLine("Location is: home");
            });

            Given("^My city is \"([^\"]*)\" and my state is \"([^\"]*)\"$", (int city, string state) =>
            {
                Console.WriteLine("City is: " + city);
                Console.WriteLine("State is: " + state);
            });

            Then("I should be on the \"([^\"]*)\" page", page =>
            {
                var s = new string[] {""};
                s[3] = "hello";
                //World.Browser.GetTitle().Should().Be.EqualTo(page);
            });

        }
    }
}

