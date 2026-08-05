using EstateManagementUI.IntegrationTests.Common;
using Reqnroll;

namespace EstateManagementUI.BlazorFastIntegrationTests.Hooks;

[Binding]
public sealed class AppHostHooks
{
    private static LocalAppHost? _appHost;
    private readonly ScenarioContext _scenarioContext;

    public AppHostHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        var projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\EstateManagementUI.BlazorServer\EstateManagementUI.BlazorServer.csproj"));
        _appHost = new LocalAppHost(projectPath);
        await _appHost.StartAsync();
    }

    [BeforeScenario(Order = 0)]
    public async Task BeforeScenario()
    {
        if (_appHost is null)
        {
            throw new InvalidOperationException("The local app host was not started.");
        }

        await _appHost.ResetAsync();
        var testingContext = new TestingContext(_appHost);
        _scenarioContext.ScenarioContainer.RegisterInstanceAs(testingContext);
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        if (_appHost is not null)
        {
            await _appHost.DisposeAsync();
            _appHost = null;
        }
    }
}
