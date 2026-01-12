using Microsoft.Playwright;
using Reqnroll;
using EstateManagementUI.IntegrationTests.Common;

namespace EstateManagementUI.IntegrationTests.Steps;

/// <summary>
/// Step definitions for Reporting integration tests
/// Links feature file scenarios to browser automation code
/// </summary>
[Binding]
public class ReportingSteps
{
    private readonly IPage _page;
    private readonly ReportingPageHelper _reportingHelper;
    private readonly ScenarioContext _scenarioContext;

    public ReportingSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        _page = scenarioContext.ScenarioContainer.Resolve<IPage>();
        
        // Get base URL from environment variable or use default
        var baseUrl = Environment.GetEnvironmentVariable("APP_URL") ?? "https://localhost:5001";
        _reportingHelper = new ReportingPageHelper(_page, baseUrl);
    }

    #region Navigation Steps

    [Given(@"the user navigates to the Reporting Dashboard")]
    [When(@"the user navigates to the Reporting Dashboard")]
    public async Task GivenTheUserNavigatesToTheReportingDashboard()
    {
        await _reportingHelper.NavigateToReportingDashboard();
    }

    [When(@"the user navigates to the Transaction Detail Report")]
    public async Task WhenTheUserNavigatesToTheTransactionDetailReport()
    {
        await _reportingHelper.NavigateToTransactionDetailReport();
    }

    [When(@"the user navigates to the Transaction Summary by Merchant Report")]
    public async Task WhenTheUserNavigatesToTheTransactionSummaryByMerchantReport()
    {
        await _reportingHelper.NavigateToTransactionSummaryMerchantReport();
    }

    [When(@"the user navigates to the Transaction Summary by Operator Report")]
    public async Task WhenTheUserNavigatesToTheTransactionSummaryByOperatorReport()
    {
        await _reportingHelper.NavigateToTransactionSummaryOperatorReport();
    }

    [When(@"the user navigates to the Product Performance Report")]
    public async Task WhenTheUserNavigatesToTheProductPerformanceReport()
    {
        await _reportingHelper.NavigateToProductPerformanceReport();
    }

    [When(@"the user navigates to the Settlement Summary Report")]
    public async Task WhenTheUserNavigatesToTheSettlementSummaryReport()
    {
        await _reportingHelper.NavigateToSettlementSummaryReport();
    }

    [When(@"the user navigates to the Settlement Reconciliation Report")]
    public async Task WhenTheUserNavigatesToTheSettlementReconciliationReport()
    {
        await _reportingHelper.NavigateToSettlementReconciliationReport();
    }

    [When(@"the user navigates to the Merchant Settlement History Report")]
    public async Task WhenTheUserNavigatesToTheMerchantSettlementHistoryReport()
    {
        await _reportingHelper.NavigateToMerchantSettlementHistoryReport();
    }

    [When(@"the user navigates to the Analytical Charts Report")]
    public async Task WhenTheUserNavigatesToTheAnalyticalChartsReport()
    {
        await _reportingHelper.NavigateToAnalyticalChartsReport();
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

    [Given(@"the user is authenticated as a ""(.*)"" user")]
    public async Task GivenTheUserIsAuthenticatedAsAViewerUser(string role)
    {
        // Store the role in scenario context for reference
        _scenarioContext["UserRole"] = role;
        
        // Note: This step assumes the application will be started in test mode
        // with the appropriate role already configured. The actual authentication
        // setup will be handled when the application startup is implemented.
        await Task.CompletedTask;
    }

    #endregion

    #region Reporting Dashboard Verification Steps

    [Then(@"the Reporting Dashboard page is displayed")]
    public async Task ThenTheReportingDashboardPageIsDisplayed()
    {
        await _reportingHelper.VerifyReportingDashboardPageIsDisplayed();
    }

    [Then(@"the page title is ""(.*)""")]
    public async Task ThenThePageTitleIs(string expectedTitle)
    {
        await _reportingHelper.VerifyPageTitle(expectedTitle);
    }

    [Then(@"the Transaction Reporting section is displayed")]
    public async Task ThenTheTransactionReportingSectionIsDisplayed()
    {
        await _reportingHelper.VerifyTransactionReportingSectionIsDisplayed();
    }

    [Then(@"the Settlement Reporting section is displayed")]
    public async Task ThenTheSettlementReportingSectionIsDisplayed()
    {
        await _reportingHelper.VerifySettlementReportingSectionIsDisplayed();
    }

    [Then(@"the Reconciliation section is displayed")]
    public async Task ThenTheReconciliationSectionIsDisplayed()
    {
        await _reportingHelper.VerifyReconciliationSectionIsDisplayed();
    }

    [Then(@"the KPI Reporting section is displayed")]
    public async Task ThenTheKpiReportingSectionIsDisplayed()
    {
        await _reportingHelper.VerifyKpiReportingSectionIsDisplayed();
    }

    #endregion

    #region Report Page Verification Steps

    [Then(@"the Transaction Detail Report page is displayed")]
    public async Task ThenTheTransactionDetailReportPageIsDisplayed()
    {
        await _reportingHelper.VerifyReportPageIsDisplayed("Transaction Detail Report");
    }

    [Then(@"the Transaction Summary by Merchant page is displayed")]
    public async Task ThenTheTransactionSummaryByMerchantPageIsDisplayed()
    {
        await _reportingHelper.VerifyReportPageIsDisplayed("Transaction Summary by Merchant");
    }

    [Then(@"the Transaction Summary by Operator page is displayed")]
    public async Task ThenTheTransactionSummaryByOperatorPageIsDisplayed()
    {
        await _reportingHelper.VerifyReportPageIsDisplayed("Transaction Summary by Operator");
    }

    [Then(@"the Product Performance Report page is displayed")]
    public async Task ThenTheProductPerformanceReportPageIsDisplayed()
    {
        await _reportingHelper.VerifyReportPageIsDisplayed("Product Performance Report");
    }

    [Then(@"the Settlement Summary Report page is displayed")]
    public async Task ThenTheSettlementSummaryReportPageIsDisplayed()
    {
        await _reportingHelper.VerifyReportPageIsDisplayed("Settlement Summary Report");
    }

    [Then(@"the Settlement Reconciliation Report page is displayed")]
    public async Task ThenTheSettlementReconciliationReportPageIsDisplayed()
    {
        await _reportingHelper.VerifyReportPageIsDisplayed("Settlement vs Transaction Reconciliation Report");
    }

    [Then(@"the Merchant Settlement History page is displayed")]
    public async Task ThenTheMerchantSettlementHistoryPageIsDisplayed()
    {
        await _reportingHelper.VerifyReportPageIsDisplayed("Merchant Settlement History");
    }

    [Then(@"the Analytical Charts page is displayed")]
    public async Task ThenTheAnalyticalChartsPageIsDisplayed()
    {
        await _reportingHelper.VerifyReportPageIsDisplayed("Analytical Charts");
    }

    [Then(@"the filters section is displayed")]
    public async Task ThenTheFiltersSectionIsDisplayed()
    {
        await _reportingHelper.VerifyFiltersSectionIsDisplayed();
    }

    [Then(@"the transaction details grid is displayed")]
    public async Task ThenTheTransactionDetailsGridIsDisplayed()
    {
        await _reportingHelper.VerifyTransactionDetailsGridIsDisplayed();
    }

    [Then(@"the merchant summary grid is displayed")]
    public async Task ThenTheMerchantSummaryGridIsDisplayed()
    {
        await _reportingHelper.VerifyMerchantSummaryGridIsDisplayed();
    }

    [Then(@"the operator summary grid is displayed")]
    public async Task ThenTheOperatorSummaryGridIsDisplayed()
    {
        await _reportingHelper.VerifyOperatorSummaryGridIsDisplayed();
    }

    [Then(@"the product performance grid is displayed")]
    public async Task ThenTheProductPerformanceGridIsDisplayed()
    {
        await _reportingHelper.VerifyProductPerformanceGridIsDisplayed();
    }

    [Then(@"the settlement summary grid is displayed")]
    public async Task ThenTheSettlementSummaryGridIsDisplayed()
    {
        await _reportingHelper.VerifySettlementSummaryGridIsDisplayed();
    }

    [Then(@"the reconciliation grid is displayed")]
    public async Task ThenTheReconciliationGridIsDisplayed()
    {
        await _reportingHelper.VerifyReconciliationGridIsDisplayed();
    }

    [Then(@"the settlement history grid is displayed")]
    public async Task ThenTheSettlementHistoryGridIsDisplayed()
    {
        await _reportingHelper.VerifySettlementHistoryGridIsDisplayed();
    }

    [Then(@"the analytical charts are displayed")]
    public async Task ThenTheAnalyticalChartsAreDisplayed()
    {
        await _reportingHelper.VerifyAnalyticalChartsAreDisplayed();
    }

    [Then(@"the product performance chart is displayed")]
    public async Task ThenTheProductPerformanceChartIsDisplayed()
    {
        await _reportingHelper.VerifyProductPerformanceChartIsDisplayed();
    }

    #endregion

    #region KPI Verification Steps

    [Then(@"the Total Transactions KPI is displayed")]
    public async Task ThenTheTotalTransactionsKpiIsDisplayed()
    {
        await _reportingHelper.VerifyKpiIsDisplayed("Total Transactions");
    }

    [Then(@"the Total Value KPI is displayed")]
    public async Task ThenTheTotalValueKpiIsDisplayed()
    {
        await _reportingHelper.VerifyKpiIsDisplayed("Total Value");
    }

    [Then(@"the Average Transaction KPI is displayed")]
    public async Task ThenTheAverageTransactionKpiIsDisplayed()
    {
        await _reportingHelper.VerifyKpiIsDisplayed("Average Transaction");
    }

    [Then(@"the Total Merchants KPI is displayed")]
    public async Task ThenTheTotalMerchantsKpiIsDisplayed()
    {
        await _reportingHelper.VerifyKpiIsDisplayed("Total Merchants");
    }

    [Then(@"the Total Products KPI is displayed")]
    public async Task ThenTheTotalProductsKpiIsDisplayed()
    {
        await _reportingHelper.VerifyKpiIsDisplayed("Total Products");
    }

    [Then(@"the Average per Product KPI is displayed")]
    public async Task ThenTheAveragePerProductKpiIsDisplayed()
    {
        await _reportingHelper.VerifyKpiIsDisplayed("Average per Product");
    }

    [Then(@"the Gross Value KPI is displayed")]
    public async Task ThenTheGrossValueKpiIsDisplayed()
    {
        await _reportingHelper.VerifyKpiIsDisplayed("Gross Value");
    }

    [Then(@"the Total Fees KPI is displayed")]
    public async Task ThenTheTotalFeesKpiIsDisplayed()
    {
        await _reportingHelper.VerifyKpiIsDisplayed("Total Fees");
    }

    [Then(@"the Net Settlement Value KPI is displayed")]
    public async Task ThenTheNetSettlementValueKpiIsDisplayed()
    {
        await _reportingHelper.VerifyKpiIsDisplayed("Net Settlement Value");
    }

    [Then(@"the Total Transactions KPI is greater than ""(.*)""")]
    public async Task ThenTheTotalTransactionsKpiIsGreaterThan(int minValue)
    {
        await _reportingHelper.VerifyTotalTransactionsKpiIsGreaterThan(minValue);
    }

    #endregion

    #region Filter Interaction Steps

    [When(@"the user sets the start date to ""(.*)"" days ago")]
    public async Task WhenTheUserSetsTheStartDateToDaysAgo(int daysAgo)
    {
        await _reportingHelper.SetStartDate(daysAgo);
    }

    [When(@"the user sets the end date to today")]
    public async Task WhenTheUserSetsTheEndDateToToday()
    {
        await _reportingHelper.SetEndDateToToday();
    }

    [When(@"the user selects a merchant from the filter dropdown")]
    public async Task WhenTheUserSelectsAMerchantFromTheFilterDropdown()
    {
        await _reportingHelper.SelectMerchantFromFilter();
    }

    [When(@"the user selects an operator from the filter dropdown")]
    public async Task WhenTheUserSelectsAnOperatorFromTheFilterDropdown()
    {
        await _reportingHelper.SelectOperatorFromFilter();
    }

    [When(@"the user selects a product from the filter dropdown")]
    public async Task WhenTheUserSelectsAProductFromTheFilterDropdown()
    {
        await _reportingHelper.SelectProductFromFilter();
    }

    [When(@"the user selects ""(.*)"" from the status filter dropdown")]
    public async Task WhenTheUserSelectsFromTheStatusFilterDropdown(string status)
    {
        await _reportingHelper.SelectStatusFromFilter(status);
    }

    [When(@"the user clicks Apply Filters")]
    public async Task WhenTheUserClicksApplyFilters()
    {
        await _reportingHelper.ClickApplyFilters();
    }

    [When(@"the user clicks Clear Filters")]
    public async Task WhenTheUserClicksClearFilters()
    {
        await _reportingHelper.ClickClearFilters();
    }

    [When(@"the user clicks Refresh")]
    public async Task WhenTheUserClicksRefresh()
    {
        await _reportingHelper.ClickRefresh();
    }

    #endregion

    #region Data Verification Steps

    [Then(@"the transaction details grid displays filtered results")]
    public async Task ThenTheTransactionDetailsGridDisplaysFilteredResults()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the transaction details grid displays results for the selected merchant")]
    public async Task ThenTheTransactionDetailsGridDisplaysResultsForTheSelectedMerchant()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"all displayed transactions match the selected merchant")]
    public async Task ThenAllDisplayedTransactionsMatchTheSelectedMerchant()
    {
        await _reportingHelper.VerifyAllTransactionsMatchSelectedMerchant();
    }

    [Then(@"the transaction details grid displays results for the selected operator")]
    public async Task ThenTheTransactionDetailsGridDisplaysResultsForTheSelectedOperator()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"all displayed transactions match the selected operator")]
    public async Task ThenAllDisplayedTransactionsMatchTheSelectedOperator()
    {
        await _reportingHelper.VerifyAllTransactionsMatchSelectedOperator();
    }

    [Then(@"the transaction details grid displays results for the selected product")]
    public async Task ThenTheTransactionDetailsGridDisplaysResultsForTheSelectedProduct()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"all displayed transactions match the selected product")]
    public async Task ThenAllDisplayedTransactionsMatchTheSelectedProduct()
    {
        await _reportingHelper.VerifyAllTransactionsMatchSelectedProduct();
    }

    [Then(@"all filter selections are reset to default values")]
    public async Task ThenAllFilterSelectionsAreResetToDefaultValues()
    {
        await _reportingHelper.VerifyFiltersAreResetToDefaultValues();
    }

    [Then(@"the transaction details grid displays unfiltered results")]
    public async Task ThenTheTransactionDetailsGridDisplaysUnfilteredResults()
    {
        await _reportingHelper.VerifyUnfilteredResultsDisplayed();
    }

    [Then(@"the merchant summary grid displays filtered results")]
    public async Task ThenTheMerchantSummaryGridDisplaysFilteredResults()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the merchant summary grid displays results for the selected merchant")]
    public async Task ThenTheMerchantSummaryGridDisplaysResultsForTheSelectedMerchant()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the merchant summary grid displays results for the selected operator")]
    public async Task ThenTheMerchantSummaryGridDisplaysResultsForTheSelectedOperator()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the operator summary grid displays filtered results")]
    public async Task ThenTheOperatorSummaryGridDisplaysFilteredResults()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the operator summary grid displays results for the selected operator")]
    public async Task ThenTheOperatorSummaryGridDisplaysResultsForTheSelectedOperator()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the product performance grid displays filtered results")]
    public async Task ThenTheProductPerformanceGridDisplaysFilteredResults()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the settlement summary grid displays filtered results")]
    public async Task ThenTheSettlementSummaryGridDisplaysFilteredResults()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the settlement summary grid displays results for the selected merchant")]
    public async Task ThenTheSettlementSummaryGridDisplaysResultsForTheSelectedMerchant()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the settlement summary grid displays only settled settlements")]
    public async Task ThenTheSettlementSummaryGridDisplaysOnlySettledSettlements()
    {
        await _reportingHelper.VerifyOnlySettledSettlementsDisplayed();
    }

    [Then(@"the reconciliation grid displays filtered results")]
    public async Task ThenTheReconciliationGridDisplaysFilteredResults()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the reconciliation grid displays results for the selected merchant")]
    public async Task ThenTheReconciliationGridDisplaysResultsForTheSelectedMerchant()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the settlement history grid displays filtered results")]
    public async Task ThenTheSettlementHistoryGridDisplaysFilteredResults()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the settlement history grid displays results for the selected merchant")]
    public async Task ThenTheSettlementHistoryGridDisplaysResultsForTheSelectedMerchant()
    {
        await _reportingHelper.VerifyFilteredResultsDisplayed();
    }

    [Then(@"the analytical charts display filtered results")]
    public async Task ThenTheAnalyticalChartsDisplayFilteredResults()
    {
        await _reportingHelper.VerifyAnalyticalChartsAreDisplayed();
    }

    #endregion

    #region Sorting and Pagination Steps

    [When(@"the user clicks on the ""(.*)"" column header")]
    public async Task WhenTheUserClicksOnTheColumnHeader(string columnName)
    {
        await _reportingHelper.ClickColumnHeader(columnName);
    }

    [Then(@"the transaction details grid is sorted by ""(.*)"" in ascending order")]
    public async Task ThenTheTransactionDetailsGridIsSortedByInAscendingOrder(string columnName)
    {
        await _reportingHelper.VerifyGridIsSortedByColumnAscending(columnName);
    }

    [When(@"the transaction details grid has multiple pages")]
    public async Task WhenTheTransactionDetailsGridHasMultiplePages()
    {
        await _reportingHelper.VerifyGridHasMultiplePages();
    }

    [When(@"the user clicks the Next Page button")]
    public async Task WhenTheUserClicksTheNextPageButton()
    {
        await _reportingHelper.ClickNextPageButton();
    }

    [Then(@"the transaction details grid displays the next page of results")]
    public async Task ThenTheTransactionDetailsGridDisplaysTheNextPageOfResults()
    {
        await _reportingHelper.VerifyNextPageOfResultsDisplayed();
    }

    #endregion

    #region View Toggle Steps

    [When(@"the user clicks Chart View button")]
    public async Task WhenTheUserClicksChartViewButton()
    {
        await _reportingHelper.ClickChartViewButton();
    }

    [When(@"the user clicks Grid View button")]
    public async Task WhenTheUserClicksGridViewButton()
    {
        await _reportingHelper.ClickGridViewButton();
    }

    #endregion

    #region Chart Type Steps

    [When(@"the user selects the Volume chart type")]
    public async Task WhenTheUserSelectsTheVolumeChartType()
    {
        await _reportingHelper.SelectChartType("Volume");
    }

    [When(@"the user selects the Value chart type")]
    public async Task WhenTheUserSelectsTheValueChartType()
    {
        await _reportingHelper.SelectChartType("Value");
    }

    [Then(@"the volume chart is displayed")]
    public async Task ThenTheVolumeChartIsDisplayed()
    {
        await _reportingHelper.VerifyVolumeChartIsDisplayed();
    }

    [Then(@"the value chart is displayed")]
    public async Task ThenTheValueChartIsDisplayed()
    {
        await _reportingHelper.VerifyValueChartIsDisplayed();
    }

    #endregion

    #region Drill-Down Steps

    [When(@"the user clicks View Details for a merchant")]
    public async Task WhenTheUserClicksViewDetailsForAMerchant()
    {
        await _reportingHelper.ClickViewDetailsForMerchant();
    }

    [Then(@"the user is navigated to the Transaction Detail Report")]
    public async Task ThenTheUserIsNavigatedToTheTransactionDetailReport()
    {
        await _reportingHelper.VerifyNavigatedToTransactionDetailReport();
    }

    [Then(@"the merchant filter is pre-populated")]
    public async Task ThenTheMerchantFilterIsPrePopulated()
    {
        await _reportingHelper.VerifyMerchantFilterIsPrePopulated();
    }

    #endregion
}
