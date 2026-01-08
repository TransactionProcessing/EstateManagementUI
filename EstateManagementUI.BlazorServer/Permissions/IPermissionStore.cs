namespace EstateManagementUI.BlazorServer.Permissions;

/// <summary>
/// Interface for storing and retrieving role-based permissions
/// </summary>
public interface IPermissionStore
{
    /// <summary>
    /// Gets a role by name with its permissions
    /// </summary>
    Task<Role?> GetRoleAsync(string roleName);

    /// <summary>
    /// Gets all available roles
    /// </summary>
    Task<List<Role>> GetAllRolesAsync();

    /// <summary>
    /// Saves a role with its permissions
    /// </summary>
    Task SaveRoleAsync(Role role);

    /// <summary>
    /// Deletes a role
    /// </summary>
    Task DeleteRoleAsync(string roleName);
}
