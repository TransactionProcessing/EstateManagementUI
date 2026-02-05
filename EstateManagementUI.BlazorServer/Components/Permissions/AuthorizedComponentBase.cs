using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Permissions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using SimpleResults;

public abstract class CustomComponentBase : ComponentBase {

    private Int32? DelayOverride;

    public void SetDelayOverride(Int32 delay) => this.DelayOverride = delay;

    public async Task WaitOnUIRefresh(Int32 delay=2500) {
        Int32 localDelay = delay;
        if (this.DelayOverride.HasValue) {
            localDelay = this.DelayOverride.GetValueOrDefault();
        }
        await Task.Delay(localDelay);
    }

    protected string? successMessage;
    protected string? errorMessage;
    protected string activeTab;
    protected void ClearMessages()
    {
        successMessage = null;
        errorMessage = null;
    }

    protected void SetActiveTab(string tab)
    {
        activeTab = tab;
        ClearMessages();
    }
}

public abstract class AuthorizedComponentBase : CustomComponentBase
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    protected ClaimsPrincipal User { get; private set; } = default!;
    protected AuthenticationState AuthState { get; private set; } = default!;
    
    protected override async Task OnInitializedAsync() {
        
        this.AuthState = await AuthenticationStateTask;
        User = this.AuthState.User;
    }

    [Inject] protected IPermissionService PermissionService { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    protected async Task<Result> RequirePermission(PermissionSection permissionSection, PermissionFunction permissionFunction)
    {
        // Do a permission check here
        Boolean hasPermission = await this.PermissionService.HasPermissionAsync(permissionSection, permissionFunction);
        if (hasPermission == false)
        {
            this.NavigationManager.NavigateToAccessDeniedPage();
            return Result.Unauthorized();
        }

        return Result.Success();
    }

    protected async Task<Guid> GetEstateId() {
        Result<Guid> estateIdResult = this.AuthState.GetEstateIdFromClaims();
        if (estateIdResult.IsFailed) {
            this.NavigationManager.NavigateToErrorPage();
        }
        return estateIdResult.Data;
    }

    protected async Task<Result> OnAfterRender(PermissionSection section,
                                               PermissionFunction function) =>
        await OnAfterRender(section, function, null);

    protected async Task<Result> OnAfterRender(PermissionSection section,
                                               PermissionFunction function,
                                               Func<Task<Result>>? loadFunc)
    {
        Result authResult = await RequirePermission(section, function);
        if (authResult.IsFailed)
            return Result.Failure();

        if (loadFunc == null)
            return Result.Success();

        Result result = await loadFunc();
        if (result.IsFailed)
        {
            this.NavigationManager.NavigateToErrorPage();
            return Result.Failure();
        }
        return Result.Success();
    }
}