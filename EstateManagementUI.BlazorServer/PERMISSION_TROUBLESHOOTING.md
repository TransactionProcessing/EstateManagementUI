# Permission System Troubleshooting Guide

## If Permissions Aren't Working Correctly

If you're seeing issues like:
- Administrator can see all menus (should only see Dashboard + Permissions)
- Viewer can see Edit/Delete buttons (should only see View button)
- Role switcher doesn't update buttons immediately

### Quick Fix: Delete permissions.json

The permission system caches role definitions in a `permissions.json` file. If this file was created before the latest role changes, it will contain outdated permissions.

**Solution:**
1. Stop the application
2. Delete the `permissions.json` file from the project root directory:
   ```bash
   rm /path/to/EstateManagementUI.BlazorServer/permissions.json
   ```
3. Restart the application

The file will be recreated with correct permissions on next startup.

### Expected Behavior by Role

**Administrator Role:**
- ✅ Can see: Dashboard menu
- ✅ Can see: Permissions menu item (user group icon)
- ❌ Cannot see: Merchant Management menu
- ❌ Cannot see: Operator Management menu
- ❌ Cannot see: Contract Management menu
- ❌ Cannot see: Estate Management menu
- ❌ Cannot see: File Processing menu

**Estate Role:**
- ✅ Can see: All estate management menus
- ✅ Can see: All Create/Edit/Delete/MakeDeposit buttons
- ❌ Cannot see: Permissions menu item

**Viewer Role:**
- ✅ Can see: All menus (for viewing)
- ✅ Can see: View buttons (eye icon)
- ❌ Cannot see: Create buttons
- ❌ Cannot see: Edit buttons (pencil icon)
- ❌ Cannot see: Delete buttons
- ❌ Cannot see: MakeDeposit buttons (money icon)
- ❌ Cannot see: Permissions menu item

### Debugging

If issues persist after deleting permissions.json:

1. **Check the console logs** when the app starts. You should see:
   ```
   Administrator role initialized with 1 permissions: Dashboard View only
   Estate role initialized with 30 permissions (full access to all sections)
   Viewer role initialized with 6 permissions (View only for all sections)
   ```

2. **Check the role switcher** shows the correct role name in yellow badge

3. **Try a hard browser refresh** (Ctrl+F5 or Cmd+Shift+R) after switching roles

4. **Check browser console** for any JavaScript errors

5. **Check application logs** for permission check messages when navigating pages

### System Role Protection

The system now prevents permissions.json from overwriting the three built-in system roles:
- Administrator
- Estate  
- Viewer

Custom roles you create in the Permission Management UI will still be saved to permissions.json.
