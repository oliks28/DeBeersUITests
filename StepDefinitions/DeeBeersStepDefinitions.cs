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
using System.Text.RegularExpressions;
using System.Xml.Schema;


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

        [Then("the user verifies that the result contains (.*) items and titles are displayed")] XmlSchemaXPath`
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
            public void ThenCheckThatThePredictedSearchAreListedAsFollows(DataTable expectedTable)
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//ul[@id='ui-id-1']")));

                Assert.IsTrue(deeBeersPage.SearchList().Displayed, "Search list is not displayed.");

                var suggestions = deeBeersPage.SearchList().FindElements(By.TagName("li"));
                Console.WriteLine($"DEBUG: Found {suggestions.Count} search suggestions.");


                Dictionary<string, int> actualSuggestions = new Dictionary<string, int>();
                foreach (var suggestion in suggestions)
                {
                    string text = suggestion.Text.Trim();
                    var match = Regex.Match(text, @"^(.*?)\s*\((\d+)\)$");

                    if (match.Success)
                    {
                        string suggestionText = match.Groups[1].Value.Trim();
                        int suggestionValue = int.Parse(match.Groups[2].Value.Trim());
                        actualSuggestions[suggestionText] = suggestionValue;
                    }
                    Console.WriteLine($"Extracted: {text}");
                }


                Dictionary<string, int> expectedSuggestions = expectedTable.Rows
                    .Skip(1)
                    .ToDictionary(row => row[0].ToString().Trim(), row => int.Parse(row[1].ToString().Trim()));


                Assert.AreEqual(expectedSuggestions.Count, actualSuggestions.Count,
                    $"Mismatch in suggestion count. Expected: {expectedSuggestions.Count}, Found: {actualSuggestions.Count}");

                foreach (var expected in expectedSuggestions)
                {
                    Assert.IsTrue(actualSuggestions.ContainsKey(expected.Key), $"Missing suggestion: {expected.Key}");
                    Assert.AreEqual(expected.Value, actualSuggestions[expected.Key],
                        $"Mismatch for '{expected.Key}'. Expected: {expected.Value}, Found: {actualSuggestions[expected.Key]}");
                }

            }


        }
    }
