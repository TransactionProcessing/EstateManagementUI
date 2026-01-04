using Microsoft.Playwright;
using Reqnroll;
using Reqnroll.BoDi;
using Shared.IntegrationTesting;

namespace EstateManagementUI.IntegrationTests.Common
{
    [Binding]
    [Scope(Tag = "playwright")]
    public class PlaywrightHooks
    {
        private readonly IObjectContainer ObjectContainer;
        private ScenarioContext ScenarioContext;
        private IPlaywright? Playwright;
        private IBrowser? Browser;

        public PlaywrightHooks(IObjectContainer objectContainer)
        {
            this.ObjectContainer = objectContainer;
        }

        [BeforeScenario(Order = 0)]
        public async Task BeforeScenario(ScenarioContext scenarioContext)
        {
            this.ScenarioContext = scenarioContext;
            String scenarioName = scenarioContext.ScenarioInfo.Title.Replace(" ", "");
            
            // Initialize Playwright
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            
            // Create browser and page
            var (browser, page) = await this.CreatePlaywrightBrowser();
            Browser = browser;
            
            // Register page for the scenario
            scenarioContext.ScenarioContainer.RegisterInstanceAs(page, scenarioName);
        }

        [AfterScenario(Order = 0)]
        public async Task AfterScenario()
        {
            String scenarioName = this.ScenarioContext.ScenarioInfo.Title.Replace(" ", "");
            
            try
            {
                IPage page = this.ScenarioContext.ScenarioContainer.Resolve<IPage>(scenarioName);
                if (page != null)
                {
                    await page.CloseAsync();
                }
            }
            catch { }

            if (Browser != null)
            {
                await Browser.CloseAsync();
                await Browser.DisposeAsync();
            }

            if (Playwright != null)
            {
                Playwright.Dispose();
            }
        }

        private async Task<(IBrowser, IPage)> CreatePlaywrightBrowser()
        {
            String? browser = Environment.GetEnvironmentVariable("Browser");
            String? isCi = Environment.GetEnvironmentVariable("IsCI");
            bool isHeadless = String.Compare(isCi, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase) == 0;

            IBrowser playwrightBrowser;
            
            switch (browser)
            {
                case "Firefox":
                    playwrightBrowser = await Playwright!.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                    {
                        Headless = isHeadless,
                        Args = new[] { "--ignore-certificate-errors" }
                    });
                    break;
                    
                case "Edge":
                    // Playwright uses Chromium-based Edge
                    playwrightBrowser = await Playwright!.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                    {
                        Channel = "msedge",
                        Headless = isHeadless,
                        Args = new[] { "--ignore-certificate-errors" }
                    });
                    break;
                    
                case null:
                case "Chrome":
                default:
                    playwrightBrowser = await Playwright!.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                    {
                        Headless = isHeadless,
                        Args = new[] { 
                            "--ignore-certificate-errors",
                            "--no-sandbox",
                            "--disable-dev-shm-usage",
                            "--disable-gpu"
                        }
                    });
                    break;
            }

            // Create context with settings
            var context = await playwrightBrowser.NewContextAsync(new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true,
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
            });

            // Create page
            var page = await context.NewPageAsync();

            return (playwrightBrowser, page);
        }
    }
}
