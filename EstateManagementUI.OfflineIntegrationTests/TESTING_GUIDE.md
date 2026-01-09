# Integration Testing Framework - Complete Guide

## Overview

This document provides a comprehensive guide to the offline integration testing framework for the EstateManagementUI Blazor application. The framework is designed to test the complete UI functionality without requiring backend API dependencies.

## Table of Contents

1. [Architecture](#architecture)
2. [Getting Started](#getting-started)
3. [Running Tests](#running-tests)
4. [Multi-Browser Testing](#multi-browser-testing)
5. [Parallel Execution](#parallel-execution)
6. [Writing New Tests](#writing-new-tests)
7. [Test Data Management](#test-data-management)
8. [Troubleshooting](#troubleshooting)
9. [CI/CD Integration](#cicd-integration)

## Architecture

### Framework Components

```
┌─────────────────────────────────────────────────────────┐
│                 Feature Files (.feature)                │
│         BDD scenarios in Gherkin syntax                 │
└─────────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────────┐
│              Step Definitions (.cs)                     │
│    Implements Given/When/Then steps                     │
└─────────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────────┐
│              UI Helpers (BlazorUiHelpers.cs)            │
│    High-level UI interaction methods                    │
└─────────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────────┐
│         Playwright Extensions                           │
│    Low-level browser automation                         │
└─────────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────────┐
│              Browser (Chrome/Firefox/WebKit)            │
│         Actual browser automation                       │
└─────────────────────────────────────────────────────────┘
```

### Test Execution Flow

1. **BeforeTestRun**: Install Playwright browsers, initialize services
2. **BeforeScenario**: Create browser page, reset test data
3. **Test Execution**: Run Given/When/Then steps
4. **AfterScenario**: Take screenshot on failure, close page
5. **AfterTestRun**: Clean up browser and Playwright

## Getting Started

### Prerequisites

- .NET 10.0 SDK
- Node.js (for Playwright browser installation)
- Git
- **The Blazor application running with TestMode enabled**

### Initial Setup

**Step 1: Start the Blazor Application**

The offline integration tests require the Blazor Server application to be running. Open a terminal and start it:

```bash
# Navigate to the Blazor Server project
cd EstateManagementUI/EstateManagementUI.BlazorServer

# Set the application to test mode to use TestDataStore
export TestMode=true  # On Linux/Mac
# OR on Windows PowerShell: $env:TestMode="true"

# Start the application (runs on https://localhost:5004)
dotnet run
```

Keep this terminal running. The application must be accessible at `https://localhost:5004` for tests to work.

**Step 2: Setup Test Project (in a new terminal)**

```bash
# Navigate to the test project
cd EstateManagementUI/EstateManagementUI.OfflineIntegrationTests

# Restore NuGet packages
dotnet restore

# Install Playwright browsers (first time only)
pwsh bin/Debug/net10.0/playwright.ps1 install
# OR on Linux/Mac:
# dotnet tool install --global Microsoft.Playwright.CLI
# playwright install
```

### Configuration

The framework uses `appsettings.json` for configuration:

```json
{
  "TestSettings": {
    "SkipRemoteCalls": true,
    "EnableTestMode": true
  }
}
```

## Running Tests

### Basic Test Execution

**Important**: Ensure the Blazor application is running on https://localhost:5004 before running tests.

**Run all tests:**
```bash
dotnet test
```

**Run with verbose output:**
```bash
dotnet test --logger "console;verbosity=detailed"
```

**Run specific feature file:**
```bash
dotnet test --filter "FullyQualifiedName~ContractTests"
dotnet test --filter "FullyQualifiedName~MerchantTests"
dotnet test --filter "FullyQualifiedName~ReportingTests"
```

**Run specific scenario:**
```bash
dotnet test --filter "FullyQualifiedName~CreateNewMerchant"
```

**Run tests with specific tag:**
```bash
dotnet test --filter "TestCategory=PRTest"
```

### Using Run Settings

```bash
# Use custom run settings file
dotnet test --settings test.runsettings

# Override specific settings
dotnet test --settings test.runsettings -- TestRunParameters.Parameter(name="Browser",value="Firefox")
```

## Multi-Browser Testing

### Supported Browsers

The framework supports three browser engines via Playwright:

1. **Chromium** (Chrome/Edge) - Default
2. **Firefox**
3. **WebKit** (Safari/Edge)

### Running Tests on Different Browsers

**Chrome (Default):**
```bash
dotnet test
# OR explicitly
Browser=Chrome dotnet test
```

**Firefox:**
```bash
Browser=Firefox dotnet test
```

**WebKit (Safari/Edge):**
```bash
Browser=WebKit dotnet test
```

### Browser-Specific Features

**Chrome/Chromium:**
- Best for debugging with DevTools
- Fastest execution
- Most stable for CI/CD

**Firefox:**
- Good for cross-browser compatibility
- Different rendering engine
- Useful for catching browser-specific issues

**WebKit:**
- Simulates Safari behavior
- Important for Mac/iOS users
- Tests Edge (Chromium) compatibility

### Headless Mode

**Local Development (Headed):**
```bash
dotnet test
```

**CI/CD (Headless):**
```bash
IsCI=true dotnet test
```

## Parallel Execution

### Configuration

Parallel execution is configured in `AssemblyInfo.cs`:

```csharp
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(3)]
```

### Parallel Execution Levels

**Fixture Level (Current):**
- Each feature file runs in parallel
- Scenarios within a feature run sequentially
- Provides good balance of speed and stability

**Scenario Level (Advanced):**
```csharp
[assembly: Parallelizable(ParallelScope.All)]
```

### Controlling Parallelism

**Use default (3 workers):**
```bash
dotnet test
```

**Custom number of workers:**
```bash
dotnet test -- NUnit.NumberOfTestWorkers=5
```

**Disable parallel execution:**
```bash
dotnet test -- NUnit.NumberOfTestWorkers=0
```

**In test.runsettings:**
```xml
<NUnit>
  <NumberOfTestWorkers>5</NumberOfTestWorkers>
</NUnit>
```

### Best Practices for Parallel Testing

1. **Test Isolation**: Each test should be independent
2. **Browser Contexts**: Each scenario gets its own browser context
3. **Test Data**: Use `Reset()` in BeforeScenario for clean state
4. **Resource Limits**: Don't exceed CPU cores (use 3-5 workers typically)
5. **CI/CD**: May need fewer workers in cloud environments

## Writing New Tests

### Creating a New Feature File

**1. Create the feature file:**
```gherkin
# Tests/NewFeatureTests.feature
@base @shared @newfeature
Feature: New Feature Tests

Background:
    # Standard setup steps
    Given I create the following roles
    | Role Name  |
    | Estate |
    
    # ... more setup
    
    Given I am on the application home page
    And I click on the Sign In Button
    Then I am presented with a login screen
    When I login with the username 'estateuser@testestate1.co.uk' and password '123456'
    Then I am presented with the Estate Administrator Dashboard

@PRTest
Scenario: Test New Feature
    When I click on the New Feature Sidebar option
    Then I am presented with the New Feature screen
    And the new feature content should be displayed
```

**2. Generate step definitions:**

Reqnroll will automatically generate code-behind files. If you need custom steps:

**Steps/NewFeatureSteps.cs:**
```csharp
using Reqnroll;
using Microsoft.Playwright;

[Binding]
public class NewFeatureSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly IPage _page;

    public NewFeatureSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        var scenarioName = scenarioContext.ScenarioInfo.Title.Replace(" ", "");
        _page = scenarioContext.ScenarioContainer.Resolve<IPage>(scenarioName);
    }

    [When(@"I click on the New Feature Sidebar option")]
    public async Task WhenIClickOnTheNewFeatureSidebarOption()
    {
        await _page.ClickLinkByText("New Feature");
    }

    [Then(@"I am presented with the New Feature screen")]
    public async Task ThenIAmPresentedWithTheNewFeatureScreen()
    {
        var title = await _page.TitleAsync();
        title.ShouldContain("New Feature");
    }

    [Then(@"the new feature content should be displayed")]
    public async Task ThenTheNewFeatureContentShouldBeDisplayed()
    {
        var isVisible = await _page.IsElementVisible("#newFeatureContent");
        isVisible.ShouldBeTrue();
    }
}
```

### Feature File Best Practices

1. **One feature file per application section**
2. **Use descriptive scenario names**
3. **Keep scenarios focused and atomic**
4. **Use Background for common setup**
5. **Tag scenarios appropriately** (@PRTest, @smoke, etc.)
6. **Document complex scenarios with comments**

### Step Definition Best Practices

1. **Reuse existing steps when possible**
2. **Keep steps small and focused**
3. **Use async/await for all Playwright calls**
4. **Add proper error handling**
5. **Use descriptive variable names**
6. **Add comments for complex logic**

## Test Data Management

### Using TestDataStore

The framework uses an in-memory TestDataStore for test data:

**Access in step definitions:**
```csharp
private readonly TestDataHelper _testDataHelper;

public MySteps(TestDataHelper testDataHelper)
{
    _testDataHelper = testDataHelper;
}

[Given(@"I have a merchant with balance '(.*)'")]
public void GivenIHaveAMerchantWithBalance(string balance)
{
    var estateId = _testDataHelper.DefaultEstateId;
    var merchant = _testDataHelper.GetMerchants(estateId).First();
    merchant.Balance = decimal.Parse(balance);
    _testDataHelper.UpdateMerchant(estateId, merchant);
}
```

### Default Test Data

The TestDataStore initializes with:

**Estate:**
- ID: `11111111-1111-1111-1111-111111111111`
- Name: "Test Estate"

**Merchants:**
- Test Merchant 1 (ID: `22222222-2222-2222-2222-222222222222`)
- Test Merchant 2 (ID: `22222222-2222-2222-2222-222222222223`)
- Test Merchant 3 (ID: `22222222-2222-2222-2222-222222222224`)

**Operators:**
- Safaricom (ID: `33333333-3333-3333-3333-333333333333`)
- Voucher (ID: `33333333-3333-3333-3333-333333333334`)

**Contracts:**
- Standard Transaction Contract
- Voucher Sales Contract

### Resetting Test Data

Test data is automatically reset before each scenario:

```csharp
[BeforeScenario]
public void BeforeScenario()
{
    _testDataHelper.Reset();
}
```

## Troubleshooting

### Common Issues

**Issue: Playwright browsers not installed**
```
Error: Executable doesn't exist at [path]
```
**Solution:**
```bash
pwsh bin/Debug/net10.0/playwright.ps1 install
```

**Issue: Tests timing out**
```
Error: Timeout 30000ms exceeded
```
**Solution:**
- Check if the Blazor app is running
- Increase timeout in PlaywrightExtensions.cs
- Check network connectivity

**Issue: Element not found**
```
Error: Locator.ClickAsync: Target closed
```
**Solution:**
- Verify element selector is correct
- Add explicit wait before interaction
- Check if element is in iframe or shadow DOM

**Issue: Tests failing in parallel**
```
Error: Browser context closed
```
**Solution:**
- Ensure each test has its own browser context
- Check for shared state between tests
- Review parallel execution level

### Debug Mode

**Run single test in headed mode:**
```bash
IsCI=false dotnet test --filter "FullyQualifiedName~SpecificTest"
```

**Enable verbose Playwright logging:**
```bash
DEBUG=pw:api dotnet test
```

**Take manual screenshots:**
```csharp
await _page.ScreenshotAsync(new() { Path = "debug.png", FullPage = true });
```

## CI/CD Integration

### GitHub Actions Example

```yaml
name: Integration Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    
    strategy:
      matrix:
        browser: [Chrome, Firefox, WebKit]
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '10.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Install Playwright
      run: pwsh EstateManagementUI.BlazorIntegrationTests/bin/Debug/net10.0/playwright.ps1 install
    
    - name: Run tests
      env:
        Browser: ${{ matrix.browser }}
        IsCI: true
      run: dotnet test --logger "trx;LogFileName=test-results.trx"
    
    - name: Upload test results
      if: always()
      uses: actions/upload-artifact@v3
      with:
        name: test-results-${{ matrix.browser }}
        path: '**/test-results.trx'
```

### Azure DevOps Example

```yaml
trigger:
  - main

pool:
  vmImage: 'ubuntu-latest'

strategy:
  matrix:
    Chrome:
      Browser: 'Chrome'
    Firefox:
      Browser: 'Firefox'
    WebKit:
      Browser: 'WebKit'

steps:
- task: UseDotNet@2
  inputs:
    version: '10.0.x'

- script: dotnet restore
  displayName: 'Restore dependencies'

- pwsh: |
    dotnet build --no-restore
    pwsh EstateManagementUI.BlazorIntegrationTests/bin/Debug/net10.0/playwright.ps1 install
  displayName: 'Build and install Playwright'

- script: dotnet test --no-build --logger trx
  env:
    Browser: $(Browser)
    IsCI: true
  displayName: 'Run integration tests'

- task: PublishTestResults@2
  condition: always()
  inputs:
    testResultsFormat: 'VSTest'
    testResultsFiles: '**/*.trx'
```

## Performance Optimization

### Tips for Faster Tests

1. **Use parallel execution** (3-5 workers)
2. **Run in headless mode** for CI/CD
3. **Minimize waits** - rely on Playwright's auto-waiting
4. **Reuse browser contexts** where appropriate
5. **Use TestDataStore** instead of setting up via UI
6. **Skip unnecessary navigation** between tests
7. **Use tags** to run only necessary tests

### Recommended Test Organization

```bash
# Fast smoke tests for every commit
dotnet test --filter "TestCategory=smoke"

# PR tests before merge
dotnet test --filter "TestCategory=PRTest"

# Full regression nightly
dotnet test
```

## Additional Resources

- [Playwright .NET Documentation](https://playwright.dev/dotnet/)
- [Reqnroll Documentation](https://docs.reqnroll.net/)
- [NUnit Parallel Execution](https://docs.nunit.org/articles/nunit/writing-tests/attributes/parallelizable.html)
- [EXTENSIBILITY_GUIDE.md](./EXTENSIBILITY_GUIDE.md) - Extending the framework
- [TEST_INFRASTRUCTURE.md](./TEST_INFRASTRUCTURE.md) - Test infrastructure details
- [MIGRATION_GUIDE.md](./MIGRATION_GUIDE.md) - Selenium to Playwright migration

## Support

For issues or questions:
1. Check this documentation
2. Review existing feature files for examples
3. Check Playwright documentation for browser automation issues
4. Open an issue in the repository
