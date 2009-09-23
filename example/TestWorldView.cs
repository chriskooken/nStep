using Selenium;

namespace Cucumber
{
    public class TestWorldView
    {
        public readonly DefaultSelenium Browser;
        public TestWorldView()
        {
            Browser = new DefaultSelenium("localhost", 4444, "*iexplore", "http://www.google.com");
            Browser.Start();
        }
    }
}