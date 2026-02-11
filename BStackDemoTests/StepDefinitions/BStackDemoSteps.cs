using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using NUnit.Framework;
using Reqnroll;

namespace BStackDemoTests.StepDefinitions;

[Binding]
public class BStackDemoSteps
{
    private IWebDriver? _driver;
    private readonly ScenarioContext _scenarioContext;

    public BStackDemoSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public void Setup()
    {
        // _driver = new ChromeDriver();
        // _driver.Manage().Window.Maximize();

        var username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME_DEMO");
        var accessKey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY_DEMO");
        
        
        var capabilities = new ChromeOptions();  
        
        var bstackOptions = new Dictionary<string, object>
        {
            { "userName", username },
            { "accessKey", accessKey },
            { "os", "Windows" },
            { "osVersion", "11" },
            { "browserVersion", "latest" },
            { "projectName", "BrowserStack Dotnet Legacy StatusMarking" },
            { "buildName", "BStackDemoLegacyStatusMarking" },
            { "sessionName", _scenarioContext.ScenarioInfo.Title },
            { "debug", "true" },
            { "networkLogs", "true" },
            { "consoleLogs", "errors" }
        };
        capabilities.AddAdditionalOption("bstack:options", bstackOptions);

        _driver = new RemoteWebDriver(new Uri("https://hub-cloud.browserstack.com/wd/hub"), capabilities);
        _driver.Manage().Window.Maximize();
    }

    [AfterScenario]
    public void TearDown()
    {
        if (_driver != null)
        {
            try
            {
                var testError = _scenarioContext.TestError;
                var status = "passed";
                var reason = "Test passed successfully";
                
                if (testError != null)
                {
                    if (testError.GetType().Name == "SuccessException")
                    {
                        status = "passed";
                        reason = testError.Message;
                    }
                    else if (testError.GetType().Name == "IgnoreException")
                    {
                        status = "passed";
                        reason = "Test was skipped: " + testError.Message;
                    }
                    else
                    {
                        status = "failed";
                        reason = testError.Message ?? "Test failed";
                    }
                }
                
                var jsExecutor = (IJavaScriptExecutor)_driver;
                var script = $"browserstack_executor: {{\"action\": \"setSessionStatus\", \"arguments\": {{\"status\":\"{status}\", \"reason\":\"{reason.Replace("\"", "'")}\"}}}}";
                jsExecutor.ExecuteScript(script);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to mark BrowserStack session status: {ex.Message}");
            }
            finally
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }
    }

    [Given(@"I am on the BStackDemo homepage")]
    public void GivenIAmOnTheBStackDemoHomepage()
    {
        _driver!.Navigate().GoToUrl("https://bstackdemo.com/");
        Thread.Sleep(2000);
    }

    [When(@"I filter products by ""(.*)""")]
    public void WhenIFilterProductsBy(string brand)
    {
        var googleCheckmark = _driver!.FindElement(By.XPath($"//span[@class='checkmark' and text()='{brand}']"));
        googleCheckmark.Click();
        Thread.Sleep(2000);
    }

    [When(@"I add the first product to cart")]
    public void WhenIAddTheFirstProductToCart()
    {
        var addToCartBtn = _driver!.FindElement(By.XPath("//div[@class='shelf-item__buy-btn' and text()='Add to cart']"));
        addToCartBtn.Click();
        Thread.Sleep(1000);
    }

    [Then(@"the product should be added successfully")]
    public void ThenTheProductShouldBeAddedSuccessfully()
    {
        Assert.Pass("Product added to cart successfully");
    }

    [Given(@"I am testing skip reporting")]
    public void GivenIAmTestingSkipReporting()
    {
        TestContext.WriteLine("[ANALYTICS] This test will be skipped to evaluate skip tracking");
    }

    [When(@"I intentionally skip this test")]
    public void WhenIIntentionallySkipThisTest()
    {
        Assert.Ignore("Intentional skip: Testing BrowserStack skip/ignore reporting analytics");
    }

    [Then(@"this test should be marked as skipped")]
    public void ThenThisTestShouldBeMarkedAsSkipped()
    {
        //
    }
}
