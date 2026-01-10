using Microsoft.Playwright;
using Reqnroll;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Integration.Features.Framework;

[Binding]
[Scope(Tag = "framework")]
public class FrameworkCheckSteps
{
    private readonly Support.DockerHelper DockerHelper;
    private readonly IPage Page;

    public FrameworkCheckSteps(ScenarioContext scenarioContext, Support.DockerHelper dockerHelper)
    {
        this.Page = scenarioContext.ScenarioContainer.Resolve<IPage>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));
        this.DockerHelper = dockerHelper;
    }

    [Given(@"the application is running in a container")]
    public void GivenTheApplicationIsRunningInAContainer()
    {
        // This step is declarative - the container should already be running
        // via the Hooks setup. We just verify that the container is running.
        this.DockerHelper.IsRunning.ShouldBeTrue("Container should be running");
        this.DockerHelper.EstateManagementUiPort.ShouldBeGreaterThan(0, 
            "Container port should be set, indicating the container is running");
        
        Console.WriteLine($"✓ Container verified - Running on port {this.DockerHelper.EstateManagementUiPort}");
    }

    [When(@"I navigate to the home page")]
    public async Task WhenINavigateToTheHomePage()
    {
        var url = $"https://localhost:{this.DockerHelper.EstateManagementUiPort}";
        Console.WriteLine($"Navigating to: {url}");
        await this.Page.GotoAsync(url);
    }

    [Then(@"the page title should be visible")]
    public async Task ThenThePageTitleShouldBeVisible()
    {
        // Wait for the page to load and check if the title or main content is visible
        // This is a basic smoke test to ensure the app is running and responding
        var titleLocator = this.Page.Locator("title");
        await titleLocator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Attached, Timeout = 30000 });
        
        var title = await this.Page.TitleAsync();
        title.ShouldNotBeNullOrEmpty("Page title should be present");
        
        Console.WriteLine($"✓ Framework check passed - Page title: '{title}'");
    }
}
