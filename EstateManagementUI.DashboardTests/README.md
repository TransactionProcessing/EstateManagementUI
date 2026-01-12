# Estate Management UI - Dashboard Integration Tests

This project contains integration tests for the Dashboard functionality of the Estate Management Blazor Server application, using Reqnroll (SpecFlow successor), Playwright, and Shouldly.

## Project Structure

```
EstateManagementUI.DashboardTests/
├── Features/              # Reqnroll feature files (Gherkin scenarios)
│   └── Dashboard.feature  # Dashboard test scenarios for all roles
├── Steps/                 # Step definitions (links features to code)
│   └── DashboardSteps.cs  # Dashboard step implementations
├── Hooks/                 # Test lifecycle hooks
│   └── BrowserHooks.cs    # Playwright browser management
├── Common/                # Helper classes and utilities
│   └── DashboardPageHelper.cs  # Page object for Dashboard interactions
└── appsettings.json       # Test configuration and hardcoded test data
```

## Key Components

### 1. Feature File (`Features/Dashboard.feature`)
- Defines test scenarios in Gherkin syntax (Given/When/Then)
- Covers three user roles: Administrator, Estate, and Viewer
- Tests dashboard visibility and data display based on role permissions
- Uses hardcoded test data values from the application

### 2. Hooks File (`Hooks/BrowserHooks.cs`)
- Manages Playwright browser lifecycle
- `BeforeTestRun`: Installs Playwright browsers and initializes Playwright
- `BeforeScenario`: Creates a new browser page for each test scenario
- `AfterScenario`: Cleans up and takes screenshots on test failures
- `AfterTestRun`: Disposes Playwright resources

### 3. Step Definitions (`Steps/DashboardSteps.cs`)
- Links Gherkin steps from feature files to C# code
- Uses DashboardPageHelper to interact with the browser
- Implements Given/When/Then steps for all Dashboard scenarios

### 4. Page Helper (`Common/DashboardPageHelper.cs`)
- Encapsulates all Dashboard page interactions using Playwright
- Provides methods for navigation, verification, and interaction
- Uses Shouldly for assertions

## Test Data

The tests are designed to assert against hardcoded test data in the application's `TestMediatorService.cs`:

### Merchant KPIs
- Merchants with Sales in Last Hour: **45**
- Merchants with No Sales Today: **12**
- Merchants with No Sales in Last 7 Days: **5**

### Today's Sales
- Transaction Count: **523**
- Sales Value: **$145,000.00**

### Failed Sales (Low Credit)
- Transaction Count: **18**
- Sales Value: **$850.00**

These values are defined in the application's `TestMediatorService` and can be updated when the test data is changed.

## User Roles Tested

### Administrator Role
- Can only view the Dashboard with a welcome message
- No access to merchant KPIs, sales data, or reports
- Limited to permission management functions

### Estate Role
- Full access to all dashboard features
- Can view all merchant KPIs
- Can view sales data and comparisons
- Can view recently created merchants
- Has full CRUD permissions across the application

### Viewer Role
- View-only access to all dashboard features
- Can view all merchant KPIs
- Can view sales data and comparisons
- Can view recently created merchants
- Cannot create, edit, or delete any data

## Running the Tests

The tests are designed to run against a running instance of the Estate Management UI application. The application startup and configuration will be handled separately.

### Prerequisites
1. .NET 10 SDK
2. Playwright browsers (automatically installed by the test hooks)

### Configuration
Set the following environment variables before running tests:
- `APP_URL`: Base URL of the application (default: `https://localhost:5001`)
- `Browser`: Browser to use - Chrome, Firefox, or WebKit (default: Chrome)
- `IsCI`: Set to "true" to run in headless mode (default: false)

### Execute Tests
```bash
dotnet test EstateManagementUI.DashboardTests.csproj
```

### Filter by Role
```bash
# Run only Administrator role tests
dotnet test --filter "Category=AdminRole"

# Run only Estate role tests
dotnet test --filter "Category=EstateRole"

# Run only Viewer role tests
dotnet test --filter "Category=ViewerRole"
```

## Notes

- **Application Startup**: The tests assume the application is already running. Application startup logic will be implemented separately.
- **Authentication**: The authentication/role setup is a placeholder. The actual implementation will depend on how the application is configured for testing.
- **Test Data**: All assertions are based on hardcoded values in the application's `TestMediatorService`. Update the feature file and `appsettings.json` if test data changes.
- **Screenshots**: On test failure, screenshots are automatically saved to the test output directory with timestamps.

## Dependencies

- **Reqnroll 3.2.1**: BDD framework (SpecFlow successor)
- **Playwright 1.49.0**: Browser automation
- **Shouldly 4.3.0**: Assertion library with readable error messages
- **NUnit 4.4.0**: Test framework
- **.NET 10**: Target framework

## Future Enhancements

When application startup is implemented:
1. Add Docker container management for the application
2. Add test data setup/teardown
3. Add user authentication simulation
4. Add role switching capabilities
5. Add test reporting and metrics
