namespace EstateManagementUI.BlazorServer.Permissions;

/// <summary>
/// Represents a role with a set of permissions
/// </summary>
public class Role
{
    public string Name { get; set; } = string.Empty;
    public List<Permission> Permissions { get; set; } = new();

    public Role()
    {
    }

    public Role(string name, List<Permission> permissions)
    {
        Name = name;
        Permissions = permissions;
    }
}
