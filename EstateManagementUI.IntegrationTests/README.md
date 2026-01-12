# Estate Management UI - Integration Tests

This project contains integration tests for the Estate Management Blazor Server application, using Reqnroll (SpecFlow successor), Playwright, and Shouldly.

## Project Structure

```
EstateManagementUI.IntegrationTests/
├── Features/                          # Reqnroll feature files (Gherkin scenarios)
│   ├── Dashboard.feature              # Dashboard test scenarios for all roles
│   ├── EstateManagement.feature       # Estate Management test scenarios
│   ├── MerchantManagement.feature     # Merchant Management test scenarios
│   ├── ContractManagement.feature     # Contract Management test scenarios
│   └── OperatorManagement.feature     # Operator Management test scenarios
├── Steps/                             # Step definitions (links features to code)
│   ├── DashboardSteps.cs              # Dashboard step implementations
│   ├── EstateManagementSteps.cs       # Estate Management step implementations
│   ├── MerchantManagementSteps.cs     # Merchant Management step implementations
│   ├── ContractManagementSteps.cs     # Contract Management step implementations
│   └── OperatorManagementSteps.cs     # Operator Management step implementations
├── Hooks/                             # Test lifecycle hooks
│   └── BrowserHooks.cs                # Playwright browser management
├── Common/                            # Helper classes and utilities
│   ├── DashboardPageHelper.cs         # Page object for Dashboard interactions
│   ├── EstateManagementPageHelper.cs  # Page object for Estate Management interactions
│   ├── MerchantManagementPageHelper.cs # Page object for Merchant Management interactions
│   ├── ContractManagementPageHelper.cs # Page object for Contract Management interactions
│   └── OperatorManagementPageHelper.cs # Page object for Operator Management interactions
└── appsettings.json                   # Test configuration and hardcoded test data
```

## Key Components

### 1. Feature Files
Feature files define test scenarios in Gherkin syntax (Given/When/Then) and cover three user roles: Administrator, Estate, and Viewer.

#### `Features/Dashboard.feature`
- Tests dashboard visibility and data display based on role permissions
- Uses hardcoded test data values from the application

#### `Features/EstateManagement.feature`
- Tests Estate Management functionality including estate details, operators, and contracts
- Tests role-based access controls

#### `Features/MerchantManagement.feature`
- Tests Merchant Management functionality including merchant list, details, and CRUD operations
- Covers merchant viewing, creation, editing, and deposit operations
- Tests role-based permissions for different operations

#### `Features/ContractManagement.feature`
- Tests Contract Management functionality including contract list and details
- Tests role-based access controls for contracts

#### `Features/OperatorManagement.feature`
- Tests Operator Management functionality including operator list, details, and CRUD operations
- Covers operator viewing, creation, and editing
- Tests role-based permissions for different operations
- Uses hardcoded test data (Safaricom, Voucher)

### 2. Hooks File (`Hooks/BrowserHooks.cs`)
- Manages Playwright browser lifecycle
- `BeforeTestRun`: Installs Playwright browsers and initializes Playwright
- `BeforeScenario`: Creates a new browser page for each test scenario
- `AfterScenario`: Cleans up and takes screenshots on test failures
- `AfterTestRun`: Disposes Playwright resources

### 3. Step Definitions
Step definitions link Gherkin steps from feature files to C# code and use Page Helpers to interact with the browser.

- `Steps/DashboardSteps.cs` - Dashboard scenarios
- `Steps/EstateManagementSteps.cs` - Estate Management scenarios
- `Steps/MerchantManagementSteps.cs` - Merchant Management scenarios
- `Steps/ContractManagementSteps.cs` - Contract Management scenarios
- `Steps/OperatorManagementSteps.cs` - Operator Management scenarios

### 4. Page Helpers
Page Helpers encapsulate all page interactions using Playwright and use Shouldly for assertions.

- `Common/DashboardPageHelper.cs` - Dashboard page interactions
- `Common/EstateManagementPageHelper.cs` - Estate Management page interactions
- `Common/MerchantManagementPageHelper.cs` - Merchant Management page interactions
- `Common/ContractManagementPageHelper.cs` - Contract Management page interactions
- `Common/OperatorManagementPageHelper.cs` - Operator Management page interactions

## Test Data

The tests are designed to assert against hardcoded test data in the application's `StubbedMediatorService.cs`:

### Dashboard Data

#### Merchant KPIs
- Merchants with Sales in Last Hour: **45**
- Merchants with No Sales Today: **12**
- Merchants with No Sales in Last 7 Days: **5**

#### Today's Sales
- Transaction Count: **523**
- Sales Value: **$145,000.00**

#### Failed Sales (Low Credit)
- Transaction Count: **15**
- Sales Value: **$850.00**

### Estate Management Data

#### Estate Details
- Estate Name: **Test Estate**
- Estate Reference: **Test Estate**
- Total Merchants: **3**
- Total Operators: **2**
- Total Contracts: **2**
- Total Users: **5**

#### Operators
- **Safaricom** (ID: 33333333-3333-3333-3333-333333333333)
  - Requires Custom Merchant Number: Yes
  - Requires Custom Terminal Number: No
- **Voucher** (ID: 33333333-3333-3333-3333-333333333334)
  - Requires Custom Merchant Number: No
  - Requires Custom Terminal Number: No

#### Contracts
- **Standard Transaction Contract** (Safaricom)
- **Voucher Sales Contract** (Voucher)

### Merchant Management Data

#### Merchants
1. **Test Merchant 1** (MERCH001)
   - Balance: £10,000.00
   - Available Balance: £8,500.00
   - Settlement Schedule: Immediate
   - Address: 123 Main Street, Test Town, Test Region, 12345, Test Country
   - Contact: John Smith, john@testmerchant.com, 555-1234

2. **Test Merchant 2** (MERCH002)
   - Balance: £5,000.00
   - Available Balance: £4,200.00
   - Settlement Schedule: Weekly

3. **Test Merchant 3** (MERCH003)
   - Balance: £15,000.00
   - Available Balance: £12,000.00
   - Settlement Schedule: Monthly

These values are defined in the application's `StubbedMediatorService` and can be updated when the test data is changed.

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
dotnet test EstateManagementUI.IntegrationTests.csproj
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

### Filter by Feature
```bash
# Run only Dashboard tests
dotnet test --filter "Category=DashboardTests"

# Run only Estate Management tests
dotnet test --filter "Category=EstateManagementTests"

# Run only Merchant Management tests
dotnet test --filter "Category=MerchantManagementTests"

# Run only Contract Management tests
dotnet test --filter "Category=ContractManagementTests"

# Run only Operator Management tests
dotnet test --filter "Category=OperatorManagementTests"
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
