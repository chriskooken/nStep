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

            Given("([Uu]ser .*) has an email address equal to (.*)", (User user, string newName) =>
        	{
        	    user.Name = newName;
        	});
            
            Given("Given ProviderLocation \"([^\"]*)\" exists", locationName =>
            {
                    Console.WriteLine("Got location name " + locationName);
            });

        	Given("^My Name is \"([^\"]*)\"$", name =>
            {
                World.Browser.Open("http://www.google.com");
                World.Browser.WaitForPageToLoad("10");
                World.Browser.Type("q", "dogs are cool things");

                Console.WriteLine("Name is: " + name);
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
                Pending();
                World.Browser.GetTitle().Should().Be.EqualTo(page);
            });

        }
    }
        public class TestWorldView
        {
            public readonly DefaultSelenium Browser;
            public TestWorldView()
            {
                Browser = new DefaultSelenium("localhost", 4444, "*iexplore", "http://www.google.com");
                Browser.Start();
            }
        }

        public class WorldViewProvider : WorldViewProviderBase<TestWorldView>
        {
            protected override TestWorldView InitializeWorldView()
            {
                return new TestWorldView();
            }
        }

    public class Environment : EnvironmentBase
    {
        public override void SessionStart()
        {
            //Maybe create the database? Init StructureMap? Start Selenium? Something else?
        }

        public override void SessionEnd()
        {
            //Destroy the database? Clean up? Stop Selenium?
        }

        public override void AfterScenario()
        {
            //Maybe clean out the database?
        }
    }

    }

