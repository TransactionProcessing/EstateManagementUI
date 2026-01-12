# Permission System Testing Guide

## Overview

The permission system controls access to different sections and functions within the Estate Management UI. Users are assigned roles, and each role has specific permissions.

## Available Roles

### 1. Estate / Administrator
- **Full access to all sections and functions**
- Can view, create, edit, and delete in all areas
- Default role for test mode

### 2. Viewer
- **Read-only access to all sections**
- Can view all areas but cannot create, edit, or delete
- Cannot make deposits

### 3. MerchantManager
- **Full access to Merchant section**: View, Create, Edit, Delete, MakeDeposit
- **Read-only access to other sections**: Dashboard, Estate, Contract, Operator, FileProcessing

### 4. OperatorManager
- **Full access to Operator section**: View, Create, Edit, Delete
- **Read-only access to other sections**: Dashboard, Estate, Contract, Merchant, FileProcessing

## Testing Different Roles

To test the application with different roles in test mode:

1. Open `appsettings.Test.json`
2. Change the `TestUserRole` value:

```json
{
  "AppSettings": {
    "TestUserRole": "Viewer"  // Change to: Estate, Viewer, MerchantManager, or OperatorManager
  }
}
```

3. Restart the application

## Permission Behavior

### Navigation Menu
- Menu items are hidden if the user has no permissions in that section
- The Dashboard is always visible to all authenticated users

### Page Actions
- **Create buttons**: Hidden if user lacks Create permission
- **Edit buttons**: Hidden if user lacks Edit permission
- **Delete buttons**: Hidden if user lacks Delete permission
- **Make Deposit button**: Hidden if user lacks MakeDeposit permission (Merchant section only)
- **View buttons**: Always visible if user has any access to the section

## Testing Scenarios

### Scenario 1: Administrator Access
```json
"TestUserRole": "Estate"
```
**Expected:**
- All menu items visible
- All action buttons (Create, Edit, Delete, Make Deposit) visible
- Full access to all features

### Scenario 2: Viewer Access
```json
"TestUserRole": "Viewer"
```
**Expected:**
- All menu items visible
- No Create, Edit, Delete, or Make Deposit buttons visible
- Only View buttons visible
- Can navigate and view data but cannot modify

### Scenario 3: Merchant Manager Access
```json
"TestUserRole": "MerchantManager"
```
**Expected:**
- All menu items visible
- In Merchants section: All action buttons visible (Create, Edit, Make Deposit)
- In other sections: Only View buttons visible, no create/edit actions

### Scenario 4: Operator Manager Access
```json
"TestUserRole": "OperatorManager"
```
**Expected:**
- All menu items visible
- In Operators section: All action buttons visible (Create, Edit)
- In other sections: Only View buttons visible, no create/edit actions

### Scenario 5: No Operator Access
Create a custom role without Operator permissions to test menu hiding:
```json
"TestUserRole": "Viewer"
```
Then modify the Viewer role permissions (this would require code changes or a role management UI)

## Permission Structure

### Sections
- Dashboard
- Estate
- Merchant
- Contract
- Operator
- FileProcessing

### Functions
- View
- Create
- Edit
- Delete
- MakeDeposit (Merchant section only)

## Implementation Details

### Components
- `RequirePermission`: Wraps buttons/actions that require specific permissions
- `RequireSectionAccess`: Wraps menu items that require access to a section

### Services
- `IPermissionService`: Checks user permissions
- `IPermissionStore`: Stores and retrieves role-based permissions
- `InMemoryPermissionStore`: Default implementation with JSON persistence

### Configuration
Permissions are stored in memory with optional JSON persistence to `permissions.json` in the application root.

## Extending the System

### Adding a New Role
Modify `InMemoryPermissionStore.InitializeDefaultRoles()` to add new roles with custom permission sets.

### Adding a New Section
1. Add to `PermissionSection` enum
2. Update default roles in `InMemoryPermissionStore` as needed
3. Wrap relevant UI components with `RequirePermission` or `RequireSectionAccess`

### Adding a New Function
1. Add to `PermissionFunction` enum
2. Update role permissions as needed
3. Use in `RequirePermission` components to control visibility
