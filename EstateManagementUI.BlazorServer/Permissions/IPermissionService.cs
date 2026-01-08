namespace EstateManagementUI.BlazorServer.Permissions;

/// <summary>
/// Service for checking user permissions
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Checks if the current user has a specific permission
    /// </summary>
    Task<bool> HasPermissionAsync(PermissionSection section, PermissionFunction function);

    /// <summary>
    /// Checks if the current user has access to view a section (has any permission in that section)
    /// </summary>
    Task<bool> HasSectionAccessAsync(PermissionSection section);

    /// <summary>
    /// Gets all permissions for the current user
    /// </summary>
    Task<List<Permission>> GetUserPermissionsAsync();

    /// <summary>
    /// Gets the current user's role name
    /// </summary>
    Task<string?> GetUserRoleAsync();
}
