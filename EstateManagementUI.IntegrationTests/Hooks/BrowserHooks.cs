using Microsoft.Playwright;
using Reqnroll;
using System.Net;
using System.Text;

namespace EstateManagementUI.IntegrationTests.Hooks;

/// <summary>
/// Hooks for managing browser lifecycle using Playwright
/// </summary>
[Binding]
public class BrowserHooks
{
    private static IPlaywright? _playwright;
    private readonly ScenarioContext _scenarioContext;
    private IBrowser? _browser;
    private IBrowserContext? _browserContext;

    public BrowserHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    /// <summary>
    /// Initialize Playwright before running any tests
    /// </summary>
    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        // Initialize Playwright
        _playwright = await Playwright.CreateAsync();
    }

    /// <summary>
    /// Create a new browser page for each scenario
    /// </summary>
    [BeforeScenario(Order = 1)]
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
        IPage? page = null;
        try
        {
            page = _scenarioContext.ScenarioContainer.Resolve<IPage>();
        }
        catch
        {
            return;
        }

        var scenarioName = _scenarioContext.ScenarioInfo.Title.Replace(" ", "_");
        var artifactDirectory = Path.Combine(Environment.CurrentDirectory, "TestResults");
        var screenshotDirectory = Path.Combine(artifactDirectory, "Screenshots");
        var diagnosticsDirectory = Path.Combine(artifactDirectory, "Diagnostics");
        var traceDirectory = Path.Combine(artifactDirectory, "Traces");
        Directory.CreateDirectory(screenshotDirectory);
        Directory.CreateDirectory(diagnosticsDirectory);
        Directory.CreateDirectory(traceDirectory);

        if (_browserContext != null)
        {
            try
            {
                var tracePath = Path.Combine(traceDirectory, $"trace-{scenarioName}-{DateTime.Now:yyyyMMddHHmmss}.zip");
                await _browserContext.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
                Console.WriteLine($"Trace saved to: {tracePath}");
            }
            catch (Exception traceException)
            {
                Console.WriteLine($"Failed to save trace: {traceException.Message}");
            }
        }
        
        if (page != null)
        {
            // Take screenshot on failure
            if (_scenarioContext.TestError != null)
            {
                try
                {
                    await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                    await page.WaitForTimeoutAsync(500);
                }
                catch
                {
                    // Best effort only. If the page is already gone, we still want the failure artifacts.
                }

                var screenshotPath = Path.Combine(screenshotDirectory, $"screenshot-{scenarioName}-{DateTime.Now:yyyyMMddHHmmss}.png");
                await page.ScreenshotAsync(new PageScreenshotOptions 
                { 
                    Path = screenshotPath, 
                    FullPage = true 
                });
                Console.WriteLine($"Screenshot saved to: {screenshotPath}");

                var diagnosticsPath = Path.Combine(diagnosticsDirectory, $"diagnostics-{scenarioName}-{DateTime.Now:yyyyMMddHHmmss}.txt");
                var diagnostics = new StringBuilder();
                diagnostics.AppendLine($"Url: {page.Url}");
                diagnostics.AppendLine($"Title: {await page.TitleAsync()}");
                diagnostics.AppendLine("Body text:");
                diagnostics.AppendLine(await page.Locator("body").InnerTextAsync());
                diagnostics.AppendLine();
                diagnostics.AppendLine("HTML:");
                diagnostics.AppendLine(await page.ContentAsync());
                await File.WriteAllTextAsync(diagnosticsPath, diagnostics.ToString());
                Console.WriteLine($"Diagnostics saved to: {diagnosticsPath}");
            }

            await page.CloseAsync();
        }

        if (_browserContext != null)
        {
            await _browserContext.CloseAsync();
            _browserContext = null;
        }

        if (_browser != null)
        {
            await _browser.CloseAsync();
            _browser = null;
        }

    }

    /// <summary>
    /// Cleanup Playwright resources after all tests complete
    /// </summary>
    [AfterTestRun]
    public static async Task AfterTestRun()
    {
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
        var securityServiceHost = Environment.GetEnvironmentVariable("SecurityServiceContainerName");
        var securityServicePortText = Environment.GetEnvironmentVariable("SecurityServicePort");
        var hostResolverRules = string.IsNullOrWhiteSpace(securityServiceHost)
            ? null
            : $"MAP {securityServiceHost} 127.0.0.1";

        Console.WriteLine($"[browser setup] Browser={browserType}, IsCI={isCI}");
        Console.WriteLine($"[browser setup] SecurityServiceContainerName={securityServiceHost ?? "<null>"}");
        Console.WriteLine($"[browser setup] SecurityServicePort={securityServicePortText ?? "<null>"}");
        
        var chromiumExecutablePath = ResolveChromiumExecutablePath();
        if (!string.IsNullOrWhiteSpace(chromiumExecutablePath))
        {
            Console.WriteLine($"[browser setup] Chromium executable: {chromiumExecutablePath}");
        }
        else
        {
            Console.WriteLine("[browser setup] No system Chromium/Edge executable found; using bundled Playwright Chromium.");
        }

        _browser = browserType switch
        {
            "Firefox" => await _playwright!.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = isCI,
                Args = hostResolverRules is null
                    ? new[] { "--ignore-certificate-errors" }
                    : new[] { "--ignore-certificate-errors", $"--host-resolver-rules={hostResolverRules}" }
            }),
            "WebKit" => await _playwright!.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = isCI,
                Args = hostResolverRules is null
                    ? new[] { "--ignore-certificate-errors" }
                    : new[] { "--ignore-certificate-errors", $"--host-resolver-rules={hostResolverRules}" }
            }),
            _ => await _playwright!.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = isCI,
                ExecutablePath = string.IsNullOrWhiteSpace(chromiumExecutablePath) ? null : chromiumExecutablePath,
                Args = hostResolverRules is null
                    ? new[]
                    {
                        "--ignore-certificate-errors",
                        "--no-sandbox",
                        "--disable-dev-shm-usage",
                        "--disable-gpu",
                        "--disable-extensions"
                    }
                    : new[]
                    {
                        "--ignore-certificate-errors",
                        "--no-sandbox",
                        "--disable-dev-shm-usage",
                        "--disable-gpu",
                        "--disable-extensions",
                        $"--host-resolver-rules={hostResolverRules}"
                    }
            })
        };

        _browserContext = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
        });

        await _browserContext.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        var page = await _browserContext.NewPageAsync();
        page.Console += (_, message) => Console.WriteLine($"[browser console] {message.Type}: {message.Text}");
        page.PageError += (_, error) => Console.WriteLine($"[browser page error] {error}");
        page.RequestFailed += (_, request) => Console.WriteLine($"[browser request failed] {request.Url} => {request.Failure}");

        return page;
    }

    private static string? ResolveChromiumExecutablePath()
    {
        var candidatePaths = new[]
        {
            @"/usr/bin/google-chrome",
            @"/usr/bin/google-chrome-stable",
            @"/usr/bin/chromium",
            @"/usr/bin/chromium-browser",
            @"/usr/bin/microsoft-edge",
            @"/snap/bin/google-chrome",
            @"/snap/bin/chromium",
            @"C:\Program Files\Google\Chrome\Application\chrome.exe",
            @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
            @"C:\Program Files\Microsoft\Edge\Application\msedge.exe",
            @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
        };

        foreach (var candidatePath in candidatePaths)
        {
            if (File.Exists(candidatePath))
            {
                return candidatePath;
            }
        }

        return null;
    }
}
