namespace EstateManagementUI.BlazorServer.Permissions;

/// <summary>
/// Provides a key that changes when authentication state changes.
/// Used to force Blazor to recreate permission components.
/// </summary>
public class PermissionKeyProvider : IPermissionKeyProvider
{
    private string _key = Guid.NewGuid().ToString();

    public string GetKey() => _key;

    public void RefreshKey()
    {
        _key = Guid.NewGuid().ToString();
    }
}
