using Microsoft.Playwright;
using Reqnroll;

namespace EstateManagementUI.IntegrationTests.Hooks;

/// <summary>
/// Hooks for managing browser lifecycle using Playwright
/// </summary>
[Binding]
public class BrowserHooks
{
    private static IPlaywright? _playwright;
    private static IBrowser? _browser;
    private readonly ScenarioContext _scenarioContext;

    public BrowserHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Install Playwright browsers and initialize Playwright before running any tests
    /// </summary>
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
        _playwright = await Playwright.CreateAsync();
    }

    /// <summary>
    /// Create a new browser page for each scenario
    /// </summary>
    [BeforeScenario(Order = 0)]
    public async Task BeforeScenario()
    {
        var page = await CreateBrowserPage();
        
        // Register the page for this scenario
        _scenarioContext.ScenarioContainer.RegisterInstanceAs(page);
    }

    /// <summary>
    /// Cleanup browser page after each scenario and take screenshot on failure
    /// </summary>
    [AfterScenario(Order = 0)]
    public async Task AfterScenario()
    {
        var page = _scenarioContext.ScenarioContainer.Resolve<IPage>();
        
        if (page != null)
        {
            // Take screenshot on failure
            if (_scenarioContext.TestError != null)
            {
                var scenarioName = _scenarioContext.ScenarioInfo.Title.Replace(" ", "_");
                var screenshotPath = $"screenshot-{scenarioName}-{DateTime.Now:yyyyMMddHHmmss}.png";
                await page.ScreenshotAsync(new PageScreenshotOptions 
                { 
                    Path = screenshotPath, 
                    FullPage = true 
                });
                Console.WriteLine($"Screenshot saved to: {screenshotPath}");
            }

            await page.CloseAsync();
        }
    }

    /// <summary>
    /// Cleanup Playwright resources after all tests complete
    /// </summary>
    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
            _browser = null;
        }

        if (_playwright != null)
        {
            _playwright.Dispose();
            _playwright = null;
        }
    }

    /// <summary>
    /// Create a new browser page with appropriate configuration
    /// </summary>
    private async Task<IPage> CreateBrowserPage()
    {
        var browserType = Environment.GetEnvironmentVariable("Browser") ?? "Chrome";
        var isCI = string.Equals(
            Environment.GetEnvironmentVariable("IsCI"), 
            "true", 
            StringComparison.InvariantCultureIgnoreCase);
        
        if (_browser == null)
        {
            _browser = browserType switch
            {
                "Firefox" => await _playwright!.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = isCI,
                    Args = new[] { "--ignore-certificate-errors" }
                }),
                "WebKit" => await _playwright!.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = isCI,
                    Args = new[] { "--ignore-certificate-errors" }
                }),
                _ => await _playwright!.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = isCI,
                    Args = new[] 
                    { 
                        "--ignore-certificate-errors",
                        "--no-sandbox",
                        "--disable-dev-shm-usage",
                        "--disable-gpu",
                        "--disable-extensions"
                    }
                })
            };
        }

        var context = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
        });

        return await context.NewPageAsync();
    }
}
