# .NET 11 and UnionResults migration scope

This migration covers the active EstateManagementUI application, business logic, and test projects:

- `EstateManagementUI.BlazorServer`
- `EstateManagmentUI.BusinessLogic`
- `EstateManagementUI.BlazorServer.Tests`
- `EstateManagementUI.IntegrationTests`
- `EstateManagementUI.BlazorFastIntegrationTests`

Archived projects under `Archive/` are excluded. They are retained as historical snapshots and are not part of the active solution migration.

The result API and client package migration must use the updated packages from Shared, FileProcessor, SecurityService, TransactionProcessor, and EstateReportingAPI. Update this note if a package dependency changes the migration scope.
