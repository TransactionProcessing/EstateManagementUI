using Microsoft.Playwright;
using Reqnroll;
using Reqnroll.BoDi;

namespace EstateManagementUI.OfflineIntegrationTests.Common
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer ObjectContainer;
        private ScenarioContext ScenarioContext;
        private static IPlaywright? playwright;
        private static IBrowser? browser;

        public Hooks(IObjectContainer objectContainer)
        {
            this.ObjectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static async Task BeforeTestRun()
        {
            // Install Playwright browsers if needed
            var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
            if (exitCode != 0)
            {
                throw new Exception($"Playwright installation failed with exit code {exitCode}");
            }

            // Initialize Playwright
            playwright = await Playwright.CreateAsync();
        }

        [BeforeScenario(Order = 0)]
        public async Task BeforeScenario(ScenarioContext scenarioContext)
        {
            this.ScenarioContext = scenarioContext;
            String scenarioName = scenarioContext.ScenarioInfo.Title.Replace(" ", "");
            
            var page = await this.CreateBrowserPage();
            
            // Register the page for this scenario
            scenarioContext.ScenarioContainer.RegisterInstanceAs(page, scenarioName);
        }

        [AfterScenario(Order = 0)]
        public async Task AfterScenario()
        {
            String scenarioName = this.ScenarioContext.ScenarioInfo.Title.Replace(" ", "");
            var page = this.ScenarioContext.ScenarioContainer.Resolve<IPage>(scenarioName);
            
            if (page != null)
            {
                // Take screenshot on failure
                if (this.ScenarioContext.TestError != null)
                {
                    var screenshotPath = $"screenshot-{scenarioName}-{DateTime.Now:yyyyMMddHHmmss}.png";
                    await page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath, FullPage = true });
                    Console.WriteLine($"Screenshot saved to: {screenshotPath}");
                }

                await page.CloseAsync();
            }
        }

        [AfterTestRun]
        public static async Task AfterTestRun()
        {
            if (browser != null)
            {
                await browser.CloseAsync();
                browser = null;
            }

            if (playwright != null)
            {
                playwright.Dispose();
                playwright = null;
            }
        }

        private async Task<IPage> CreateBrowserPage()
        {
            String? browserType = Environment.GetEnvironmentVariable("Browser");
            String? isCi = Environment.GetEnvironmentVariable("IsCI");
            
            if (browser == null)
            {
                switch (browserType)
                {
                    case "Firefox":
                        browser = await playwright!.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                        {
                            Headless = String.Compare(isCi, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase) == 0,
                            Args = new[] { "--ignore-certificate-errors" }
                        });
                        break;
                    case "WebKit":
                        browser = await playwright!.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
                        {
                            Headless = String.Compare(isCi, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase) == 0,
                            Args = new[] { "--ignore-certificate-errors" }
                        });
                        break;
                    case null:
                    case "Chrome":
                    default:
                        browser = await playwright!.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                        {
                            Headless = String.Compare(isCi, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase) == 0,
                            Args = new[] 
                            { 
                                "--ignore-certificate-errors",
                                "--no-sandbox",
                                "--disable-dev-shm-usage",
                                "--disable-gpu",
                                "--disable-extensions"
                            }
                        });
                        break;
                }
            }

            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true,
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
            });

            var page = await context.NewPageAsync();
            
            return page;
        }
    }
}
