using System;
using Selenium;
using Nucumber.Framework;

namespace Cucumber
{

    public class bar
    {
        public string value;
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
            Transform("(user .*)", userName =>
                {
                    return new bar {value = userName};
                });

            Given("blah (user .*) blah (.*) blah", new { userName = null as bar, foo = "" }, parms =>
            {
                Console.Write(parms.foo);
                Console.Write(parms.userName.value);
            });

            Given("^My Name is \"([^\"]*)\"$", (name) =>
            {
                World.selenium.Open("http://www.google.com");
                World.selenium.WaitForPageToLoad("10");
                World.selenium.Type("q", "dogs are cool things");
                
                //Console.WriteLine("Name is: "+ name);
                //throw new Exception("you suck");
            });

            Given("^I live at \"([^\"]*)\"$", (location) =>
            {
                Console.WriteLine("Location is: " + location);
            });

            Given("^My city is \"([^\"]*)\" and my state is \"([^\"]*)\"$", (city, state) =>
            {
                //Console.WriteLine("City is: " + city);
                //Console.WriteLine("State is: " + state);
            });
        }
    }
        public class TestWorldView
        {
            public readonly DefaultSelenium selenium;
            public TestWorldView()
            {
                selenium = new DefaultSelenium("localhost", 4444, "*iexplore", "http://www.google.com");
                selenium.Start();
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

