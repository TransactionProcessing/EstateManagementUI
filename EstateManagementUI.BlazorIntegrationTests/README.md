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

## Current Status

### ✅ Completed
- Project structure created
- Playwright dependencies added
- Core Playwright extension methods implemented
- Basic UI helper methods implemented
- Docker infrastructure adapted for Blazor
- Test hooks implemented with Playwright lifecycle management
- Feature files copied from original tests

### 🚧 In Progress
- Additional UI helper methods need implementation
- More step definitions needed to cover all scenarios
- Build and test execution validation

### 📝 Next Steps
1. Complete all UI helper methods in `BlazorUiHelpers.cs`
2. Implement all step definitions in `BlazorUiSteps.cs`
3. Ensure all feature files are compatible with Blazor UI
4. Build and run tests to verify functionality
5. Add CI/CD integration

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
