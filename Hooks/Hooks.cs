using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using WebDriverManager.DriverConfigs.Impl;


namespace DeeBeersProject.Hooks
{
        [Binding]
        public sealed class Hooks
        {

        IWebDriver driver;
        private IObjectContainer _container;
            public Hooks(IObjectContainer container)
            {
                _container = container;

            }


            [BeforeScenario(Order = 1)]
            public void FirstBeforeScenario(ScenarioContext scenarioContext)
            {
              new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
              driver = new ChromeDriver();
              driver.Manage().Window.Maximize();
              driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
              _container.RegisterInstanceAs<IWebDriver>(driver);
                
            }

            [AfterScenario]
            public void AfterScenario()
            {
                var driver = _container.Resolve<IWebDriver>();

                if (driver != null)
                {
                    driver.Quit();
            }

        }
           
        }
    }
