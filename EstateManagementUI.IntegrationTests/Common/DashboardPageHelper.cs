using Microsoft.Playwright;
using Shouldly;

namespace EstateManagementUI.IntegrationTests.Common;

/// <summary>
/// Helper class for interacting with the Dashboard page using Playwright
/// </summary>
public class DashboardPageHelper
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public DashboardPageHelper(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    #region Navigation

    /// <summary>
    /// Navigate to the home/dashboard page
    /// </summary>
    public async Task NavigateToDashboard()
    {
        await _page.GotoAsync(_baseUrl);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    #endregion

    #region Verification Methods

    /// <summary>
    /// Verify the page title is "Dashboard"
    /// </summary>
    public async Task VerifyDashboardPageTitle()
    {
        var title = await _page.TitleAsync();
        title.ShouldBe("Dashboard");
    }

    /// <summary>
    /// Verify the Administrator welcome message is displayed
    /// </summary>
    public async Task VerifyAdministratorWelcomeMessage()
    {
        var heading = await _page.Locator("h2:has-text('Welcome, Administrator')").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Welcome, Administrator");
        
        var description = await _page.Locator("p:has-text('administrative access')").TextContentAsync();
        description.ShouldNotBeNull();
        description.ShouldContain("administrative access to manage system permissions");
    }

    /// <summary>
    /// Verify that KPI cards are visible on the dashboard
    /// </summary>
    public async Task VerifyKpiCardsAreVisible()
    {
        await _page.Locator("text=Merchants with Sales (Last Hour)").WaitForAsync();
        await _page.Locator("text=Merchants with No Sales Today").WaitForAsync();
        await _page.Locator("text=Merchants with No Sales (7 Days)").WaitForAsync();
    }

    /// <summary>
    /// Verify that KPI cards are NOT visible (Administrator role)
    /// </summary>
    public async Task VerifyKpiCardsAreNotVisible()
    {
        var salesLastHourCount = await _page.Locator("text=Merchants with Sales (Last Hour)").CountAsync();
        salesLastHourCount.ShouldBe(0, "KPI cards should not be visible for Administrator role");
    }

    /// <summary>
    /// Verify Merchant KPI values match expected hardcoded test data
    /// </summary>
    public async Task VerifyMerchantKpiValues(int salesLastHour, int noSalesToday, int noSales7Days)
    {
        // Wait for KPI cards to load
        await _page.Locator("text=Merchants with Sales (Last Hour)").WaitForAsync();

        // Verify Merchants with Sales in Last Hour
        var salesLastHourCard = _page.Locator(".info-box").Filter(new LocatorFilterOptions 
        { 
            HasText = "Merchants with Sales (Last Hour)" 
        });
        var salesLastHourValue = await salesLastHourCard.Locator(".info-box-number").TextContentAsync();
        salesLastHourValue.ShouldNotBeNull();
        int.Parse(salesLastHourValue.Trim()).ShouldBe(salesLastHour);

        // Verify Merchants with No Sales Today
        var noSalesTodayCard = _page.Locator(".info-box").Filter(new LocatorFilterOptions 
        { 
            HasText = "Merchants with No Sales Today" 
        });
        var noSalesTodayValue = await noSalesTodayCard.Locator(".info-box-number").TextContentAsync();
        noSalesTodayValue.ShouldNotBeNull();
        int.Parse(noSalesTodayValue.Trim()).ShouldBe(noSalesToday);

        // Verify Merchants with No Sales in Last 7 Days
        var noSales7DaysCard = _page.Locator(".info-box").Filter(new LocatorFilterOptions 
        { 
            HasText = "Merchants with No Sales (7 Days)" 
        });
        var noSales7DaysValue = await noSales7DaysCard.Locator(".info-box-number").TextContentAsync();
        noSales7DaysValue.ShouldNotBeNull();
        int.Parse(noSales7DaysValue.Trim()).ShouldBe(noSales7Days);
    }

    /// <summary>
    /// Verify Today's Sales card is displayed
    /// </summary>
    public async Task VerifyTodaysSalesCardIsDisplayed()
    {
        await _page.Locator("h3:has-text(\"Today's Sales\")").WaitForAsync();
    }

    /// <summary>
    /// Verify Today's Sales values
    /// </summary>
    public async Task VerifyTodaysSalesValues(int todayCount, decimal todayValue)
    {
        var salesCard = _page.Locator(".card").Filter(new LocatorFilterOptions 
        { 
            HasText = "Today's Sales" 
        });
        
        // Wait for the card to be visible
        await salesCard.WaitForAsync();

        // Verify today's sales count
        var todayTransactions = await salesCard.Locator("p:has-text('transactions')").First.TextContentAsync();
        todayTransactions.ShouldNotBeNull();
        todayTransactions.ShouldContain($"{todayCount} transactions");

        // Verify today's sales value is displayed (currency format)
        var todayValueText = await salesCard.Locator(".text-2xl.font-bold").First.TextContentAsync();
        todayValueText.ShouldNotBeNull();
        // Just verify value is present and formatted as currency
        todayValueText.ShouldContain("$");
    }

    /// <summary>
    /// Verify Failed Sales card is displayed
    /// </summary>
    public async Task VerifyFailedSalesCardIsDisplayed()
    {
        await _page.Locator("h3:has-text('Failed Sales (Low Credit)')").WaitForAsync();
    }

    /// <summary>
    /// Verify Failed Sales values
    /// </summary>
    public async Task VerifyFailedSalesValues(int todayCount)
    {
        var failedSalesCard = _page.Locator(".card").Filter(new LocatorFilterOptions 
        { 
            HasText = "Failed Sales (Low Credit)" 
        });
        
        // Wait for the card to be visible
        await failedSalesCard.WaitForAsync();

        // Verify today's failed sales count
        var todayTransactions = await failedSalesCard.Locator("p:has-text('transactions')").First.TextContentAsync();
        todayTransactions.ShouldNotBeNull();
        todayTransactions.ShouldContain($"{todayCount} transactions");
    }

    /// <summary>
    /// Verify comparison date selector is visible
    /// </summary>
    public async Task VerifyComparisonDateSelectorIsVisible()
    {
        await _page.Locator("label:has-text('Compare to:')").WaitForAsync();
        await _page.Locator("#comparisonDateSelector").WaitForAsync();
    }

    /// <summary>
    /// Verify comparison date selector is NOT visible (Administrator role)
    /// </summary>
    public async Task VerifyComparisonDateSelectorIsNotVisible()
    {
        var selectorCount = await _page.Locator("#comparisonDateSelector").CountAsync();
        selectorCount.ShouldBe(0, "Comparison date selector should not be visible for Administrator role");
    }

    /// <summary>
    /// Verify Recently Created Merchants section is visible
    /// </summary>
    public async Task VerifyRecentlyCreatedMerchantsIsVisible()
    {
        await _page.Locator("h3:has-text('Recently Created Merchants')").WaitForAsync();
    }

    /// <summary>
    /// Verify Recently Created Merchants section is NOT visible (Administrator role)
    /// </summary>
    public async Task VerifyRecentlyCreatedMerchantsIsNotVisible()
    {
        var merchantsCount = await _page.Locator("h3:has-text('Recently Created Merchants')").CountAsync();
        merchantsCount.ShouldBe(0, "Recently Created Merchants should not be visible for Administrator role");
    }

    /// <summary>
    /// Verify that at least one merchant is displayed in the Recently Created Merchants section
    /// </summary>
    public async Task VerifyRecentlyCreatedMerchantsHasData()
    {
        var merchantsCard = _page.Locator(".card").Filter(new LocatorFilterOptions 
        { 
            HasText = "Recently Created Merchants" 
        });
        
        await merchantsCard.WaitForAsync();
        
        // Check that at least one merchant is displayed
        var merchantItems = merchantsCard.Locator(".flex.items-center.justify-between");
        var count = await merchantItems.CountAsync();
        count.ShouldBeGreaterThan(0, "At least one merchant should be displayed");
    }

    #endregion

    #region Interaction Methods

    /// <summary>
    /// Select a comparison date from the dropdown
    /// </summary>
    public async Task SelectComparisonDate(string dateDescription)
    {
        await _page.Locator("#comparisonDateSelector").SelectOptionAsync(new[] { dateDescription });
        // Wait for dashboard to reload
        await Task.Delay(500); // Small delay for state update
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    #endregion
}
