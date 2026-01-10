using Microsoft.Playwright;
using Reqnroll;
using Reqnroll.BoDi;

namespace EstateManagementUI.BlazorServer.Tests.Integration.Support;

[Binding]
public class Hooks
{
    private readonly IObjectContainer ObjectContainer;
    private ScenarioContext ScenarioContext;
    private static IPlaywright? playwright;
    private static IBrowser? browser;
    private static DockerHelper? dockerHelper;

    public Hooks(IObjectContainer objectContainer)
    {
        this.ObjectContainer = objectContainer;
    }

    [BeforeTestRun(Order = 0)]
    public static async Task BeforeTestRun_StartContainers()
    {
        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("  TESTCONTAINERS INTEGRATION TEST FRAMEWORK");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine();
        
        // Start Docker containers first
        dockerHelper = new DockerHelper();
        await dockerHelper.StartContainerAsync();
    }

    [BeforeTestRun(Order = 100)]
    public static async Task BeforeTestRun_SetupPlaywright()
    {
        // Install Playwright browsers if needed
        Console.WriteLine();
        Console.WriteLine("Setting up Playwright browsers...");
        var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
        if (exitCode != 0)
        {
            throw new Exception($"Playwright installation failed with exit code {exitCode}");
        }

        // Initialize Playwright
        playwright = await Playwright.CreateAsync();
        Console.WriteLine("✓ Playwright initialized");
        Console.WriteLine();
    }

    [BeforeScenario(Order = 0)]
    public async Task BeforeScenario(ScenarioContext scenarioContext)
    {
        this.ScenarioContext = scenarioContext;
        String scenarioName = scenarioContext.ScenarioInfo.Title.Replace(" ", "");
        
        // Create browser page for this scenario
        var page = await this.CreateBrowserPage();
        
        // Register the page and DockerHelper for this scenario
        scenarioContext.ScenarioContainer.RegisterInstanceAs(page, scenarioName);
        
        // Register DockerHelper so steps can access container information
        if (dockerHelper != null)
        {
            scenarioContext.ScenarioContainer.RegisterInstanceAs(dockerHelper);
        }
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
                Console.WriteLine($"📸 Screenshot saved to: {screenshotPath}");
            }

            await page.CloseAsync();
        }
    }

    [AfterTestRun(Order = 0)]
    public static async Task AfterTestRun_ClosePlaywright()
    {
        Console.WriteLine();
        Console.WriteLine("Closing Playwright...");
        
        if (browser != null)
        {
            await browser.CloseAsync();
            browser = null;
            Console.WriteLine("✓ Browser closed");
        }

        if (playwright != null)
        {
            playwright.Dispose();
            playwright = null;
            Console.WriteLine("✓ Playwright disposed");
        }
    }

    [AfterTestRun(Order = 100)]
    public static async Task AfterTestRun_StopContainers()
    {
        // Stop Docker containers last
        if (dockerHelper != null)
        {
            await dockerHelper.StopContainerAsync();
            await dockerHelper.DisposeAsync();
            dockerHelper = null;
        }
        
        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("  Test run completed - All resources cleaned up");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine();
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
