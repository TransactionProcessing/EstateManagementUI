using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Permissions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using EstateManagementUI.BlazorServer.Components.Common;
using SimpleResults;

public abstract class AuthorizedComponentBase : CustomComponentBase
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    protected ClaimsPrincipal User { get; private set; } = default!;
    protected AuthenticationState AuthState { get; private set; } = default!;
    
    protected override async Task OnInitializedAsync() {

    }

    [Inject] protected IPermissionService PermissionService { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    protected async Task<Result> RequirePermission(PermissionSection permissionSection, PermissionFunction permissionFunction)
    {
        // Do a permission check here
        Boolean hasPermission = await this.PermissionService.HasPermissionAsync(permissionSection, permissionFunction);
        Console.WriteLine($"[AuthorizedComponentBase {DateTime.Now:HH:mm:ss.fff}] RequirePermission section={permissionSection}, function={permissionFunction}, result={hasPermission}");
        if (hasPermission == false)
        {
            Console.WriteLine($"[AuthorizedComponentBase {DateTime.Now:HH:mm:ss.fff}] Permission denied - navigating to access denied page");
            this.NavigationManager.NavigateToAccessDeniedPage();
            return Result.Unauthorized();
        }

        return Result.Success();
    }

    protected async Task<Guid> GetEstateId() {
        Result<Guid> estateIdResult = this.AuthState.GetEstateIdFromClaims();
        if (estateIdResult.IsFailed) {
            Console.WriteLine($"[AuthorizedComponentBase {DateTime.Now:HH:mm:ss.fff}] Failed to resolve estateId from claims");
            this.NavigationManager.NavigateToErrorPage();
        }
        else
        {
            Console.WriteLine($"[AuthorizedComponentBase {DateTime.Now:HH:mm:ss.fff}] Resolved estateId={estateIdResult.Data}");
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
        this.AuthState = await AuthenticationStateTask;
        User = this.AuthState.User;
        Console.WriteLine($"[AuthorizedComponentBase {DateTime.Now:HH:mm:ss.fff}] OnAfterRender authState authenticated={User.Identity?.IsAuthenticated}, name={User.Identity?.Name ?? "<null>"}, authType={User.Identity?.AuthenticationType ?? "<null>"}, claims={string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}"))}");

        Result authResult = await RequirePermission(section, function);
        if (authResult.IsFailed)
        {
            Console.WriteLine($"[AuthorizedComponentBase {DateTime.Now:HH:mm:ss.fff}] OnAfterRender stopped because permission check failed");
            return Result.Failure();
        }

        if (loadFunc == null)
        {
            Console.WriteLine($"[AuthorizedComponentBase {DateTime.Now:HH:mm:ss.fff}] OnAfterRender completed without loadFunc");
            return Result.Success();
        }

        Result result = await loadFunc();
        if (result.IsFailed)
        {
            Console.WriteLine($"[AuthorizedComponentBase {DateTime.Now:HH:mm:ss.fff}] OnAfterRender loadFunc failed");
            this.NavigationManager.NavigateToErrorPage();
            return Result.Failure();
        }
        Console.WriteLine($"[AuthorizedComponentBase {DateTime.Now:HH:mm:ss.fff}] OnAfterRender loadFunc succeeded");
        return Result.Success();
    }
}
