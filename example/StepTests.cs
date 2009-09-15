using System;
using Selenium;
using nucumber.Framework;

namespace Cucumber
{
    public class StepTests : StepSetBase<TestWorldView>
    {
        public StepTests()
        {
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

        public class WorldViewInitializerTests : WorldViewInitializer<TestWorldView>
        {
            public WorldViewInitializerTests()
            {
                WorldView = new TestWorldView();
            }
        }


    }

