namespace EstateManagementUI.BlazorServer.Permissions;

/// <summary>
/// Represents a permission that grants access to a specific function within a section
/// </summary>
public class Permission
{
    public PermissionSection Section { get; set; }
    public PermissionFunction Function { get; set; }

    public Permission(PermissionSection section, PermissionFunction function)
    {
        Section = section;
        Function = function;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Permission other)
        {
            return Section == other.Section && Function == other.Function;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Section, Function);
    }

    public override string ToString()
    {
        return $"{Section}.{Function}";
    }
}
