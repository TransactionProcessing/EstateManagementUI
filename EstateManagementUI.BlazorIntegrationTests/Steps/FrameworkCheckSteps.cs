using Reqnroll;
using System;
using System.Threading.Tasks;
using Shared.IntegrationTesting;
using EstateManagementUI.BlazorIntegrationTests.Common;
using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll.BoDi;
using Shouldly;

namespace EstateManagementUI.BlazorIntegrationTests.Steps
{
    [Binding]
    [Scope(Tag = "framework")]
    public class FrameworkCheckSteps
    {
        private readonly TestingContext TestingContext;
        private readonly BlazorUiHelpers UiHelpers;
        private readonly IPage Page;

        public FrameworkCheckSteps(ScenarioContext scenarioContext, TestingContext testingContext, IObjectContainer container)
        {
            this.Page = scenarioContext.ScenarioContainer.Resolve<IPage>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));
            this.TestingContext = testingContext;
            this.UiHelpers = new BlazorUiHelpers(this.Page, this.TestingContext.DockerHelper.EstateManagementUiPort);
        }

        [Given(@"the application is running in a container")]
        public void GivenTheApplicationIsRunningInAContainer()
        {
            // This step is declarative - the container should already be running
            // via the Hooks setup. We just verify that the port is set.
            this.TestingContext.DockerHelper.EstateManagementUiPort.ShouldBeGreaterThan(0, 
                "Container port should be set, indicating the container is running");
        }

        [When(@"I navigate to the home page")]
        public async Task WhenINavigateToTheHomePage()
        {
            await this.UiHelpers.NavigateToHomePage();
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
            
            Console.WriteLine($"✓ Framework check passed - Page title: {title}");
        }
    }
}
