using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;
using Reqnroll;
using Shouldly;
using Shared.IntegrationTesting;

namespace EstateManagementUI.IntegrationTests.Common;

public sealed class ReportingPageHelper
{
    private const string TableRowsSelector = "table tbody tr";
    private const string ValueText = "Value";
    private const string ValueColumnName = ValueText;
    private const string DateFormat = "yyyy-MM-dd";

    private readonly IPage _page;

    public ReportingPageHelper(IPage page)
    {
        _page = page;
    }

    public async Task OpenReportingDashboardAsync()
    {
        await NavigateAsync("/reporting");
    }

    public async Task OpenTransactionDetailReportAsync()
    {
        await NavigateAsync("/reporting/transaction-detail");
    }

    public async Task OpenTransactionSummaryByMerchantReportAsync()
    {
        await NavigateAsync("/reporting/transaction-summary-merchant");
    }

    public async Task OpenTransactionSummaryByOperatorReportAsync()
    {
        await NavigateAsync("/reporting/transaction-summary-operator");
    }

    public async Task OpenProductPerformanceReportAsync()
    {
        await NavigateAsync("/reporting/product-performance");
    }

    public async Task OpenSettlementSummaryReportAsync()
    {
        await NavigateAsync("/reporting/settlement-summary");
    }

    public async Task OpenMerchantSettlementHistoryReportAsync()
    {
        await NavigateAsync("/reporting/merchant-settlement-history");
    }

    public async Task OpenSettlementReconciliationReportAsync()
    {
        await NavigateAsync("/reporting/settlement-reconciliation");
    }

    public async Task AssertReportingDashboardHeadingVisibleAsync()
    {
        await _page.GetByRole(AriaRole.Heading, new() { Name = "Reporting Dashboard" }).WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 10000
        });
    }

    public async Task AssertReportingDashboardLinksVisibleAsync()
    {
        await AssertVisibleAsync("Transaction Detail Report");
        await AssertVisibleAsync("Transaction Summary by Merchant");
        await AssertVisibleAsync("Transaction Summary by Operator");
        await AssertVisibleAsync("Product Performance Report");
        await AssertVisibleAsync("Settlement Summary Report");
        await AssertVisibleAsync("Merchant Settlement History");
        await AssertVisibleAsync("Settlement vs Transaction Reconciliation Report");
        await AssertVisibleAsync("Analytical Charts (Volume & Value)");
    }

    public async Task AssertTransactionDetailHeadingVisibleAsync()
    {
        await AssertHeadingVisibleAsync("Transaction Detail Report");
    }

    public async Task AssertTransactionDetailSummaryValuesAsync(DataTable table)
    {
        await AssertSummaryValuesAsync(table);
    }

    public async Task AssertTransactionDetailRowsAsync(DataTable table)
    {
        var rows = _page.Locator(TableRowsSelector);
        (await rows.CountAsync()).ShouldBe(table.Rows.Count);

        for (var index = 0; index < table.Rows.Count; index++)
        {
            var expectedRow = table.Rows[index];
            var row = rows.Nth(index);
            await AssertCellTextAsync(row, 1, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Merchant"));
            await AssertCellTextAsync(row, 3, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Product"));
            await AssertCellTextAsync(row, 4, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Transaction Number"));
            await AssertCellTextAsync(row, 5, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Type"));
            await AssertCellTextAsync(row, 6, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Status"));
        }
    }

    public async Task AssertTransactionSummaryByMerchantHeadingVisibleAsync()
    {
        await AssertHeadingVisibleAsync("Transaction Summary by Merchant");
    }

    public async Task AssertMerchantSummaryValuesAsync(DataTable table)
    {
        await AssertSummaryValuesAsync(table);
    }

    public async Task AssertMerchantSummaryRowsAsync(DataTable table)
    {
        var rows = _page.Locator(TableRowsSelector);
        (await rows.CountAsync()).ShouldBe(table.Rows.Count);

        for (var index = 0; index < table.Rows.Count; index++)
        {
            var expectedRow = table.Rows[index];
            var row = rows.Nth(index);
            await AssertCellTextAsync(row, 0, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Merchant"));
            await AssertCellTextAsync(row, 1, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Total Count"));
            await AssertCellTextAsync(row, 2, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Total Value"));
            await AssertCellTextAsync(row, 3, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Average Value"));
            await AssertCellTextAsync(row, 4, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Successful"));
            await AssertCellTextAsync(row, 5, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Failed"));
            await AssertCellTextAsync(row, 6, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Success Rate"), normalizePercent: true);
        }
    }

    public async Task AssertTransactionSummaryByOperatorHeadingVisibleAsync()
    {
        await AssertHeadingVisibleAsync("Transaction Summary by Operator");
    }

    public async Task AssertOperatorSummaryValuesAsync(DataTable table)
    {
        await AssertSummaryValuesAsync(table);
    }

    public async Task AssertOperatorSummaryRowsAsync(DataTable table)
    {
        var rows = _page.Locator(TableRowsSelector);
        (await rows.CountAsync()).ShouldBe(table.Rows.Count);

        for (var index = 0; index < table.Rows.Count; index++)
        {
            var expectedRow = table.Rows[index];
            var row = rows.Nth(index);
            await AssertCellTextAsync(row, 0, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Operator"));
            await AssertCellTextAsync(row, 1, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Transaction Count"));
            await AssertCellTextAsync(row, 2, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Transaction Value"));
            await AssertCellTextAsync(row, 3, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Average Value"));
            await AssertCellTextAsync(row, 4, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Successful"));
            await AssertCellTextAsync(row, 5, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Failed"));
            await AssertCellTextAsync(row, 6, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Success Rate"), normalizePercent: true);
        }
    }

    public async Task AssertProductPerformanceHeadingVisibleAsync()
    {
        await AssertHeadingVisibleAsync("Product Performance Report");
    }

    public async Task AssertProductPerformanceSummaryValuesAsync(DataTable table)
    {
        await AssertSummaryValuesAsync(table);
    }

    public async Task AssertProductPerformanceRowsAsync(DataTable table)
    {
        var rows = _page.Locator(TableRowsSelector);
        (await rows.CountAsync()).ShouldBe(table.Rows.Count);

        for (var index = 0; index < table.Rows.Count; index++)
        {
            var expectedRow = table.Rows[index];
            var row = rows.Nth(index);
            await AssertCellTextAsync(row, 0, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Product"));
            await AssertCellTextAsync(row, 1, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Transaction Count"));
            await AssertCellTextAsync(row, 2, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Transaction Value"));
            await AssertCellTextAsync(row, 3, ReqnrollTableHelper.GetStringRowValue(expectedRow, "Percentage"), normalizePercent: true);
        }
    }

    public async Task AssertProductPerformancePercentageSplitAsync()
    {
        await _page.GetByRole(AriaRole.Button, new() { Name = "Chart View" }).ClickAsync();
        await AssertTextVisibleAsync("Percentages sum to 100% - Data validated");
    }

    public async Task AssertSettlementSummaryHeadingVisibleAsync()
    {
        await AssertHeadingVisibleAsync("Settlement Summary Report");
    }

    public async Task AssertSettlementSummaryPlaceholderVisibleAsync()
    {
        await AssertTextVisibleAsync("No settlement data available for the selected period");
    }

    public async Task AssertMerchantSettlementHistoryHeadingVisibleAsync()
    {
        await AssertHeadingVisibleAsync("Merchant Settlement History");
    }

    public async Task AssertMerchantSettlementHistoryPlaceholderVisibleAsync()
    {
        await AssertTextVisibleAsync("No settlement history found for the selected criteria");
    }

    public async Task AssertSettlementReconciliationHeadingVisibleAsync()
    {
        await AssertHeadingVisibleAsync("Settlement vs Transaction Reconciliation Report");
    }

    public async Task AssertSettlementReconciliationPlaceholderVisibleAsync()
    {
        await AssertTextVisibleAsync("Settlement vs transaction reconciliation report functionality will be implemented here.");
    }

    public async Task OpenAnalyticalChartsReportAsync()
    {
        await NavigateAsync("/reporting/analytical-charts");
    }

    public async Task AssertAnalyticalChartsHeadingVisibleAsync()
    {
        await _page.GetByRole(AriaRole.Heading, new() { Name = "Analytical Charts (Volume & Value)" }).WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 10000
        });
    }

    public async Task AssertAnalyticalChartsSummaryValuesAsync(DataTable table)
    {
        await WaitForAnalyticalChartsReadyAsync();

        foreach (var row in table.Rows)
        {
            var label = ReqnrollTableHelper.GetStringRowValue(row, "Label");
            var value = ReqnrollTableHelper.GetStringRowValue(row, ValueColumnName);

            if (IsMoneyLabel(label))
            {
                await AssertMoneyInfoBoxValueAsync(label, decimal.Parse(value, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
            }
            else
            {
                await AssertInfoBoxValueAsync(label, int.Parse(value, NumberStyles.Integer | NumberStyles.AllowThousands, CultureInfo.InvariantCulture));
            }
        }
    }

    public async Task AssertAnalyticalChartsComparisonDateAsync(string comparisonDate)
    {
        var dateToSelect = ResolveComparisonDateValue(comparisonDate);
        var expectedLabel = DateTime.ParseExact(dateToSelect, DateFormat, CultureInfo.InvariantCulture).ToString("MMM dd", CultureInfo.InvariantCulture);

        await _page.Locator("#comparisonDateSelector").SelectOptionAsync(new SelectOptionValue { Value = dateToSelect });
        await WaitForAnalyticalChartsReadyAsync();

        await _page.WaitForFunctionAsync($@"() => {{
            const chart = Chart.getChart('volumeChart');
            if (!chart || !chart.data || !chart.data.datasets || chart.data.datasets.length < 2) return false;
            const label = chart.data.datasets[1].label ?? '';
            return label.includes('{expectedLabel}');
        }}", new FrameWaitForFunctionOptions { Timeout = 120000 });
    }

    public async Task AssertAnalyticalChartsChartTotalsAsync(DataTable table)
    {
        var volumeChart = await ReadChartAsync("volumeChart");

        foreach (var row in table.Rows)
        {
            var chart = ReqnrollTableHelper.GetStringRowValue(row, "Chart");
            var today = decimal.Parse(ReqnrollTableHelper.GetStringRowValue(row, "Today"), CultureInfo.InvariantCulture);
            var comparison = decimal.Parse(ReqnrollTableHelper.GetStringRowValue(row, "Comparison"), CultureInfo.InvariantCulture);

            if (chart.Equals("Volume", StringComparison.OrdinalIgnoreCase))
            {
                volumeChart.TotalTodayCount.ShouldBe((int)today);
                volumeChart.TotalComparisonCount.ShouldBe((int)comparison);
            }
            else if (chart.Equals(ValueText, StringComparison.OrdinalIgnoreCase))
            {
                var valueChart = await ReadChartAsync("valueChart");
                valueChart.TotalTodayValue.ShouldBe(today);
                valueChart.TotalComparisonValue.ShouldBe(comparison);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(table), $"Unsupported chart '{chart}'.");
            }
        }
    }

    public async Task AssertAnalyticalChartsHourlyPointsAsync(DataTable table)
    {
        var volumeChart = await ReadChartSeriesAsync("volumeChart");
        var valueChart = await ReadChartSeriesAsync("valueChart");

        foreach (var row in table.Rows)
        {
            var chart = ReqnrollTableHelper.GetStringRowValue(row, "Chart");
            var hour = ReqnrollTableHelper.GetStringRowValue(row, "Hour");
            var today = decimal.Parse(ReqnrollTableHelper.GetStringRowValue(row, "Today"), CultureInfo.InvariantCulture);
            var comparison = decimal.Parse(ReqnrollTableHelper.GetStringRowValue(row, "Comparison"), CultureInfo.InvariantCulture);

            if (chart.Equals("Volume", StringComparison.OrdinalIgnoreCase))
            {
                AssertChartSeriesPoint(volumeChart, hour, today, comparison);
            }
            else if (chart.Equals(ValueText, StringComparison.OrdinalIgnoreCase))
            {
                AssertChartSeriesPoint(valueChart, hour, today, comparison);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(table), $"Unsupported chart '{chart}'.");
            }
        }
    }

    private async Task NavigateAsync(string path)
    {
        await _page.GotoAsync(
            $"{ResolveBaseUrl()}{path}",
            new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = 120000
            });

        await _page.WaitForLoadStateAsync(
            LoadState.DOMContentLoaded,
            new PageWaitForLoadStateOptions { Timeout = 120000 });
    }

    private async Task AssertHeadingVisibleAsync(string heading)
    {
        await _page.GetByRole(AriaRole.Heading, new() { Name = heading }).WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 10000
        });
    }

    private string ResolveBaseUrl()
    {
        return "http://127.0.0.1:5004";
    }

    private static bool IsMoneyLabel(string label)
    {
        return label.Contains(ValueText, StringComparison.OrdinalIgnoreCase) ||
               label.Contains("Amount", StringComparison.OrdinalIgnoreCase) ||
               label.Contains("Fee", StringComparison.OrdinalIgnoreCase) ||
               label.Contains("Settlement", StringComparison.OrdinalIgnoreCase) ||
               label.Contains("Average", StringComparison.OrdinalIgnoreCase);
    }

    private async Task WaitForAnalyticalChartsReadyAsync()
    {
        var spinner = _page.Locator(".animate-spin");
        if (await spinner.CountAsync() > 0)
        {
            await spinner.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Detached,
                Timeout = 60000
            });
        }

        await _page.WaitForFunctionAsync(@"() => typeof Chart !== 'undefined'", new FrameWaitForFunctionOptions { Timeout = 120000 });

        await _page.WaitForFunctionAsync(@"() => {
            const volumeChart = Chart.getChart('volumeChart');
            const valueChart = Chart.getChart('valueChart');
            return !!volumeChart && !!valueChart && volumeChart.data.datasets.length >= 2 && valueChart.data.datasets.length >= 2;
        }", new FrameWaitForFunctionOptions { Timeout = 120000 });
    }

    private async Task AssertVisibleAsync(string text)
    {
        (await _page.GetByRole(AriaRole.Link, new() { Name = text }).IsVisibleAsync()).ShouldBeTrue();
    }

    private async Task AssertTextVisibleAsync(string text)
    {
        (await _page.GetByText(text, new() { Exact = false }).IsVisibleAsync()).ShouldBeTrue();
    }

    private async Task AssertInfoBoxValueAsync(string label, int expectedValue)
    {
        var infoBox = _page.Locator(".info-box").Filter(new() { Has = _page.GetByText(label, new() { Exact = true }) });
        var text = await WaitForInfoBoxValueAsync(infoBox, expectedValue.ToString(CultureInfo.CurrentCulture));
        int.Parse(text!, NumberStyles.Integer | NumberStyles.AllowThousands, CultureInfo.CurrentCulture).ShouldBe(expectedValue);
    }

    private async Task AssertMoneyInfoBoxValueAsync(string label, decimal expectedValue)
    {
        var infoBox = _page.Locator(".info-box").Filter(new() { Has = _page.GetByText(label, new() { Exact = true }) });
        var text = await WaitForInfoBoxValueAsync(infoBox, expectedValue.ToString("C", CultureInfo.CurrentCulture));
        ParseMoney(text!).ShouldBe(expectedValue);
    }

    private static async Task<string?> WaitForInfoBoxValueAsync(ILocator infoBox, string expectedValue)
    {
        const int attempts = 40;

        for (var attempt = 0; attempt < attempts; attempt++)
        {
            var text = await infoBox.Locator(".info-box-number").First.TextContentAsync();
            if (string.Equals(text?.Trim(), expectedValue, StringComparison.CurrentCulture))
            {
                return text;
            }

            await Task.Delay(500);
        }

        return await infoBox.Locator(".info-box-number").First.TextContentAsync();
    }

    private static decimal ParseMoney(string value)
    {
        if (decimal.TryParse(value, NumberStyles.Currency | NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed))
        {
            return parsed;
        }

        if (decimal.TryParse(value, NumberStyles.Currency | NumberStyles.Number, CultureInfo.InvariantCulture, out parsed))
        {
            return parsed;
        }

        var normalized = new string(value.Where(character =>
            char.IsDigit(character) ||
            character == '.' ||
            character == ',' ||
            character == '-').ToArray());

        if (decimal.TryParse(normalized, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out parsed))
        {
            return parsed;
        }

        throw new FormatException($"Unable to parse money value '{value}'.");
    }

    private async Task AssertSummaryValuesAsync(DataTable table)
    {
        foreach (var row in table.Rows)
        {
            var label = ReqnrollTableHelper.GetStringRowValue(row, "Label");
            var value = ReqnrollTableHelper.GetStringRowValue(row, ValueColumnName);

            if (IsMoneyLabel(label))
            {
                await AssertMoneyInfoBoxValueAsync(label, decimal.Parse(value, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
            }
            else
            {
                await AssertInfoBoxValueAsync(label, int.Parse(value, NumberStyles.Integer | NumberStyles.AllowThousands, CultureInfo.InvariantCulture));
            }
        }
    }

    private async Task AssertCellTextAsync(ILocator row, int columnIndex, string expectedText, bool normalizePercent = false)
    {
        var actualText = await row.Locator("td").Nth(columnIndex).InnerTextAsync();
        actualText = actualText?.Trim() ?? string.Empty;
        expectedText = expectedText.Trim();

        if (normalizePercent)
        {
            var actualPercent = actualText.TrimEnd('%').Trim();
            var expectedPercent = expectedText.TrimEnd('%').Trim();
            decimal.Parse(actualPercent, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture)
                .ShouldBe(decimal.Parse(expectedPercent, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture));
            return;
        }

        if (decimal.TryParse(expectedText, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var expectedMoney))
        {
            ParseMoney(actualText).ShouldBe(expectedMoney);
            return;
        }

        actualText.ShouldBe(expectedText);
    }

    private static string ResolveComparisonDateValue(string comparisonDate)
    {
        if (DateOnly.TryParseExact(comparisonDate, DateFormat, out var date))
        {
            return date.ToString(DateFormat, CultureInfo.InvariantCulture);
        }

        var resolved = ReqnrollTableHelper.GetDateForDateString(comparisonDate, DateTime.Now);
        return resolved.ToString(DateFormat, CultureInfo.InvariantCulture);
    }

    private static void AssertChartSeriesPoint(ChartSeriesSnapshot chart, string hour, decimal expectedToday, decimal expectedComparison)
    {
        var index = Array.FindIndex(chart.Labels, label => string.Equals(label, hour, StringComparison.OrdinalIgnoreCase));
        index.ShouldBeGreaterThanOrEqualTo(0, $"Could not find hourly bucket '{hour}'.");
        chart.TodaySeries[index].ShouldBe(expectedToday);
        chart.ComparisonSeries[index].ShouldBe(expectedComparison);
    }

    private async Task<(int TotalTodayCount, int TotalComparisonCount, decimal TotalTodayValue, decimal TotalComparisonValue)> ReadChartAsync(string chartId)
    {
        var chart = await ReadChartSeriesAsync(chartId);

        if (chartId == "volumeChart")
        {
            return (chart.TodaySeries.Select(Convert.ToInt32).Sum(), chart.ComparisonSeries.Select(Convert.ToInt32).Sum(), 0m, 0m);
        }

        return (0, 0, chart.TodaySeries.Sum(), chart.ComparisonSeries.Sum());
    }

    private async Task<ChartSeriesSnapshot> ReadChartSeriesAsync(string chartId)
    {
        var json = await _page.EvaluateAsync<string>($@"() => {{
            const chart = Chart.getChart('{chartId}');
            if (!chart) return '';
            return JSON.stringify({{
                labels: Array.from(chart.data.labels ?? []),
                datasets: chart.data.datasets.map(d => Array.from(d.data))
            }});
        }}");

        json.ShouldNotBeNullOrWhiteSpace();

        using var document = JsonDocument.Parse(json);
        var labels = document.RootElement.GetProperty("labels").EnumerateArray().Select(x => x.GetString() ?? string.Empty).ToArray();
        var datasets = document.RootElement.GetProperty("datasets");
        var todaySeries = datasets[0].EnumerateArray().Select(x => x.GetDecimal()).ToArray();
        var comparisonSeries = datasets[1].EnumerateArray().Select(x => x.GetDecimal()).ToArray();

        return new ChartSeriesSnapshot(labels, todaySeries, comparisonSeries);
    }

    private sealed record ChartSeriesSnapshot(string[] Labels, decimal[] TodaySeries, decimal[] ComparisonSeries);
}
