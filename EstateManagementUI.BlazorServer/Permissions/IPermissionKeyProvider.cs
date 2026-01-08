namespace EstateManagementUI.BlazorServer.Permissions;

/// <summary>
/// Provides a key that changes when authentication state changes.
/// Used to force Blazor to recreate permission components.
/// </summary>
public interface IPermissionKeyProvider
{
    string GetKey();
    void RefreshKey();
}
