using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Selenium;

namespace nStep.Selenium
{
	static class SeleniumExtensions
	{
        public static void ClickLinkByText(this DefaultSelenium browser, string linkText)
        {
            browser.Click("link="+linkText);
        }

        public static void ClickButtonByText(this DefaultSelenium browser, string linkText)
        {
            browser.Click("//input[@value='" + linkText + "' and (@type='button' or @type='submit')]");
        }  
        
        public static void ClickById(this DefaultSelenium browser, string id)
        {
            browser.Click("id="+id);
        }

        public static void ClickByName(this DefaultSelenium browser, string name)
        {
            browser.Click("name=" + name);
        }

        public static void ClickByCssLocator(this DefaultSelenium browser, string locator)
        {
            browser.Click("css=" + locator);
        }

        public static Uri GetUrl(this DefaultSelenium browser)
        {
            Uri uri = new Uri(browser.GetLocation());
            return uri;
        }
	}
}
