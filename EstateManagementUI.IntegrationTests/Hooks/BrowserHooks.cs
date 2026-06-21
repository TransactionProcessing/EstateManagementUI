using Microsoft.Playwright;
using Reqnroll;
using System.Net;
using System.Net.Sockets;

namespace EstateManagementUI.IntegrationTests.Hooks;

/// <summary>
/// Hooks for managing browser lifecycle using Playwright
/// </summary>
[Binding]
public class BrowserHooks
{
    private static IPlaywright? _playwright;
    private static IBrowser? _browser;
    private static TcpListener? _securityServiceForwarder;
    private static CancellationTokenSource? _forwarderCancellation;
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
        var securityServiceHost = Environment.GetEnvironmentVariable("SecurityServiceContainerName");
        var securityServiceLocalPortText = Environment.GetEnvironmentVariable("SecurityServiceLocalPort");
        var securityServicePortText = Environment.GetEnvironmentVariable("SecurityServicePort");
        var hostResolverRules = string.IsNullOrWhiteSpace(securityServiceHost)
            ? null
            : $"MAP {securityServiceHost} 127.0.0.1";

        if (int.TryParse(securityServiceLocalPortText, out var localPort) &&
            int.TryParse(securityServicePortText, out var targetPort) &&
            localPort != targetPort)
        {
            await EnsureSecurityServiceForwarderAsync(localPort, targetPort);
        }
        
        if (_browser == null)
        {
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
        }

        var context = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
        });

        return await context.NewPageAsync();
    }

    private static async Task EnsureSecurityServiceForwarderAsync(int localPort, int targetPort)
    {
        if (_securityServiceForwarder != null)
        {
            return;
        }

        _forwarderCancellation ??= new CancellationTokenSource();
        _securityServiceForwarder = new TcpListener(IPAddress.Loopback, localPort);
        _securityServiceForwarder.Start();

        _ = Task.Run(async () =>
        {
            while (!_forwarderCancellation.IsCancellationRequested)
            {
                TcpClient? inboundClient = null;
                TcpClient? outboundClient = null;

                try
                {
                    inboundClient = await _securityServiceForwarder.AcceptTcpClientAsync(_forwarderCancellation.Token);
                    outboundClient = new TcpClient();
                    await outboundClient.ConnectAsync(IPAddress.Loopback, targetPort, _forwarderCancellation.Token);

                    var inboundStream = inboundClient.GetStream();
                    var outboundStream = outboundClient.GetStream();

                    var inboundToOutbound = inboundStream.CopyToAsync(outboundStream, _forwarderCancellation.Token);
                    var outboundToInbound = outboundStream.CopyToAsync(inboundStream, _forwarderCancellation.Token);

                    await Task.WhenAny(Task.WhenAll(inboundToOutbound, outboundToInbound), Task.Delay(Timeout.Infinite, _forwarderCancellation.Token));
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch
                {
                    // Ignore connection churn; the browser may open and close several sockets during navigation.
                }
                finally
                {
                    outboundClient?.Close();
                    inboundClient?.Close();
                }
            }
        });
    }
}
