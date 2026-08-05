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
