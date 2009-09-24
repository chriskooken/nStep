using Selenium;

namespace Cucumber
{
    public class SeleniumWorldView
    {
        public readonly DefaultSelenium Browser;
        public SeleniumWorldView()
        {
            Browser = new DefaultSelenium("localhost", 4444, "*iexplore", "http://www.google.com");
            Browser.Start();
        }
    }
}