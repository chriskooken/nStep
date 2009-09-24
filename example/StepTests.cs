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

    public class StepTests : StepSetBase<SeleniumWorldView>
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
                });

            When("I type \"([^\"]*)\" in the \"([^\"]*)\" field", (string value, string field) =>
                {
                    World.Browser.Type("q", value);
                });

            When("I click the \"([^\"]*)\" button", (string arg1) =>
                {
                    World.Browser.Click("btnG");
                });

            When("I wait for the page to load", () =>
                {
                    World.Browser.WaitForPageToLoad("15000");
                });

            Then("I should be on the \"([^\"]*)\" page", page =>
                {
                    var x = page;
                    World.Browser.GetTitle().Should().Be.EqualTo(page);
                });

            When("I click the \"([^\"]*)\" link", (string link) =>
            {
                World.Browser.Click("link="+link);
            });

            Given("My Name is \"([^\"]*)\"", (string arg1) =>
                {

                });

            Given("I live at \"([^\"]*)\"", (string arg1) =>
                {

                });

            Given("My city is \"([^\"]*)\" and my state is \"([^\"]*)\"", (string arg1, string arg2) =>
                {

                });
        }
    }
}

