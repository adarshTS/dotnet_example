using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using Reqnroll;

namespace BStackDemoTests.StepDefinitions;

[Binding]
public class BStackDemoSteps
{
    private IWebDriver? _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
    }

    [AfterScenario]
    public void TearDown()
    {
        _driver?.Quit();
        _driver?.Dispose();
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
        // This won't execute due to Assert.Ignore above
    }
}
