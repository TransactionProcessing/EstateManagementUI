using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "base")]
public sealed class EstateLoginSteps
{
    private readonly IPage _page;
    private readonly TestingContext _testingContext;

    public EstateLoginSteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
    }

    [Given("I click on the Sign In Button")]
    [When("I click on the Sign In Button")]
    [Then("I click on the Sign In Button")]
    public Task WhenIClickOnTheSignInButton() => GetHelper().ClickSignInButtonAsync();

    [Then("I am presented with a login screen")]
    public Task ThenIAmPresentedWithALoginScreen() => GetHelper().AssertLoginScreenVisibleAsync();

    [When("I login with the username {string} and password {string}")]
    public Task WhenILoginWithTheUsernameAndPassword(string username, string password) => GetHelper().LoginAsync(username, password);

    [Then("I should see the dashboard heading")]
    public Task ThenIShouldSeeTheDashboardHeading() => GetHelper().AssertDashboardShellVisibleAsync();

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);
}
