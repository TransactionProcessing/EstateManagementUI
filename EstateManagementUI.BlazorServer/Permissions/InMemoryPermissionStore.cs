using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace EstateManagementUI.BlazorServer.Permissions;

/// <summary>
/// In-memory implementation of permission store with JSON-based persistence
/// Provides default roles and allows for customization
/// </summary>
public class InMemoryPermissionStore : IPermissionStore
{
    private readonly Dictionary<string, Role> _roles = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILogger<InMemoryPermissionStore> _logger;
    private readonly string _storageFilePath;
    private readonly SemaphoreSlim _fileLock = new(1, 1);

    public InMemoryPermissionStore(ILogger<InMemoryPermissionStore> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _storageFilePath = Path.Combine(environment.ContentRootPath, "permissions.json");
        
        // Initialize with default roles
        InitializeDefaultRoles();
        
        // Try to load from file if it exists
        LoadFromFile();
    }

    private void InitializeDefaultRoles()
    {
        // Administrator role - ONLY has access to permission management screens (Dashboard view only)
        var administratorPermissions = new List<Permission>
        {
            new Permission(PermissionSection.Dashboard, PermissionFunction.View)
        };
        _roles["Administrator"] = new Role("Administrator", administratorPermissions);
        _logger.LogInformation("Administrator role initialized with {Count} permissions: Dashboard View only", 
            administratorPermissions.Count);

        // Estate role - full access to all estate operations but NOT permission management
        var estatePermissions = new List<Permission>();
        foreach (PermissionSection section in Enum.GetValues(typeof(PermissionSection)))
        {
            foreach (PermissionFunction function in Enum.GetValues(typeof(PermissionFunction)))
            {
                estatePermissions.Add(new Permission(section, function));
            }
        }
        _roles["Estate"] = new Role("Estate", estatePermissions);
        _logger.LogInformation("Estate role initialized with {Count} permissions (full access to all sections)", 
            estatePermissions.Count);

        // Viewer role - can only view, no create/edit/delete
        var viewerPermissions = new List<Permission>();
        foreach (PermissionSection section in Enum.GetValues(typeof(PermissionSection)))
        {
            viewerPermissions.Add(new Permission(section, PermissionFunction.View));
        }
        _roles["Viewer"] = new Role("Viewer", viewerPermissions);
        _logger.LogInformation("Viewer role initialized with {Count} permissions (View only for all sections)", 
            viewerPermissions.Count);

        _logger.LogInformation("Initialized {Count} default roles", _roles.Count);
    }

    private void LoadFromFile()
    {
        try
        {
            if (File.Exists(_storageFilePath))
            {
                var json = File.ReadAllText(_storageFilePath);
                var roles = JsonSerializer.Deserialize<List<Role>>(json);
                
                if (roles != null)
                {
                    // Only load custom roles (not system roles) to preserve default system role permissions
                    var systemRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase) 
                    { 
                        "Administrator", 
                        "Estate", 
                        "Viewer" 
                    };
                    
                    foreach (var role in roles)
                    {
                        // Only load non-system roles from file
                        if (!systemRoles.Contains(role.Name))
                        {
                            _roles[role.Name] = role;
                        }
                    }
                    _logger.LogInformation("Loaded {Count} custom roles from {Path}", 
                        roles.Count(r => !systemRoles.Contains(r.Name)), _storageFilePath);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not load permissions from file {Path}", _storageFilePath);
        }
    }

    private async Task SaveToFileAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            var roles = _roles.Values.ToList();
            var json = JsonSerializer.Serialize(roles, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            await File.WriteAllTextAsync(_storageFilePath, json);
            _logger.LogInformation("Saved {Count} roles to {Path}", roles.Count, _storageFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving permissions to file {Path}", _storageFilePath);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public Task<Role?> GetRoleAsync(string roleName)
    {
        _roles.TryGetValue(roleName, out var role);
        return Task.FromResult(role);
    }

    public Task<List<Role>> GetAllRolesAsync()
    {
        return Task.FromResult(_roles.Values.ToList());
    }

    public async Task SaveRoleAsync(Role role)
    {
        _roles[role.Name] = role;
        await SaveToFileAsync();
    }

    public async Task DeleteRoleAsync(string roleName)
    {
        _roles.Remove(roleName);
        await SaveToFileAsync();
    }
}
