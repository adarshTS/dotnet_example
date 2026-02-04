using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BStackDemoTests;

public class Tests
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
    }

    [TearDown]
    public void TearDown()
    {
        driver?.Quit();
        driver?.Dispose();
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
}
