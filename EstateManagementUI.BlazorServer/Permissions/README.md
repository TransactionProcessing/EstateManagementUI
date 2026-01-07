# Blazor Permission System

## Overview

The Blazor Permission System provides role-based access control (RBAC) for the Estate Management UI. It controls:
- Which navigation menu items users can see
- Which buttons and actions are available on each page
- Access to different sections and functions of the application

## Architecture

### Core Components

#### 1. Permission Models
- **PermissionSection**: Enum defining application sections (Dashboard, Estate, Merchant, Contract, Operator, FileProcessing)
- **PermissionFunction**: Enum defining actions (View, Create, Edit, Delete, MakeDeposit)
- **Permission**: Combines a section and function into a single permission
- **Role**: Named collection of permissions

#### 2. Services
- **IPermissionService**: Interface for checking user permissions
- **PermissionService**: Implementation that checks permissions based on authenticated user's role
- **IPermissionStore**: Interface for storing and retrieving roles and permissions
- **InMemoryPermissionStore**: Default implementation with JSON file persistence

#### 3. Blazor Components
- **RequirePermission**: Conditional rendering component for specific permissions
- **RequireSectionAccess**: Conditional rendering component for section-level access

## Default Roles

### Administrator / Estate
Full access to all sections and functions. This is the superuser role.

**Permissions**: All sections × All functions

### Viewer
Read-only access to all sections. Cannot create, edit, or delete.

**Permissions**: All sections × View only

### MerchantManager
Full access to merchant management, read-only access to other sections.

**Permissions**:
- Merchant: View, Create, Edit, Delete, MakeDeposit
- Other sections: View only

### OperatorManager
Full access to operator management, read-only access to other sections.

**Permissions**:
- Operator: View, Create, Edit, Delete
- Other sections: View only

## Usage

### In Razor Components

#### Hide/Show Based on Specific Permission
```razor
@using EstateManagementUI.BlazorServer.Permissions

<RequirePermission Section="PermissionSection.Merchant" Function="PermissionFunction.Create">
    <button class="btn btn-primary" @onclick="CreateMerchant">
        Create Merchant
    </button>
</RequirePermission>
```

#### Hide/Show Navigation Menu Items
```razor
<RequireSectionAccess Section="PermissionSection.Operator">
    <NavLink href="operators">Operator Management</NavLink>
</RequireSectionAccess>
```

#### In Code-Behind
```csharp
@inject IPermissionService PermissionService

@code {
    private bool canEdit;
    
    protected override async Task OnInitializedAsync()
    {
        canEdit = await PermissionService.HasPermissionAsync(
            PermissionSection.Merchant, 
            PermissionFunction.Edit
        );
    }
}
```

### Service Registration

The permission services are registered in `Program.cs`:

```csharp
// Register Permission services
builder.Services.AddSingleton<IPermissionStore, InMemoryPermissionStore>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
```

## Configuration

### Storage

Permissions are stored in memory and optionally persisted to `permissions.json` in the application root. The file is automatically created and updated when roles are modified.

### Test Mode

In test mode, you can configure the user's role in `appsettings.Test.json`:

```json
{
  "AppSettings": {
    "TestMode": true,
    "TestUserRole": "Viewer"  // or Estate, MerchantManager, OperatorManager
  }
}
```

## Extending the System

### Adding a New Section

1. Add to `PermissionSection` enum:
```csharp
public enum PermissionSection
{
    Dashboard,
    Estate,
    Merchant,
    Contract,
    Operator,
    FileProcessing,
    Reports  // New section
}
```

2. Update default roles in `InMemoryPermissionStore.InitializeDefaultRoles()` to include permissions for the new section.

3. Wrap UI elements with permission components:
```razor
<RequireSectionAccess Section="PermissionSection.Reports">
    <NavLink href="reports">Reports</NavLink>
</RequireSectionAccess>
```

### Adding a New Function

1. Add to `PermissionFunction` enum:
```csharp
public enum PermissionFunction
{
    View,
    Create,
    Edit,
    Delete,
    MakeDeposit,
    Export  // New function
}
```

2. Update role permissions as needed in `InMemoryPermissionStore`.

3. Use in UI:
```razor
<RequirePermission Section="PermissionSection.Merchant" Function="PermissionFunction.Export">
    <button @onclick="ExportData">Export</button>
</RequirePermission>
```

### Creating a Custom Role

Modify `InMemoryPermissionStore.InitializeDefaultRoles()`:

```csharp
// Custom role: Finance Manager
var financeManagerPermissions = new List<Permission>
{
    // Full merchant access for deposits
    new Permission(PermissionSection.Merchant, PermissionFunction.View),
    new Permission(PermissionSection.Merchant, PermissionFunction.MakeDeposit),
    
    // View-only access to reports
    new Permission(PermissionSection.FileProcessing, PermissionFunction.View),
    
    // No other access
};
_roles["FinanceManager"] = new Role("FinanceManager", financeManagerPermissions);
```

## Security Considerations

1. **Server-Side Enforcement**: While UI elements are hidden based on permissions, critical operations should also verify permissions on the server side.

2. **Role Claims**: The system relies on role claims in the authenticated user's identity. Ensure your authentication system sets the `ClaimTypes.Role` claim correctly.

3. **Permission Bypass**: In the old BusinessLogic permission system, there was a permission bypass flag. The new Blazor system does not include this feature for security reasons.

4. **Persistence**: The `permissions.json` file should be protected with appropriate file system permissions in production.

## Migration from Old System

The old application used SQLite database for permissions. The new system uses:
- In-memory storage with optional JSON persistence
- Enum-based sections and functions (more type-safe)
- Component-based UI protection
- Simplified role management

Key differences:
- **Old**: Database-driven, string-based section/function names
- **New**: Code-driven, enum-based, strongly-typed
- **Old**: Separate permission checking in controllers/handlers
- **New**: Integrated with Blazor component lifecycle

## Testing

See [PERMISSIONS_TESTING.md](PERMISSIONS_TESTING.md) for detailed testing instructions.

Unit tests are provided in `PermissionServiceTests.cs`:
- Permission checking logic
- Role-based access control
- Authentication state handling
- Permission store operations

## Troubleshooting

### Menu items not hiding
- Verify user has correct role claim
- Check role name matches exactly (case-sensitive)
- Ensure permission service is registered
- Check browser console for errors

### Buttons still visible when they shouldn't be
- Verify `RequirePermission` component wraps the button
- Check Section and Function parameters are correct
- Ensure user's role has/doesn't have the permission

### Changes to permissions not taking effect
- Restart the application (permissions are loaded at startup)
- Delete `permissions.json` to reset to defaults
- Verify `InMemoryPermissionStore` is registered as singleton

## Future Enhancements

Potential improvements:
1. **Permission Management UI**: Admin interface to manage roles and permissions
2. **Database Storage**: Option to store permissions in database instead of JSON
3. **Hierarchical Roles**: Support for role inheritance
4. **Dynamic Permissions**: Load permissions from configuration without code changes
5. **Audit Logging**: Track permission checks and access attempts
6. **Permission Caching**: Cache permission checks for better performance
