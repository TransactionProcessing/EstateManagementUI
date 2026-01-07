# Manual Verification Checklist for Permission System

## Prerequisites
- Application running in Test Mode
- Access to modify `appsettings.Test.json`

## Test Scenarios

### Test 1: Administrator/Estate Role - Full Access
**Setup:**
```json
"TestUserRole": "Estate"
```

**Verification Steps:**
1. Navigate to application homepage
2. Check sidebar navigation menu
   - [ ] All menu items visible (Dashboard, Estate Management, Merchant Management, Contract Management, Operator Management, File Processing)
3. Navigate to Merchants page
   - [ ] "Add New Merchant" button visible
   - [ ] Each merchant has View, Edit, and Make Deposit buttons visible
4. Navigate to Operators page
   - [ ] "Add New Operator" button visible
   - [ ] Each operator has View and Edit buttons visible
5. Navigate to Contracts page
   - [ ] "Add New Contract" button visible
   - [ ] Each contract has View and Edit buttons visible
6. Navigate to File Processing page
   - [ ] "Upload File" button visible

**Expected Result:** All buttons and menu items should be visible

---

### Test 2: Viewer Role - Read-Only Access
**Setup:**
```json
"TestUserRole": "Viewer"
```

**Verification Steps:**
1. Navigate to application homepage
2. Check sidebar navigation menu
   - [ ] All menu items visible
3. Navigate to Merchants page
   - [ ] "Add New Merchant" button NOT visible
   - [ ] Each merchant has ONLY View button (no Edit or Make Deposit buttons)
4. Navigate to Operators page
   - [ ] "Add New Operator" button NOT visible
   - [ ] Each operator has ONLY View button (no Edit button)
5. Navigate to Contracts page
   - [ ] "Add New Contract" button NOT visible
   - [ ] Each contract has ONLY View button (no Edit button)
6. Navigate to File Processing page
   - [ ] "Upload File" button NOT visible

**Expected Result:** Only View buttons should be visible, no create/edit/deposit actions

---

### Test 3: Merchant Manager Role - Merchant Full Access
**Setup:**
```json
"TestUserRole": "MerchantManager"
```

**Verification Steps:**
1. Navigate to application homepage
2. Check sidebar navigation menu
   - [ ] All menu items visible
3. Navigate to Merchants page
   - [ ] "Add New Merchant" button visible
   - [ ] Each merchant has View, Edit, and Make Deposit buttons visible
4. Navigate to Operators page
   - [ ] "Add New Operator" button NOT visible
   - [ ] Each operator has ONLY View button
5. Navigate to Contracts page
   - [ ] "Add New Contract" button NOT visible
   - [ ] Each contract has ONLY View button
6. Navigate to File Processing page
   - [ ] "Upload File" button NOT visible

**Expected Result:** Full access to Merchants, read-only access to other sections

---

### Test 4: Operator Manager Role - Operator Full Access
**Setup:**
```json
"TestUserRole": "OperatorManager"
```

**Verification Steps:**
1. Navigate to application homepage
2. Check sidebar navigation menu
   - [ ] All menu items visible
3. Navigate to Merchants page
   - [ ] "Add New Merchant" button NOT visible
   - [ ] Each merchant has ONLY View button (no Edit or Make Deposit buttons)
4. Navigate to Operators page
   - [ ] "Add New Operator" button visible
   - [ ] Each operator has View and Edit buttons visible
5. Navigate to Contracts page
   - [ ] "Add New Contract" button NOT visible
   - [ ] Each contract has ONLY View button
6. Navigate to File Processing page
   - [ ] "Upload File" button NOT visible

**Expected Result:** Full access to Operators, read-only access to other sections

---

## Browser DevTools Verification

For each test scenario, also check in browser DevTools:

1. Open Developer Console (F12)
2. Navigate to different pages
3. Verify no console errors related to permissions
4. Check that hidden buttons are truly not rendered (not just hidden with CSS)

### How to Verify Elements Not Rendered:
1. Right-click on the area where a button should be
2. Select "Inspect Element"
3. Verify the button element is completely absent from the DOM (not just `display: none`)

---

## Role Configuration Testing

### Verify Role Configuration Loads Correctly:
1. With application running as "Estate" role
2. Open browser console
3. Look for log message: "Getting permissions for role: Estate"
4. Verify no permission-related errors

### Verify JSON Persistence (Optional):
1. Run application with "Estate" role
2. Stop application
3. Check for `permissions.json` file in application root
4. Verify file contains role definitions
5. Modify a role's permissions in the file
6. Restart application
7. Verify changes are applied

---

## Edge Cases to Test

### Test 5: Empty Role (Security Test)
**Setup:** Manually modify TestAuthenticationHandler to use non-existent role
```csharp
new Claim(ClaimTypes.Role, "NonExistentRole")
```

**Expected Result:**
- All menu items may show (section-level)
- No action buttons should be visible
- No console errors

### Test 6: No Authentication
**Setup:** Modify TestAuthenticationHandler to return non-authenticated user

**Expected Result:**
- Redirect to login or show appropriate message
- No permission-related errors

---

## Performance Check

Test with multiple rapid navigation actions:
1. Quickly navigate between pages
2. Verify no lag or delays
3. Check for any permission caching issues
4. Confirm buttons show/hide instantly

---

## Accessibility Check

1. Use screen reader to verify:
   - Hidden buttons are not announced
   - Visible buttons have appropriate labels
2. Keyboard navigation works correctly
3. Focus management is correct when buttons are hidden

---

## Completion Criteria

✅ All test scenarios pass
✅ No console errors during testing
✅ Hidden elements are not rendered (not just CSS hidden)
✅ Role switching works correctly after application restart
✅ Performance is acceptable
✅ No accessibility issues

---

## Notes

- Take screenshots of each test scenario for documentation
- Record any issues or unexpected behavior
- Test on multiple browsers if possible (Chrome, Firefox, Edge)
- Verify mobile responsive behavior with different roles
