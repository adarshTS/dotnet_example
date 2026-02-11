using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using NUnit.Framework;

namespace BStackDemoTests;

public class Tests
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        // Local Chrome setup
        // driver = new ChromeDriver();
        // driver.Manage().Window.Maximize();

        // BrowserStack RemoteWebDriver setup
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
            { "sessionName", TestContext.CurrentContext.Test.Name },
            { "debug", "true" },
            { "networkLogs", "true" },
            { "consoleLogs", "errors" }
        };
        capabilities.AddAdditionalOption("bstack:options", bstackOptions);

        driver = new RemoteWebDriver(new Uri("https://hub-cloud.browserstack.com/wd/hub"), capabilities);
        driver.Manage().Window.Maximize();
    }

    [TearDown]
    public void TearDown()
    {
        if (driver != null)
        {
            try
            {
                var outcome = TestContext.CurrentContext.Result.Outcome.Status;
                var testPassed = outcome == NUnit.Framework.Interfaces.TestStatus.Passed;
                var status = testPassed ? "passed" : "failed";
                var reason = testPassed ? "Test passed successfully" : TestContext.CurrentContext.Result.Message ?? "Test failed";
                
                // Mark session status using JavaScript executor
                var jsExecutor = (IJavaScriptExecutor)driver;
                var script = $"browserstack_executor: {{\"action\": \"setSessionStatus\", \"arguments\": {{\"status\":\"{status}\", \"reason\":\"{reason.Replace("\"", "'")}\"}}}}";
                jsExecutor.ExecuteScript(script);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to mark BrowserStack session status: {ex.Message}");
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }

    [Test]
    public void TestBStackDemo()
    {
        driver.Navigate().GoToUrl("https://bstackdemo.com/");
    
        System.Threading.Thread.Sleep(2000);
        
        var selGoogle = driver.FindElement(By.XPath("//span[@class='checkmark' and text()='Google']"));
        selGoogle.Click();
        
        System.Threading.Thread.Sleep(1000);
        
        var addToCartBtn = driver.FindElement(By.XPath("//div[@class='shelf-item__buy-btn' and text()='Add to cart']"));
        addToCartBtn.Click();
        
        System.Threading.Thread.Sleep(2000);
        
        Assert.Pass();
    }

    [Test]
    public void Analytics_TC003_SkipStatus_VerifySkipReporting()
    {
        TestContext.WriteLine("[ANALYTICS] This test will be skipped to evaluate skip tracking");
        Assert.Ignore("Test");
    }
}
