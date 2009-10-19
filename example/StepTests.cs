using System;
using Selenium;
using nStep.Framework;
using NUnit.Framework;
using nStep.Selenium;

namespace nStep
{

    public class User
    {
        public string Name;
    }

    public sealed class StepTests : StepSetBase<SeleniumWorldView>
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

            Then("I should see \"([^\"]*)\"", (string arg1) =>
            {
                Pending();
            });

            Transform("[Uu]ser \"(.*)\"", userName =>
                {
                    return new User {Name = userName};
                });

            Given("I am on the google homepage", () =>
                {
                    World.Browser.Open("http://www.google.com");
                    World.Browser.WaitForPageToLoad("10");
                });

            Given("^(User \".*\") is logged in", (User user) =>
                 {
                    Console.WriteLine(user.Name);  
                 });

            When("I type \"([^\"]*)\" in the \"([^\"]*)\" field", (string value, string field) =>
                {
                    World.Browser.Type("q", value);
                });

            When("I click the \"([^\"]*)\" button", (string buttonName) =>
                {
                    World.Browser.ClickButtonByText(buttonName);
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
                    World.Browser.ClickLinkByText(link);
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

			Given("^user \"([^\"]*)\" is logged in$", (string arg1) =>
				{
					
				});
		}
    }
}

