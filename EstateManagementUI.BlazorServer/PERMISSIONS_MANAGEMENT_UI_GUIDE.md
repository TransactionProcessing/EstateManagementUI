# Permission Management UI Guide

## Overview

The permission management screens allow administrators to view, create, edit, and delete user roles and their associated permissions.

## Accessing Permission Management

1. Log in as a user with **Administrator** or **Estate** role
2. Look for **"Permissions"** in the sidebar navigation menu
3. Click to access the permission management area

**Note**: The Permissions menu item is only visible to Administrator and Estate users.

## Features

### 1. View All Roles (`/permissions`)

The main permissions page shows all configured roles in a grid layout.

**Features:**
- View all roles (system and custom)
- See permission counts for each role
- Distinguish between system and custom roles
- Quick access to view, edit, or delete actions

**System Roles** (cannot be edited/deleted):
- Administrator
- Estate
- Viewer
- MerchantManager
- OperatorManager

**Actions:**
- **View**: See detailed permissions for any role
- **Edit**: Modify permissions (custom roles only)
- **Delete**: Remove custom roles with confirmation
- **Create New Role**: Add a new custom role

### 2. View Role Details (`/permissions/{roleName}`)

View detailed information about a specific role.

**Features:**
- Role information (name, type, permission count)
- Permissions grouped by section
- Permission matrix showing all section/function combinations
- Visual checkmarks for granted permissions

**Components:**
- **Role Information**: Basic details about the role
- **Permissions by Section**: List of permissions organized by application section
- **Permission Matrix**: Grid showing all possible permissions with visual indicators

### 3. Create New Role (`/permissions/new`)

Create a custom role with selected permissions.

**Steps:**
1. Click "Create New Role" button
2. Enter a unique role name
3. Select permissions:
   - Check individual permissions
   - Use "Toggle All" to select/deselect all permissions in a section
   - Use "Select All" to grant all permissions
   - Use "Clear All" to remove all permissions
4. Click "Create Role" to save

**Validation:**
- Role name is required
- Role name must be unique
- Cannot create a role with a system role name

### 4. Edit Role (`/permissions/{roleName}/edit`)

Modify permissions for existing custom roles.

**Features:**
- Same permission selection interface as create
- Cannot edit system roles
- Cannot change role name
- Permissions are saved immediately

**Restrictions:**
- System roles cannot be edited
- Attempting to edit a system role redirects back to the list

### 5. Delete Role

Remove custom roles from the system.

**Steps:**
1. Click the delete button (trash icon) on a custom role
2. Confirm deletion in the modal dialog
3. Role is permanently removed

**Restrictions:**
- Only custom roles can be deleted
- System roles have no delete button
- Deletion requires confirmation

## Permission Structure

### Sections
- Dashboard
- Estate
- Merchant
- Contract
- Operator
- FileProcessing

### Functions
- View: Read-only access
- Create: Ability to create new items
- Edit: Ability to modify existing items
- Delete: Ability to remove items
- MakeDeposit: Special permission for merchant deposits

## Creating Custom Roles - Best Practices

### Example: Report Viewer Role
A user who only needs to view reports and data:
```
Sections: All sections
Functions: View only
```

### Example: Merchant Administrator
A user who manages merchants but nothing else:
```
Merchant section: View, Create, Edit, Delete, MakeDeposit
Other sections: No permissions
```

### Example: Operator Support
A user who can view everything and edit operators:
```
All sections: View
Operator section: View, Create, Edit, Delete
```

## Tips

1. **Start with a system role**: Use existing roles as templates
2. **Use bulk operations**: Toggle entire sections at once for faster setup
3. **Test thoroughly**: Verify permissions work as expected before deploying
4. **Document custom roles**: Keep a record of what each custom role is for
5. **Review regularly**: Audit roles periodically to ensure they're still needed

## Troubleshooting

### Cannot see Permissions menu
- Verify you're logged in as Administrator or Estate user
- Check user's role claim in authentication

### Cannot edit a role
- System roles (Administrator, Estate, Viewer, MerchantManager, OperatorManager) cannot be edited
- Create a new custom role instead

### Cannot delete a role
- System roles cannot be deleted
- Only custom roles can be removed

### Changes not taking effect
- Permissions are stored in `permissions.json` file
- Changes are immediate but users may need to log out/in
- Check file permissions if save fails

## Security Considerations

1. **Access Control**: Only Administrator/Estate users can manage permissions
2. **System Role Protection**: Built-in roles are protected from modification
3. **Validation**: Role names must be unique and non-empty
4. **Confirmation**: Destructive actions (delete) require confirmation
5. **Audit Trail**: Consider implementing logging for permission changes (future enhancement)

## File Storage

Permissions are stored in `permissions.json` in the application root directory:
- File is created automatically on first save
- JSON format for easy reading/backup
- Can be version controlled for deployment

## Future Enhancements

Potential improvements:
- Permission history/audit log
- Bulk role operations
- Import/export roles
- Role templates
- Permission inheritance
- User-to-role assignment UI
