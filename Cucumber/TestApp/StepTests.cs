using System;
using System.Linq;
using System.Text;
using Selenium;

namespace Cucumber
{
    public class StepTests : StepSetBase<TestWorldView>
    {
        DefaultSelenium selenium;

        public StepTests()
        {
            Given("^My Name is \"([^\"]*)\"$", (name) =>
            {
                selenium = new DefaultSelenium("localhost", 4444, "*iexplore", "http://www.google.com");
                selenium.Start();
                selenium.Open("http://www.google.com");
                selenium.WaitForPageToLoad("10");
                selenium.Type("q","dogs are cool things");
                
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
            public TestWorldView()
            {
                Console = System.Console.Out;
            }

            public readonly System.IO.TextWriter Console;
        }

        public class WorldViewInitializerTests : WorldViewInitializer<TestWorldView>
        {
            public WorldViewInitializerTests()
            {
                WorldView = new TestWorldView();
            }
        }


    }

