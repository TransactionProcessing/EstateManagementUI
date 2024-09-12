using System.Diagnostics.Contracts;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using Reqnroll;
using Reqnroll.BoDi;
using Shared.IntegrationTesting;

namespace EstateManagementUI.IntegrationTests.Common
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer ObjectContainer;
        private ScenarioContext ScenarioContext;


        public Hooks(IObjectContainer objectContainer)
        {
            this.ObjectContainer = objectContainer;
        }

        [BeforeScenario(Order = 0)]
        public async Task BeforeScenario(ScenarioContext scenarioContext){
            this.ScenarioContext = scenarioContext;
            String scenarioName = scenarioContext.ScenarioInfo.Title.Replace(" ", "");
            IWebDriver webDriver = await this.CreateWebDriver();
            
            webDriver.Manage().Window.Maximize();
            //this.ObjectContainer.RegisterInstanceAs(this.WebDriver, scenarioName);
            scenarioContext.ScenarioContainer.RegisterInstanceAs(webDriver, scenarioName);
        }

        [AfterScenario(Order = 0)]
        public void AfterScenario()
        {
            String scenarioName = this.ScenarioContext.ScenarioInfo.Title.Replace(" ", "");
            IWebDriver webDriver = this.ScenarioContext.ScenarioContainer.Resolve<IWebDriver>(this.ScenarioContext.ScenarioInfo.Title.Replace(" ", ""));
            if (webDriver != null)
            {
                webDriver.Quit(); //.Dispose();
            }
        }

        private async Task<IWebDriver> CreateWebDriver(){
            IWebDriver webDriver = null;

            String? browser = Environment.GetEnvironmentVariable("Browser");
            String? isCi = Environment.GetEnvironmentVariable("IsCI");
            
            //browser = "Edge";
            switch (browser)
            {
                case null:
                case "Chrome":
                {
                    ChromeOptions options = new ChromeOptions();
                    
                    options = options.WithNoSandBox()
                                     .WithAcceptInsecureCertificate()
                                     .WithDisableDevShmUsage()
                                     .WithDisableExtensions()
                                     .WithDisableGpu()
                                     .WithDisableInfobars()
                                     .WithHeadless(isCi)
                                     .WithWindowSize(1280, 1024);

                    webDriver = new ChromeDriver(options);
                    break;
                }
                case "Firefox":
                {
                    FirefoxOptions options = new FirefoxOptions();
                    options = options.WithAcceptInsecureCertificate()
                                     .WithHeadless(isCi)
                                     .WithNetworkCookieBehaviour();

                    await Retry.For(async () =>
                                    {
                                        webDriver = new FirefoxDriver(options);
                                    }, TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(60));
                    break;
                }
                case "Edge":
                {
                    EdgeOptions options = new EdgeOptions();
                    options = options.WithAcceptInsecureCertificate()
                                     .WithHeadless(isCi)
                                     .WithWindowSize(1280, 1024);
                    await Retry.For(async () =>
                                    {
                                        webDriver = new EdgeDriver(options);
                                    }, TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(60));
                    break;
                }
            }
            return webDriver;
        }
    }
    
    public static class ChromeOptionsBuilder{

        [Pure]
        public static ChromeOptions WithDisableGpu(this ChromeOptions options){
            options.AddArgument("--disable-gpu");
            return options;
        }

        [Pure]
        public static ChromeOptions WithNoSandBox(this ChromeOptions options){
            options.AddArgument("--no-sandbox");
            return options;
        }

        [Pure]
        public static ChromeOptions WithDisableDevShmUsage(this ChromeOptions options){
            options.AddArgument("--disable-dev-shm-usage");
            return options;
        }

        [Pure]
        public static ChromeOptions WithHeadless(this ChromeOptions options, String isCI){
            if (String.Compare(isCI, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase) == 0){
                options.AddArgument("--headless");
            }
            return options;
        }

        [Pure]
        public static ChromeOptions WithDisableInfobars(this ChromeOptions options){
            options.AddArguments("--disable-infobars");
            return options;
        }

        [Pure]
        public static ChromeOptions WithDisableExtensions(this ChromeOptions options)
        {
            options.AddArguments("--disable-extensions");
            return options;
        }

        [Pure]
        public static ChromeOptions WithWindowSize(this ChromeOptions options, Int32 height, Int32 width){
            String windowSize = $"--window-size={height}x{width}";
            options.AddArguments(windowSize);
            return options;
        }

        public static ChromeOptions WithAcceptInsecureCertificate(this ChromeOptions options){
            options.AcceptInsecureCertificates = true;
            return options;
        }
    }

    public static class FirefoxOptionsBuilder{
        [Pure]
        public static FirefoxOptions WithHeadless(this FirefoxOptions options, String isCi){
            if (String.Compare(isCi, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                options.AddArguments("--headless");
            }
            return options;
        }

        [Pure]
        public static FirefoxOptions WithNetworkCookieBehaviour(this FirefoxOptions options){
            options.SetPreference("network.cookie.cookieBehavior", 0);
            return options;
        }
        
        [Pure]
        public static FirefoxOptions WithAcceptInsecureCertificate(this FirefoxOptions options)
        {
            options.AcceptInsecureCertificates = true;
            return options;
        }
    }

    public static class EdgeOptionsBuilder{

        [Pure]
        public static EdgeOptions WithAcceptInsecureCertificate(this EdgeOptions options)
        {
            options.AcceptInsecureCertificates = true;
            return options;
        }

        [Pure]
        public static EdgeOptions WithHeadless(this EdgeOptions options, String isCi)
        {
            if (String.Compare(isCi, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                options.AddArguments("--headless");
            }
            return options;
        }

        [Pure]
        public static EdgeOptions WithWindowSize(this EdgeOptions options, Int32 height, Int32 width)
        {
            String windowSize = $"--window-size={height}x{width}";
            options.AddArguments(windowSize);
            return options;
        }
    }
}
