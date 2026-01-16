using System.Security.Claims;
using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shared.Exceptions;
using Shared.General;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages;

public partial class Home
{
    // Response code for low credit failures
    private const string LOW_CREDIT_RESPONSE_CODE = "1009";

    private bool isLoading = true;
    private bool isAdministrator;
    private string? errorMessage;

    private MerchantKpiModel? merchantKpi;
    private TodaysSalesModel? todaysSales;
    private TodaysSalesModel? todaysFailedSales;
    private List<ComparisonDateModel>? comparisonDates;
    private List<RecentMerchantsModel>? recentMerchants;
    private string _selectedComparisonDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
    private int changeEventCounter = 0;

    protected override async Task OnInitializedAsync()
    {
        // Keep prerender work minimal. This will still run during prerender,
        // so avoid doing heavy/interactive-only tasks here.
        await this.LogToConsole("OnInitializedAsync (prerender/early) START");
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
        await this.LogToConsole("OnAfterRenderAsync FIRST RENDER (interactive) - performing interactive initialization");

        try
        {
            AuthenticationState authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal user = authState.User;

            // Redirect unauthenticated users to entry screen
            if (!user.Identity?.IsAuthenticated ?? true) {
                NavigationManager.NavigateToEntryPage();
                return;
            }

            // Determine role and admin flag now that we're interactive
            var role = await PermissionService.GetUserRoleAsync();
            this.isAdministrator = role == "Administrator";
            await this.LogToConsole($"User role: {role}, isAdministrator: {this.isAdministrator}");

            CorrelationId correlationId = new CorrelationId(Guid.NewGuid());
            var estateIdResult = authState.GetEstateIdFromClaims();
            if (estateIdResult.IsFailed) {
                this.NavigationManager.NavigateToErrorPage();
                return;
            }

            // Only load dashboard data for non-admins
            if (this.isAdministrator == false) {
                var result = await this.LoadDashboardData(correlationId, estateIdResult.Data);
                if (result.IsFailed) {
                    this.NavigationManager.NavigateToErrorPage();
                    return;
                }
            }
            else {
                this.isLoading = false;
                this.StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = $"Initialization error: {ex.Message}";
            this.isLoading = false;
            this.StateHasChanged();
            this.NavigationManager.NavigateToErrorPage();
            return;
        }

        await base.OnAfterRenderAsync(firstRender);
    }
        
    private async Task<Result> LoadDashboardData(CorrelationId correlationId, Guid estateId)
    {
        await this.LogToConsole($"LoadDashboardData START - selectedDate: {this._selectedComparisonDate}");
        try {
            this.isLoading = true;
            this.errorMessage = null;
            this.StateHasChanged();
                
            // Load comparison dates first (only if not already loaded)
            Result<DateTime> comparisonDateResult = await this.LoadComparisonDates(correlationId, estateId);
            if (comparisonDateResult.IsFailed) {
                return ResultHelpers.CreateFailure(comparisonDateResult);
            }

            // Load all dashboard data in parallel
            var kpiTask = Mediator.Send(new Queries.GetMerchantKpiQuery(correlationId, estateId));
            var salesTask = Mediator.Send(new Queries.GetTodaysSalesQuery(correlationId, estateId, comparisonDateResult.Data));
            var failedSalesTask = Mediator.Send(new Queries.GetTodaysFailedSalesQuery(correlationId, estateId, LOW_CREDIT_RESPONSE_CODE, comparisonDateResult.Data));
            var merchantsTask = Mediator.Send(new Queries.GetRecentMerchantsQuery(correlationId, estateId));

            await Task.WhenAll(kpiTask, salesTask, failedSalesTask, merchantsTask);

            // Process results
            if (kpiTask.Result.IsFailed)
                return ResultHelpers.CreateFailure<BusinessLogic.Models.MerchantKpiModel>(kpiTask.Result);

            this.merchantKpi = ModelFactory.ConvertFrom((BusinessLogic.Models.MerchantKpiModel)kpiTask.Result.Data);

            if (salesTask.Result.IsFailed)
                return ResultHelpers.CreateFailure<BusinessLogic.Models.TodaysSalesModel>(salesTask.Result);

            this.todaysSales = ModelFactory.ConvertFrom((BusinessLogic.Models.TodaysSalesModel)salesTask.Result.Data);

            if (failedSalesTask.Result.IsFailed)
                return ResultHelpers.CreateFailure<BusinessLogic.Models.TodaysSalesModel>(failedSalesTask.Result);

            this.todaysFailedSales = ModelFactory.ConvertFrom((BusinessLogic.Models.TodaysSalesModel)failedSalesTask.Result.Data);

            if (merchantsTask.Result.IsFailed)
                return ResultHelpers.CreateFailure<List<BusinessLogic.Models.RecentMerchantsModel>>(merchantsTask.Result);

            // Note: API returns merchants in creation order (newest first)
            // If ordering is incorrect, would need CreatedDate field in the model
            this.recentMerchants = ModelFactory.ConvertFrom((List<BusinessLogic.Models.RecentMerchantsModel>)merchantsTask.Result.Data);

            return Result.Success();
        }
        catch (Exception ex) {
            this.errorMessage = $"Failed to load dashboard data: {ex.Message}";
            return Result.Failure(ex.GetCombinedExceptionMessages());
        }
        finally {
            this.isLoading = false;
            this.StateHasChanged();
            await this.LogToConsole("LoadDashboardData END");
        }
    }

    private async Task<Result<DateTime>> LoadComparisonDates(CorrelationId correlationId, Guid estateId) {

        Result<List<BusinessLogic.Models.ComparisonDateModel>> comparisonDatesResult;
        if (this.comparisonDates == null || !this.comparisonDates.Any()) {
            comparisonDatesResult = await Mediator.Send(new Queries.GetComparisonDatesQuery(correlationId, estateId));
            if (comparisonDatesResult.IsFailed) {
                return ResultHelpers.CreateFailure(comparisonDatesResult);
            }


            this.comparisonDates = ModelFactory.ConvertFrom(comparisonDatesResult.Data);
            if (this.comparisonDates != null && this.comparisonDates.Any()) {
                // Set default comparison date to the first one only on initial load
                this._selectedComparisonDate = this.comparisonDates.First().Date.ToString("yyyy-MM-dd");
            }
        }

        if (!DateTime.TryParse(this._selectedComparisonDate, out DateTime comparisonDate)) {
            // Fallback to a week ago if parse fails
            comparisonDate = DateTime.Now.AddDays(-7);
        }
        
        return Result.Success(comparisonDate);
    }

    private async Task OnComparisonDateChanged()
    {
        this.changeEventCounter++;
        await this.LogToConsole($"🔥 OnComparisonDateChanged FIRED! Count: {this.changeEventCounter}, New value: {this._selectedComparisonDate}");

        // This is called after _selectedComparisonDate is updated by @bind-Value
        if (this.isAdministrator == false) {

            CorrelationId correlationId = new CorrelationId(Guid.NewGuid());
            AuthenticationState authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            Result<Guid> estateIdResult = authState.GetEstateIdFromClaims();
            if (estateIdResult.IsFailed) {
                this.NavigationManager.NavigateToErrorPage();
                return;
            }

            await this.LogToConsole($"Loading dashboard data for date: {this._selectedComparisonDate}");
            var loadResult = await this.LoadDashboardData(correlationId,estateIdResult.Data);
            if (loadResult.IsFailed) {
                this.NavigationManager.NavigateToErrorPage();
                return;
            }
            this.StateHasChanged();
            await this.LogToConsole("Dashboard data reload complete");
        }
        else {
            await this.LogToConsole("User is administrator - skipping data reload");
        }
    }

    private async Task LogToConsole(string message)
    {
        try
        {
            await JSRuntimeExtensions.InvokeVoidAsync((IJSRuntime)this.JSRuntime, "console.log", $"[Home.razor {DateTime.Now:HH:mm:ss.fff}] {message}");
        }
        catch
        {
            // Ignore JS interop errors during prerendering
        }
    }
}


