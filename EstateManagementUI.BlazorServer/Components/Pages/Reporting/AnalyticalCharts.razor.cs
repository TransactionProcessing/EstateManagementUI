using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.General;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Reporting
{
    public partial class AnalyticalCharts
    {
        private bool isLoading = true;
        
        private List<ComparisonDateModel>? comparisonDates;
        private string _selectedComparisonDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
        private string _selectedChartType = "line";

        private decimal totalValue = 0;
        private int totalCount = 0;
        private decimal averageValue = 0;
        private decimal netSettlement = 0;

        private List<TransactionModels.TodaysSalesByHourModel>? salesByHourData;
        private TransactionModels.TodaysSalesModel? todaysSales;
        private TransactionModels.TodaysSettlementModel? todaysSettlement;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            Result result = await OnAfterRender(PermissionSection.Reporting, PermissionFunction.AnalyticalChartsReport, this.LoadDashboardData);
            if (result.IsFailed)
            {
                return;
            }

            // Give Chart.js time to load from CDN
            await this.WaitOnUIRefresh();

            // TODO:: move to a function that can be called after data loads as well, to handle cases where Chart.js loads after first render
            if (!isLoading && this.salesByHourData != null)
            {
                try
                {
                    // Check if Chart.js is available
                    var isChartJsLoaded = await JSRuntime.InvokeAsync<bool>("eval", "typeof Chart !== 'undefined'");
                    if (!isChartJsLoaded)
                    {
                        return;
                    }

                    await UpdateCharts();
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        private async Task<Result> LoadDashboardData() {
            try
            {
                isLoading = true;
                errorMessage = null;
                StateHasChanged();

                var correlationId = new CorrelationId(Guid.NewGuid());
                Guid estateId = await this.GetEstateId();

                // Load comparison dates first (only if not already loaded)
                if (comparisonDates == null || !comparisonDates.Any())
                {
                    var comparisonDatesResult = await this.CalendarUiService.GetComparisonDates(correlationId, estateId);
                    if (comparisonDatesResult.IsSuccess)
                    {
                        comparisonDates = comparisonDatesResult.Data;
                        if (comparisonDates != null && comparisonDates.Any())
                        {
                            _selectedComparisonDate = comparisonDates.First().Date.ToString("yyyy-MM-dd");
                        }
                    }
                }

                if (!DateTime.TryParse(_selectedComparisonDate, out var comparisonDate))
                {
                    comparisonDate = DateTime.Now.AddDays(-7);
                }

                // Load all data in parallel
                var salesByHourTask = this.TransactionUiService.GetTodaysSalesByHour(correlationId, estateId, comparisonDate);
                var todaysSalesTask = this.TransactionUiService.GetTodaysSales(correlationId, estateId, comparisonDate);
                var settlementTask = this.TransactionUiService.GetTodaysSettlement(correlationId, estateId, comparisonDate);

                await Task.WhenAll(salesByHourTask, todaysSalesTask, settlementTask);

                // Process results
                if (salesByHourTask.Result.IsSuccess)
                    this.salesByHourData = salesByHourTask.Result.Data;

                if (todaysSalesTask.Result.IsSuccess)
                    todaysSales = todaysSalesTask.Result.Data;

                if (settlementTask.Result.IsSuccess)
                    todaysSettlement = settlementTask.Result.Data;

                // Calculate KPIs
                CalculateKPIs();
                return Result.Success();
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to load data: {ex.Message}";
                return Result.Failure(this.errorMessage);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void CalculateKPIs()
        {
            if (todaysSales != null)
            {
                totalValue = todaysSales.TodaysSalesValue;
                totalCount = todaysSales.TodaysSalesCount;
                averageValue = totalCount > 0 ? totalValue / totalCount : 0;
            }

            if (todaysSettlement != null)
            {
                netSettlement = todaysSettlement.TodaysSettlementValue;
            }
        }

        private async Task OnFiltersChanged()
        {
            await LoadDashboardData();
            await UpdateCharts();
        }

        private async Task UpdateCharts()
        {
            try
            {
                if (this.salesByHourData == null)
                {
                    return;
                }
                
                // Create labels with date and time context
                var today = DateTime.Today;
                var comparisonDateParsed = DateTime.TryParse(_selectedComparisonDate, out var compDate) ? compDate : DateTime.Today.AddDays(-7);

                var labels = salesByHourData.Select(d => $"{d.Hour:00}:00").ToArray();
                var todaysCountData = this.salesByHourData.Select(d => d.TodaysSalesCount).ToArray();
                var comparisonCountData = this.salesByHourData.Select(d => d.ComparisonSalesCount).ToArray();

                var todaysValueData = this.salesByHourData.Select(d => (double)d.TodaysSalesValue).ToArray();
                var comparisonValueData = this.salesByHourData.Select(d => (double)d.ComparisonSalesValue).ToArray();

                var comparisonLabel = GetComparisonLabel();
                var todayLabel = today.ToString("MMM dd");
                var comparisonDateLabel = comparisonDateParsed.ToString("MMM dd");

                // Update Volume Chart
                await JSRuntime.InvokeVoidAsync("updateOrCreateChart",
                    "volumeChart",
                    _selectedChartType,
                    labels,
                    new object[]
                    {
                    new { label = $"Today ({todayLabel}) Volume", data = todaysCountData, borderColor = "rgb(59, 130, 246)", backgroundColor = "rgba(59, 130, 246, 0.1)", tension = 0.4 },
                    new { label = $"{comparisonLabel} ({comparisonDateLabel}) Volume", data = comparisonCountData, borderColor = "rgb(156, 163, 175)", backgroundColor = "rgba(156, 163, 175, 0.1)", tension = 0.4 }
                    },
                    "Transaction Count"
                );

                // Update Value Chart
                await JSRuntime.InvokeVoidAsync("updateOrCreateChart",
                    "valueChart",
                    _selectedChartType,
                    labels,
                    new object[]
                    {
                    new { label = $"Today ({todayLabel}) Value", data = todaysValueData, borderColor = "rgb(16, 185, 129)", backgroundColor = "rgba(16, 185, 129, 0.1)", tension = 0.4 },
                    new { label = $"{comparisonLabel} ({comparisonDateLabel}) Value", data = comparisonValueData, borderColor = "rgb(156, 163, 175)", backgroundColor = "rgba(156, 163, 175, 0.1)", tension = 0.4 }
                    },
                    "Transaction Value (KES)"
                );
            }
            catch (Exception ex)
            {
                
            }
        }

        private string GetComparisonLabel()
        {
            if (comparisonDates == null) return "Comparison";
            if (!DateTime.TryParse(_selectedComparisonDate, out var date))
                return "Comparison";
            var comparisonDate = comparisonDates.FirstOrDefault(d => d.Date.Date == date.Date);
            return comparisonDate?.Description ?? date.ToString("MMM dd");
        }
    }
}
