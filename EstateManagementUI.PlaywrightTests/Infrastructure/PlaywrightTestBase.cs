using Microsoft.Playwright;
using NUnit.Framework;

namespace EstateManagementUI.PlaywrightTests.Infrastructure;

public class PlaywrightTestBase
{
    protected IPlaywright? Playwright { get; private set; }
    protected IBrowser? Browser { get; private set; }
    protected IBrowserContext? Context { get; private set; }
    protected IPage? Page { get; private set; }
    protected DockerHelper? DockerHelper { get; private set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Start Docker containers
        DockerHelper = new DockerHelper();
        await DockerHelper.StartContainers();
    }

    [SetUp]
    public async Task SetUp()
    {
        // Initialize Playwright
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        
        // Launch browser
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = IsRunningInCI(),
            Args = new[] { "--ignore-certificate-errors" }
        });

        // Create context
        Context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
        });

        // Create page
        Page = await Context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (Page != null)
        {
            await Page.CloseAsync();
        }

        if (Context != null)
        {
            await Context.CloseAsync();
        }

        if (Browser != null)
        {
            await Browser.CloseAsync();
        }

        if (Playwright != null)
        {
            Playwright.Dispose();
        }
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (DockerHelper != null)
        {
            await DockerHelper.StopContainers();
        }
    }

    private static bool IsRunningInCI()
    {
        var isCI = Environment.GetEnvironmentVariable("IsCI");
        var ci = Environment.GetEnvironmentVariable("CI");
        return string.Equals(isCI, "true", StringComparison.InvariantCultureIgnoreCase) ||
               string.Equals(ci, "true", StringComparison.InvariantCultureIgnoreCase);
    }
}
