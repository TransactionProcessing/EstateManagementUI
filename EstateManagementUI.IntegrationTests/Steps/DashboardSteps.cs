using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "dashboard")]
public sealed class DashboardSteps
{
    private readonly IPage _page;
    private readonly TestingContext TestingContext;

    public DashboardSteps(IPage page, TestingContext testingContext) {
        _page = page;
        this.TestingContext = testingContext;
    }

    [Given("the user navigates to the app address")]
    [When("the user navigates to the app address")]
    public async Task GivenTheUserNavigatesToTheAppAddress()
    {
        await GetHelper().NavigateToAppAddressAsync();
    }

    [Given("I click on the Sign In Button")]
    public async Task WhenIClickOnTheSignInButton()
    {
        await GetHelper().ClickSignInButtonAsync();
    }

    [Then("I am presented with a login screen")]
    public async Task ThenIAmPresentedWithALoginScreen()
    {
        await GetHelper().AssertLoginScreenVisibleAsync();
    }

    [When("I login with the username {string} and password {string}")]
    public async Task WhenILoginWithTheUsernameAndPassword(string username, string password)
    {
        //var loginPassword = password;

        //if (this.TestingContext.Users.TryGetValue(username, out var seededPassword) && !string.IsNullOrWhiteSpace(seededPassword))
        //{
        //    loginPassword = seededPassword;
        //}

        await GetHelper().LoginAsync(username, password);
    }

    [Then("I should see the dashboard heading")]
    public async Task ThenIShouldSeeTheDashboardHeading()
    {
        await GetHelper().AssertDashboardShellVisibleAsync();
    }

    [Then("the home page is displayed")]
    public async Task ThenTheHomePageIsDisplayed()
    {
        await GetHelper().AssertHomePageVisibleAsync();
    }

    [Then("I should see the dashboard welcome message")]
    public async Task ThenIShouldSeeTheDashboardWelcomeMessage()
    {
        await GetHelper().AssertDashboardWelcomeMessageVisibleAsync();
    }

    [Then("I should see the comparison date selector")]
    public async Task ThenIShouldSeeTheComparisonDateSelector()
    {
        await GetHelper().AssertComparisonDateSelectorVisibleAsync();
    }

    [Then("I should see the merchant KPI summary cards")]
    public async Task ThenIShouldSeeTheMerchantKpiSummaryCards()
    {
        await GetHelper().AssertMerchantKpiSummaryCardsVisibleAsync();
    }

    [Then("I should see the sales comparison cards")]
    public async Task ThenIShouldSeeTheSalesComparisonCards()
    {
        await GetHelper().AssertSalesComparisonCardsVisibleAsync();
    }

    [Then("I should see the recent merchants section")]
    public async Task ThenIShouldSeeTheRecentMerchantsSection()
    {
        await GetHelper().AssertRecentMerchantsSectionVisibleAsync();
    }

    [Then("I should see the administrator welcome panel")]
    public async Task ThenIShouldSeeTheAdministratorWelcomePanel()
    {
        await GetHelper().AssertAdministratorDashboardVisibleAsync();
    }

    [Then("I should not see the merchant KPI summary cards")]
    public async Task ThenIShouldNotSeeTheMerchantKpiSummaryCards()
    {
        await GetHelper().AssertMerchantKpiSummaryCardsNotVisibleAsync();
    }

    [Then("I should not see the sales comparison cards")]
    public async Task ThenIShouldNotSeeTheSalesComparisonCards()
    {
        await GetHelper().AssertSalesComparisonCardsNotVisibleAsync();
    }

    [Then("I should not see the recent merchants section")]
    public async Task ThenIShouldNotSeeTheRecentMerchantsSection()
    {
        await GetHelper().AssertRecentMerchantsSectionNotVisibleAsync();
    }

    [Then("I should see the dashboard navigation link")]
    public async Task ThenIShouldSeeTheDashboardNavigationLink()
    {
        await GetHelper().AssertDashboardNavigationLinkVisibleAsync();
    }

    private DashboardPageHelper GetHelper()
    {
        return new DashboardPageHelper(_page, this.TestingContext);
    }
}
