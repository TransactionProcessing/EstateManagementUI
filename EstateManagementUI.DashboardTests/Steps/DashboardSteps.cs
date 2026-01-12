using Microsoft.Playwright;
using Reqnroll;
using EstateManagementUI.DashboardTests.Common;

namespace EstateManagementUI.DashboardTests.Steps;

/// <summary>
/// Step definitions for Dashboard integration tests
/// Links feature file scenarios to browser automation code
/// </summary>
[Binding]
public class DashboardSteps
{
    private readonly IPage _page;
    private readonly DashboardPageHelper _dashboardHelper;
    private readonly ScenarioContext _scenarioContext;

    public DashboardSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        _page = scenarioContext.ScenarioContainer.Resolve<IPage>();
        
        // Get base URL from environment variable or use default
        var baseUrl = Environment.GetEnvironmentVariable("APP_URL") ?? "https://localhost:5001";
        _dashboardHelper = new DashboardPageHelper(_page, baseUrl);
    }

    #region Navigation Steps

    [Given(@"the user navigates to the Dashboard")]
    [When(@"the user navigates to the Dashboard")]
    public async Task GivenTheUserNavigatesToTheDashboard()
    {
        await _dashboardHelper.NavigateToDashboard();
    }

    #endregion

    #region Authentication/Role Steps

    [Given(@"the user is authenticated as an ""(.*)"" user")]
    public async Task GivenTheUserIsAuthenticatedAsAUser(string role)
    {
        // Store the role in scenario context for reference
        _scenarioContext["UserRole"] = role;
        
        // Note: This step assumes the application will be started in test mode
        // with the appropriate role already configured. The actual authentication
        // setup will be handled when the application startup is implemented.
        await Task.CompletedTask;
    }

    #endregion

    #region Verification Steps - Common

    [Then(@"the Dashboard page is displayed")]
    public async Task ThenTheDashboardPageIsDisplayed()
    {
        await _dashboardHelper.VerifyDashboardPageTitle();
    }

    #endregion

    #region Verification Steps - Administrator Role

    [Then(@"the Administrator welcome message is displayed")]
    public async Task ThenTheAdministratorWelcomeMessageIsDisplayed()
    {
        await _dashboardHelper.VerifyAdministratorWelcomeMessage();
    }

    [Then(@"no merchant KPI cards are displayed")]
    public async Task ThenNoMerchantKpiCardsAreDisplayed()
    {
        await _dashboardHelper.VerifyKpiCardsAreNotVisible();
    }

    [Then(@"no sales data cards are displayed")]
    public async Task ThenNoSalesDataCardsAreDisplayed()
    {
        await _dashboardHelper.VerifyComparisonDateSelectorIsNotVisible();
        await _dashboardHelper.VerifyRecentlyCreatedMerchantsIsNotVisible();
    }

    #endregion

    #region Verification Steps - Estate/Viewer Roles

    [Then(@"the merchant KPI cards are displayed")]
    public async Task ThenTheMerchantKpiCardsAreDisplayed()
    {
        await _dashboardHelper.VerifyKpiCardsAreVisible();
    }

    [Then(@"the Merchants with Sales in Last Hour shows ""(.*)""")]
    public async Task ThenTheMerchantsWithSalesInLastHourShows(int expectedValue)
    {
        // This will be verified as part of the full KPI verification
        _scenarioContext["ExpectedSalesLastHour"] = expectedValue;
    }

    [Then(@"the Merchants with No Sales Today shows ""(.*)""")]
    public async Task ThenTheMerchantsWithNoSalesTodayShows(int expectedValue)
    {
        _scenarioContext["ExpectedNoSalesToday"] = expectedValue;
    }

    [Then(@"the Merchants with No Sales in Last 7 Days shows ""(.*)""")]
    public async Task ThenTheMerchantsWithNoSalesInLast7DaysShows(int expectedValue)
    {
        _scenarioContext["ExpectedNoSales7Days"] = expectedValue;
        
        // Now verify all KPI values
        var salesLastHour = (int)_scenarioContext["ExpectedSalesLastHour"];
        var noSalesToday = (int)_scenarioContext["ExpectedNoSalesToday"];
        var noSales7Days = expectedValue;
        
        await _dashboardHelper.VerifyMerchantKpiValues(salesLastHour, noSalesToday, noSales7Days);
    }

    [Then(@"the Today's Sales card is displayed")]
    public async Task ThenTheTodaysSalesCardIsDisplayed()
    {
        await _dashboardHelper.VerifyTodaysSalesCardIsDisplayed();
    }

    [Then(@"the Today's Sales card shows ""(.*)"" transactions")]
    public async Task ThenTheTodaysSalesCardShowsTransactions(int transactionCount)
    {
        // Store for later verification
        _scenarioContext["TodaysSalesCount"] = transactionCount;
    }

    [Then(@"the Today's Sales card shows a value greater than \$(.*)")]
    public async Task ThenTheTodaysSalesCardShowsAValueGreaterThan(decimal minimumValue)
    {
        // Verify sales values
        var salesCount = (int)_scenarioContext["TodaysSalesCount"];
        await _dashboardHelper.VerifyTodaysSalesValues(salesCount, minimumValue);
    }

    [Then(@"the Failed Sales card is displayed")]
    public async Task ThenTheFailedSalesCardIsDisplayed()
    {
        await _dashboardHelper.VerifyFailedSalesCardIsDisplayed();
    }

    [Then(@"the Failed Sales card shows ""(.*)"" transactions")]
    public async Task ThenTheFailedSalesCardShowsTransactions(int transactionCount)
    {
        await _dashboardHelper.VerifyFailedSalesValues(transactionCount);
    }

    [Then(@"the comparison date selector is displayed")]
    public async Task ThenTheComparisonDateSelectorIsDisplayed()
    {
        await _dashboardHelper.VerifyComparisonDateSelectorIsVisible();
    }

    [Then(@"the Recently Created Merchants section is displayed")]
    public async Task ThenTheRecentlyCreatedMerchantsSectionIsDisplayed()
    {
        await _dashboardHelper.VerifyRecentlyCreatedMerchantsIsVisible();
    }

    [Then(@"at least ""(.*)"" merchant is shown in Recently Created Merchants")]
    public async Task ThenAtLeastMerchantIsShownInRecentlyCreatedMerchants(int minCount)
    {
        if (minCount > 0)
        {
            await _dashboardHelper.VerifyRecentlyCreatedMerchantsHasData();
        }
    }

    #endregion

    #region Interaction Steps

    [When(@"the user selects ""(.*)"" from the comparison date selector")]
    public async Task WhenTheUserSelectsFromTheComparisonDateSelector(string dateOption)
    {
        await _dashboardHelper.SelectComparisonDate(dateOption);
    }

    #endregion
}
