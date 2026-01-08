using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace EstateManagementUI.BlazorServer.Permissions;

/// <summary>
/// Implementation of permission service that uses role-based permissions
/// </summary>
public class PermissionService : IPermissionService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IPermissionStore _permissionStore;
    private readonly ILogger<PermissionService> _logger;

    public PermissionService(
        AuthenticationStateProvider authenticationStateProvider,
        IPermissionStore permissionStore,
        ILogger<PermissionService> logger)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _permissionStore = permissionStore;
        _logger = logger;
    }

    public async Task<bool> HasPermissionAsync(PermissionSection section, PermissionFunction function)
    {
        try
        {
            var permissions = await GetUserPermissionsAsync();
            var hasPermission = permissions.Any(p => p.Section == section && p.Function == function);
            _logger.LogInformation("Checking permission: Section={Section}, Function={Function}, Result={Result}, TotalPermissions={Count}", 
                section, function, hasPermission, permissions.Count);
            return hasPermission;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking permission for section {Section}, function {Function}", section, function);
            return false;
        }
    }

    public async Task<bool> HasSectionAccessAsync(PermissionSection section)
    {
        try
        {
            var permissions = await GetUserPermissionsAsync();
            return permissions.Any(p => p.Section == section);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking section access for {Section}", section);
            return false;
        }
    }

    public async Task<List<Permission>> GetUserPermissionsAsync()
    {
        try
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                _logger.LogWarning("User is not authenticated");
                return new List<Permission>();
            }

            // Get the user's role from claims
            var roleClaim = user.FindFirst(ClaimTypes.Role) ?? user.FindFirst("role");
            if (roleClaim == null)
            {
                _logger.LogWarning("User has no role claim");
                return new List<Permission>();
            }

            var roleName = roleClaim.Value;
            _logger.LogInformation("Getting permissions for role: {Role}", roleName);

            // Get permissions from the store
            var role = await _permissionStore.GetRoleAsync(roleName);
            if (role == null)
            {
                _logger.LogWarning("Role {Role} not found in permission store", roleName);
                return new List<Permission>();
            }

            _logger.LogInformation("Role {Role} has {Count} permissions", roleName, role.Permissions.Count);
            return role.Permissions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user permissions");
            return new List<Permission>();
        }
    }

    public async Task<string?> GetUserRoleAsync()
    {
        try
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            var roleClaim = user.FindFirst(ClaimTypes.Role) ?? user.FindFirst("role");
            return roleClaim?.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user role");
            return null;
        }
    }
}
