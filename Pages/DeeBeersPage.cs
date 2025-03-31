using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace deeBeersProject.Pages
{
    public class DeeBeersPage
    {
        public IWebDriver driver;

        public DeeBeersPage(IWebDriver driver)
        {
            this.driver = driver;
        }


        By cookies = By.XPath("//button[@id='onetrust-accept-btn-handler']");
        By about_us = By.XPath("(//span[contains(text(),'About Us')])[2]");
        By tech = By.XPath("//a[contains(text(),'Technology')]");
        By text = By.XPath("(//strong[normalize-space()='World-leading Technology'])[1]");
        By searchIcon = By.XPath("(//i[@class='icon-search'])[1]");
        By searchBox = By.XPath("//input[@id='searchTextbox']");
        By resultSummary = By.XPath("//div[@id='result-summary']");
        By resultCount = By.XPath("//span[@class='resultURL']");
        By title1 = By.XPath("//a[contains(text(),'Diamond verification device, DiamondProof, lands i')]");
        By title2 = By.XPath("//a[normalize-space()='Interim Financial Results for 2024']");
        By title3 = By.XPath("//a[normalize-space()='Production Report for the Third Quarter of 2024']");
        By title4 = By.XPath("//a[normalize-space()='Preliminary Financial Results for 2024']");
        By title5 = By.XPath("//a[normalize-space()='Industry News and Events']");
        By searchList = By.XPath("//ul[@id='ui-id-1']");

        public IWebElement Cookies() => driver.FindElement(cookies);
        public IWebElement AboutUs() => driver.FindElement(about_us);
        public IWebElement Technology() => driver.FindElement(tech);
        public IWebElement Text() => driver.FindElement(text);
        public IWebElement SearchIcon() => driver.FindElement(searchIcon);
        public IWebElement SearchBox() => driver.FindElement(searchBox);
        public IWebElement Result() => driver.FindElement(resultSummary);
        public IList<IWebElement> Results() => driver.FindElements(resultCount);
        public IWebElement Title1_DiamondVerificationDevice() => driver.FindElement(title1);
        public IWebElement Title2_InterimFinancialResult() => driver.FindElement(title2);
        public IWebElement Title3_Production_report() => driver.FindElement(title3);
        public IWebElement Title4_PriliminaryFinancial() => driver.FindElement(title4);
        public IWebElement Title5_IndustryNews() => driver.FindElement(title5);
        public IWebElement SearchList() => driver.FindElement(searchList);
        public IList<IWebElement> SearchListItems() => driver.FindElements(searchList);



        public IWebElement HoverToElement(IWebElement element)
        {
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
            return element;
        }

        public static void JsClickWithScroll(IWebDriver driver, IWebElement element)
        {            
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            js.ExecuteScript("arguments[0].click();", element);
        }

    }
}
