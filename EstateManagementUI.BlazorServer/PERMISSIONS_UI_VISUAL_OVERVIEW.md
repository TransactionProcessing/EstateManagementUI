# Permission Management UI - Visual Overview

## 1. Permissions List Page (`/permissions`)

```
┌─────────────────────────────────────────────────────────────────────┐
│ Permission Management                    [+ Create New Role]         │
├─────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐              │
│  │ Administrator│  │   Viewer     │  │ Custom Role  │              │
│  │  [System]    │  │  [System]    │  │  [Custom]    │              │
│  │              │  │              │  │              │              │
│  │ 30 permissions│  │ 6 permissions│  │ 12 permissions│              │
│  │              │  │              │  │              │              │
│  │ [View] [Edit]│  │ [View] [Edit]│  │ [View][Edit] │              │
│  │    [Delete]  │  │    [Delete]  │  │    [Delete]  │              │
│  └──────────────┘  └──────────────┘  └──────────────┘              │
│                                                                       │
│  ┌──────────────┐  ┌──────────────┐                                │
│  │MerchantMngr  │  │ OperatorMngr │                                │
│  │  [System]    │  │  [System]    │                                │
│  │              │  │              │                                │
│  │ 11 permissions│  │ 11 permissions│                                │
│  │              │  │              │                                │
│  │ [View] [Edit]│  │ [View] [Edit]│                                │
│  │    [Delete]  │  │    [Delete]  │                                │
│  └──────────────┘  └──────────────┘                                │
│                                                                       │
└─────────────────────────────────────────────────────────────────────┘
```

## 2. Role Details Page (`/permissions/{roleName}`)

```
┌─────────────────────────────────────────────────────────────────────┐
│ [← Back]  Role: Administrator                    [Edit Role]         │
├─────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  Role Information                                                     │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │ Role Name: Administrator     Role Type: [System Role]       │   │
│  │ Total Permissions: 30                                        │   │
│  └─────────────────────────────────────────────────────────────┘   │
│                                                                       │
│  Permissions                                                          │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │ Dashboard                                                     │   │
│  │ [View] [Create] [Edit] [Delete] [MakeDeposit]               │   │
│  └─────────────────────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │ Merchant                                                      │   │
│  │ [View] [Create] [Edit] [Delete] [MakeDeposit]               │   │
│  └─────────────────────────────────────────────────────────────┘   │
│  ... (other sections)                                                │
│                                                                       │
│  Permission Matrix                                                    │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │         │ View │Create│ Edit │Delete│MakeDeposit│            │   │
│  ├─────────┼──────┼──────┼──────┼──────┼───────────┤            │   │
│  │Dashboard│  ✓   │  ✓   │  ✓   │  ✓   │    ✓      │            │   │
│  │Estate   │  ✓   │  ✓   │  ✓   │  ✓   │    ✓      │            │   │
│  │Merchant │  ✓   │  ✓   │  ✓   │  ✓   │    ✓      │            │   │
│  │Contract │  ✓   │  ✓   │  ✓   │  ✓   │    ✓      │            │   │
│  │Operator │  ✓   │  ✓   │  ✓   │  ✓   │    ✓      │            │   │
│  │FilePrcss│  ✓   │  ✓   │  ✓   │  ✓   │    ✓      │            │   │
│  └─────────────────────────────────────────────────────────────┘   │
│                                                                       │
└─────────────────────────────────────────────────────────────────────┘
```

## 3. Create/Edit Role Page (`/permissions/new` or `/permissions/{roleName}/edit`)

```
┌─────────────────────────────────────────────────────────────────────┐
│ [← Back]  Create New Role                                            │
├─────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  Role Name *                                                          │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │ Enter role name (e.g., ReportViewer)                        │   │
│  └─────────────────────────────────────────────────────────────┘   │
│                                                                       │
│  Permissions                      [Select All] [Clear All]           │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │ Dashboard                                      Toggle All     │   │
│  │ ☑ View  ☐ Create  ☐ Edit  ☐ Delete  ☐ MakeDeposit         │   │
│  └─────────────────────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │ Estate                                         Toggle All     │   │
│  │ ☑ View  ☐ Create  ☐ Edit  ☐ Delete  ☐ MakeDeposit         │   │
│  └─────────────────────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │ Merchant                                       Toggle All     │   │
│  │ ☑ View  ☑ Create  ☑ Edit  ☑ Delete  ☑ MakeDeposit         │   │
│  └─────────────────────────────────────────────────────────────┘   │
│  ... (other sections)                                                │
│                                                                       │
│                                    [Cancel] [Create Role]            │
│                                                                       │
└─────────────────────────────────────────────────────────────────────┘
```

## 4. Delete Confirmation Modal

```
                    ┌─────────────────────────┐
                    │         ⚠️               │
                    │   Delete Role            │
                    │                         │
                    │  Are you sure you want  │
                    │  to delete the role     │
                    │  "CustomRole"?          │
                    │                         │
                    │  This action cannot     │
                    │  be undone.             │
                    │                         │
                    │  [Delete]  [Cancel]     │
                    └─────────────────────────┘
```

## Key Features Implemented

### Navigation
- New "Permissions" menu item in sidebar (visible only to Administrators/Estate users)
- Icon: User group icon
- Located below File Processing menu item

### List View Features
- Grid layout showing role cards
- Visual distinction between System and Custom roles
- Permission counts displayed
- Quick action buttons (View, Edit, Delete)
- "Create New Role" button in header

### Detail View Features
- Role information section
- Permissions grouped by section with badges
- Permission matrix with visual checkmarks
- Back navigation
- Edit button (for custom roles)

### Create/Edit Features
- Form with role name input
- Interactive checkboxes for each permission
- Section-level toggle buttons
- Bulk select/deselect options
- Real-time validation
- Save/Cancel buttons
- Loading states during save

### Protection Features
- System roles show "System" badge
- System roles cannot be edited
- System roles cannot be deleted
- Editing system role redirects to list
- Delete requires confirmation

## Access Control
- Menu item only visible to Administrator/Estate roles
- All pages protected (would show 403/redirect for unauthorized users)
- Create/Edit/Delete restricted to authorized users

## Color Scheme
- Primary blue for main actions
- Gray for system roles
- Blue for custom roles
- Green for success indicators
- Red for delete actions
- Clean, modern design matching existing UI
