using System;
using System.IO;
using deeBeersProject.Pages;
using OpenQA.Selenium;
using Reqnroll;
using FluentAssertions;
using NUnit.Framework;
using RazorEngine.Configuration;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace deeBeersProject.StepDefinitions
{
    [Binding]
    public class DeeBeersStepDefinitions
    {
        private IWebDriver driver;
        public DeeBeersPage deeBeersPage;
        private string baseUrl;


        public DeeBeersStepDefinitions(IWebDriver driver, DeeBeersPage deeBeersPage)
        {
            this.driver = driver;
            this.deeBeersPage = deeBeersPage;

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            baseUrl = config["AppSettings:BaseUrl"]!;

        }

        [Given("the user navigates to the homepage")]
        public void GivenGoToTheUrl()
        {
            driver.Url = baseUrl;
            driver.Navigate().Refresh();
        }

        [When("the user hover on about us and click on the technology")]
        public void WhenHoverOnAboutUsAndClickOnTheTechnology()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[@id='onetrust-accept-btn-handler']")));
            deeBeersPage.Cookies().Click();
            deeBeersPage.HoverToElement(deeBeersPage.AboutUs());
            deeBeersPage.Technology().Click();

        }

        [Then("Verify the presence of {string} text")]
        public void ThenVerifyTextExists(string text)
        {
            DeeBeersPage.JsClickWithScroll(driver, deeBeersPage.Text());
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//strong[normalize-space()='World-leading Technology'])[1]")));
            deeBeersPage.Text().Text.Should().Contain(text);

        }

        [When("the user click on the search box and search {string}")]
        public void WhenClickOnTheSearchBoxAndSearch(string diamondProof)
        {
            DeeBeersPage.JsClickWithScroll(driver, deeBeersPage.SearchIcon());
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", deeBeersPage.SearchIcon());
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='searchTextbox']")));
            deeBeersPage.SearchBox().Clear();
            deeBeersPage.SearchBox().SendKeys(diamondProof);
            deeBeersPage.SearchBox().SendKeys(Keys.Enter);

        }

        [Then("the user verifies that the result contains (.*) items and titles are displayed")]
        public void WhenCheckThatTheResultContainsItemsAndTitlesAreDisplayed(string itemCount)
        {
            Assert.IsTrue(deeBeersPage.Result().Displayed);
            Assert.IsTrue(deeBeersPage.Result().Text.Contains(itemCount));
            Assert.AreEqual(Int32.Parse(itemCount), deeBeersPage.Results().Count());
            Assert.IsTrue(deeBeersPage.Title1_DiamondVerificationDevice().Displayed);
            Assert.IsTrue(deeBeersPage.Title2_InterimFinancialResult().Displayed);
            Assert.IsTrue(deeBeersPage.Title3_Production_report().Displayed);
            Assert.IsTrue(deeBeersPage.Title4_PriliminaryFinancial().Displayed);
            Assert.IsTrue(deeBeersPage.Title5_IndustryNews().Displayed);

        }

        [When("the user click on the search box and search for {string}")]
        public void WhenClickOnTheSearchBoxAndSearchFor(string text)
        {
            deeBeersPage.SearchIcon().Click();
            deeBeersPage.SearchBox().SendKeys(Keys.Control + "a");
            deeBeersPage.SearchBox().SendKeys(Keys.Backspace);
            deeBeersPage.SearchBox().SendKeys(text);
        }

        [Then("the user verifies that the predicted search are listed as follows")]
        public void ThenTheUserVerifiesThatThePredictedSearchAreListedAsFollows(DataTable table)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//ul[@id='ui-id-1']")));
            Assert.IsTrue(deeBeersPage.SearchList().Displayed);

            var suggestions = deeBeersPage.SearchList().FindElements(By.TagName("li"));

            Assert.AreNotEqual(0, suggestions.Count, "No search predictions displayed! Possible issues: XPath incorrect or dropdown not visible.");
            Assert.AreEqual(10, suggestions.Count, $"The number of search predictions does not match the expected count. Expected: 10, But was: {suggestions.Count}");


            Dictionary<string, string> expectedResults = new Dictionary<string, string>
            {
                { "jwaneng", "130" },
                { "jwaneng mine", "60" },
                { "jwaneng cut", "28" },
                { "jwaneng mines", "23" },
                { "jwaneng orapa", "12" },
                { "jwaneng production", "10" },
                { "jwaneng 1982", "6" },
                { "jwaneng hospital", "5" },
                { "jwaneng botswana", "4" },
                { "jwaneng community", "4" }
            };
            
            foreach (var suggestion in suggestions)
            {
                var parts = suggestion.Text.Split('(');
                var text = parts[0].Trim();
                var value = parts.Length > 1 ? parts[1].Trim(')') : "N/A";

                // Assert that each suggestion exists in expectedResults and has the correct value
                Assert.IsTrue(expectedResults.ContainsKey(text), $"Unexpected suggestion found: {text}");
                Assert.AreEqual(expectedResults[text], value, $"Mismatch for suggestion '{text}': Expected {expectedResults[text]}, but found {value}");
            }
        }

       }
   
    }
