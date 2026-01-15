using System.Security.Claims;
using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shared.Exceptions;
using Shared.General;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages
{
    public partial class Home
    {
        // Response code for low credit failures
        private const string LOW_CREDIT_RESPONSE_CODE = "1008";

        private bool isLoading = true;
        private bool isAdministrator;
        private string? errorMessage;

        private MerchantKpiModel? merchantKpi;
        private TodaysSalesModel? todaysSales;
        private TodaysSalesModel? todaysFailedSales;
        private List<ComparisonDateModel>? comparisonDates;
        private List<MerchantModel>? recentMerchants;
        private string _selectedComparisonDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
        private int changeEventCounter = 0;

        protected override async Task OnInitializedAsync()
        {
            // Keep prerender work minimal. This will still run during prerender,
            // so avoid doing heavy/interactive-only tasks here.
            await LogToConsole("OnInitializedAsync (prerender/early) START");
            // Do not call LoadDashboardData() here to avoid double-load when prerendering.
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }

            // This runs once after the interactive circuit is established.
            await LogToConsole("OnAfterRenderAsync FIRST RENDER (interactive) - performing interactive initialization");

            try
            {
                AuthenticationState authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                ClaimsPrincipal user = authState.User;

                // Redirect unauthenticated users to entry screen
                if (!user.Identity?.IsAuthenticated ?? true) {
                    NavigationManager.NavigateTo("/entry", replace: true);
                    return;
                }

                // Determine role and admin flag now that we're interactive
                var role = await PermissionService.GetUserRoleAsync();
                isAdministrator = role == "Administrator";
                await LogToConsole($"User role: {role}, isAdministrator: {isAdministrator}");

                // Only load dashboard data for non-admins
                if (isAdministrator == false) {
                    var result = await LoadDashboardData();
                    if (result.IsFailed) {
                        NavigationManager.NavigateTo("/error", replace: true);
                        return;
                    }
                }
                else {
                    isLoading = false;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Initialization error: {ex.Message}";
                isLoading = false;
                StateHasChanged();
                NavigationManager.NavigateTo("/error", replace: true);
                return;
            }

            await base.OnAfterRenderAsync(firstRender);
        }
        
        private async Task<Result> LoadDashboardData()
        {
            await LogToConsole($"LoadDashboardData START - selectedDate: {_selectedComparisonDate}");
            try {
                isLoading = true;
                errorMessage = null;
                StateHasChanged();

                var correlationId = new CorrelationId(Guid.NewGuid());
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                
                var estateIdResult = authState.GetEstateIdFromClaims();
                if (estateIdResult.IsFailed) {
                    return ResultHelpers.CreateFailure(estateIdResult);
                }
                var accessToken = "stubbed-token";

                // Load comparison dates first (only if not already loaded)
                if (comparisonDates == null || !comparisonDates.Any()) {
                    var comparisonDatesResult = await Mediator.Send(new Queries.GetComparisonDatesQuery(correlationId, estateIdResult.Data));
                    if (comparisonDatesResult.IsFailed) {
                        return ResultHelpers.CreateFailure(comparisonDatesResult);
                    }

                    comparisonDates = ModelFactory.ConvertFrom(comparisonDatesResult.Data);
                    if (comparisonDates != null && comparisonDates.Any()) {
                        // Set default comparison date to the first one only on initial load
                        _selectedComparisonDate = comparisonDates.First().Date.ToString("yyyy-MM-dd");
                    }
                }

                if (!DateTime.TryParse(_selectedComparisonDate, out DateTime comparisonDate)) {
                    // Fallback to a week ago if parse fails
                    comparisonDate = DateTime.Now.AddDays(-7);
                }

                // Load all dashboard data in parallel
                var kpiTask = Mediator.Send(new Queries.GetMerchantKpiQuery(correlationId, accessToken, estateIdResult.Data));
                var salesTask = Mediator.Send(new Queries.GetTodaysSalesQuery(correlationId, accessToken, estateIdResult.Data, comparisonDate));
                var failedSalesTask = Mediator.Send(new Queries.GetTodaysFailedSalesQuery(correlationId, accessToken, estateIdResult.Data, LOW_CREDIT_RESPONSE_CODE, comparisonDate));
                var merchantsTask = Mediator.Send(new Queries.GetMerchantsQuery(correlationId, accessToken, estateIdResult.Data));

                await Task.WhenAll(kpiTask, salesTask, failedSalesTask, merchantsTask);

                // Process results
                if (kpiTask.Result.IsSuccess)
                    merchantKpi = ModelFactory.ConvertFrom(kpiTask.Result.Data);

                if (salesTask.Result.IsSuccess)
                    todaysSales = ModelFactory.ConvertFrom(salesTask.Result.Data);

                if (failedSalesTask.Result.IsSuccess)
                    todaysFailedSales = ModelFactory.ConvertFrom(failedSalesTask.Result.Data);

                if (merchantsTask.Result.IsSuccess)
                    // Note: API returns merchants in creation order (newest first)
                    // If ordering is incorrect, would need CreatedDate field in the model
                    recentMerchants = ModelFactory.ConvertFrom(merchantsTask.Result.Data?.ToList());

                return Result.Success();
            }
            catch (Exception ex) {
                errorMessage = $"Failed to load dashboard data: {ex.Message}";
                return Result.Failure(ex.GetCombinedExceptionMessages());

            }
            finally {
                isLoading = false;
                StateHasChanged();
                await LogToConsole("LoadDashboardData END");
            }
        }
        
        private async Task OnComparisonDateChanged()
        {
            changeEventCounter++;
            await LogToConsole($"🔥 OnComparisonDateChanged FIRED! Count: {changeEventCounter}, New value: {_selectedComparisonDate}");

            // This is called after _selectedComparisonDate is updated by @bind-Value
            if (isAdministrator == false) {
                await LogToConsole($"Loading dashboard data for date: {_selectedComparisonDate}");
                var loadResult = await LoadDashboardData();
                if (loadResult.IsFailed) {
                    NavigationManager.NavigateTo("/error", replace: true);
                    return;
                }
                StateHasChanged();
                await LogToConsole("Dashboard data reload complete");
            }
            else {
                await LogToConsole("User is administrator - skipping data reload");
            }
        }

        private async Task LogToConsole(string message)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("console.log", $"[Home.razor {DateTime.Now:HH:mm:ss.fff}] {message}");
            }
            catch
            {
                // Ignore JS interop errors during prerendering
            }
        }
    }
}



public static class Helpers
{
    public static string GetComparisonLabel(List<ComparisonDateModel> comparisonDates, string selectedComparisonDate)
    {
        if (comparisonDates == null) return "Comparison";
        if (!DateTime.TryParse(selectedComparisonDate, out var date))
            return "Comparison";
        ComparisonDateModel? comparisonDate = comparisonDates.FirstOrDefault(d => d.Date.Date == date.Date);
        return comparisonDate?.Description ?? date.ToString("MMM dd");
    }

    public static Result<Guid> GetEstateIdFromClaims(this AuthenticationState authState) {
        ClaimsPrincipal user = authState.User;
        Result<Claim>? estateIdClaim = ClaimsHelper.GetUserClaim(user, "estateId");
        if (estateIdClaim.IsFailed)
            return ResultHelpers.CreateFailure(estateIdClaim);
        Guid estateId = Guid.Parse(estateIdClaim.Data.Value);
        return Result.Success(estateId);
    }

    private static decimal GetSalesVariance(TodaysSalesModel todaysSales)
    {
        if (todaysSales == null) return 0;
        if (todaysSales.ComparisonSalesValue == 0)
        {
            // If comparison is 0 and today is 0, no change
            if (todaysSales.TodaysSalesValue == 0) return 0;
            // If comparison is 0 but today has sales, treat as maximum positive change
            return todaysSales.TodaysSalesValue > 0 ? 999m : 0;
        }

        return (todaysSales.TodaysSalesValue - todaysSales.ComparisonSalesValue) / todaysSales.ComparisonSalesValue;
    }

    public static string GetSalesBackgroundClass(TodaysSalesModel todaysSales)
    {
        var variance = GetSalesVariance(todaysSales);
        if (variance < 0) return "bg-red-50"; // Worse
        if (variance == 0) return "bg-blue-50"; // Same
        if (variance > 0 && variance < 0.2m) return "bg-yellow-50"; // Slightly better
        return "bg-green-50"; // Much better
    }

    public static string GetSalesTextClass(TodaysSalesModel todaysSales, double opacity = 1.0)
    {
        var variance = GetSalesVariance(todaysSales);
        if (opacity < 1.0)
        {
            if (variance < 0) return "text-red-700";
            if (variance == 0) return "text-blue-700";
            if (variance > 0 && variance < 0.2m) return "text-yellow-700";
            return "text-green-700";
        }
        if (variance < 0) return "text-red-900";
        if (variance == 0) return "text-blue-900";
        if (variance > 0 && variance < 0.2m) return "text-yellow-900";
        return "text-green-900";
    }

    public static string GetSalesBorderClass(TodaysSalesModel todaysSales)
    {
        var variance = GetSalesVariance(todaysSales);
        if (variance < 0) return "border-red-200";
        if (variance == 0) return "border-blue-200";
        if (variance > 0 && variance < 0.2m) return "border-yellow-200";
        return "border-green-200";
    }

    public static string GetSalesVarianceDisplay(TodaysSalesModel todaysSales)
    {
        var variance = GetSalesVariance(todaysSales);
        // Special case: comparison was 0, now has sales
        if (variance >= 999m) return "NEW";
        var percentageChange = variance * 100;
        var sign = variance > 0 ? "+" : "";
        return $"{sign}{percentageChange:F1}%";
    }

    public static string GetFailedSalesBackgroundClass(TodaysSalesModel todaysSales)
    {
        var variance = GetSalesVariance(todaysSales);
        if (variance < 0) return "bg-green-50"; // Good - fewer failures
        if (variance == 0) return "bg-blue-50"; // Same
        if (variance > 0 && variance < 0.2m) return "bg-yellow-50"; // Slightly worse
        return "bg-red-50"; // Much worse
    }

    public static string GetFailedSalesTextClass(TodaysSalesModel todaysSales, double opacity = 1.0)
    {
        var variance = GetSalesVariance(todaysSales);
        if (opacity < 1.0)
        {
            if (variance < 0) return "text-green-700";
            if (variance == 0) return "text-blue-700";
            if (variance > 0 && variance < 0.2m) return "text-yellow-700";
            return "text-red-700";
        }
        if (variance < 0) return "text-green-900";
        if (variance == 0) return "text-blue-900";
        if (variance > 0 && variance < 0.2m) return "text-yellow-900";
        return "text-red-900";
    }

    public static string GetFailedSalesBorderClass(TodaysSalesModel todaysSales)
    {
        var variance = GetSalesVariance(todaysSales);
        if (variance < 0) return "border-green-200";
        if (variance == 0) return "border-blue-200";
        if (variance > 0 && variance < 0.2m) return "border-yellow-200";
        return "border-red-200";
    }

    public static string GetFailedSalesVarianceDisplay(TodaysSalesModel todaysSales)
    {
        var variance = GetSalesVariance(todaysSales);
        // Special case: comparison was 0, now has failures
        if (variance >= 999m) return "NEW";
        var percentageChange = variance * 100;
        var sign = variance > 0 ? "+" : "";
        return $"{sign}{percentageChange:F1}%";
    }
}