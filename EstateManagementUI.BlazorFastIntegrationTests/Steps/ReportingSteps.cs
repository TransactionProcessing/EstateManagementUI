using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;
using Shared.IntegrationTesting;

namespace EstateManagementUI.BlazorFastIntegrationTests.Steps;

[Binding]
[Scope(Tag = "reporting")]
public sealed class ReportingSteps
{
    private readonly ReportingPageHelper _helper;

    public ReportingSteps(IPage page)
    {
        _helper = new ReportingPageHelper(page);
    }

    [When("I open the reporting dashboard")]
    public Task WhenIOpenTheReportingDashboard() => _helper.OpenReportingDashboardAsync();

    [Then("I should see the reporting dashboard heading")]
    public Task ThenIShouldSeeTheReportingDashboardHeading() => _helper.AssertReportingDashboardHeadingVisibleAsync();

    [Then("I should see the reporting dashboard links")]
    public Task ThenIShouldSeeTheReportingDashboardLinks() => _helper.AssertReportingDashboardLinksVisibleAsync();

    [When("I open the transaction detail report")]
    public Task WhenIOpenTheTransactionDetailReport() => _helper.OpenTransactionDetailReportAsync();

    [Then("I should see the transaction detail report heading")]
    public Task ThenIShouldSeeTheTransactionDetailReportHeading() => _helper.AssertTransactionDetailHeadingVisibleAsync();

    [Then("the transaction detail report should show these summary values")]
    public Task ThenTheTransactionDetailReportShouldShowTheseSummaryValues(DataTable table) => _helper.AssertTransactionDetailSummaryValuesAsync(table);

    [Then("the transaction detail report should list these transactions")]
    public Task ThenTheTransactionDetailReportShouldListTheseTransactions(DataTable table) => _helper.AssertTransactionDetailRowsAsync(table);

    [When("I open the transaction summary by merchant report")]
    public Task WhenIOpenTheTransactionSummaryByMerchantReport() => _helper.OpenTransactionSummaryByMerchantReportAsync();

    [Then("I should see the transaction summary by merchant report heading")]
    public Task ThenIShouldSeeTheTransactionSummaryByMerchantReportHeading() => _helper.AssertTransactionSummaryByMerchantHeadingVisibleAsync();

    [Then("the merchant summary report should show these summary values")]
    public Task ThenTheMerchantSummaryReportShouldShowTheseSummaryValues(DataTable table) => _helper.AssertMerchantSummaryValuesAsync(table);

    [Then("the merchant summary report should list these merchant rows")]
    public Task ThenTheMerchantSummaryReportShouldListTheseMerchantRows(DataTable table) => _helper.AssertMerchantSummaryRowsAsync(table);

    [When("I open the transaction summary by operator report")]
    public Task WhenIOpenTheTransactionSummaryByOperatorReport() => _helper.OpenTransactionSummaryByOperatorReportAsync();

    [Then("I should see the transaction summary by operator report heading")]
    public Task ThenIShouldSeeTheTransactionSummaryByOperatorReportHeading() => _helper.AssertTransactionSummaryByOperatorHeadingVisibleAsync();

    [Then("the operator summary report should show these summary values")]
    public Task ThenTheOperatorSummaryReportShouldShowTheseSummaryValues(DataTable table) => _helper.AssertOperatorSummaryValuesAsync(table);

    [Then("the operator summary report should list these operator rows")]
    public Task ThenTheOperatorSummaryReportShouldListTheseOperatorRows(DataTable table) => _helper.AssertOperatorSummaryRowsAsync(table);

    [When("I open the product performance report")]
    public Task WhenIOpenTheProductPerformanceReport() => _helper.OpenProductPerformanceReportAsync();

    [Then("I should see the product performance report heading")]
    public Task ThenIShouldSeeTheProductPerformanceReportHeading() => _helper.AssertProductPerformanceHeadingVisibleAsync();

    [Then("the product performance report should show these summary values")]
    public Task ThenTheProductPerformanceReportShouldShowTheseSummaryValues(DataTable table) => _helper.AssertProductPerformanceSummaryValuesAsync(table);

    [Then("the product performance report should list these product rows")]
    public Task ThenTheProductPerformanceReportShouldListTheseProductRows(DataTable table) => _helper.AssertProductPerformanceRowsAsync(table);

    [Then("the product performance report should validate the percentage split")]
    public Task ThenTheProductPerformanceReportShouldValidateThePercentageSplit() => _helper.AssertProductPerformancePercentageSplitAsync();

    [When("I open the settlement summary report")]
    public Task WhenIOpenTheSettlementSummaryReport() => _helper.OpenSettlementSummaryReportAsync();

    [Then("I should see the settlement summary report heading")]
    public Task ThenIShouldSeeTheSettlementSummaryReportHeading() => _helper.AssertSettlementSummaryHeadingVisibleAsync();

    [Then("I should see the settlement summary placeholder")]
    public Task ThenIShouldSeeTheSettlementSummaryPlaceholder() => _helper.AssertSettlementSummaryPlaceholderVisibleAsync();

    [When("I open the merchant settlement history report")]
    public Task WhenIOpenTheMerchantSettlementHistoryReport() => _helper.OpenMerchantSettlementHistoryReportAsync();

    [Then("I should see the merchant settlement history report heading")]
    public Task ThenIShouldSeeTheMerchantSettlementHistoryReportHeading() => _helper.AssertMerchantSettlementHistoryHeadingVisibleAsync();

    [Then("I should see the merchant settlement history placeholder")]
    public Task ThenIShouldSeeTheMerchantSettlementHistoryPlaceholder() => _helper.AssertMerchantSettlementHistoryPlaceholderVisibleAsync();

    [When("I open the settlement reconciliation report")]
    public Task WhenIOpenTheSettlementReconciliationReport() => _helper.OpenSettlementReconciliationReportAsync();

    [Then("I should see the settlement reconciliation report heading")]
    public Task ThenIShouldSeeTheSettlementReconciliationReportHeading() => _helper.AssertSettlementReconciliationHeadingVisibleAsync();

    [Then("I should see the settlement reconciliation placeholder")]
    public Task ThenIShouldSeeTheSettlementReconciliationPlaceholder() => _helper.AssertSettlementReconciliationPlaceholderVisibleAsync();

    [When("I open the analytical charts report")]
    public Task WhenIOpenTheAnalyticalChartsReport() => _helper.OpenAnalyticalChartsReportAsync();

    [Then("I should see the analytical charts report heading")]
    public Task ThenIShouldSeeTheAnalyticalChartsReportHeading() => _helper.AssertAnalyticalChartsHeadingVisibleAsync();

    [Then("the analytical charts report should show these summary values")]
    public Task ThenTheAnalyticalChartsReportShouldShowTheseSummaryValues(DataTable table) => _helper.AssertAnalyticalChartsSummaryValuesAsync(table);

    [Then("the analytical charts report should use this comparison date")]
    public async Task ThenTheAnalyticalChartsReportShouldUseThisComparisonDate(DataTable table)
    {
        var row = table.Rows.Single();
        await _helper.AssertAnalyticalChartsComparisonDateAsync(ReqnrollTableHelper.GetStringRowValue(row, "Comparison Date"));
    }

    [Then("the analytical charts report should compare these chart totals")]
    public Task ThenTheAnalyticalChartsReportShouldCompareTheseChartTotals(DataTable table) => _helper.AssertAnalyticalChartsChartTotalsAsync(table);

    [Then("the analytical charts report should compare these hourly chart points")]
    public Task ThenTheAnalyticalChartsReportShouldCompareTheseHourlyChartPoints(DataTable table) => _helper.AssertAnalyticalChartsHourlyPointsAsync(table);
}
