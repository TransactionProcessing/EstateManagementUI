# Role Switcher - Visual Guide

## What It Looks Like

When running in Test Mode, you'll see a yellow badge in the top right header:

```
┌────────────────────────────────────────────────────────────────┐
│ [Dashboard]                    [👤 Estate ▼] [⛶] [Profile]    │
└────────────────────────────────────────────────────────────────┘
                                    ↑
                            Role Switcher Badge
                            (Yellow background)
```

## Dropdown Menu

Click the role badge to see available roles:

```
┌─────────────────────────────────────┐
│ TEST MODE - SWITCH ROLE             │
├─────────────────────────────────────┤
│ ✓ 👤 Estate                         │
│   Full access (Administrator)       │
├─────────────────────────────────────┤
│   👤 Administrator                  │
│   Full access                       │
├─────────────────────────────────────┤
│   👤 Viewer                         │
│   Read-only access to all sections  │
├─────────────────────────────────────┤
│   👤 MerchantManager                │
│   Full merchant access, view else.. │
├─────────────────────────────────────┤
│   👤 OperatorManager                │
│   Full operator access, view else.. │
└─────────────────────────────────────┘
```

## How It Works

### 1. **Default Role** (Estate/Administrator)
- Can see ALL menu items
- Can see ALL action buttons (Create, Edit, Delete, Make Deposit)
- Has access to Permissions management

### 2. **Switch to Viewer**
Click "Viewer" in dropdown:
- ALL menu items still visible
- NO create/edit/delete buttons visible
- ONLY view buttons remain
- NO access to Permissions management (menu item hidden)

### 3. **Switch to MerchantManager**
Click "MerchantManager" in dropdown:
- ALL menu items visible
- In Merchants section: ALL buttons visible (Create, Edit, Make Deposit)
- In other sections: ONLY view buttons
- NO access to Permissions management

### 4. **Switch to OperatorManager**
Click "OperatorManager" in dropdown:
- ALL menu items visible
- In Operators section: ALL buttons visible (Create, Edit)
- In other sections: ONLY view buttons
- NO access to Permissions management

## Visual Changes When Switching Roles

### User Profile Display
The user profile in the header changes with each role:

**Estate Role:**
```
┌─────────────────────┐
│ Estate Manager      │
│ Estate              │
└─────────────────────┘
```

**Viewer Role:**
```
┌─────────────────────┐
│ View Only User      │
│ Viewer              │
└─────────────────────┘
```

**MerchantManager Role:**
```
┌─────────────────────┐
│ Merchant Manager    │
│ MerchantManager     │
└─────────────────────┘
```

## Testing Scenarios

### Test Case 1: Admin Access
1. Start with Estate/Administrator role (default)
2. Navigate to Merchants page
3. ✓ See "Add New Merchant" button
4. ✓ See Edit and Make Deposit buttons on each merchant

### Test Case 2: Viewer Restrictions
1. Click role switcher, select "Viewer"
2. Navigate to Merchants page
3. ✗ NO "Add New Merchant" button
4. ✗ NO Edit or Make Deposit buttons
5. ✓ Only View button visible

### Test Case 3: Menu Visibility
1. Start with Estate role
2. ✓ See "Permissions" menu item in sidebar
3. Switch to Viewer role
4. ✗ "Permissions" menu item disappears
5. Switch back to Estate
6. ✓ "Permissions" menu item reappears

### Test Case 4: Section-Specific Access
1. Switch to MerchantManager role
2. Go to Merchants page → ✓ All buttons visible
3. Go to Operators page → ✗ Only View button
4. Go to Contracts page → ✗ Only View button

## Technical Details

### Role Persistence
- Selected role is stored in session
- Survives page navigation
- Cleared on browser restart
- Can also use `?switchRole=Viewer` in URL

### User Names by Role
Each role has a unique test user name:
- **Estate** → "Estate Manager"
- **Administrator** → "Admin User"
- **Viewer** → "View Only User"
- **MerchantManager** → "Merchant Manager"
- **OperatorManager** → "Operator Manager"

### Test Mode Detection
The role switcher only appears when:
```json
{
  "AppSettings": {
    "TestMode": true
  }
}
```

In production (TestMode: false), the switcher is completely hidden.

## Color Scheme

- **Role Switcher Badge**: Yellow background (`bg-yellow-100`)
- **Current Role Indicator**: Blue checkmark
- **Dropdown**: White background with gray borders
- **Hover State**: Light gray background

## Screenshot Locations

When the app is running:
1. Top right corner - Yellow badge with role name
2. Click badge - Dropdown appears below
3. Current role has blue checkmark
4. Profile dropdown shows role name
5. Sidebar menu adjusts based on permissions
6. Page buttons adjust based on permissions
