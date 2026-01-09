# EstateManagementUI.BlazorIntegrationTests

This is a new integration test project that targets the **Blazor Server UI** (`EstateManagementUI.BlazorServer`) using **Playwright** for browser automation.

## Overview

This project replaces the Selenium-based integration tests in `EstateManagementUI.IntegrationTests` with a Playwright-based implementation targeting the new Blazor UI.

### Key Differences from Original Integration Tests

1. **Target Application**: Tests the Blazor Server UI (`EstateManagementUI.BlazorServer`) instead of the old ASP.NET Core UI (`EstateManagementUI`)
2. **Browser Automation**: Uses **Microsoft Playwright** instead of Selenium WebDriver
3. **Docker Image**: Targets the `estatemanagementuiblazor` Docker image

## Project Structure

```
EstateManagementUI.BlazorIntegrationTests/
├── Common/
│   ├── BlazorUiHelpers.cs         # Playwright-based UI helper methods
│   ├── PlaywrightExtensions.cs    # Extension methods for Playwright IPage
│   ├── Hooks.cs                    # Reqnroll hooks for Playwright setup/teardown
│   ├── DockerHelper.cs             # Docker infrastructure (adapted for Blazor)
│   ├── TestingContext.cs           # Shared test context
│   ├── Setup.cs                    # Test setup configuration
│   ├── SharedSteps.cs              # Shared step definitions
│   └── GenericSteps.cs             # Generic step definitions
├── Steps/
│   └── BlazorUiSteps.cs           # Blazor UI-specific step definitions
├── Tests/
│   ├── ContractTests.feature       # Contract feature tests
│   ├── EstateTests.feature         # Estate feature tests
│   ├── MerchantTests.feature       # Merchant feature tests
│   └── OperatorTests.feature       # Operator feature tests
└── EstateManagementUI.BlazorIntegrationTests.csproj
```

## Key Components

### Playwright Extensions (`PlaywrightExtensions.cs`)

Provides extension methods for `IPage` that mirror the Selenium extension patterns but use Playwright APIs:
- `FillIn()`, `FillInById()`, `FillInNumeric()` - Form field interactions
- `FindButtonById()`, `FindButtonByText()` - Button locators
- `ClickButtonById()`, `ClickButtonByText()` - Button click actions
- `SelectDropDownByIdAndText()`, `SelectDropDownByIdAndValue()` - Dropdown interactions
- `GetTextById()`, `GetValueById()` - Element value retrieval
- `WaitForElement()`, `IsElementVisible()` - Element visibility checks

### Blazor UI Helpers (`BlazorUiHelpers.cs`)

High-level test methods for interacting with the Blazor UI:
- Navigation methods (e.g., `NavigateToHomePage()`, `ClickContractsSidebarOption()`)
- Screen verification methods (e.g., `VerifyOnTheContractsListScreen()`)
- Table validation methods (e.g., `VerifyTheContractDetailsAreInTheList()`)

### Hooks (`Hooks.cs`)

Manages Playwright browser lifecycle:
- **BeforeTestRun**: Installs Playwright browsers
- **BeforeScenario**: Creates a new browser page for each scenario
- **AfterScenario**: Takes screenshots on failure and closes the page
- **AfterTestRun**: Closes browser and cleans up Playwright

### Docker Helper (`DockerHelper.cs`)

Manages Docker containers for integration tests, adapted for Blazor:
- Starts the Blazor UI container with proper environment variables
- Uses `Authentication:` prefix for OIDC settings (instead of `AppSettings:`)
- Configures API client credentials
- Sets up database connections and other dependencies

## Environment Configuration

The Blazor container is configured with:
- **Authentication**: OIDC with Security Service
- **API Client**: Backend service credentials
- **Database**: Transaction Processor Read Model connection
- **Development Mode**: Configured for integration testing

## Running Tests

### Prerequisites

1. .NET 10.0 SDK
2. Docker
3. Access to private NuGet feed (feedz.io)

### Installation

```bash
# Install Playwright browsers
pwsh bin/Debug/net10.0/playwright.ps1 install
```

### Running Tests

```bash
cd EstateManagementUI.BlazorIntegrationTests
dotnet test
```

### Environment Variables

- `Browser`: Browser type (`Chrome`, `Firefox`, `WebKit`) - defaults to Chrome
- `IsCI`: Set to `true` for headless mode in CI/CD

### Multi-Browser Testing

The framework supports testing across multiple browsers:

**Chrome (Default):**
```bash
dotnet test
# OR
Browser=Chrome dotnet test
```

**Firefox:**
```bash
Browser=Firefox dotnet test
```

**Microsoft Edge/Safari (WebKit):**
```bash
Browser=WebKit dotnet test
```

### Parallel Test Execution

Tests are configured to run in parallel for faster execution:

**Using Default Settings (3 parallel workers):**
```bash
dotnet test
```

**Using Custom Run Settings:**
```bash
dotnet test --settings test.runsettings
```

**Custom Number of Workers:**
```bash
dotnet test -- NUnit.NumberOfTestWorkers=5
```

**Disable Parallel Execution:**
```bash
dotnet test -- NUnit.NumberOfTestWorkers=0
```

The parallel execution is configured at the fixture level, meaning:
- Each feature file runs as a separate fixture
- Multiple feature files can run in parallel
- Scenarios within a feature file run sequentially
- Each test gets its own browser context for isolation

### Running Specific Test Categories

Tests are organized by tags matching application sections:

**Run all tests:**
```bash
dotnet test
```

**Run only Contract tests:**
```bash
dotnet test --filter "TestCategory=shared|TestCategory=uigeneral"
```

**Run specific feature file:**
```bash
dotnet test --filter "FullyQualifiedName~ContractTests"
```

**Run only PR tests:**
```bash
dotnet test --filter "TestCategory=PRTest"
```

## Test Organization

### Feature Files by Application Section

The tests are organized into separate feature files for each major application section:

1. **EstateTests.feature** - Estate management functionality
2. **MerchantTests.feature** - Merchant creation, editing, and management
3. **OperatorTests.feature** - Operator management
4. **ContractTests.feature** - Contract and product management
5. **FileProcessingTests.feature** - File upload and processing
6. **PermissionsTests.feature** - User permissions and role management
7. **ReportingTests.feature** - All reporting features with query filters:
   - Transaction Detail Report (with filters: date, merchant, operator, status)
   - Transaction Summary by Merchant (with grouping options)
   - Transaction Summary by Operator
   - Settlement Summary
   - Settlement Reconciliation
   - Merchant Settlement History
   - Product Performance
   - Analytical Charts
   - Export functionality (CSV, Excel)

### Test Data Management

The framework uses an in-memory TestDataStore when running in offline mode:

- **Default Test Data**: Pre-populated with estates, merchants, operators, contracts
- **CRUD Operations**: Full support for Create, Read, Update, Delete
- **Test Isolation**: Reset between scenarios for clean state
- **Thread-Safe**: Concurrent dictionary implementation

See [TEST_INFRASTRUCTURE.md](./TEST_INFRASTRUCTURE.md) for details on test data management.

## Current Status

### ✅ Completed
- Project structure created
- Playwright dependencies added (supports Chrome, Firefox, WebKit)
- Core Playwright extension methods implemented
- UI helper methods implemented
- Docker infrastructure adapted for Blazor
- Test hooks implemented with Playwright lifecycle management
- Feature files for all application sections:
  - Estate, Merchant, Operator, Contract tests (existing)
  - FileProcessing tests (new)
  - Permissions tests (new)
  - Reporting tests with comprehensive query filters (new)
- Parallel test execution configured (NUnit)
- Multi-browser support enabled
- Test data infrastructure with TestDataStore
- Offline testing mode (no backend dependencies)
- Extensibility framework for future online mode
- Documentation complete

### 🎯 Framework Capabilities

✅ **Multi-Browser Support**: Chrome, Firefox, Edge/Safari (WebKit)  
✅ **Parallel Execution**: Configurable worker threads (default: 3)  
✅ **Reqnroll/SpecFlow**: BDD-style feature files  
✅ **Offline Testing**: No backend API required  
✅ **NUnit Test Backend**: Industry-standard test framework  
✅ **Feature Files by Section**: Organized by application area  
✅ **Extensible Architecture**: Ready for real auth/backend integration  

See [EXTENSIBILITY_GUIDE.md](./EXTENSIBILITY_GUIDE.md) for information on extending the framework to use real authentication endpoints and backend APIs.

## Differences from Selenium Tests

### API Changes

| Selenium | Playwright |
|----------|------------|
| `IWebDriver` | `IPage` |
| `FindElement(By.Id("x"))` | `Locator("#x")` |
| `element.SendKeys(text)` | `locator.FillAsync(text)` |
| `element.Click()` | `locator.ClickAsync()` |
| `element.Text` | `await locator.TextContentAsync()` |
| `element.GetDomProperty("value")` | `await locator.InputValueAsync()` |

### Async/Await

Playwright is fully async, so all methods must use `await` and return `Task`.

### Locators

Playwright uses modern selectors and encourages role-based selection:
- `page.GetByRole(AriaRole.Button, new() { Name = "Login" })`
- `page.Locator("#elementId")`
- `page.Locator("[name='fieldName']")`

## Notes

- The Docker image name for the Blazor UI is assumed to be `estatemanagementuiblazor`
- Authentication configuration uses the `Authentication:` prefix to match the Blazor app's configuration
- All tests use the same Docker infrastructure as the original integration tests
- The project maintains compatibility with Reqnroll (SpecFlow successor) for BDD-style tests
