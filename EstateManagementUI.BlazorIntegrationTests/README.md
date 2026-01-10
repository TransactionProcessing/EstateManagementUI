# EstateManagementUI.BlazorIntegrationTests

This is a **Testcontainers-based integration test project** that targets the **Blazor Server UI** (`EstateManagementUI.BlazorServer`) using **Playwright** for browser automation.

## Overview

This project provides a "zero-process" testing environment where the Blazor application is automatically built from its Dockerfile, deployed in a Docker container, and tested using BDD-style scenarios with Reqnroll and Playwright.

### Key Features

1. **Zero-Process Testing**: The application is automatically built from the Dockerfile and deployed in a container - no manual setup required
2. **Testcontainers Integration**: Uses DotNet.Testcontainers to manage the full container lifecycle
3. **Browser Automation**: Uses **Microsoft Playwright** for cross-browser testing (Chromium, Firefox, WebKit)
4. **BDD Framework**: **Reqnroll** (modern SpecFlow successor) with **NUnit** test runner
5. **Parallel Execution**: Tests run in parallel at the Children level for faster execution
6. **Test Environment**: Application runs in "Test" mode with isolated data (no external API calls)

## Technical Architecture

### Container Management (Clean DockerHelper)

The test framework uses a **clean, minimal DockerHelper** class that:

1. **Builds the Docker image** from `EstateManagementUI.BlazorServer/Dockerfile` using `ImageFromDockerfileBuilder`
2. **Starts a container** with Test environment configuration
3. **Manages the lifecycle** from setup to teardown
4. **No external dependencies** - pure Testcontainers implementation

**Key Features:**
- Automatic image building from Dockerfile
- Clean resource disposal (implements `IAsyncDisposable`)
- Comprehensive logging for debugging
- Test-specific environment configuration

```csharp
// Build image from Dockerfile
var imageBuilder = new ImageFromDockerfileBuilder()
    .WithDockerfileDirectory(repoRoot)
    .WithDockerfile("EstateManagementUI.BlazorServer/Dockerfile")
    .WithName($"estatemanagementuiblazor-test:{Guid.NewGuid():N}")
    .WithCleanUp(true);

IImage image = await imageBuilder.Build();

// Start container with Test environment
var container = new ContainerBuilder()
    .WithImage(image)
    .WithPortBinding(5004, true)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Test")
    .WithEnvironment("AppSettings:TestMode", "true")
    .Build();

await container.StartAsync();
```

### Test Lifecycle (Hooks)

Tests use Reqnroll hooks to manage the complete lifecycle with proper ordering:

- **BeforeTestRun (Order 0)**: 
  - Builds Docker image from Dockerfile
  - Starts the Blazor Server container
  - Exposes mapped port for testing
  
- **BeforeTestRun (Order 100)**: 
  - Installs Playwright browsers
  - Initializes Playwright runtime
  
- **BeforeScenario**: 
  - Creates a new browser context and page
  - Registers the page and DockerHelper for the scenario
  
- **AfterScenario**: 
  - Takes screenshot on test failure
  - Closes browser page
  
- **AfterTestRun (Order 0)**: 
  - Closes browser
  - Disposes Playwright runtime
  
- **AfterTestRun (Order 100)**: 
  - Stops the container
  - Cleans up Docker resources
  - Removes test images

### Parallel Execution

NUnit is configured for parallel execution at the Children level via `AssemblyInfo.cs`:

```csharp
[assembly: Parallelizable(ParallelScope.Children)]
[assembly: LevelOfParallelism(4)]
```

This allows multiple scenarios to run simultaneously across different browser instances.

## Project Structure

```
EstateManagementUI.BlazorIntegrationTests/
├── Common/
│   ├── BlazorUiHelpers.cs         # Playwright-based UI helper methods
│   ├── PlaywrightExtensions.cs    # Extension methods for Playwright IPage
│   ├── Hooks.cs                    # Reqnroll hooks for Playwright setup/teardown
│   ├── DockerHelper.cs             # Testcontainers-based Docker infrastructure
│   ├── TestingContext.cs           # Shared test context
│   ├── Setup.cs                    # Test setup configuration
│   ├── SharedSteps.cs              # Shared step definitions
│   └── GenericSteps.cs             # Generic step definitions
├── Features/
│   └── Framework/
│       └── FrameworkCheck.feature  # Framework smoke test
├── Steps/
│   ├── BlazorUiSteps.cs           # Blazor UI-specific step definitions
│   └── FrameworkCheckSteps.cs     # Framework verification step definitions
├── Tests/
│   ├── ContractTests.feature       # Contract feature tests
│   ├── EstateTests.feature         # Estate feature tests
│   ├── MerchantTests.feature       # Merchant feature tests
│   └── OperatorTests.feature       # Operator feature tests
├── Properties/
│   └── AssemblyInfo.cs             # NUnit parallel execution configuration
└── EstateManagementUI.BlazorIntegrationTests.csproj
```

## Key Components

### Framework Check Feature

The `FrameworkCheck.feature` provides a smoke test to verify the Testcontainers infrastructure:

```gherkin
@framework @smoke
Feature: Framework Check
    As a test automation engineer
    I want to verify that the Testcontainers-based testing framework is working correctly
    So that I can run integration tests against the containerized Blazor application

Background:
    Given the application is running in a container

Scenario: Home page is accessible
    When I navigate to the home page
    Then the page title should be visible
```

This test validates:
- ✓ Docker image builds successfully from Dockerfile
- ✓ Container starts with proper configuration
- ✓ Playwright can connect to the containerized application
- ✓ Basic navigation and page loading works

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
- **BeforeTestRun**: Installs Playwright browsers and initializes runtime
- **BeforeScenario**: Creates a new browser context and page for each scenario
- **AfterScenario**: Takes screenshots on failure and closes the page
- **AfterTestRun**: Closes browser and cleans up Playwright

### Docker Helper (`DockerHelper.cs`)

**Clean, minimal Docker helper** for Testcontainers-based testing. This is a from-scratch implementation with zero dependencies on legacy Docker infrastructure.

**Key Features:**
- **Image Building**: Automatically builds from `EstateManagementUI.BlazorServer/Dockerfile`
- **Container Management**: Starts, monitors, and stops the Blazor container
- **Test Configuration**: Configures app for Test environment with isolated data
- **Resource Cleanup**: Implements `IAsyncDisposable` for proper cleanup
- **Comprehensive Logging**: Detailed console output for debugging

**API:**
```csharp
public class DockerHelper : IAsyncDisposable
{
    public int EstateManagementUiPort { get; }      // Mapped host port
    public bool IsRunning { get; }                   // Container status
    
    public Task StartContainerAsync();               // Build & start
    public Task StopContainerAsync();                // Stop & cleanup
    public ValueTask DisposeAsync();                 // IAsyncDisposable
}
```

**Environment Configuration:**
- **ASPNETCORE_ENVIRONMENT**: `Test` (uses appsettings.Test.json)
- **AppSettings:TestMode**: `true` (enables in-memory test data)
- **AppSettings:TestUserRole**: `Estate` (default test user role)
- **AppSettings:HttpClientIgnoreCertificateErrors**: `true` (for testing)

## Test Environment

The Blazor container runs in Test mode with the following characteristics:
- **Isolated Data**: Uses in-memory test data, no external API calls
- **No Dependencies**: Doesn't require backend services (Security, Transaction Processor, etc.)
- **Fast Startup**: Minimal configuration for quick test execution
- **SSL Support**: Runs on HTTPS with certificate validation disabled for testing

## Running Tests

### Prerequisites

1. **.NET 10.0 SDK** or later
2. **Docker** (Docker Desktop on Windows/Mac, Docker Engine on Linux)
3. **Access to private NuGet feed** (feedz.io) - configured in NuGet.Config

### Quick Start

```bash
# Navigate to the test project
cd EstateManagementUI.BlazorIntegrationTests

# Run all tests (builds container automatically)
dotnet test

# Run only framework smoke tests
dotnet test --filter "Category=framework"

# Run with specific browser
Browser=Firefox dotnet test

# Run in headless mode (CI)
IsCI=true dotnet test
```

### Test Execution Flow

1. **Build Phase**: Testcontainers builds the Docker image from `EstateManagementUI.BlazorServer/Dockerfile`
2. **Container Start**: The built image is started with test-specific configuration
3. **Browser Setup**: Playwright initializes and launches the configured browser(s)
4. **Test Run**: Tests execute in parallel (up to 4 scenarios simultaneously)
5. **Cleanup**: Containers are stopped, browsers closed, and Docker resources cleaned up

### Environment Variables

- **`Browser`**: Browser type (`Chrome`, `Firefox`, `WebKit`) - defaults to Chrome
- **`IsCI`**: Set to `true` for headless mode in CI/CD - defaults to false (headed mode)

### Parallel Execution

Tests run in parallel at the scenario level (NUnit Children scope). The default parallelism is 4 concurrent scenarios, configured in `Properties/AssemblyInfo.cs`:

```csharp
[assembly: Parallelizable(ParallelScope.Children)]
[assembly: LevelOfParallelism(4)]
```

### Test Output

- **Console Output**: Real-time test execution logs
- **Screenshots**: Captured on test failure in the test output directory
- **Container Logs**: Available via Docker logs if needed for debugging

## Troubleshooting

### Docker Build Fails

If the Docker build fails:
1. Check Docker is running: `docker ps`
2. Verify the Dockerfile exists: `ls ../EstateManagementUI.BlazorServer/Dockerfile`
3. Ensure you have Node.js available in the Docker build (required for Tailwind CSS)

### Container Won't Start

If the container fails to start:
1. Check Docker logs: `docker logs <container-name>`
2. Verify port 5004 is not already in use
3. Check Docker has sufficient resources allocated

### Playwright Issues

If Playwright fails to connect:
1. Ensure Playwright browsers are installed (done automatically in BeforeTestRun)
2. Check the container is accessible: `curl -k https://localhost:<port>`
3. Verify SSL certificate handling is configured

### NuGet Restore Fails

If package restore fails:
1. Verify access to feedz.io private feed
2. Check credentials in `NuGet.Config`
3. Clear NuGet cache: `dotnet nuget locals all --clear`

## Implementation Status

### ✅ Completed
- **Testcontainers Integration**: DockerHelper builds from Dockerfile on-the-fly
- **Parallel Execution**: NUnit configured for Children-level parallelism (4 concurrent scenarios)
- **Framework Check**: Smoke test feature to verify infrastructure works correctly
- **Playwright Setup**: Browser lifecycle management via Hooks
- **Test Environment**: Application configured for Test mode with isolated data
- **Project Structure**: Features folder established for organizing tests

### 📝 Future Enhancements
1. Expand feature coverage (Contracts, Estates, Merchants, Operators)
2. Add more UI helper methods as needed
3. Implement additional step definitions for business scenarios
4. CI/CD pipeline integration
5. Performance and load testing scenarios

## Technical Notes

### Differences from Pre-built Image Approach

The key innovation in this framework is using **ImageFromDockerfileBuilder** instead of pulling pre-built images:

**Before:**
```csharp
.WithImage("estatemanagementuiblazorserver")  // Pre-built image
```

**After:**
```csharp
var imageBuilder = new ImageFromDockerfileBuilder()
    .WithDockerfileDirectory(repoRoot)
    .WithDockerfile("EstateManagementUI.BlazorServer/Dockerfile")
    .WithName($"estatemanagementuiblazorserver-test:{this.TestId:N}")
    .WithCleanUp(true);
IImage image = await imageBuilder.Build();
```

This ensures tests always run against the latest code changes without requiring manual image builds.

### Playwright vs Selenium

| Selenium | Playwright |
|----------|------------|
| `IWebDriver` | `IPage` |
| `FindElement(By.Id("x"))` | `Locator("#x")` |
| `element.SendKeys(text)` | `locator.FillAsync(text)` |
| `element.Click()` | `locator.ClickAsync()` |
| `element.Text` | `await locator.TextContentAsync()` |
| `element.GetDomProperty("value")` | `await locator.InputValueAsync()` |

Playwright is fully async and provides better cross-browser support with modern selectors.
