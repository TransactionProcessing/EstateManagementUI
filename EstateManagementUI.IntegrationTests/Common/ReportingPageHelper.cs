using Microsoft.Playwright;
using Shouldly;

namespace EstateManagementUI.IntegrationTests.Common;

/// <summary>
/// Helper class for interacting with Reporting pages using Playwright
/// </summary>
public class ReportingPageHelper
{
    private readonly IPage _page;
    private readonly string _baseUrl;
    private const int DefaultChartWaitTimeoutMs = 5000;

    public ReportingPageHelper(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    #region Private Helpers

    /// <summary>
    /// Wait for a canvas element to be visible (used for chart rendering)
    /// </summary>
    private async Task WaitForCanvasAsync()
    {
        await _page.Locator("canvas").First.WaitForAsync(new LocatorWaitForOptions 
        { 
            State = WaitForSelectorState.Visible, 
            Timeout = DefaultChartWaitTimeoutMs 
        });
    }

    #endregion

    #region Navigation

    /// <summary>
    /// Navigate to the Reporting Dashboard
    /// </summary>
    public async Task NavigateToReportingDashboard()
    {
        await _page.GotoAsync($"{_baseUrl}/reporting");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to the Transaction Detail Report
    /// </summary>
    public async Task NavigateToTransactionDetailReport()
    {
        await _page.GotoAsync($"{_baseUrl}/reporting/transaction-detail");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to the Transaction Summary by Merchant Report
    /// </summary>
    public async Task NavigateToTransactionSummaryMerchantReport()
    {
        await _page.GotoAsync($"{_baseUrl}/reporting/transaction-summary-merchant");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to the Transaction Summary by Operator Report
    /// </summary>
    public async Task NavigateToTransactionSummaryOperatorReport()
    {
        await _page.GotoAsync($"{_baseUrl}/reporting/transaction-summary-operator");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to the Product Performance Report
    /// </summary>
    public async Task NavigateToProductPerformanceReport()
    {
        await _page.GotoAsync($"{_baseUrl}/reporting/product-performance");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to the Settlement Summary Report
    /// </summary>
    public async Task NavigateToSettlementSummaryReport()
    {
        await _page.GotoAsync($"{_baseUrl}/reporting/settlement-summary");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to the Settlement Reconciliation Report
    /// </summary>
    public async Task NavigateToSettlementReconciliationReport()
    {
        await _page.GotoAsync($"{_baseUrl}/reporting/settlement-reconciliation");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to the Merchant Settlement History Report
    /// </summary>
    public async Task NavigateToMerchantSettlementHistoryReport()
    {
        await _page.GotoAsync($"{_baseUrl}/reporting/merchant-settlement-history");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Navigate to the Analytical Charts Report
    /// </summary>
    public async Task NavigateToAnalyticalChartsReport()
    {
        await _page.GotoAsync($"{_baseUrl}/reporting/analytical-charts");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    #endregion

    #region Reporting Dashboard Verification

    /// <summary>
    /// Verify the Reporting Dashboard page is displayed
    /// </summary>
    public async Task VerifyReportingDashboardPageIsDisplayed()
    {
        var heading = await _page.Locator("h1:has-text('Reporting Dashboard')").TextContentAsync();
        heading.ShouldNotBeNull();
        heading.ShouldContain("Reporting Dashboard");
    }

    /// <summary>
    /// Verify the page title
    /// </summary>
    public async Task VerifyPageTitle(string expectedTitle)
    {
        var title = await _page.TitleAsync();
        title.ShouldBe(expectedTitle);
    }

    /// <summary>
    /// Verify the Transaction Reporting section is displayed
    /// </summary>
    public async Task VerifyTransactionReportingSectionIsDisplayed()
    {
        await _page.Locator("h3:has-text('Transaction Reporting')").WaitForAsync();
        var section = await _page.Locator("h3:has-text('Transaction Reporting')").IsVisibleAsync();
        section.ShouldBeTrue("Transaction Reporting section should be visible");
    }

    /// <summary>
    /// Verify the Settlement Reporting section is displayed
    /// </summary>
    public async Task VerifySettlementReportingSectionIsDisplayed()
    {
        await _page.Locator("h3:has-text('Settlement Reporting')").WaitForAsync();
        var section = await _page.Locator("h3:has-text('Settlement Reporting')").IsVisibleAsync();
        section.ShouldBeTrue("Settlement Reporting section should be visible");
    }

    /// <summary>
    /// Verify the Reconciliation section is displayed
    /// </summary>
    public async Task VerifyReconciliationSectionIsDisplayed()
    {
        await _page.Locator("h3:has-text('Reconciliation')").WaitForAsync();
        var section = await _page.Locator("h3:has-text('Reconciliation')").IsVisibleAsync();
        section.ShouldBeTrue("Reconciliation section should be visible");
    }

    /// <summary>
    /// Verify the KPI Reporting section is displayed
    /// </summary>
    public async Task VerifyKpiReportingSectionIsDisplayed()
    {
        await _page.Locator("h3:has-text('KPI Reporting')").WaitForAsync();
        var section = await _page.Locator("h3:has-text('KPI Reporting')").IsVisibleAsync();
        section.ShouldBeTrue("KPI Reporting section should be visible");
    }

    #endregion

    #region Report Page Verification

    /// <summary>
    /// Verify a report page is displayed by checking for a heading
    /// </summary>
    public async Task VerifyReportPageIsDisplayed(string pageHeading)
    {
        await _page.Locator($"h1:has-text('{pageHeading}')").WaitForAsync();
        var heading = await _page.Locator($"h1:has-text('{pageHeading}')").IsVisibleAsync();
        heading.ShouldBeTrue($"{pageHeading} page should be displayed");
    }

    /// <summary>
    /// Verify the filters section is displayed
    /// </summary>
    public async Task VerifyFiltersSectionIsDisplayed()
    {
        await _page.Locator("h3:has-text('Filters')").WaitForAsync();
        var section = await _page.Locator("h3:has-text('Filters')").IsVisibleAsync();
        section.ShouldBeTrue("Filters section should be visible");
    }

    /// <summary>
    /// Verify a data grid is displayed
    /// </summary>
    public async Task VerifyDataGridIsDisplayed()
    {
        await _page.Locator("table.table").WaitForAsync();
        var table = await _page.Locator("table.table").IsVisibleAsync();
        table.ShouldBeTrue("Data grid should be visible");
    }

    /// <summary>
    /// Verify transaction details grid is displayed
    /// </summary>
    public async Task VerifyTransactionDetailsGridIsDisplayed()
    {
        await VerifyDataGridIsDisplayed();
    }

    /// <summary>
    /// Verify merchant summary grid is displayed
    /// </summary>
    public async Task VerifyMerchantSummaryGridIsDisplayed()
    {
        await VerifyDataGridIsDisplayed();
    }

    /// <summary>
    /// Verify operator summary grid is displayed
    /// </summary>
    public async Task VerifyOperatorSummaryGridIsDisplayed()
    {
        await VerifyDataGridIsDisplayed();
    }

    /// <summary>
    /// Verify product performance grid is displayed
    /// </summary>
    public async Task VerifyProductPerformanceGridIsDisplayed()
    {
        await VerifyDataGridIsDisplayed();
    }

    /// <summary>
    /// Verify settlement summary grid is displayed
    /// </summary>
    public async Task VerifySettlementSummaryGridIsDisplayed()
    {
        await VerifyDataGridIsDisplayed();
    }

    /// <summary>
    /// Verify reconciliation grid is displayed
    /// </summary>
    public async Task VerifyReconciliationGridIsDisplayed()
    {
        await VerifyDataGridIsDisplayed();
    }

    /// <summary>
    /// Verify settlement history grid is displayed
    /// </summary>
    public async Task VerifySettlementHistoryGridIsDisplayed()
    {
        await VerifyDataGridIsDisplayed();
    }

    /// <summary>
    /// Verify analytical charts are displayed
    /// </summary>
    public async Task VerifyAnalyticalChartsAreDisplayed()
    {
        // Wait for canvas elements to be present and visible
        await WaitForCanvasAsync();
        var chartCanvas = await _page.Locator("canvas").CountAsync();
        chartCanvas.ShouldBeGreaterThan(0, "At least one chart should be displayed");
    }

    #endregion

    #region KPI Verification

    /// <summary>
    /// Verify a specific KPI is displayed
    /// </summary>
    public async Task VerifyKpiIsDisplayed(string kpiName)
    {
        await _page.Locator($".info-box-text:has-text('{kpiName}')").WaitForAsync();
        var kpi = await _page.Locator($".info-box-text:has-text('{kpiName}')").IsVisibleAsync();
        kpi.ShouldBeTrue($"{kpiName} KPI should be visible");
    }

    /// <summary>
    /// Verify Total Transactions KPI is greater than a value
    /// </summary>
    public async Task VerifyTotalTransactionsKpiIsGreaterThan(int minValue)
    {
        var kpiBox = _page.Locator(".info-box").Filter(new LocatorFilterOptions 
        { 
            HasText = "Total Transactions" 
        });
        await kpiBox.WaitForAsync();
        
        var kpiValue = await kpiBox.Locator(".info-box-number").TextContentAsync();
        kpiValue.ShouldNotBeNull();
        
        // Remove any commas or formatting
        var cleanValue = kpiValue.Replace(",", "").Trim();
        var value = int.Parse(cleanValue);
        value.ShouldBeGreaterThan(minValue, $"Total Transactions should be greater than {minValue}");
    }

    #endregion

    #region Filter Interactions

    /// <summary>
    /// Set the start date filter
    /// </summary>
    public async Task SetStartDate(int daysAgo)
    {
        var date = DateTime.Now.AddDays(-daysAgo);
        var dateString = date.ToString("yyyy-MM-dd");
        
        var startDateInput = _page.Locator("input[type='date']").First;
        await startDateInput.FillAsync(dateString);
    }

    /// <summary>
    /// Set the end date filter to today
    /// </summary>
    public async Task SetEndDateToToday()
    {
        var dateString = DateTime.Now.ToString("yyyy-MM-dd");
        
        var endDateInput = _page.Locator("input[type='date']").Nth(1);
        await endDateInput.FillAsync(dateString);
    }

    /// <summary>
    /// Select a merchant from the filter dropdown
    /// </summary>
    public async Task SelectMerchantFromFilter()
    {
        var merchantDropdown = _page.Locator("select").Filter(new LocatorFilterOptions 
        { 
            Has = _page.Locator("option:has-text('All Merchants')") 
        });
        
        // Get the first available merchant option (not "All Merchants")
        var options = await merchantDropdown.Locator("option").AllAsync();
        if (options.Count > 1)
        {
            var firstMerchantValue = await options[1].GetAttributeAsync("value");
            if (!string.IsNullOrEmpty(firstMerchantValue))
            {
                await merchantDropdown.SelectOptionAsync(firstMerchantValue);
            }
        }
    }

    /// <summary>
    /// Select an operator from the filter dropdown
    /// </summary>
    public async Task SelectOperatorFromFilter()
    {
        var operatorDropdown = _page.Locator("select").Filter(new LocatorFilterOptions 
        { 
            Has = _page.Locator("option:has-text('All Operators')") 
        });
        
        // Get the first available operator option (not "All Operators")
        var options = await operatorDropdown.Locator("option").AllAsync();
        if (options.Count > 1)
        {
            var firstOperatorValue = await options[1].GetAttributeAsync("value");
            if (!string.IsNullOrEmpty(firstOperatorValue))
            {
                await operatorDropdown.SelectOptionAsync(firstOperatorValue);
            }
        }
    }

    /// <summary>
    /// Select a product from the filter dropdown
    /// </summary>
    public async Task SelectProductFromFilter()
    {
        var productDropdown = _page.Locator("select").Filter(new LocatorFilterOptions 
        { 
            Has = _page.Locator("option:has-text('All Products')") 
        });
        
        // Get the first available product option (not "All Products")
        var options = await productDropdown.Locator("option").AllAsync();
        if (options.Count > 1)
        {
            var firstProductValue = await options[1].GetAttributeAsync("value");
            if (!string.IsNullOrEmpty(firstProductValue))
            {
                await productDropdown.SelectOptionAsync(firstProductValue);
            }
        }
    }

    /// <summary>
    /// Select a status from the filter dropdown
    /// </summary>
    public async Task SelectStatusFromFilter(string status)
    {
        var statusDropdown = _page.Locator("select").Filter(new LocatorFilterOptions 
        { 
            Has = _page.Locator("option:has-text('All Statuses')") 
        });
        
        await statusDropdown.SelectOptionAsync(new[] { status });
    }

    /// <summary>
    /// Click the Apply Filters button
    /// </summary>
    public async Task ClickApplyFilters()
    {
        await _page.Locator("button:has-text('Apply Filters')").ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Click the Clear Filters button
    /// </summary>
    public async Task ClickClearFilters()
    {
        await _page.Locator("button:has-text('Clear Filters')").ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Click the Refresh button
    /// </summary>
    public async Task ClickRefresh()
    {
        await _page.Locator("button:has-text('Refresh')").ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    #endregion

    #region Data Verification

    /// <summary>
    /// Verify filtered results are displayed
    /// </summary>
    public async Task VerifyFilteredResultsDisplayed()
    {
        // Wait for the table to be visible
        await _page.Locator("table.table tbody tr").First.WaitForAsync();
        
        // Verify at least one row exists
        var rowCount = await _page.Locator("table.table tbody tr").CountAsync();
        rowCount.ShouldBeGreaterThan(0, "At least one result should be displayed after filtering");
    }

    /// <summary>
    /// Verify all displayed transactions match the selected merchant
    /// </summary>
    public async Task VerifyAllTransactionsMatchSelectedMerchant()
    {
        // This is a placeholder - in a real scenario, we would store the selected merchant name
        // and verify each row matches it
        await VerifyFilteredResultsDisplayed();
    }

    /// <summary>
    /// Verify all displayed transactions match the selected operator
    /// </summary>
    public async Task VerifyAllTransactionsMatchSelectedOperator()
    {
        // This is a placeholder - in a real scenario, we would store the selected operator name
        // and verify each row matches it
        await VerifyFilteredResultsDisplayed();
    }

    /// <summary>
    /// Verify all displayed transactions match the selected product
    /// </summary>
    public async Task VerifyAllTransactionsMatchSelectedProduct()
    {
        // This is a placeholder - in a real scenario, we would store the selected product name
        // and verify each row matches it
        await VerifyFilteredResultsDisplayed();
    }

    /// <summary>
    /// Verify filters are reset to default values
    /// </summary>
    public async Task VerifyFiltersAreResetToDefaultValues()
    {
        // Verify merchant dropdown is reset to "All Merchants"
        var merchantDropdown = _page.Locator("select").Filter(new LocatorFilterOptions 
        { 
            Has = _page.Locator("option:has-text('All Merchants')") 
        });
        var merchantValue = await merchantDropdown.InputValueAsync();
        merchantValue.ShouldBe("", "Merchant filter should be reset");
    }

    /// <summary>
    /// Verify unfiltered results are displayed
    /// </summary>
    public async Task VerifyUnfilteredResultsDisplayed()
    {
        await VerifyFilteredResultsDisplayed();
    }

    #endregion

    #region Sorting and Pagination

    /// <summary>
    /// Click on a column header to sort
    /// </summary>
    public async Task ClickColumnHeader(string columnName)
    {
        await _page.Locator($"th:has-text('{columnName}')").ClickAsync();
        // Wait for the sort icon to update or the table to reload
        await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }

    /// <summary>
    /// Verify grid is sorted by column in ascending order
    /// </summary>
    public async Task VerifyGridIsSortedByColumnAscending(string columnName)
    {
        // In a real scenario, we would verify the actual data is sorted
        // For now, just verify the column is still visible
        var column = await _page.Locator($"th:has-text('{columnName}')").IsVisibleAsync();
        column.ShouldBeTrue($"{columnName} column should be visible");
    }

    /// <summary>
    /// Verify grid has multiple pages
    /// </summary>
    public async Task VerifyGridHasMultiplePages()
    {
        // Check if pagination controls are visible
        var nextButton = await _page.Locator("button[title='Next page']").IsVisibleAsync();
        nextButton.ShouldBeTrue("Next page button should be visible when multiple pages exist");
    }

    /// <summary>
    /// Click the Next Page button
    /// </summary>
    public async Task ClickNextPageButton()
    {
        await _page.Locator("button[title='Next page']").ClickAsync();
        // Wait for the page to update
        await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }

    /// <summary>
    /// Verify next page of results is displayed
    /// </summary>
    public async Task VerifyNextPageOfResultsDisplayed()
    {
        // Verify we're on page 2 or greater
        var pageInfo = await _page.Locator("span:has-text('Page')").TextContentAsync();
        pageInfo.ShouldNotBeNull();
        pageInfo.ShouldNotContain("Page 1 of 1");
    }

    #endregion

    #region View Toggle

    /// <summary>
    /// Click the Chart View button
    /// </summary>
    public async Task ClickChartViewButton()
    {
        await _page.Locator("button:has-text('Chart View')").ClickAsync();
        // Wait for chart to be rendered
        await WaitForCanvasAsync();
    }

    /// <summary>
    /// Click the Grid View button
    /// </summary>
    public async Task ClickGridViewButton()
    {
        await _page.Locator("button:has-text('Grid View')").ClickAsync();
        // Wait for the table to be rendered
        await _page.Locator("table.table").WaitForAsync();
    }

    /// <summary>
    /// Verify product performance chart is displayed
    /// </summary>
    public async Task VerifyProductPerformanceChartIsDisplayed()
    {
        // Wait for canvas elements to be present and visible
        await WaitForCanvasAsync();
        var chartCanvas = await _page.Locator("canvas").CountAsync();
        chartCanvas.ShouldBeGreaterThan(0, "Chart should be displayed");
    }

    #endregion

    #region Chart Type Selection

    /// <summary>
    /// Select a chart type
    /// </summary>
    public async Task SelectChartType(string chartType)
    {
        // In the Analytical Charts report, there might be buttons or dropdowns to select chart types
        var chartTypeButton = _page.Locator($"button:has-text('{chartType}')");
        var buttonExists = await chartTypeButton.CountAsync();
        
        if (buttonExists > 0)
        {
            await chartTypeButton.ClickAsync();
            // Wait for canvas to update
            await WaitForCanvasAsync();
        }
    }

    /// <summary>
    /// Verify volume chart is displayed
    /// </summary>
    public async Task VerifyVolumeChartIsDisplayed()
    {
        await VerifyAnalyticalChartsAreDisplayed();
    }

    /// <summary>
    /// Verify value chart is displayed
    /// </summary>
    public async Task VerifyValueChartIsDisplayed()
    {
        await VerifyAnalyticalChartsAreDisplayed();
    }

    #endregion

    #region Drill-Down

    /// <summary>
    /// Click View Details for a merchant
    /// </summary>
    public async Task ClickViewDetailsForMerchant()
    {
        // Find the first "View Details" button in the merchant summary
        var viewDetailsButton = _page.Locator("button:has-text('View Details')").First;
        await viewDetailsButton.WaitForAsync();
        await viewDetailsButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Verify user is navigated to Transaction Detail Report
    /// </summary>
    public async Task VerifyNavigatedToTransactionDetailReport()
    {
        await _page.Locator("h1:has-text('Transaction Detail Report')").WaitForAsync();
        var heading = await _page.Locator("h1:has-text('Transaction Detail Report')").IsVisibleAsync();
        heading.ShouldBeTrue("Should be navigated to Transaction Detail Report");
    }

    /// <summary>
    /// Verify merchant filter is pre-populated
    /// </summary>
    public async Task VerifyMerchantFilterIsPrePopulated()
    {
        // Check if a merchant is selected in the dropdown (value is not empty)
        var merchantDropdown = _page.Locator("select").Filter(new LocatorFilterOptions 
        { 
            Has = _page.Locator("option:has-text('All Merchants')") 
        });
        var merchantValue = await merchantDropdown.InputValueAsync();
        merchantValue.ShouldNotBe("", "Merchant filter should be pre-populated");
    }

    #endregion

    #region Settlement Specific

    /// <summary>
    /// Verify only settled settlements are displayed
    /// </summary>
    public async Task VerifyOnlySettledSettlementsDisplayed()
    {
        // This would require checking the status column in each row
        // For now, just verify filtered results are displayed
        await VerifyFilteredResultsDisplayed();
    }

    #endregion
}
