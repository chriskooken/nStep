using nStep.Framework.WorldViews;
using Selenium;

namespace nStep
{
    public class SeleniumWorldView : IAmWorldView 
    {
        public readonly DefaultSelenium Browser;
        public SeleniumWorldView()
        {
            Browser = new DefaultSelenium("localhost", 4444, "*iexplore", "http://www.google.com");
            Browser.Start();
        }
    }
}